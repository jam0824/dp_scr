using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour {
		const int START_TIME = 5;

		public int gameFrame = 0;
		public int gameSecond = 90;
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
		public bool isR18Mode = false;

		public int bulletNum = 0;

		public TextAsset scenarioFile;
		List<scenarioBean> scenario = new List<scenarioBean>();

		//設定用プレハブ
		public GameObject bossPrefab;
		public GameObject middleBossPrefab;
		public GameObject bgPrefab;
		public GameObject lifePrefab;
		public GameObject bombPrefab;
		public GameObject startMsgPrefab;
		public GameObject warningMsgPrefab;
		public GameObject hitodamaPrefab;


		public List<GameObject> formaitonManager = new List<GameObject>();

		public bool bombFlag = false;
		bool playingFlag = true;	//本編フラグ。
		bool bossAppearFlag = false;

		Text scoreText;
		Text fpsText;
		float fps;

		SoundManager soundManager;
		EffectManager effectManager;
		Common common;
		BGMplay bgm;



		void Awake(){
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				common = GameObject.Find("Common").GetComponent<Common>();
				//破棄しないようにする
				DontDestroyOnLoad(this.gameObject);
				Resources.UnloadUnusedAssets();

		}

		// Use this for initialization
		void Start () {


				ScenarioLoader loader = new ScenarioLoader ();
				scenario = loader.fileRead (scenarioFile);

				//黒→透明のフェード
				GameObject fadein = effectManager.makeFade("colorToTrans", 60, 0.0f, 0.0f, 0.0f);
				makeBG ();

				bgm = (!isR18Mode)? soundManager.playBGM ("bgm00") : soundManager.playBGM ("R18battle");

				scoreText = GameObject.Find("score").GetComponent<Text>();
				fpsText = GameObject.Find("fps").GetComponent<Text>();
				initLifeBombUI (lifePrefab, life, 25, -110, 50, 0.5f);
				initLifeBombUI (bombPrefab, bomb, 25, -142, 32, 2f);


		}
		
		// Update is called once per frame
		void Update () {
				if (!playingFlag)
						return;
				if (Time.timeScale == 0)
						return;	//時間停止時
				
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
						RectTransform rt = GameObject.Find ("statusView").GetComponent<RectTransform> ();
						switch(scenario[0].getCommand()){
						case "Start":
								makeMsg (startMsgPrefab);	//スタートメッセージ
								break;
						case "Formation":
								makeFormation (scenario[0].getParam());
								break;
						case "Boss":
								bossAppearFlag = true;
								StartCoroutine (common.moveUI(rt, -1f, -50f, 0.01f)); //コールチンでステータス移動
								makeBoss ();
								break;
						case "MiddleBoss":
								StartCoroutine (common.moveUI(rt, -1f, -50f, 0.01f)); //コールチンでステータス移動
								makeMiddleBoss ();
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
						case "PlayBossBGM":
								bgm = (!isR18Mode) ? soundManager.playBGM ("bgm01") : soundManager.playBGM ("R18boss");
								break;
						}
						scenario.RemoveAt(0);//先頭要素削除。常に先頭を参照
						Resources.UnloadUnusedAssets();
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
		//middleボス作成
		void makeMiddleBoss(){
				GameObject boss = Instantiate (middleBossPrefab, new Vector3(0.0f, 3.0f, 0f), this.transform.rotation) as GameObject;
		}
		//**********************************************************

		//ボムオン。ボムがオンの時は玉が消える
		public void bombOn(){
				bombFlag = true;
		}

		//ボムオフ。ボム終了時に呼ばれる。
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
		/// ライフとボム描画
		/// </summary>
		/// <param name="LBPrefab">prefab.</param>
		/// <param name="num">描画個数</param>
		/// <param name="x">x</param>
		/// <param name="y">y</param>
		/// <param name="w">The width.</param>
		void initLifeBombUI(GameObject LBPrefab, int num, int x, int y, int w, float scale){
				for(int i = 0; i < num; i++){
						GameObject prefab = Instantiate (LBPrefab, this.transform.position, this.transform.rotation) as GameObject;
						prefab.transform.parent = GameObject.Find ("statusView").transform;	//Canvasを親にする

						RectTransform rt = prefab.GetComponent<RectTransform>();
						rt.localScale = new Vector3 (scale, scale, 1);	//スケールを２倍にする
						rt.anchoredPosition = new Vector2(x, y);	//位置変更
						x += w;
				}
		}

		//ライフとボムを再描画
		void redrawLifeBombUI(int lifeNum, int bombNum){
				deleteObject ("UILife");
				deleteObject ("UIBomb");
				initLifeBombUI (lifePrefab, lifeNum, 25, -110, 50, 0.5f);
				initLifeBombUI (bombPrefab, bombNum, 25, -142, 32, 2f);
		}

		//指定したタグのオブジェクトを全て削除
		void deleteObject(string tagName){
				GameObject[] prefabs = GameObject.FindGameObjectsWithTag (tagName);
				for(int i = 0; i < prefabs.Length; i++){
						Destroy (prefabs[i]);
				}
		}

		/// <summary>
		/// ライフを減らす
		/// </summary>
		public void decLife(){
				life--;
				redrawLifeBombUI (life, bomb);
				//ゲームオーバー
				if (life == 0) {
						playingFlag = false;
						gameOver ();
				}
		}


		/// <summary>
		/// ボムを減らす
		/// </summary>
		public void decBomb(){
				bomb--;
				useBombs++;
				redrawLifeBombUI (life, bomb);
		}
				



		// ///////////////////////////////////////
		//ゲーム中の「operation start」といったメッセージオブジェクト描画
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
				soundManager.fadeOutBGM (bgm, 0.04f, 0.1f);
				gameOver ();
		}
				

		//*********************************************
		//人魂をつくる
		void makeHitodama(){
				if (fps < 50)
						return;
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
						fps = 1 / Time.deltaTime;
						fpsText.text = "FPS " + fps.ToString ("N2");
				}
		}

		//*******************************************
		//レベルを書く
		public void drawLv(int power, int range){
				Text level = GameObject.Find ("level").GetComponent<Text> ();
				float f = power / range;
				level.text = "Lv." + (f + 1);
		}

		//**********************************************************************
		/// <summary>
		/// ゲーム終了。フェードアウトさせて３秒後に結果画面遷移
		/// </summary>
		public void gameOver(){
				//透明→色のフェード
				GameObject fadein = effectManager.makeFade("transToColor", 180, 255.0f, 255.0f, 255.0f);
				Invoke ("goResult", 3f);
		}

		/// <summary>
		/// Result画面に遷移
		/// </summary>
		public void goResult(){
				Application.LoadLevel ("SceneResult");
		}
				


}
