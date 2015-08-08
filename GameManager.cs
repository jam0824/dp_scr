using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
		const int GAME_FPS = 60;
		const int START_TIME = 5;

		public int gameFrame = 0;
		public int gameSecond = 0;
		public int frameRate = 60;

		public int power = 0;
		public int score = 0;
		public int graze = 0;
		public int life = 3;
		public int bomb = 3;

		public GameObject bossPrefab;
		public GameObject bgPrefab;


		public bool bombFlag = false;
		public GameObject FormationManager;

		public GameObject lifePrefab;
		public GameObject bombPrefab;
		public GameObject startMsgPrefab;
		public GameObject warningMsgPrefab;

		public GameObject flashPrefab;
		public GameObject fadePrefab;

		bool bossAppearFlag = false;

		Text scoreText;
		Text fpsText;
		SoundManager soundManager;

		BGMplay bgm;

		List<GameObject> lifeObjects = new List<GameObject>();
		List<GameObject> bombObjects = new List<GameObject>();

		// Use this for initialization
		void Start () {
				Application.targetFrameRate = GAME_FPS;
				fadeIn ();
				makeBG ();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				bgm = soundManager.playBGM ("bgm00");

				scoreText = GameObject.Find("score").GetComponent<Text>();
				fpsText = GameObject.Find("fps").GetComponent<Text>();
				lifeObjects = initLifeBombUI (lifePrefab, life, 25, -110, 32);
				bombObjects = initLifeBombUI (bombPrefab, bomb, 25, -142, 32);

				makeMsg (startMsgPrefab);	//スタートメッセージ
		}
		
		// Update is called once per frame
		void Update () {
		
				gameFrame++;
				//カウントは１秒で
				if(gameFrame % frameRate == 0){
						//一定時間後にフォーメーション作成
						if((!bossAppearFlag) &&(gameSecond > START_TIME) &&(gameSecond % 10 == 0)){
								makeFormation ();
						}
						else if((!bossAppearFlag) &&(gameSecond > 10)){
								appearBoss ();
						}
						gameSecond++;
				}
				drawFPS ();
				scoreText.text = string.Format("{0}", score);
		}

		//フォーメーション作成
		void makeFormation(){
				string dataSource = "//画面左側に真下に進む。自分に向けて１発,,,¥n0,50,0,0¥n120,50,0,0¥n240,50,0,0¥n360,50,0,0¥n480,50,0,0¥n600,50,0,0¥n720,50,0,0¥n840,50,0,0¥n960,50,0,0¥n1080,0,0,9999";

				GameObject formation = Instantiate (FormationManager, this.transform.position, this.transform.rotation) as GameObject;
				FormationManager s = formation.GetComponent<FormationManager>();
				s.Create(dataSource);
		}


		//ボス出現処理
		void appearBoss(){
				bossAppearFlag = true;
				makeMsg (warningMsgPrefab);	//ワーニングメッセージ
				soundManager.deleteBGM (bgm);
				bgm = soundManager.playBGM ("bgm01");
				makeBoss ();
		}
				

		//ボス作成
		void makeBoss(){
				GameObject boss = Instantiate (bossPrefab, new Vector3(-0.4f, 5.0f, 0f), this.transform.rotation) as GameObject;
		}

		//フラッシュ作成
		public void flash(){
				GameObject flash = Instantiate (flashPrefab, this.transform.position, this.transform.rotation) as GameObject;
		}

		//フェードイン（黒→背景）
		public void fadeIn(){
				GameObject fadein = Instantiate (fadePrefab, this.transform.position, this.transform.rotation) as GameObject;
				fadein.GetComponent<fade> ().Init (0, 60, 0, 0, 0);

				RectTransform rt = fadein.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(0, 0);	//位置変更

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
				makeWhiteOut ();
		}

		void makeWhiteOut(){
				GameObject fadein = Instantiate (fadePrefab, this.transform.position, this.transform.rotation) as GameObject;
				fadein.GetComponent<fade> ().Init (1, 180, 255.0f, 255.0f, 255.0f);

				RectTransform rt = fadein.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(0, 0);	//位置変更
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
