using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	private float bullet_direction;
	// 移動速度を設定
	// @param direction 角度
	// @param speed 速さ
	// @param rotationTrigger(bool)  rotate image
	public void Create(Vector2 v, float direction, float speed, bool rotationTrigger) {
		bullet_direction = direction;
		//Rotate image
		if(rotationTrigger) this.transform.Rotate (0,0,direction - 90);

		rigidbody2D.velocity = v.normalized * speed;
	}

	void Update(){

	}
	
	//if bullet go out of screen, delete it
	void OnTriggerEnter2D(Collider2D c){
		int layer = LayerMask.NameToLayer ("delete_area");
		if(c.gameObject.layer == layer){
			Destroy(gameObject);
		}
	}
}
