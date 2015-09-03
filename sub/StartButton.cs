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
				GameObject fadein = new Common ().makeFade (fadePrefab, this.gameObject, 1, 60, 0.0f, 0.0f, 0.0f);
				Invoke ("changeLevel", 1f);
		}

		void changeLevel(){
				Application.LoadLevel (loadLevel);
		}
				
}
