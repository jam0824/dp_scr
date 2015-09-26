using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class EnterName : MonoBehaviour {



		// Use this for initialization
		void Start () {

		}
	
		// Update is called once per frame
		void Update () {
	
		}

		//クリックされたら
		public void checkString(){
				Text  nameText = GameObject.Find("NameText").GetComponent<Text>();
				if (!checkHalfByte (nameText.text)) {
						inputError ();
				} else {
						saveUserName (nameText.text);
						//DB書き込み
						try{
						writeMySql (nameText.text, GameObject.Find ("ResultBase").GetComponent<Result> ().stat.score);
						}catch(UnityException e){

						}
						GameObject.Find ("ResultBase").GetComponent<Result> ().preMoveToRanking ();
				}
		}


		public bool checkHalfByte(string str){
				bool b = (Regex.IsMatch (str, "^[a-zA-Z0-9 !-/:-@¥[-`{-~]+$"));
				return b;
		}

		void inputError(){
				Debug.Log ("error");
		}

		//入力された名前を変数とPlayerPrefsに保存
		void saveUserName(string name){
				GameObject.Find ("ResultBase").GetComponent<Result> ().stat.userName = name;
				PlayerPrefs.SetString ("UserName", name);
		}

		/// <summary>
		/// Data save to DB
		/// </summary>
		void writeMySql(string name, int score){
				string word = "milk0824";
				ConnectAPI w = GameObject.Find("ConnectAPI").GetComponent<ConnectAPI>();
				Dictionary<string,string> dic = new Dictionary<string,string>();
				string query = "insert into ANGEL_BEATS_RANKING (type,name,score,date) values (" + getMode() + ",'" + name + "'," + score + ",CURDATE());";
				dic.Add ("md5", new Common().calcMd5(query + word));
				dic.Add ("query", query);
				WWW results = w.POST(w.apiUrl, dic);
		}
		//typeを返す
		int getMode(){
				//R18モードのときはセーブポジションを変える。
				if (GameObject.Find ("ResultBase").GetComponent<Result> ().stat.isR18Mode) {
						return 1;
				} else {
						return 0;
				}
		}
}
