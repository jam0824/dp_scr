using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Result : MonoBehaviour {

		//ランキングデータ格納クラス
		public class Status{
				public bool isR18Mode = false;
				public string userName = "YOU";
				public int playTime = 60 * 60 * 1;
				public int score = 1234;
				public int life = 2;
				public int graze = 256;
				public int power = 256;
				public int enemies = 320;
				public int useBombs = 2;
				public int lifeScore = 500000;
				public int grazeScore = 5000;
				public int powerScore = 10000;
				public int timeScore = 1000;
		}
				

		public GameObject noMissPrefab;
		public GameObject rankingPrefab;
		public GameObject enterNamePrefab;

		public Status stat = new Status();

		GameManager gameManager;
		EffectManager effectManager;
		SoundManager soundManager;

		string messageLeft = "";
		string messageRight = "";


		Text resultBoxLeft;
		Text resultBoxRight;
		Text resultBoxAll;

		float waitTime = 0.5f;
		bool mouseFlag = false;
		GameObject fadein;
		BGMplay bgm;

		// Use this for initialization
		void Start () {
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();


				//gameObjectからデータを引き継ぐ
				GameObject gm = GameObject.Find ("GameManager");
				if (gm != null) {
						gameManager = gm.GetComponent<GameManager>();
						stat = getStatus ();
						Destroy (gm);
				}
						
				fadein = effectManager.makeFade("colorToTrans", 60, 255.0f, 255.0f, 255.0f);

				resultBoxLeft = GameObject.Find ("messageLeft").GetComponent<Text> ();
				resultBoxRight = GameObject.Find ("messageRight").GetComponent<Text> ();
				resultBoxAll = GameObject.Find ("messageAll").GetComponent<Text> ();
				//最初に計算前結果を入れておく
				resultBoxAll.text = string.Format ("{0:#,0}", stat.score);

				//順番に結果表示
				Invoke ("redrawLife", 1f);
				Invoke ("redrawGraze", 1f + waitTime * 1);
				Invoke ("redrawPower", 1f + waitTime * 2);
				Invoke ("redrawPlayTime", 1f + waitTime * 3);
				Invoke ("redrawUseBombs", 1f + waitTime * 4);
		}
	
		// Update is called once per frame
		void Update () {
				//何かボタンが押されたら名前ボックスを出す。
				if (Input.GetButton ("Fire1") || (Input.GetKey (KeyCode.Z)) || (Input.GetMouseButton (0))) {
						if (mouseFlag) {
								//クリックされたら名前入力ボックス作成
								makeEnterNameBox ();
								mouseFlag = false;
						}
				}
	
		}
				
		void redrawLife(){
				messageLeft += "Life\n" + string.Format ("{0:#,0}", stat.lifeScore)+ " × " + stat.life + "\n";
				//カンマ区切りで表示
				messageRight += "\n= " + string.Format ("{0:#,0}", (stat.lifeScore* stat.life)) + "\n";
				stat.score += stat.lifeScore * stat.life;
				inputMessage ();
		}
		void redrawGraze(){
				messageLeft += "Graze\n" + string.Format ("{0:#,0}", stat.grazeScore)+ " × " + stat.graze + "\n";
				messageRight += "\n= " + string.Format ("{0:#,0}", (stat.grazeScore * stat.graze)) + "\n";
				stat.score += stat.grazeScore * stat.graze;
				inputMessage ();
		}
		void redrawPower(){
				messageLeft += "Power\n" + string.Format ("{0:#,0}", stat.powerScore)+ " × " + stat.power + "\n";
				messageRight += "\n= " + string.Format ("{0:#,0}", (stat.powerScore * stat.power)) + "\n";
				stat.score += stat.powerScore * stat.power;
				inputMessage ();
		}
		void redrawPlayTime(){
				int baseTime = (stat.isR18Mode) ? 400 : 300;
				int time = (int)(baseTime - (Mathf.Floor (stat.playTime) / 60));
				//プレイ時間が短いときもペナルティ
				int timeScore = ((Mathf.Floor (stat.playTime) / 60) < 120) ? -stat.timeScore : stat.timeScore;

				messageLeft += "Play Time(Base:" + baseTime + "sec)\n" + baseTime + " - " + Mathf.Floor(stat.playTime / 60) + "\n";
				messageRight += "\n= " + string.Format ("{0:#,0}", (timeScore * time)) + "\n";

				stat.score += timeScore * time;
				inputMessage ();
		}
		void redrawUseBombs(){
				messageLeft += "Use Bombs\n-40,000 × " + stat.useBombs + "\n";
				messageRight += "\n= " + string.Format ("{0:#,0}", (-40000 * stat.useBombs)) + "\n";
				stat.score += -40000 * stat.useBombs;
				inputMessage ();

				//ノーミス分岐。ライフ数で判別
				if (stat.life == 3) {
						Invoke ("makeNoMissBox", waitTime);
				} else {
						Invoke ("redrawTotal", waitTime);
				}

		}
		//ノーミスのボックス作成
		void makeNoMissBox(){
				GameObject noMiss = Instantiate (noMissPrefab, this.transform.position, this.transform.rotation) as GameObject;
				noMiss.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				noMiss.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
				stat.score += 2000000;
				Invoke ("redrawTotal", waitTime);
		}
		//トータルスコア表示
		void redrawTotal(){
				resultBoxAll.text = string.Format ("{0:#,0}", stat.score);
				mouseFlag = true;	//クリックを許可
				bgm = soundManager.playBGM ("result");
		}
		//名前入力ボックス作成
		void makeEnterNameBox(){
				GameObject enterName = Instantiate (enterNamePrefab, this.transform.position, this.transform.rotation) as GameObject;
				enterName.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				enterName.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す

				//名前が保存されていたらinputboxに名前挿入
				if (getUserName () != "") {
						GameObject.Find("InputField").GetComponent<InputField>().text = getUserName ();
				}
		}


		//*******************************************************************
		//ゲームマネージャーからデータ取得
		Status getStatus(){
				Status data = new Status ();
				data.isR18Mode = gameManager.isR18Mode;
				data.score = gameManager.score;
				data.life = gameManager.life;
				data.power = gameManager.power;
				data.graze = gameManager.graze;
				data.enemies = gameManager.defeatEnemies;
				data.useBombs = gameManager.useBombs;
				data.playTime = gameManager.gameFrame;
				return data;
		}

		//ランキング画面に移る前のフェード
		public void preMoveToRanking(){
				effectManager.changeScene ("black", rankingPrefab, this.transform.position);
				//fadein = effectManager.makeFade("transToColor", 60, 0.0f, 0.0f, 0.0f);
				//Invoke ("changeLevel", 1f);
		}
		//ランキング遷移
		void changeLevel(){
				Destroy (fadein);	//フェードを削除しておく
				GameObject ranking = Instantiate (rankingPrefab, this.transform.position, this.transform.rotation) as GameObject;
				ranking.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				ranking.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
				fadein = effectManager.makeFade("colorToTrans", 60, 0.0f, 0.0f, 0.0f);
				//Destroy (this.gameObject);
		}

		//**********************************************************************
		//保存されている名前取得
		string getUserName(){
				return PlayerPrefs.GetString("UserName", "");
		}
		//描画処理
		void inputMessage(){
				soundManager.playSE ("result_draw");
				resultBoxLeft.text = messageLeft;
				resultBoxRight.text = messageRight;
				resultBoxAll.text = string.Format ("{0:#,0}", stat.score);
		}


}
