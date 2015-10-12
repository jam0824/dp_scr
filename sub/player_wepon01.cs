﻿using UnityEngine;
using System.Collections;

public class player_wepon01 : MonoBehaviour {
		public int damage;

	// 移動速度を設定
	// @param direction 角度
	// @param speed 速さ
	public void Create(float direction, float speed) {
		Vector2 v;
		//Rotate image
				//this.transform.RotateAround(new Vector3 (0, 0, 0), transform.up, direction - 90);
				this.transform.Rotate (0,0,direction - 90);

				v.x = Mathf.Cos (Mathf.Deg2Rad * direction) * speed;
				v.y = Mathf.Sin (Mathf.Deg2Rad * direction) * speed;
				GetComponent<Rigidbody2D>().velocity = v;
	}
				

		void OnBecameInvisible(){
				Destroy (this.gameObject);
		}
}
