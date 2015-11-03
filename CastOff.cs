using UnityEngine;
using System.Collections;

public class CastOff : MonoBehaviour {

		const float BREAK_PER = 0.5f;
		SpriteRenderer spriteRenderer;
		public Sprite afterBreak;

		public GameObject itemPrefab;

		int MaxHP = 0;
		public int HP = 0;
		public Vector3 centerPos;
		public bool breakTrigger = false;

		GameManager gameManager;
		Boss bossScript;
		SoundManager soundManager;
		EffectManager effectManager;
		Common common;

		void Awake(){
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
				common = GameObject.Find("Common").GetComponent<Common>();
		}

		// Use this for initialization
		void Start () {
				bossScript = GameObject.FindWithTag("boss").GetComponent<Boss>();
				MaxHP = HP;
				bossScript.HP += HP;
				bossScript.MAX_HP += HP;
		}
		
		// Update is called once per frame
		void Update () {

		
		}


		//bulletヒット時
		public void hitBullet(int damage){
				if (!bossScript.setStartPosition)
						return;
				HP -= damage;
				bossScript.HP -= damage;
				checkHP ();
		}
		void checkHP(){
				//定数割合以下でキャストオフ
				if ((HP <= MaxHP * BREAK_PER) && (!breakTrigger)) {
						breakTrigger = true;
						castOff ();
						castOffAnimation ();
				} else if (HP <= 0) {
						//服を消す
						deleteEnemy ();
				}
		}
		//エフェクトはキャスト側で出す。
		public void makeEffect(string tag, Vector3 bulletPos){
				int r = Random.Range (0,8);
				if ((tag == "p_bullet") && (r == 0)) {
						Vector3 pos = common.randomPos (bulletPos, 0.5f);
						effectManager.makeEffect ("smallExplosion", pos);
				}
				else if (tag == "missile") {
						Vector3 pos = common.randomPos (bulletPos, 0.5f);
						effectManager.makeEffect ("fireExplosion", pos);
						soundManager.playSE ("exp_missile");
				}
		}
		//if bullet go out of screen, delete it
		/*
		void OnTriggerEnter2D(Collider2D c){
				if (!bossScript.setStartPosition)
						return;

				if((c.gameObject.tag == "p_bullet")||(c.gameObject.tag == "missile")){
						int damage = c.GetComponent<WeponStatBean> ().damage;
						HP -= damage;
						bossScript.HP -= damage;


						if (c.gameObject.tag == "p_bullet") {
								ObjectPool.instance.ReleaseGameObject (c.gameObject);
						} else {
								Destroy (c.gameObject);
						}
						//処理落ちするため、たまにしかダメージエフェクトを出さない
						int r = Random.Range (0,8);
						if ((c.gameObject.tag == "p_bullet") && (r == 0)) {
								Vector3 pos = common.randomPos (c.transform.position, 0.5f);
								effectManager.makeEffect ("smallExplosion", pos);
						}
						else if (c.gameObject.tag == "missile") {
								Vector3 pos = common.randomPos (c.transform.position, 0.5f);
								effectManager.makeEffect ("fireExplosion", pos);
								soundManager.playSE ("exp_missile");
						}
				}
		}
		*/
		//TODO : bom判定なし。
		/*
		void OnTriggerStay2D (Collider2D c){
				if(c.gameObject.tag == "bomb"){
						HP--;
						bossScript.HP--;
						//定数割合以下でキャストオフ
						if ((HP <= MaxHP * BREAK_PER) && (!breakTrigger)) {
								breakTrigger = true;
								castOff ();
								castOffAnimation ();
						} else if (HP <= 0) {
								deleteEnemy ();
						}
						Vector3 pos = common.randomPos (this.transform.position, 0.5f);
						effectManager.makeEffect ("middleExplosion", pos);
				}

		}
		*/
		/// <summary>
		/// Deletes the enemy.
		/// </summary>
		void deleteEnemy(){
				makeItem (gameManager.power);
				lastBreakAnimation ();
				Destroy(gameObject);
		}

		//アイテム作成
		private void makeItem(int power){
				GameObject item = Instantiate (itemPrefab, this.transform.position, this.transform.rotation) as GameObject;
		}

		//衣装ブレイク
		void castOff(){
				SpriteRenderer currentCos = GetComponent<SpriteRenderer> ();
				currentCos.sprite = afterBreak;
				//PolygonCollider2D[] p = GetComponents<PolygonCollider2D>();
				//p [0].enabled = false; //ノーマルのコライダーオフ
				//p [1].enabled = true;	//破れのコライダーオン
		}

		//衣装ブレイク時のエフェクト作成
		void castOffAnimation(){
				effectManager.flash ();	//フラッシュ作成
				soundManager.playSE ("castOff");	//効果音
				float w = 2.0f;	//エフェクト発生の幅
				for(int i = 0; i < 20; i++){
						Vector3 pos = transform.position;
						pos.x += Random.value * w - 0.5f * w + centerPos.x;
						pos.y += Random.value - 0.5f + centerPos.y;
						effectManager.makeEffect ("middleExplosion", pos);
				}

		}

		void lastBreakAnimation(){
				effectManager.flash ();	//フラッシュ作成
				float w = 2.0f;	//エフェクト発生の幅
				for(int i = 0; i < 30; i++){
						Vector3 pos = transform.position;
						pos.x += Random.value * w - 0.5f * w + centerPos.x;
						pos.y += Random.value * w - 0.5f * w + centerPos.y;
						effectManager.makeEffect ("middleExplosion", pos);

				}
		}

}
