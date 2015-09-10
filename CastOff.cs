using UnityEngine;
using System.Collections;

public class CastOff : MonoBehaviour {

		public const int POWER_MAX = 5;
		const float BREAK_PER = 0.2f;
		SpriteRenderer spriteRenderer;
		public Sprite afterBreak;

		public GameObject explosionBig;  //Big explosion
		public GameObject explosionSmall;  //very small explosion
		public GameObject itemPrefab;
		public GameObject subItemPrefab;

		int MaxHP = 0;
		public int HP = 0;
		public Vector3 centerPos;
		public bool breakTrigger = false;

		GameManager gameManager;
		Boss bossScript;
		SoundManager soundManager;

		// Use this for initialization
		void Start () {
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				bossScript = GameObject.FindWithTag("boss").GetComponent<Boss>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				MaxHP = HP;
				bossScript.HP += HP;
				bossScript.MAX_HP += HP;
		}
		
		// Update is called once per frame
		void Update () {
		
		}




		//if bullet go out of screen, delete it
		void OnTriggerEnter2D(Collider2D c){
				if (c.gameObject.tag == "p_bullet") {
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
						Destroy (c.gameObject);
						GameObject e = Instantiate (explosionSmall, c.transform.position, c.transform.rotation) as GameObject;
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
				gameManager.flash ();	//フラッシュ作成
				soundManager.playSE ("castOff");	//効果音
				float w = 2.0f;	//エフェクト発生の幅
				for(int i = 0; i < 20; i++){
						Vector3 pos = transform.position;
						pos.x += Random.value * w - 0.5f * w + centerPos.x;
						pos.y += Random.value - 0.5f + centerPos.y;
						GameObject e = Instantiate (explosionBig, pos, this.transform.rotation) as GameObject;
				}

		}

		void lastBreakAnimation(){
				gameManager.flash ();	//フラッシュ作成
				float w = 2.0f;	//エフェクト発生の幅
				for(int i = 0; i < 30; i++){
						Vector3 pos = transform.position;
						pos.x += Random.value * w - 0.5f * w + centerPos.x;
						pos.y += Random.value * w - 0.5f * w + centerPos.y;
						GameObject e = Instantiate (explosionBig, pos, this.transform.rotation) as GameObject;
				}
		}

}
