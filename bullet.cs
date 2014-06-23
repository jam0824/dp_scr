using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	// 移動速度を設定
	// @param direction 角度
	// @param speed 速さ
	// @param rotationTrigger(bool)  rotate image
	public void Create(float direction, float speed, bool rotationTrigger) {
		Vector2 v;
		//Rotate image
		if(rotationTrigger) this.transform.Rotate (0,0,direction - 90);
		
		v.x = Mathf.Cos (Mathf.Deg2Rad * direction) * speed;
		v.y = Mathf.Sin (Mathf.Deg2Rad * direction) * speed;
		rigidbody2D.velocity = v;
	}
	
	//if bullet go out of screen, delete it
	void OnTriggerEnter2D(Collider2D c){
		int layer = LayerMask.NameToLayer ("delete_area");
		if(c.gameObject.layer == layer){
			Destroy(gameObject);
		}
	}
}
