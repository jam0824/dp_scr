using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StaffRoleScroll : MonoBehaviour {

		public GameObject fadePrefab;
		public string loadLevel = "title";

		float v = 0.5f;
		int sizeStaff = 1300;
		RectTransform rect;

		bool mouseFlag = true;

		// Use this for initialization
		void Start () {
				rect = this.GetComponent<RectTransform> ();
		}
	
		// Update is called once per frame
		void Update () {
				if (rect.position.y < (sizeStaff + 240)) {
						scroll ();
				} else {
						//マウスがクリックされていないときは遷移
						if (mouseFlag) {
								mouseFlag = false;
								preChange ();
						}
				}
				//キー入力でスキップ
				if (Input.GetButton ("Fire1") || (Input.GetKey (KeyCode.Z)) || (Input.GetMouseButton (0))) {
						if (mouseFlag) {
								mouseFlag = false;
								preChange ();
						}
				}

		}

		void scroll(){
				Vector3 pos = rect.position;
				pos.y += v;
				//this.GetComponent<RectTransform> ().position = pos;
				rect.position = pos;
		}

		void preChange(){
				GameObject fadein = new Common ().makeFade (fadePrefab, this.gameObject, 1, 60, 255.0f, 255.0f, 255.0f);
				Invoke ("changeLevel",1f);
		}

		void changeLevel(){
				Application.LoadLevel (loadLevel);
		}
}
