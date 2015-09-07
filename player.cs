using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

		const int NODAMAGE_TIME = 50;	//やられた時の無敵時間

		public GameObject prefab;
		public GameObject explosion01;
		public GameObject bombPrefab;

		int speed = 150;
		float moveOffset = 0.04f;
		float shotDelay = 0.05f;
		Vector3 oldPos;

		int gameCount = 0;
		int bulletWaitTime = 5;	//弾を打つまでの待ちフレーム
		int noDamageCount = 0;	//無敵時間カウント

		GameManager gameManager;
		SoundManager soundManager;

		//******************************************************
		// Use this for initialization
		void Start () {
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		}

		//******************************************************
		// Update is called once per frame
		void Update () {
				movePlayerToTouch ();	//マウスで移動
				keyBoardMove ();		//padなどで移動

				//定期フレームごとに弾発射
				if(gameCount % bulletWaitTime == 0){
						keyCheck ();	//キーチェック
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
				if (Input.GetButton ("Fire1") || (Input.GetKey (KeyCode.Z))) {
						Shot ();
				}
				//セカンドボタンでボム
				if (Input.GetButton ("Fire2") || (Input.GetKey (KeyCode.X)) || (Input.GetMouseButton (1))) {
						makeBomb ();
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
				if (GetComponent<SpriteRenderer> ().enabled) {
						GetComponent<SpriteRenderer> ().enabled = false;
				} else {
						GetComponent<SpriteRenderer> ().enabled = true;
				}
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
				// 右・左
				float x = Input.GetAxis("Horizontal");
				// 上・下
				float y = Input.GetAxis ("Vertical");

				Vector3 myPos = transform.position;
				myPos.x += x * speed;
				myPos.y += y * speed;

				Vector3 pos = Camera.main.WorldToViewportPoint (myPos);

				if ((pos.x < 0.0f) || (pos.x > 1.0f) || (pos.y < 0.0f) || (pos.y > 1.0f)) {
						transform.position = oldPos;	//前回の位置に戻しておく（０以下で動かなくなってしまうため）
				} else {
						// 移動する向きとスピードを代入する
						transform.position = myPos;
						oldPos = transform.position;	//位置を保持しておく
						setAnimeTrigger (x, y);
				}


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
					Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
						//画面外だったら
						if ((pos.x < 0.0f) || (pos.x > 1.0f) || (pos.y < 0.0f) || (pos.y > 1.0f)) {
											transform.position = oldPos;	//前回の位置に戻しておく（０以下で動かなくなってしまうため）
						} else {
								this.GetComponent<Rigidbody2D> ().AddForce (nVec * speed);
								oldPos = transform.position;	//位置を保持しておく
										setAnimeTrigger (nVec.x, nVec.y);
						}
					
				}
				
			}
			return;
		}

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
		void makeBomb(){
				if ((gameManager.bombFlag) || (gameManager.bomb == 0)) {
						return;
				}
				gameManager.decBomb ();
				gameManager.bombFlag = true;
				GameObject gs = Instantiate (bombPrefab, new Vector3(0,0,0), this.transform.rotation) as GameObject;
		}

		//******************************************************
		void Shot(){
				makeShot (90.0f + Random.value * 5 - 2.5f , 15.0f);
				soundManager.playSE ("playerBullet");
		}
				
		//Making one bullet object
		void makeShot(float direction, float speed){
				Vector3 pos = this.transform.position;
				pos.y += 1.0f;
				GameObject shot = Instantiate (prefab, pos, this.transform.rotation) as GameObject;
			player_wepon01 s = shot.GetComponent<player_wepon01>();
			s.Create(direction, speed);
		}

		//******************************************************
		//When hit enemy bullets
		public void Damage(){
				noDamageCount = NODAMAGE_TIME;	//無敵時間代入
				soundManager.playSE ("playerDamage");
				GameObject e = Instantiate (explosion01, this.transform.position, this.transform.rotation) as GameObject;

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
						gameManager.graze++;
						soundManager.playSE ("graze");
						gameManager.score += gameManager.grazeScore;
						break;

					case "power_item1":	//パワーアップアイテム
							Destroy (c.gameObject);
							gameManager.power++;
							break;

					case "score_item":	//スコアアイテム
							Destroy (c.gameObject);
							gameManager.score += 1000;
							break;
					}


			
		}
}
