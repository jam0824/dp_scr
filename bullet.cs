using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
		public GameObject prefab;
		public bool isGraze = false;

		GameManager gameManager;
		float bullet_direction;

		int gameCount = 0;
		int deleteFrame = 420;

	// 移動速度を設定
	// @param direction 角度
	// @param speed 速さ
	// @param rotationTrigger(bool)  rotate image
		void Start() {
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

		void Update(){
				//もしボムが発動中だったら削除
				if ((gameManager.bombFlag == true) ||(gameCount > deleteFrame)){
						delete ();
				}

				gameCount++;
		}
		

		void delete(){
				GameObject explosion = Instantiate (prefab, this.transform.position, this.transform.rotation) as GameObject;
				Destroy(this.gameObject);
		}
		//画面外にでたとき
		void OnBecameInvisible(){
				Destroy (this.gameObject);
		}
}
