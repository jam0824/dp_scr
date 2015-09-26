using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButton : MonoBehaviour {


		public GameObject normalPrefab;
		public GameObject r18Prefab;
		EffectManager effectManager;
		SoundManager soundManager;

		bool isClick = false;

		// Use this for initialization
		void Start () {
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
		}
	
		// Update is called once per frame
		void Update () {
				if (Input.GetButtonUp ("Fire1") || (Input.GetKeyUp (KeyCode.Z))) {
						if((!Input.GetMouseButton (0)) && (!isClick)){
								changeNormalScene ();
						}
				}


		}

		/// <summary>
		/// Changes the normal scene.
		/// </summary>
		public void changeNormalScene(){
				changeScene (normalPrefab);
		}
		/// <summary>
		/// Changes the r18 scene.
		/// </summary>
		public void changeR18Scene(){
				changeScene (r18Prefab);
		}
		/// <summary>
		/// シーン切り替え処理
		/// </summary>
		/// <param name="prefab">Prefab.</param>
		void changeScene(GameObject prefab){
				if (!isClick) {
						isClick = true;
						soundManager.playSE ("OK");
						//位置取得にタイトルを利用しているだけ
						effectManager.changeScene ("black", prefab, GameObject.Find("TJ_logo").transform.position);
				}
		}


				
}
