using UnityEngine;
using System.Collections;

public class player_wepon01 : MonoBehaviour {
		public int damage;
		float speed = 30f;
	
		void Start(){
				damage = GetComponent<WeponStatBean> ().damage;
		}

		//有効化した時に呼ばれる
		void OnEnable ()
		{
				// 弾の移動
				GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
		}
		//画面外に出た時
		void OnBecameInvisible(){
				ObjectPool.instance.ReleaseGameObject (gameObject);
		}

		//衝突判定
		void OnTriggerEnter2D(Collider2D c){
				switch(c.gameObject.tag){
				case "close":
						CastOff castOff = c.gameObject.GetComponent<CastOff> ();
						castOff.hitBullet (damage);
						castOff.makeEffect (transform.tag, transform.position);
						deleteBullet ();
						break;
				case "enemy":
						enemy enemyScript = c.gameObject.GetComponent<enemy> ();
						enemyScript.hitBullet (damage);
						enemyScript.makeEffect(transform.tag, transform.position);
						deleteBullet ();
						break;
				case "middleBoss":
						MiddleBoss middleBoss = c.gameObject.GetComponent<MiddleBoss> ();
						middleBoss.hitBullet (damage);
						middleBoss.makeEffect(transform.tag, transform.position);
						deleteBullet ();
						break;
				}

		}

		void deleteBullet(){
				ObjectPool.instance.ReleaseGameObject (gameObject);
		}

}
