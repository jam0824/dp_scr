using UnityEngine;
using System.Collections;

public class HitArea : MonoBehaviour {

		const int HIT_WAIT = 20;
		public player p;
		public GameObject hitPrefab;

		int count = 0;

		// Use this for initialization
		void Start () {
					p = GameObject.Find ("player").GetComponent<player>();
		}
		
		// Update is called once per frame
		void Update () {
				//遊びを作る
				if(count > 0){
						count--;
						if (count == 0) {
								playerDamage ();
						}
						//喰らいボム
						if (Input.GetButtonDown ("Fire2")) {
								bool b = p.makeBomb ();
								if(b) count = 0;//ボムが発動した時はリセット
						}
				}
		}

		//敵弾,的に当たった時
		void OnTriggerEnter2D(Collider2D c){
				if ((c.gameObject.tag == "e_bullet") || (c.gameObject.tag == "enemy")) {
						if (c.gameObject.tag == "e_bullet") {
								c.gameObject.GetComponent<bullet> ().delete ();

						}
						GameObject e = Instantiate (hitPrefab, this.transform.position, this.transform.rotation) as GameObject;
						count = HIT_WAIT;
				}	
		}

		//hit実行
		void playerDamage(){
				p.Damage ();
				GetComponent<CircleCollider2D>().enabled = false;
		}

		//コライダーおん
		public void HitAreaTrue(){
				GetComponent<CircleCollider2D> ().enabled = true;
		}

}
