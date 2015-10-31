using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tj_logo : MonoBehaviour {

		public GameObject fadePrefab;
		public GameObject titlePrefab;
		GameObject fadein;
		SoundManager soundManager;
		EffectManager effectManager;
		Common common;

		int gameCount = 0;
		int viewTitleTime = 60 * 4;
		public bool isClick = false;

		// Use this for initialization
		void Start () {
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				common = GameObject.Find("Common").GetComponent<Common>();
				BGMplay bgm = soundManager.playBGM ("opening");
				fadein = common.makeFade (fadePrefab, this.gameObject, 0, 60, 255.0f, 255.0f, 255.0f);
		}
	
		// Update is called once per frame
		void Update () {
				if (Input.GetButton ("Fire1") || (Input.GetKey (KeyCode.Z)) || (Input.GetMouseButton (0))
						|| (gameCount > viewTitleTime)) {
						if (!isClick) {
								effectManager.changeScene ("white", titlePrefab, this.transform.position);
								isClick = true;
						}
				}

				if (!isClick)
						gameCount++;

		}

}
