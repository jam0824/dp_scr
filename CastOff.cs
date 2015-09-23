using UnityEngine;
using System.Collections;

public class CastOff : MonoBehaviour {

		public const int POWER_MAX = 5;
		const float BREAK_PER = 0.2f;
		SpriteRenderer spriteRenderer;
		public Sprite afterBreak;

		public GameObject itemPrefab;
		public GameObject subItemPrefab;

		int MaxHP = 0;
		public int HP = 0;
		public Vector3 centerPos;
		public bool breakTrigger = false;

		GameManager gameManager;
		Boss bossScript;
		SoundManager soundManager;
		EffectManager effectManager;

		// Use this for initialization
		void Start () {
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				bossScript = GameObject.FindWithTag("boss").GetComponent<Boss>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
				MaxHP = HP;
				bossScript.HP += HP;
				bossScript.MAX_HP += HP;
		}
		
		// Update is called once per frame
		void Update () {
		
		}




		//if bullet go out of screen, delete it
		void OnTriggerEnter2D(Collider2D c){
				if (!bossScript.setStartPosition)
						return;
				if((c.gameObject.tag == "p_bullet")||(c.gameObject.tag == "missile")){
						int damage = c.GetComponent<WeponStatBean> ().damage;
						HP -= damage;
						bossScript.HP -= damage;
						//定数割合以下でキャストオフ
						if ((HP <= MaxHP * BREAK_PER) && (!breakTrigger)) {
								breakTrigger = true;
								castOff ();
								castOffAnimation ();
						} else if (HP <= 0) {
								deleteEnemy ();
						}
						Destroy (c.gameObject);
						Vector3 pos = c.transform.position;
						float v = 0.5f;
						pos.x += (Random.value * v) - v / 2;
						pos.y += (Random.value * v) - v / 2;
						if (c.gameObject.tag == "p_bullet") {
								effectManager.makeEffect ("smallExplosion", pos);
						}
						else if (c.gameObject.tag == "missile") {
								effectManager.makeEffect ("fireExplosion", pos);
								soundManager.playSE ("exp_missile");
						}
				}
		}

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
						Vector3 pos = this.transform.position;
						float v = 0.5f;
						pos.x += (Random.value * v) - v / 2;
						pos.y += (Random.value * v) - v / 2;
						effectManager.makeEffect ("middleExplosion", pos);
				}

		}
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
				if (power < POWER_MAX) {

						GameObject item = Instantiate (itemPrefab, this.transform.position, this.transform.rotation) as GameObject;
				} else {
						GameObject item = Instantiate (subItemPrefab, this.transform.position, this.transform.rotation) as GameObject;
				}
		}

		void castOff(){
				SpriteRenderer currentCos = GetComponent<SpriteRenderer> ();
				currentCos.sprite = afterBreak;
				PolygonCollider2D[] p = GetComponents<PolygonCollider2D>();
				p [0].enabled = false; //ノーマルのコライダーオフ
				p [1].enabled = true;	//破れのコライダーオン
		}

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
