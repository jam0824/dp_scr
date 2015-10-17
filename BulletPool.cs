using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour {
		public int bulletNum = 0;
		public float fps = 0.0f;
		GameManager gameManager;

		void Start(){
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		}
	
		// Update is called once per frame
		void Update () {
				bulletNum = this.transform.childCount;
				fps = 1 / Time.deltaTime;
				if (gameManager.bombFlag == true)
						checkBomb ();
				//処理落ちの時
				/*
				if (fps < 10) {
						deleteRandomBullet (bulletNum);
				}
				*/
		}
				
		//ボム時に子を消す
		void checkBomb(){
				foreach (Transform child in transform)
				{

					child.GetComponent<bullet> ().delete ();

				}
		}

		void deleteRandomBullet(int bulletNum){
				if (bulletNum == 0)
						return;
				Transform child = transform.GetChild (bulletNum - 1);
				child.GetComponent<bullet> ().deleteExec ();
		}

}
