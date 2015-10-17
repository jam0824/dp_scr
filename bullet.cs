using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
		public GameObject prefab;
		public bool isGraze = false;
		public int gameCount = 0;

		float bullet_direction;


		int deleteFrame = 420;

	// 移動速度を設定
	// @param direction 角度
	// @param speed 速さ
	// @param rotationTrigger(bool)  rotate image
		void Start() {
				//bulletPool = GameObject.Find("BulletPool");
				//transform.parent = bulletPool.transform;
		}
		void OnEnable ()
		{
				gameCount = 0;
		}


		void Update(){
				//もしボムが発動中だったら削除
				if (gameCount > deleteFrame){
						delete ();
				}

				gameCount++;
		}
		

		public void delete(){
				GameObject explosion = Instantiate (prefab, this.transform.position, this.transform.rotation) as GameObject;
				Destroy(this.gameObject);
				//deleteExec ();
		}
		//画面外にでたとき
		void OnBecameInvisible(){
				Destroy (this.gameObject);
				//deleteExec ();
		}
		public void deleteExec(){
				//gameCount = 0;
				//Destroy(this.GetComponent("BulletScript"));
				//BulletPool.instance.ReleaseGameObject (gameObject);
				Destroy (gameObject);
		}
}
