﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartNormalGame : MonoBehaviour {

		public GameObject fadePrefab;
		public Sprite howToPlaySpImage;

		SoundManager soundManager;
		EffectManager effectManager;

		public string loadLevel = "demon_princess";
		bool isClick = false;

		// Use this for initialization
		void Start () {
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				//SPだった時は操作方法表示を変える
				if (Application.platform == RuntimePlatform.Android) {
						Image screen = GetComponent<Image> ();
						screen.sprite = howToPlaySpImage;
				}
		}

		// Update is called once per frame
		void Update () {
				if (Input.GetButton ("Fire1") || (Input.GetKey (KeyCode.Z)) || (Input.GetMouseButton (0))) {
						if (!isClick) {
								soundManager.playSE ("OK");
								BGMplay bgm = GameObject.Find ("BGMplay(Clone)").GetComponent<BGMplay> ();
								soundManager.fadeOutBGM (bgm, 0.04f, 0.1f);

								changeScene ();
								isClick = true;
						}
				}

		}

		public void changeScene(){
				GameObject fadein = effectManager.makeFade("transToColor", 60, 0.0f, 0.0f, 0.0f);
				Invoke ("changeLevel", 1f);
		}

		void changeLevel(){
				Application.LoadLevel (loadLevel);
		}
}
