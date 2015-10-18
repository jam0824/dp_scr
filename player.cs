﻿using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

		const int NODAMAGE_TIME = 50;	//やられた時の無敵時間

		public GameObject prefab;
		public GameObject explosion01;
		public GameObject bombPrefab;
		public GameObject missilePrefab;
		public GameObject smokePrefab;

		int speed = 150;
		float moveOffset = 0.04f;
		float shotDelay = 0.05f;
		Vector3 oldPos;

		int gameCount = 0;
		int bulletWaitTime = 5;	//弾を打つまでの待ちフレーム
		int noDamageCount = 0;	//無敵時間カウント
		int powerUpNum = 25;	//パワーアップ必要アイテム数

		GameManager gameManager;
		SoundManager soundManager;
		EffectManager effectManager;

		//******************************************************
		// Use this for initialization
		void Start () {
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
		}

		//******************************************************
		// Update is called once per frame
		void Update () {
				if (gameManager.life == 0)
						return;

				movePlayerToTouch ();
				keyBoardMove ();

				//定期フレームごとに弾発射
				if(gameCount % bulletWaitTime == 0){
						keyCheck ();
						//ダメージの際の処理
						if(noDamageCount > 0)
							noDamageCount = DamageCheck (noDamageCount);
				}
						
				gameCount++;

		}

		/// <summary>
		/// Keies the check.
		/// </summary>
		void keyCheck(){
				if ((Input.GetButton ("Fire1")) || (Input.GetMouseButton (0))) {
						Shot ();
						if(gameCount % 180 == 0){
								makeMissile ();
						}
				}
				//セカンドボタンでボム
				if (Input.GetButton ("Fire2")) {
						bool b = makeBomb ();
				}
		}

		//******************************************************
		//ダメージ処理
		/// <summary>
		/// Damages the check.
		/// </summary>
		/// <returns>The check.</returns>
		/// <param name="cnt">Count.</param>
		int DamageCheck(int cnt){
				//やられの点滅処理
				GetComponent<SpriteRenderer> ().enabled = (GetComponent<SpriteRenderer> ().enabled) ? false : true;

				cnt--;
				//無敵時間終了
				if(cnt == 0){
						returnDamageMode ();
				}
				return cnt;
		}

		//******************************************************
		/// <summary>
		/// キーボードでの移動
		/// </summary>
		void keyBoardMove(){
				//マウス入力だったら返す
				if (Input.GetMouseButton (0))
						return;

				float speed = 0.05f;
				float x = Input.GetAxis("Horizontal");
				float y = Input.GetAxis ("Vertical");

				Vector3 myPos = transform.position;
				myPos.x += x * speed;
				myPos.y += y * speed;

				transform.position = myPos;
				//画面外に出そうになった時の処理
				oldPos = checkOutOfScreen (Camera.main.WorldToViewportPoint (myPos), oldPos, x, y);
		}
				
		//******************************************************
		//player movement to use touching
		void movePlayerToTouch(){
			if (Input.GetMouseButton(0)){
				Vector3 screen_vec = Input.mousePosition;	//Get toch points (screen)
				screen_vec.z = 5.4f;
				
				//Exchange screenPotison to world position
				Vector3 world_vec = Camera.main.ScreenToWorldPoint(screen_vec);
				Vector3 nVec = Vector3.Normalize(world_vec - this.transform.position);


				//if distance so close between mouse and player, don't move it;
				if((Mathf.Abs(world_vec.x - this.transform.position.x) < moveOffset) && 
				   (Mathf.Abs(world_vec.y - this.transform.position.y) < moveOffset)){
				}
				else{
						//画面外に出そうな時の処理
						oldPos = checkOutOfScreen (Camera.main.WorldToViewportPoint (transform.position), oldPos, nVec.x, nVec.y);
						this.GetComponent<Rigidbody2D> ().AddForce (nVec * speed);
				
				}
				
			}
			return;
		}

		/// <summary>
		/// Checks the out of screen.
		/// </summary>
		/// <param name="pos">Position.</param>
		/// <param name="oldPos">Old position.</param>
		/// <param name="vx">Vx.xの増分</param>
		/// <param name="vy">Vy.yの増分</param>
		Vector3 checkOutOfScreen(Vector3 pos, Vector3 oldPos, float vx, float vy){
				//画面外だったら
				if ((pos.x < 0.0f) || (pos.x > 1.0f) || (pos.y < 0.0f) || (pos.y > 1.0f)) {
						this.transform.position = oldPos;	//前回の位置に戻しておく（０以下で動かなくなってしまうため）
						return oldPos;
				} else {
						oldPos = transform.position;	//位置を保持しておく
						setAnimeTrigger (vx, vy);
						return oldPos;
				}
		}

		//******************************************
		//移動アニメーション
		void setAnimeTrigger(float x, float y){

				if((x < 0) && (y < 0) && (x < y)){
						this.GetComponent<Animator>().SetTrigger("leftTrigger");
				}
				else if((x < 0) && (y < 0) && (x >= y)){
						this.GetComponent<Animator>().SetTrigger("frontTrigger");
				}
				else if((x < 0) && (y >= 0) && (x < -y)){
						this.GetComponent<Animator>().SetTrigger("leftTrigger");
				}
				else if((x < 0) && (y >= 0) && (x >= -y)){
						this.GetComponent<Animator>().SetTrigger("frontTrigger");
				}
				else if((x >= 0) && (y < 0) && (-y < x)){
						this.GetComponent<Animator>().SetTrigger("rightTrigger");
				}
				else if((x >= 0) && (y < 0) && (-y >= x)){
						this.GetComponent<Animator>().SetTrigger("frontTrigger");
				}
				else if((x >= 0) && (y >= 0) && (y < x)){
						this.GetComponent<Animator>().SetTrigger("rightTrigger");
				}
				else if((x >= 0) && (y >= 0) && (y >= x)){
						this.GetComponent<Animator>().SetTrigger("frontTrigger");
				}


		}

		//******************************************************
		public bool makeBomb(){
				if ((gameManager.bombFlag) || (gameManager.bomb == 0)) {
						return false;
				}
				gameManager.decBomb ();
				gameManager.bombFlag = true;
				GameObject gs = Instantiate (bombPrefab, new Vector3(0,0,0), this.transform.rotation) as GameObject;
				soundManager.playSE ("bomb");
				return true;
		}

		//******************************************************

		void Shot(){
				float angle = 3f;
				float speed = 30.0f;
				float n = Mathf.Floor (gameManager.power / powerUpNum);
				float startAngle = 90 - (n * angle);	//発射の初期位置決め

				//奇数回数描画
				for(int i = 0; i < (2 * n + 1); i++){
						float r = 5f;
						makeShot (startAngle + (Random.value * r - r / 2), speed);
						startAngle += angle;
				}

				soundManager.playSE ("playerBullet");
		}
		//Making one bullet object
		void makeShot(float direction, float speed){
				Vector3 pos = this.transform.position;
				pos.y += 0.3f;
				GameObject go = ObjectPool.instance.GetGameObject (prefab, pos, this.transform.rotation, direction);
				go.transform.rotation = Quaternion.Euler(0, 0, 0);
				go.transform.Rotate (0,0,direction - 90);
				go.transform.position = pos;

		}

		/*
		//*********************For Android
		void Shot(){
				float angle = 2f;
				float speed = 30.0f;
				float n = Mathf.Floor (gameManager.power / powerUpNum);
				//float startAngle = 90 - (n * angle);	//発射の初期位置決め
				float w = 0.2f;
				Vector3 pos = this.transform.position;
				pos.x -= (n * w) / 2;

				//奇数回数描画
				for(int i = 0; i < n + 1; i++){
						float r = 5f;
						makeShot (pos, 90f, speed);
						pos.x += w;
				}

				soundManager.playSE ("playerBullet");
		}
		void makeShot(Vector3 pos, float direction, float speed){

				pos.y += 0.3f;
				GameObject go = ObjectPool.instance.GetGameObject (prefab, pos, this.transform.rotation);
				go.transform.position = pos;
				go.transform.Rotate (0,0,direction - 90);
		}
		*/

		//Making one missile object
		void makeMissile(){
				int powerUpNumMissile = powerUpNum * 2;
				float angle = 10f;
				//最初はミサイルなし
				if (gameManager.power < powerUpNumMissile)return;

				float n = Mathf.Floor (gameManager.power / powerUpNumMissile);
				//float n = Mathf.Floor (100 / powerUpNum);
				float startAngle = 90 - (n * angle);
				soundManager.playSE ("missile");
				//make smoke
				GameObject smoke = Instantiate (smokePrefab, this.transform.position, this.transform.rotation) as GameObject;
				for(int i = 0; i < (2 * n + 1); i++){
						GameObject missile = Instantiate (missilePrefab, this.transform.position, this.transform.rotation) as GameObject;
						missile.GetComponent<Missail> ().Create (startAngle);
						startAngle += angle;
				}
		}

		//******************************************************
		//When hit enemy bullets
		public void Damage(){
				noDamageCount = NODAMAGE_TIME;	//無敵時間代入
				soundManager.playSE ("playerDamage");
				effectManager.makeEffect ("smallExplosion", this.transform.position);

				if (gameManager.life > 0) {
						//ライフを減らす
						gameManager.decLife ();
				} else {
						//ゲームオーバー
						//TODO
				}
		}

		//ダメージからの復帰
		void returnDamageMode(){
				//ヒットエリアを復帰
				GameObject.Find ("hit_area").GetComponent<HitArea> ().HitAreaTrue ();
		}

		//******************************************************
		//if bullet go out of screen, delete it
		void OnTriggerEnter2D(Collider2D c){
					switch(c.gameObject.tag){
					case "e_bullet":	//敵弾
						bullet b = c.gameObject.GetComponent<bullet> ();
						if(!b.isGraze){
							b.isGraze = true;
							gameManager.graze++;
							soundManager.playSE ("graze");
							gameManager.score += gameManager.grazeScore;
						}
						break;

					case "power_item1":	//パワーアップアイテム
							Destroy (c.gameObject);
							gameManager.power++;
							soundManager.playSE ("get_item");
						//パワーアップの幅ちょうどになったときにレベル書き換え
							if(gameManager.power % powerUpNum == 0){
								soundManager.playSE ("power_up");
								gameManager.drawLv (gameManager.power, powerUpNum);
							}
							break;

					case "score_item":	//スコアアイテム
							Destroy (c.gameObject);
							gameManager.score += 1000;
							break;
					}


			
		}
}
