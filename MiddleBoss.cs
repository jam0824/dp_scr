﻿using UnityEngine;
using System.Collections;

public class MiddleBoss : MonoBehaviour {

		const float START_POSISION_Y = 1.0f;

		public int MAX_HP;
		int HP;
		public int gameFrame = 0;
		public GameObject lifebarPrefab;
		public GameObject lifebarWakuPrefab;
		public GameObject bulletPrefab;
		public GameObject itemPrefab;

		GameObject prefabWaku;

		GameManager gameManager;
		SoundManager soundManager;
		EffectManager effectManager;
		RectTransform lifebar;
		bool setStartPosition = false;

		// Use this for initialization
		void Start () {
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
				HP = MAX_HP;
				makeLifeBar ();
				//コールチンスタート
				StartCoroutine (startingMoving (-0.01f));
		}
	
		// Update is called once per frame
		void Update () {

		}

		//******************************************************************************
		//玉を作る
		private void makeBullet(){
				GameObject gs = Instantiate (bulletPrefab, this.transform.position, this.transform.rotation) as GameObject;
				gs.transform.parent = this.transform;	//親にする
		}

		//**********************************************************
		//UI系
		void makeLifeBar(){
				int x = 0;
				int y = 0;
				prefabWaku = Instantiate (lifebarWakuPrefab, this.transform.position, this.transform.rotation) as GameObject;
				prefabWaku.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				RectTransform waku = prefabWaku.GetComponent<RectTransform>();
				waku.localScale = new Vector3 (2, 2, 1);	//スケールを元に戻す
				waku.anchoredPosition = new Vector2(x, y);	//位置変更

				GameObject prefab = Instantiate (lifebarPrefab, this.transform.position, this.transform.rotation) as GameObject;
				prefab.transform.parent = prefabWaku.transform;	//枠を親にする
				lifebar = prefab.GetComponent<RectTransform>();
				lifebar.localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
				lifebar.anchoredPosition = new Vector2(0, 0);	//位置変更

				StartCoroutine (new Common().moveUI(waku, -1f, -40f, 0.01f));	//コールチンでバー移動
		}

		//バーを再描画
		void drawLifeBar(){
				int w = 400;
				int h = 23;
				lifebar.sizeDelta = new Vector2 ((int)Mathf.Floor(w * HP / MAX_HP), h);
		}

		//if bullet go out of screen, delete it
		void OnTriggerEnter2D(Collider2D c){
				//ポジションにいないときはダメージ処理なし
				if (!setStartPosition) {
						return;
				}
				//ダメージ処理
				//TODO : bombが機能していない 
				if((c.gameObject.tag == "p_bullet")||(c.gameObject.tag == "missile") || (c.gameObject.tag == "bomb")){
						drawLifeBar ();
						if(c.gameObject.tag == "bomb"){
								HP--;
						}else{
								int damage = c.GetComponent<WeponStatBean> ().damage;
								HP -= damage;
								Destroy (c.gameObject);
						}

						if(HP <= 0) deleteEnemy();

						Vector3 pos = c.transform.position;
						float v = 0.5f;
						pos.x += (Random.value * v) - v / 2;
						pos.y += (Random.value * v) - v / 2;

						if(c.gameObject.tag == "p_bullet"){
								effectManager.makeEffect ("middleExplosion", pos);

						}
						else if(c.gameObject.tag == "missile"){
								effectManager.makeEffect ("fireExplosion", pos);
								soundManager.playSE ("exp_missile");
						}
				}

				//デリートエリア到着で削除
				if(c.gameObject.tag == "delete_area"){
						Destroy(gameObject);
				}
		}
				
		/// <summary>
		/// Deletes the enemy.
		/// </summary>
		void deleteEnemy(){
				makeItem (gameManager.power);	//アイテム作成
				moveStatus ();	//ステータスの移動

				soundManager.playSE ("exp_big");
				effectManager.makeEffect ("bigExplosion", this.transform.position);

				Destroy(prefabWaku);	//ライフバー削除
				Destroy(gameObject);

		}
		/// <summary>
		/// ステータスを移動させる
		/// </summary>
		void moveStatus(){
				RectTransform rt = GameObject.Find ("statusView").GetComponent<RectTransform> ();
				Vector2 pos = rt.anchoredPosition;
				pos.y = 0;
				rt.anchoredPosition = pos;
		}

		//アイテム作成
		private void makeItem(int power){
			GameObject item = Instantiate (itemPrefab, this.transform.position, this.transform.rotation) as GameObject;
		}

		//コールチンで初回移動
		public IEnumerator startingMoving(float v) {
				//無限ループ防止
				for (int i = 0; i < 10000; i++) {
						transform.Translate (0, v, 0);
						//規定位置についたら終了
						if(transform.position.y <= START_POSISION_Y){
								setStartPosition = true;
								makeBullet ();	//弾幕開始
								yield break;
						}
						yield return new WaitForSeconds (0.016f);

				}
				yield break;
		}


}
