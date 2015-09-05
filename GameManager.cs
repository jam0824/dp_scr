using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour {
		const int GAME_FPS = 60;
		const int START_TIME = 5;

		public int gameFrame = 0;
		public int gameSecond = 0;
		public int frameRate = 60;

		//メトリクス
		public int power = 0;
		public int score = 0;
		public int graze = 0;
		public int grazeScore = 1000;
		public int life = 3;
		public int bomb = 3;
		public int useBombs = 0;
		public int defeatEnemies = 0;


		public TextAsset scenarioFile;

		public GameObject bossPrefab;
		public GameObject bgPrefab;
		public GameObject lifePrefab;
		public GameObject bombPrefab;
		public GameObject startMsgPrefab;
		public GameObject warningMsgPrefab;
		public GameObject flashPrefab;
		public GameObject fadePrefab;
		public GameObject hitodamaPrefab;

		public bool bombFlag = false;
		public List<GameObject> formaitonManager = new List<GameObject>();

		bool playingFlag = true;	//本編フラグ。

		bool bossAppearFlag = false;

		Text scoreText;
		Text fpsText;

		SoundManager soundManager;
		BGMplay bgm;

		List<GameObject> lifeObjects = new List<GameObject>();
		List<GameObject> bombObjects = new List<GameObject>();
		List<scenarioBean> scenario = new List<scenarioBean>();

		void Awake(){
				//破棄しないようにする
				DontDestroyOnLoad(this.gameObject);
		}

		// Use this for initialization
		void Start () {
				Application.targetFrameRate = GAME_FPS;
				ScenarioLoader loader = new ScenarioLoader ();
				scenario = loader.fileRead (scenarioFile);

				//黒→透明のフェード
				GameObject fadein = new Common ().makeFade (fadePrefab, this.gameObject, 0, 60, 0.0f, 0.0f, 0.0f);
				makeBG ();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				bgm = soundManager.playBGM ("bgm00");

				scoreText = GameObject.Find("score").GetComponent<Text>();
				fpsText = GameObject.Find("fps").GetComponent<Text>();
				lifeObjects = initLifeBombUI (lifePrefab, life, 25, -110, 32);
				bombObjects = initLifeBombUI (bombPrefab, bomb, 25, -142, 32);


		}
		
		// Update is called once per frame
		void Update () {
				if (!playingFlag)
						return;

				gameFrame++;
				//カウントは１秒で
				if(Mathf.Floor(gameFrame % frameRate) == 0){
						gameSecond++;
						if(!bossAppearFlag) makeHitodama ();
				}
				if(scenario.Count > 0) checkScenario (gameSecond);	//１秒おきにシナリオチェック
				drawFPS ();
				scoreText.text = string.Format("{0}", score);
		}

		//シナリオチェック
		void checkScenario(int gameSecond){
				if(gameSecond == scenario[0].getTime()){
						switch(scenario[0].getCommand()){
						case "Start":
								makeMsg (startMsgPrefab);	//スタートメッセージ
								break;
						case "Formation":
								makeFormation (scenario[0].getParam());
								break;
						case "Boss":
								bossAppearFlag = true;
								makeBoss ();
								break;
						case "Warning":
								makeMsg (warningMsgPrefab);	//ワーニングメッセージ
								break;
						case "DeleteBGM":
								soundManager.deleteBGM (bgm);
								break;
						case "PlayBGM":
								string name = scenario [0].getParam () [0];
								bgm = soundManager.playBGM (name);
								break;
						}
						scenario.RemoveAt(0);//先頭要素削除。常に先頭を参照
				}
		}

		//フォーメーション作成
		void makeFormation(List<string> param){
				GameObject formation = Instantiate (formaitonManager[int.Parse(param[0])], this.transform.position, this.transform.rotation) as GameObject;

		}

		//**********************************************************
		//ボス作成
		void makeBoss(){
				GameObject boss = Instantiate (bossPrefab, new Vector3(-0.4f, 5.0f, 0f), this.transform.rotation) as GameObject;
		}
		//**********************************************************

		//フラッシュ作成
		public void flash(){
				GameObject flash = Instantiate (flashPrefab, this.transform.position, this.transform.rotation) as GameObject;
		}

		//ボムオン
		public void bombOn(){
				bombFlag = true;
		}

		//ボムオフ
		public void bombOff(){
				bombFlag = false;
		}

		// /////////////////////////////////////////////
		/// <summary>
		/// 背景作成。４枚ローテーション
		/// </summary>
		public void makeBG(){
				float y = -3.0f;
				float z = 2.0f;
				float v = 7.0f;
				int num = 4;
				Vector3 pos = new Vector3 (0.0f, y, z);

				for(int i = 0; i < num; i++){
						GameObject prefab = Instantiate (bgPrefab) as GameObject;
						prefab.transform.position = pos;
						pos.y += v;
						pos.z += v;

				}

		}
		// ////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Deletes the BG
		/// </summary>
		public void deleteBG(){
				GameObject[] bg = GameObject.FindGameObjectsWithTag ("bg");
				foreach(GameObject x in bg){
						Destroy (x);
				}
		}

		// ////////////////////////////////////////////////////////////////////////////////
		//ライフ・ボム描画
		/// <summary>
		/// Inits the life bomb U.
		/// </summary>
		/// <returns>List of UI</returns>
		/// <param name="LBPrefab">LB prefab.</param>
		/// <param name="num">Number.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="w">The width.</param>
		List<GameObject> initLifeBombUI(GameObject LBPrefab, int num, int x, int y, int w){
				List<GameObject> objects = new List<GameObject>();
				for(int i = 0; i < num; i++){
						GameObject prefab = Instantiate (LBPrefab, this.transform.position, this.transform.rotation) as GameObject;
						prefab.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする

						RectTransform rt = prefab.GetComponent<RectTransform>();
						rt.localScale = new Vector3 (2, 2, 1);	//スケールを元に戻す
						rt.anchoredPosition = new Vector2(x, y);	//位置変更
						x += w;
						objects.Add (prefab);
				}
				return objects;
		}

		/// <summary>
		/// ライフを減らす
		/// </summary>
		public void decLife(){
				int max = lifeObjects.Count;
				Destroy (lifeObjects [max - 1]);
				lifeObjects.RemoveAt (max - 1);
				life--;
				//ゲームオーバー
				if (life == 0) {
						playingFlag = false;
						gameOver ();
				}
		}

		/// <summary>
		/// ゲームオーバー
		/// </summary>
		public void gameOver(){
				//透明→色のフェード
				GameObject fadein = new Common ().makeFade (fadePrefab, this.gameObject, 1, 180, 255.0f, 255.0f, 255.0f);
				Invoke ("goResult", 3f);
		}

		/// <summary>
		/// Result画面に遷移
		/// </summary>
		public void goResult(){
				Application.LoadLevel ("SceneResult");
		}
		/// <summary>
		/// ボムを減らす
		/// </summary>
		public void decBomb(){
				int max = bombObjects.Count;
				Destroy (bombObjects [max - 1]);
				bombObjects.RemoveAt (max - 1);
				bomb--;
		}


		// ///////////////////////////////////////
		void makeMsg(GameObject basePrefab){
				int x = 0;
				int y = 0;
				GameObject prefab = Instantiate (basePrefab, this.transform.position, this.transform.rotation) as GameObject;
				prefab.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする

				RectTransform rt = prefab.GetComponent<RectTransform>();
				rt.localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
				rt.anchoredPosition = new Vector2(x, y);	//位置変更

		}

		//*******************************************
		//ボスを倒したあと
		public void finishGame(){
				soundManager.stopBGM (bgm);
				//透明→色のフェード
				GameObject fadein = new Common ().makeFade (fadePrefab, this.gameObject, 1, 180, 255.0f, 255.0f, 255.0f);
				//makeWhiteOut ();
		}

		//*******************************************
		//ホワイトアウト
		void makeWhiteOut(){
				GameObject fadein = Instantiate (fadePrefab, this.transform.position, this.transform.rotation) as GameObject;
				fadein.GetComponent<fade> ().Init (1, 180, 255.0f, 255.0f, 255.0f);

				RectTransform rt = fadein.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(0, 0);	//位置変更
		}

		//*********************************************
		//人魂をつくる
		void makeHitodama(){
				float speed = (Random.value + 0.1f);
				Vector3 pos = new Vector3 (Random.value * 4 - 2, Random.value * 6 - 3,0);
				GameObject hitodama = Instantiate (hitodamaPrefab, pos, this.transform.rotation) as GameObject;
				Vector2 vec = new Vector2 (0f, 1f).normalized;	//単位ベクトル
				hitodama.GetComponent<Rigidbody2D>().AddForce(vec * speed);
		}

		//*******************************************
		void drawFPS(){
				if (Time.frameCount % Application.targetFrameRate == 0)
				{
						//fpsText.text = string.Format("{0}", 1 / Time.deltaTime);
						fpsText.text = "FPS " + (1 / Time.deltaTime).ToString ("N2");
				}
		}



}
