using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButton : MonoBehaviour {

		public GameObject fadePrefab;
		SoundManager soundManager;

		public string loadLevel = "demon_princess";


		// Use this for initialization
		void Start () {
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		}
	
		// Update is called once per frame
		void Update () {

		}

		public void changeScene(){
				BGMplay bgm = GameObject.Find ("BGMplay(Clone)").GetComponent<BGMplay> ();
				soundManager.fadeOutBGM (bgm, 0.04f, 0.1f);
				GameObject fadein = new Common ().makeFade (fadePrefab, this.gameObject, 1, 60, 0.0f, 0.0f, 0.0f);

				Invoke ("changeLevel", 1f);
		}

		void changeLevel(){
				Application.LoadLevel (loadLevel);
		}
				
}
