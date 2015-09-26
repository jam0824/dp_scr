using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButton : MonoBehaviour {

		const int VIEW_NET_RANKING_TIME = 60 * 20;

		public GameObject netRankingPrefab;
		public GameObject normalPrefab;
		EffectManager effectManager;
		SoundManager soundManager;

		bool isClick = false;
		int gameCount = 0;
		bool isR18Mode = false;
		int selectItem = 0;
		RectTransform yubi;


		// Use this for initialization
		void Start () {
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
				yubi = GameObject.Find("yubi").GetComponent<RectTransform>();
				isR18Mode = getR18mode ();
				//isR18Mode = true;
				drawYubi (selectItem);
		}
	
		// Update is called once per frame
		void Update () {
				keyCheck ();
				if (Input.GetButtonUp ("Fire1") || (Input.GetKeyUp (KeyCode.Z))) {
						if((!Input.GetMouseButton (0)) && (!isClick)){
								//ノーマルモード
								if(selectItem == 0){
										changeNormalScene ();
								}
								//R18モード
								else if(selectItem == 1){
										changeR18Scene ();
								}
						}
				}
				//時間がきたら自動的にネットランキング表示
				if((!isClick) && (gameCount > VIEW_NET_RANKING_TIME)){
						changeNetRanking (netRankingPrefab);
						isClick = true;
				}
				gameCount++;
		}

		void keyCheck(){
				float y = Input.GetAxis ("Vertical");
				if(y > 0){
						selectItem = 0;
				}
				else if((y < 0) && (isR18Mode)){

						selectItem = 1;
				}
				drawYubi (selectItem);
		}
		/// <summary>
		/// Changes the normal scene.
		/// </summary>
		public void changeNormalScene(){
				changeScene (normalPrefab);
		}
		/// <summary>
		/// Changes the r18 scene.
		/// </summary>
		public void changeR18Scene(){
				if ((!isR18Mode) || (isClick))
						return;
				GameObject fadein = effectManager.makeFade("transToColor", 60, 0.0f, 0.0f, 0.0f);
				isClick = true;
				soundManager.playSE ("OK");
				Invoke ("changeLevel", 1f);
		}
		void changeLevel(){
				Application.LoadLevel ("R18mode");
		}
		/// <summary>
		/// シーン切り替え処理
		/// </summary>
		/// <param name="prefab">Prefab.</param>
		void changeScene(GameObject prefab){
				if (!isClick) {
						isClick = true;
						soundManager.playSE ("OK");
						//位置取得にタイトルを利用しているだけ
						effectManager.changeScene ("black", prefab, GameObject.Find("TJ_logo").transform.position);
				}
		}

		/// <summary>
		/// ネットランキングページを表示
		/// </summary>
		/// <param name="prefab">Prefab.</param>
		void changeNetRanking(GameObject prefab){
				effectManager.changeScene ("black", prefab, GameObject.Find("TJ_logo").transform.position);
		}

		//**********************************************************************
		//R18モード解禁を確認
		bool getR18mode(){
				return (PlayerPrefs.GetString ("R18Mode", "") == "iris") ? true : false;
		}

		//**********************************************************************
		void drawYubi(int selectItemNo){
				Vector2 pos = new Vector2 ();
				if (selectItemNo == 0) {
						pos.x = -260;
						pos.y = -180;
				} else {
						pos.x = -260;
						pos.y = -255;
				}
				yubi.anchoredPosition = pos;
		}

}
