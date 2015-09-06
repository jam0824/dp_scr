using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MiniJSON;


public class RankingLocal : MonoBehaviour {

		public GameObject fadePrefab;
		public GameObject staffRolePrefab;
		GameObject fadein;
		bool mouseFlag = true;

		int myScore = 720000;
		string myName = "m";
		int rank = 10;


		// Use this for initialization
		void Start () {
				string json = loadPlayerPrefs ();
				List<Dictionary<string, string>> dic = new Common().decodeJson(json);

				//自分のデータをリストの最後に追加
				myScore = GameObject.Find ("ResultBase").GetComponent<Result> ().stat.score;
				myName = GameObject.Find ("ResultBase").GetComponent<Result> ().stat.userName;
				dic.Add (makeMyProfile(myName, myScore));
				//並び替え
				dic = new Common().sortDictionaryList (dic, "score");
				//表示
				redrawMessage (makeMsgLeft(dic, rank), makeMsgRight(dic, rank));
				//保存
				savePlayerPrefs (new Common ().encodeDictionaryToJson (dic, rank));

		}
	
		// Update is called once per frame
		void Update () {
				//キー入力でスキップ
				if (Input.GetButton ("Fire1") || (Input.GetKey (KeyCode.Z)) || (Input.GetMouseButton (0))) {
						if (mouseFlag) {
								mouseFlag = false;
								preChange ();
						}
				}
		}

		void redrawMessage(string msgLeft, string msgRight){
				GameObject.Find ("RankingMsgLeft").GetComponent<Text> ().text = msgLeft;
				GameObject.Find ("RankingMsgRight").GetComponent<Text> ().text = msgRight;
		}

		//左側のメッセージ作成
		string makeMsgLeft(List<Dictionary<string, string>> dic, int rank){

				string str = "";
				for(int i = 0; i < rank; i++){
						str += "" + (i + 1) + ". " + dic [i] ["name"] + "\n";
				}
				return str;
		}
		//右側のメッセージ作成
		string makeMsgRight(List<Dictionary<string, string>> dic, int rank){
				string str = "";
				for(int i = 0; i < rank; i++){
						str += string.Format ("{0:#,0}",int.Parse(dic [i] ["score"])) + "\n";
				}
				return str;
		}

		Dictionary<string, string> makeMyProfile(string name, int score){
				Dictionary<string, string> dic = new Dictionary<string, string> ();
				dic.Add ("name", name);
				dic.Add ("score", score.ToString());
				dic.Add ("date", new Common().getDate());
				return dic;
		}


		//セーブ
		void savePlayerPrefs(string json){
				PlayerPrefs.SetString ("RankingData", json);
		}

		//ロード
		string loadPlayerPrefs(){
				string testJson = "[" +
						"{\"name\":\"UMR\",\"score\":\"1000000\",\"date\":\"2015/9/4 18:21\"}," +
						"{\"name\":\"AAA\",\"score\":\"100000\",\"date\":\"2015/9/2 1:02\"}," +
						"{\"name\":\"TSF\",\"score\":\"900000\",\"date\":\"2015/9/2 1:02\"}," +
						"{\"name\":\"BBB\",\"score\":\"600000\",\"date\":\"2015/9/2 1:02\"}," +
						"{\"name\":\"CCC\",\"score\":\"700000\",\"date\":\"2015/9/2 1:02\"}," +
						"{\"name\":\"DDD\",\"score\":\"500000\",\"date\":\"2015/9/2 1:02\"}," +
						"{\"name\":\"EEE\",\"score\":\"300000\",\"date\":\"2015/9/2 1:02\"}," +
						"{\"name\":\"FFF\",\"score\":\"400000\",\"date\":\"2015/9/2 1:02\"}," +
						"{\"name\":\"GGG\",\"score\":\"200000\",\"date\":\"2015/9/2 1:02\"}," +
						"{\"name\":\"HHH\",\"score\":\"50000\",\"date\":\"2015/9/2 1:02\"}," +
						"]";

				string data = PlayerPrefs.GetString("RankingData", "");
				return (data != "") ? data : testJson;
		}

		//遷移処理
		void preChange(){

				//ライフが残っていたらスタッフロール。それ以外はタイトル
				if (GameObject.Find ("ResultBase").GetComponent<Result> ().stat.life > 0) {
						GameObject fadein = new Common ().makeFade (fadePrefab, this.gameObject, 1, 60, 0.0f, 0.0f, 0.0f);
						Invoke ("changeLevel", 1f);
				} else {
						GameObject fadein = new Common ().makeFade (fadePrefab, this.gameObject, 1, 60, 255.0f, 255.0f, 255.0f);
						Invoke ("changeLevelTitle", 1f);
				}

		}

		//staffロール開始
		void changeLevel(){
				Destroy (fadein);
				GameObject prefab = Instantiate (staffRolePrefab) as GameObject;
				prefab.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				prefab.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
		}

		//タイトルに戻る
		void changeLevelTitle(){
				Application.LoadLevel ("title");
		}
				
}
