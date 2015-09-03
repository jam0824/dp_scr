using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tj_logo : MonoBehaviour {

		public GameObject fadePrefab;
		public GameObject titlePrefab;
		GameObject fadein;

		int timer = 0;
		public bool isClick = false;

		// Use this for initialization
		void Start () {
				makeWhiteOut (0);
		}
	
		// Update is called once per frame
		void Update () {
				if (Input.GetButton ("Fire1") || (Input.GetKey (KeyCode.Z)) || (Input.GetMouseButton (0))) {
						if (!isClick) {
								timer = makeWhiteOut (1);
								isClick = true;
						}
				}

				if(timer > 0){
						timer--;
						if (timer == 0) {
								makeTitle ();
						}
				}
		}

		//*******************************************
		//ホワイトアウト
		int makeWhiteOut(int type){
				int t = 60;
				fadein = Instantiate (fadePrefab, this.transform.position, this.transform.rotation) as GameObject;
				fadein.GetComponent<fade> ().Init (type, t, 255.0f, 255.0f, 255.0f);

				RectTransform rt = fadein.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(0, 0);	//位置変更
				rt.transform.parent = this.transform;	//親にする
				return t;
		}

		void makeTitle(){
				Destroy (fadein);
				GameObject title = Instantiate (titlePrefab, this.transform.position, this.transform.rotation) as GameObject;
				title.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				Destroy (this.gameObject);
		}
}
