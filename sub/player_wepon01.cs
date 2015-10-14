using UnityEngine;
using System.Collections;

public class player_wepon01 : MonoBehaviour {
		public int damage;
		float speed = 30f;
	
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
}
