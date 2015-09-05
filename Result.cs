using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Result : MonoBehaviour {

		public class Status{
				public int score = 1234;
				public int life = 3;
				public int graze = 256;
				public int power = 256;
				public int enemies = 320;
				public int useBombs = 2;
		}

		public GameObject fadePrefab;
		public GameObject noMissPrefab;
		public GameObject rankingPrefab;

		public Status stat = new Status();

		GameManager gameManager;

		string messageLeft = "";
		string messageRight = "";


		Text resultBoxLeft;
		Text resultBoxRight;
		Text resultBoxAll;

		float waitTime = 0.5f;
		bool mouseFlag = false;
		GameObject fadein;

		void Awake(){


		}

		// Use this for initialization
		void Start () {
				GameObject fade = new Common ().makeFade (fadePrefab, this.gameObject, 0, 60, 255.0f, 255.0f, 255.0f);
				//gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				resultBoxLeft = GameObject.Find ("messageLeft").GetComponent<Text> ();
				resultBoxRight = GameObject.Find ("messageRight").GetComponent<Text> ();
				resultBoxAll = GameObject.Find ("messageAll").GetComponent<Text> ();

				Invoke ("redrawLife", 1f);
				Invoke ("redrawGraze", 1f + waitTime * 1);
				Invoke ("redrawPower", 1f + waitTime * 2);
				Invoke ("redrawEnemies", 1f + waitTime * 3);
				Invoke ("redrawUseBombs", 1f + waitTime * 4);
		}
	
		// Update is called once per frame
		void Update () {
				//何かボタンが押されたら
				if (Input.GetButton ("Fire1") || (Input.GetKey (KeyCode.Z)) || (Input.GetMouseButton (0))) {
						if (mouseFlag) {
								preMoveToRanking ();
								mouseFlag = false;
						}
				}
	
		}

		void redrawLife(){
				messageLeft += "Life\n100,000 × " + stat.life + "\n";
				messageRight += "\n= " + string.Format ("{0:#,0}", (100000 * stat.life)) + "\n";
				stat.score += 100000 * stat.life;
				inputMessage ();
		}

		void redrawGraze(){
				messageLeft += "Graze\n10,000 × " + stat.graze + "\n";
				messageRight += "\n= " + string.Format ("{0:#,0}", (10000 * stat.graze)) + "\n";
				stat.score += 10000 * stat.graze;
				inputMessage ();
		}

		void redrawPower(){
				messageLeft += "Power\n5,000 × " + stat.power + "\n";
				messageRight += "\n= " + string.Format ("{0:#,0}", (5000 * stat.power)) + "\n";
				stat.score += 5000 * stat.power;
				inputMessage ();
		}

		void redrawEnemies(){
				messageLeft += "Enemy\n5,000 × " + stat.enemies + "\n";
				messageRight += "\n= " + string.Format ("{0:#,0}", (5000 * stat.power)) + "\n";
				stat.score += 5000 * stat.enemies;
				inputMessage ();
		}

		void redrawUseBombs(){
				messageLeft += "Use Bombs\n-40,000 × " + stat.useBombs + "\n";
				messageRight += "\n= " + string.Format ("{0:#,0}", (-40000 * stat.useBombs)) + "\n";
				stat.score += -40000 * stat.useBombs;
				inputMessage ();

				//ノーミス分岐
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
		}

		void inputMessage(){
				resultBoxLeft.text = messageLeft;
				resultBoxRight.text = messageRight;
		}

		//ランキング画面に移る前のクロフェード
		void preMoveToRanking(){
				fadein = new Common ().makeFade (fadePrefab, this.gameObject, 1, 60, 0.0f, 0.0f, 0.0f);
				Invoke ("changeLevel", 1f);
		}

		void changeLevel(){
				Destroy (fadein);
				GameObject ranking = Instantiate (rankingPrefab, this.transform.position, this.transform.rotation) as GameObject;
				ranking.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				ranking.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
				fadein = new Common ().makeFade (fadePrefab, this.gameObject, 0, 60, 0.0f, 0.0f, 0.0f);
				//Destroy (this.gameObject);
		}
}
