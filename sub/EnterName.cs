using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class EnterName : MonoBehaviour {

		// Use this for initialization
		void Start () {

		}
	
		// Update is called once per frame
		void Update () {
	
		}

		public void checkString(){
				Text  nameText = GameObject.Find("NameText").GetComponent<Text>();
				if (!checkHalfByte (nameText.text)) {
						inputError ();
				} else {
						saveUserName (nameText.text);
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
}
