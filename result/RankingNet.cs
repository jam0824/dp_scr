using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RankingNet : MonoBehaviour {


		public GameObject staffRolePrefab;


		ConnectAPI api;
		bool isGetResult = false;
		bool mouseFlag = true;

		int myScore = 0;
		string myName = "";
		int rank = 10;	//ランク表示数

		GameObject fadein;
		EffectManager effectManager;
		SoundManager soundManager;
		BGMplay bgm;

		// Use this for initialization
		void Start () {
				readMySql ();
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				GameObject rs = GameObject.Find ("ResultBase");

				if (rs != null) {
						myScore = rs.GetComponent<Result> ().stat.score;
						myName = rs.GetComponent<Result> ().stat.userName;
				}



		}
	
		// Update is called once per frame
		void Update () {
				if (!isGetResult) {
						if(api.result != ""){
								isGetResult = true;
								//Debug.Log ("unity = " + api.result);
								drawJsonString (api.result);
								mouseFlag = true;
						}
				}
				//キー入力でスキップ
				if (Input.GetButton ("Fire1") || (Input.GetKey (KeyCode.Z)) || (Input.GetMouseButton (0))) {
						if (mouseFlag) {
								mouseFlag = false;
								preChange ();
						}
				}
		}

		//描画
		void drawJsonString(string json){
				List<Dictionary<string, string>> dic = new Common().decodeJson(json);
				//クリア後ではないときに呼ばれたら１位から描画
				int redrawPosition = 0;
				if (myScore != 0) {
						redrawPosition = getRankPosition (dic);
						Debug.Log ("pos = " + redrawPosition);
				}
				//表示
				redrawMessage (makeMsgLeft(dic, rank, redrawPosition), makeMsgRight(dic, rank, redrawPosition));
		}

		//描画位置を探す
		int getRankPosition(List<Dictionary<string, string>> dic){
				int resultRank = 0;
				for(int i = 0; i < dic.Count; i++){
						//スコアと名前が一致する内容を探す。
						if ((int.Parse (dic [i] ["score"]) == myScore) && (dic[i]["name"] == myName)){
								resultRank = i;
								break;
						}
				}
				//自分の位置から4こ戻す
				resultRank -= 4;
				if (resultRank < 0)
						resultRank = 0;

				return resultRank;
		}

		//メッセージ表示
		void redrawMessage(string msgLeft, string msgRight){
				GameObject.Find ("NetRankingMsgLeft").GetComponent<Text> ().text = msgLeft;
				GameObject.Find ("NetRankingMsgRight").GetComponent<Text> ().text = msgRight;
		}
		//左側のメッセージ作成
		string makeMsgLeft(List<Dictionary<string, string>> dic, int rank, int pos){

				string str = "";
				for(int i = pos; i < dic.Count; i++){
						str += "" + dic[i]["rank"] + ". " + dic [i] ["name"] + "\n";
				}
				return str;
		}
		//右側のメッセージ作成
		string makeMsgRight(List<Dictionary<string, string>> dic, int rank, int pos){
				string str = "";
				for(int i = pos; i < dic.Count; i++){
						str += string.Format ("{0:#,0}",int.Parse(dic [i] ["score"])) + "\n";
				}
				return str;
		}
		//遷移処理
		void preChange(){
				GameObject rs = GameObject.Find ("ResultBase");
				BGMplay bgm = GameObject.Find ("BGMplay(Clone)").GetComponent<BGMplay> ();
				soundManager.fadeOutBGM (bgm, 0.04f, 0.1f);

				if (rs != null) {
						//result画面からの遷移ならこっち。
						//ライフが残っていたらスタッフロール。それ以外はタイトル
						if (rs.GetComponent<Result> ().stat.life > 0) {
								fadein = effectManager.makeFade ("transToColor", 60, 0.0f, 0.0f, 0.0f);
								Invoke ("changeLevel", 1f);
						} else {
								fadein = effectManager.makeFade ("transToColor", 60, 255.0f, 255.0f, 255.0f);
								Invoke ("changeLevelTitle", 1f);
						}
				} else {
						//タイトルで呼ばれたときなどはこっち
						fadein = effectManager.makeFade ("transToColor", 60, 255.0f, 255.0f, 255.0f);
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

		/// <summary>
		/// Data save to DB
		/// </summary>
		void readMySql(){
				string word = "milk0824";
				api = GameObject.Find("ConnectAPI").GetComponent<ConnectAPI>();
				api.resultReset ();
				Dictionary<string,string> dic = new Dictionary<string,string>();
				string query = "SELECT *,(SELECT COUNT(*) FROM ANGEL_BEATS_RANKING b WHERE a.score < b.score AND type=" + getMode() + ") + 1 AS rank FROM ANGEL_BEATS_RANKING a WHERE type=" + getMode() + " order by rank asc;";
				dic.Add ("md5", new Common().calcMd5(query + word));
				dic.Add ("query", query);
				WWW results = api.POST(api.apiUrl, dic);
		}
		//typeを返す
		int getMode(){
				GameObject rs = GameObject.Find ("ResultBase");
				//Result画面から飛んだとき
				if (rs != null) {
						//R18モードのときはセーブポジションを変える。
						if (GameObject.Find ("ResultBase").GetComponent<Result> ().stat.isR18Mode) {
								return 1;
						} else {
								return 0;
						}
				} else {
						//タイトルなどから飛んだとき
						return 0;
				}
		}
}
