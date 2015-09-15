using UnityEngine;
using System.Collections;

public class player_wepon01 : MonoBehaviour {
		public int damage;

	// 移動速度を設定
	// @param direction 角度
	// @param speed 速さ
	public void Create(float direction, float speed) {
		Vector2 v;
		//Rotate image
		this.transform.Rotate (0,0,direction - 90);

		v.x = Mathf.Cos (Mathf.Deg2Rad * direction) * speed;
		v.y = Mathf.Sin (Mathf.Deg2Rad * direction) * speed;
		GetComponent<Rigidbody2D>().velocity = v;
	}

	//if bullet go out of screen, delete it
	void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.tag == "delete_area"){
			Destroy(gameObject);
		}
	}
}
