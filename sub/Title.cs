using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Title : MonoBehaviour {

		public GameObject fadePrefab;

		// Use this for initialization
		void Start () {
				makeWhiteIn (0);
		}
	
		// Update is called once per frame
		void Update () {
				if (Input.GetButton ("Fire1") || (Input.GetKey (KeyCode.Z)) || (Input.GetMouseButton (0))) {
						changeScene ();
				}
		}

		//*******************************************
		//ホワイトイン
		void makeWhiteIn(int type){
				GameObject fadein = Instantiate (fadePrefab, this.transform.position, this.transform.rotation) as GameObject;
				fadein.GetComponent<fade> ().Init (type, 180, 255.0f, 255.0f, 255.0f);

				RectTransform rt = fadein.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(0, 0);	//位置変更
		}


		void changeScene(){
				makeWhiteIn (1);
				Application.LoadLevel ("demon_princess");
		}
}
