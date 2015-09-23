﻿using UnityEngine;
using System.Collections;

public class Missail : MonoBehaviour {

		int pattern = 0;
		float speed = 8f;
		float maxAngle = 15f;
		float oldDirection = 45f;
		int gameCount = 0;
		int timing = 1;
		Common common;

		// Use this for initialization
		void Start () {
				common = new Common ();

		}

		//初期設定
		public void Create(float direction){
				decidePattern ();
				SetVelocityForRigidbody2D (direction, speed);
				this.transform.rotation = Quaternion.Euler (0, 0, direction - 90f);
				oldDirection = direction;
		}
	
		//パターンを決める。0:劣等生、１：普通、２：優等生
		void decidePattern(){
				int r = Random.Range (0, 3);
				switch (r) {
				case 0:
						timing = 30;
						break;
				case 1:
						timing = 15;
						speed = 6f;
						break;
				case 2:
						timing = 2;
						speed = 5f;
						break;

				}
		}
		// Update is called once per frame
		void Update () {
				if (gameCount > 5) {
						//角度調整はtimingおきに行う
						if (gameCount % timing == 0) {
								turnDirection ();
						}
				}
				gameCount++;
		}

		//方向を変える
		void turnDirection(){
				Vector2 pos = checkEnemyPos ();

				float direction = common.getAim (this.transform.position, pos);
				//角度調整
				if(direction - oldDirection > maxAngle){
						direction = oldDirection + maxAngle;
				}
				else if(direction - oldDirection < maxAngle){
						direction = oldDirection - maxAngle;
				}
				this.transform.rotation = Quaternion.Euler (0, 0, direction - 90f);
				SetVelocityForRigidbody2D (direction, speed);
				oldDirection = direction;
		}

		Vector2 checkEnemyPos(){
				Vector2 pos;
				GameObject enemy = GameObject.FindGameObjectWithTag ("enemy");
				if (enemy != null) {
						pos = enemy.transform.position;
				} else {
						pos = new Vector2 (0, 5);
				}
				return pos;
		}

		// 速度を設定
		// @param 角度
		// @param 速さ
		public void SetVelocityForRigidbody2D(float direction, float speed) {
				// Setting velocity.
				Vector2 v;
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
