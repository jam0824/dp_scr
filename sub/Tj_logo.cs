using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tj_logo : MonoBehaviour {

		public GameObject fadePrefab;
		public GameObject titlePrefab;
		GameObject fadein;
		SoundManager soundManager;

		int gameCount = 0;
		int viewTitleTime = 60 * 4;
		public bool isClick = false;

		// Use this for initialization
		void Start () {
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				BGMplay bgm = soundManager.playBGM ("opening");
				fadein = new Common ().makeFade (fadePrefab, this.gameObject, 0, 60, 255.0f, 255.0f, 255.0f);
		}
	
		// Update is called once per frame
		void Update () {
				if (Input.GetButton ("Fire1") || (Input.GetKey (KeyCode.Z)) || (Input.GetMouseButton (0))
						|| (gameCount > viewTitleTime)) {
						if (!isClick) {
								fadein = new Common ().makeFade (fadePrefab, this.gameObject, 1, 60, 255.0f, 255.0f, 255.0f);
								Invoke ("makeTitle", 1f);
								isClick = true;
						}
				}

				if (!isClick)
						gameCount++;

		}
				
		void makeTitle(){
				Destroy (fadein);
				GameObject title = Instantiate (titlePrefab, this.transform.position, this.transform.rotation) as GameObject;
				title.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				title.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
				fadein = new Common ().makeFade (fadePrefab, this.gameObject, 0, 60, 255.0f, 255.0f, 255.0f);
				Destroy (this.gameObject);
		}
}
