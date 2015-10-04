using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StaffRoleScroll : MonoBehaviour {

		public GameObject fadePrefab;
		public string loadLevel = "title";

		float v = 0.5f;
		//int sizeStaff = 1300;
		int stopPos = 2250;
		RectTransform rect;

		bool mouseFlag = true;
		SoundManager soundManager;
		BGMplay bgm;

		// Use this for initialization
		void Start () {
				openR18 ();
				rect = this.GetComponent<RectTransform> ();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				bgm = soundManager.playBGM ("ending");
		}
	
		// Update is called once per frame
		void Update () {
				//スクロール処理
				if (this.GetComponent<RectTransform> ().anchoredPosition.y < stopPos) {
						scroll ();
				} else {
						//マウスがクリックされていないときは遷移
						if (mouseFlag) {
								mouseFlag = false;
								preChange ();
						}
				}
				//キー入力でスキップ
				if (Input.GetButton ("Fire1") || (Input.GetMouseButton (0))) {
						if (mouseFlag) {
								mouseFlag = false;
								preChange ();
						}
				}

		}

		//スクロール
		void scroll(){
				Vector3 pos = this.GetComponent<RectTransform> ().anchoredPosition;
				pos.y += v;
				this.GetComponent<RectTransform> ().anchoredPosition = pos;
				//rect.position = pos;
		}

		//タイトルに戻る
		void preChange(){
				soundManager.fadeOutBGM (bgm, 0.04f, 0.1f);
				GameObject fadein = new Common ().makeFade (fadePrefab, this.gameObject, 1, 60, 255.0f, 255.0f, 255.0f);
				Invoke ("changeLevel",1f);
		}

		void changeLevel(){
				Application.LoadLevel (loadLevel);
		}

		//R18モード解禁
		void openR18(){
				PlayerPrefs.SetString ("R18Mode", "iris");
		}
}
