using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButton : MonoBehaviour {

		const int VIEW_NET_RANKING_TIME = 60 * 20;

		public GameObject netRankingPrefab;
		public GameObject normalPrefab;	//HowToPlayのプレハブ
		public Sprite r18StringSprite;	//R18のアクティブ時の画像

		EffectManager effectManager;
		SoundManager soundManager;

		int gameCount = 0;
		int selectItem = 0;	//０がノーマルモード選択時、１がR18モード選択時

		RectTransform selecter;
		bool isClick = false;
		bool isR18Mode = false;

		// Use this for initialization
		void Start () {
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
				selecter = GameObject.Find("selecter").GetComponent<RectTransform>();

				isR18Mode = getR18mode ();
				//isR18Mode = true;
				changeR18String (r18StringSprite);
				drawSelecter (selectItem);
		}
	

		//****************************************************
		//モード選択
		void keyCheck(){
				float y = Input.GetAxis ("Vertical");
				if((y > 0) && (selectItem != 0)){
						selectItem = 0;
						drawSelecter (selectItem);
				}
				else if((y < 0) && (isR18Mode) && (selectItem != 1)){
						selectItem = 1;
						drawSelecter (selectItem);
				}

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
						BGMplay bgm = GameObject.Find ("BGMplay(Clone)").GetComponent<BGMplay> ();
						soundManager.fadeOutBGM (bgm, 0.04f, 0.1f);

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

		//R18モード解禁時のR18ボタンアクティブ化
		void changeR18String(Sprite sprite){
				if (isR18Mode) {
						Image img = GameObject.Find ("startR18").GetComponent<Image> ();
						img.sprite = sprite;
						GameObject.Find ("startR18").GetComponent<Button> ().enabled = true;
						Debug.Log ("R18 enable");
				}
		}
				
		//**********************************************************************
		//モードセレクトの選択を表示
		void drawSelecter(int selectItemNo){
				Vector2 pos = new Vector2 ();
				pos.x = 0;
				pos.y = (selectItemNo == 0) ? -210 : -280;
				selecter.anchoredPosition = pos;
		}
		// Update is called once per frame
		void Update () {
				keyCheck ();

				if (Input.GetButtonDown ("Fire1") || (Input.GetKeyDown (KeyCode.Z))) {
						Debug.Log ("押された");
						//クリックは例外にしておく
						if(!isClick){
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

}
