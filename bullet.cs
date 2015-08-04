using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
		public GameManager gameManager;
		public GameObject prefab;
		private float bullet_direction;


	// 移動速度を設定
	// @param direction 角度
	// @param speed 速さ
	// @param rotationTrigger(bool)  rotate image
		void Start() {
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

		void Update(){
				//もしボムが発動中だったら削除
				if (gameManager.bombFlag == true) {
						delete ();
				}
		}
		
		//if bullet go out of screen, delete it
		void OnTriggerEnter2D(Collider2D c){
			int layer = LayerMask.NameToLayer ("delete_area");
			if(c.gameObject.layer == layer){
				Destroy(gameObject);
			}
		}

		void delete(){
				GameObject explosion = Instantiate (prefab, this.transform.position, this.transform.rotation) as GameObject;
				Destroy(gameObject);
		}
}
