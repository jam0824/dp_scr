﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MiniJSON;


public class RankingLocal : MonoBehaviour {

		public GameObject netRankingPrefab;


		bool mouseFlag = true;
		int myScore = 720000;
		string myName = "m";
		int rank = 10;

		GameObject fadein;
		EffectManager effectManager;


		// Use this for initialization
		void Start () {
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();

				//セーブデータロード
				string json = loadPlayerPrefs ();
				//DictionaryのListに変換
				List<Dictionary<string, string>> dic = new Common().decodeJson(json);

				//自分のデータをリストの最後に追加
				myScore = GameObject.Find ("ResultBase").GetComponent<Result> ().stat.score;
				myName = GameObject.Find ("ResultBase").GetComponent<Result> ().stat.userName;
				dic.Add (makeMyProfile(myName, myScore));
				//並び替え
				dic = new Common().sortDictionaryList (dic, "score");
				//保存
				savePlayerPrefs (new Common ().encodeDictionaryToJson (dic, rank));

				//表示
				redrawMessage (makeMsgLeft(dic, rank), makeMsgRight(dic, rank));
		}
	
		// Update is called once per frame
		void Update () {
				//キー入力でスキップ
				if (Input.GetButton ("Fire1") || (Input.GetKey (KeyCode.Z)) || (Input.GetMouseButton (0))) {
						if (mouseFlag) {
								mouseFlag = false;
								preMoveToRanking ();
						}
				}
		}

		//メッセージ表示
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
		//自分のデータをDictionaryにする
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
		//ランキング画面に移る前のフェード
		public void preMoveToRanking(){
				effectManager.changeScene ("black", netRankingPrefab, this.transform.position);
				//fadein = effectManager.makeFade("transToColor", 60, 0.0f, 0.0f, 0.0f);
				//Invoke ("changeLevel", 1f);
		}
		//ランキング遷移
		void changeLevel(){
				Destroy (fadein);	//フェードを削除しておく
				GameObject ranking = Instantiate (netRankingPrefab, this.transform.position, this.transform.rotation) as GameObject;
				ranking.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				ranking.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
				fadein = effectManager.makeFade("colorToTrans", 60, 0.0f, 0.0f, 0.0f);
				//Destroy (this.gameObject);
		}

		//ローカルランキングを書く
		void showLocalRanking(){
				//セーブデータロード
				string json = loadPlayerPrefs ();
				//DictionaryのListに変換
				List<Dictionary<string, string>> dic = new Common().decodeJson(json);
				//並び替え
				dic = new Common().sortDictionaryList (dic, "score");
				//表示
				redrawMessage (makeMsgLeft(dic, rank), makeMsgRight(dic, rank));
		}
				
}
