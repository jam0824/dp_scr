using UnityEngine;
using System.Collections;

public class HitArea : MonoBehaviour {

		const int HIT_WAIT = 10;
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
						if (Input.GetButton ("Fire2") || (Input.GetKey (KeyCode.X)) || (Input.GetMouseButton (2))) {
								p.makeBomb ();
								count = 0;
						}
				}
		}

		void OnTriggerEnter2D(Collider2D c){
		
				switch(c.gameObject.tag){
				case "e_bullet":	//敵弾
						Destroy (c.gameObject);
						GameObject e = Instantiate (hitPrefab, this.transform.position, this.transform.rotation) as GameObject;
						count = HIT_WAIT;
						break;

				case "enemy":	//敵
						GameObject hit = Instantiate (hitPrefab, this.transform.position, this.transform.rotation) as GameObject;
						count = HIT_WAIT;
						break;

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
