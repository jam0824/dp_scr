using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButton : MonoBehaviour {

		public GameObject fadePrefab;

		public string loadLevel = "demon_princess";

		// Use this for initialization
		void Start () {
	
		}
	
		// Update is called once per frame
		void Update () {

		}

		public void changeScene(){
				makeWhiteIn (1);
				Debug.Log ("押された");
				Application.LoadLevel (loadLevel);
		}
		//*******************************************
		//ホワイトイン
		void makeWhiteIn(int type){
				int t = 60;
				GameObject fadein = Instantiate (fadePrefab, this.transform.position, this.transform.rotation) as GameObject;
				fadein.GetComponent<fade> ().Init (type, 60, 255.0f, 255.0f, 255.0f);

				RectTransform rt = fadein.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(0, 0);	//位置変更
		}
				
}
