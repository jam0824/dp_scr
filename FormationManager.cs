using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class FormationManager : MonoBehaviour {

		public class lineData{
				public int triggerFrame;
				public float x;
				public float y;
				public int enemyNo;

				//コンストラクタ
				public lineData(string dataSource){
						Debug.Log("source=" + dataSource);
						string[] data = dataSource.Split (","[0]);
						triggerFrame = int.Parse(data[0]);
						x = float.Parse(data[1]);
						y = float.Parse(data[2]);
						enemyNo = int.Parse(data[3]);
				}
		}

		const int OBJECT_LIFE = 3600;	//オブジェクトの寿命
		public int objectFrame = 0;
		public GameObject[] enemyPrefab;
		public TextAsset formationFile;
		public List<lineData> formationData = new List<lineData>();

		// Use this for initialization
		void Start () {
				init (formationFile);
		}
		
		// Update is called once per frame
		void Update () {
				checkFormationData ();
				objectFrame++;
				//寿命がきたら削除
				if(objectFrame > OBJECT_LIFE){
						delete ();
				}
		}
				
		/* ///////////////////////////////////////////////フォーメーションデータを確認して的作成 */
		private void checkFormationData(){
				for(int i = 0; i < formationData.Count; i++){
						if(formationData[i].triggerFrame == objectFrame){
								makeEnemy (formationData[i]);	//make enemy
						}
				}
		}

		private void makeEnemy(lineData enemyData){
				Debug.Log ("enemyNo = " + enemyData.enemyNo);
				//9999を見つけたら削除
				if (enemyData.enemyNo == 9999) {
						delete ();
				} else {
						GameObject enemy = Instantiate (enemyPrefab[enemyData.enemyNo], this.transform.position, this.transform.rotation) as GameObject;
				}


		}

		private void init(TextAsset t){
				StringReader reader = new StringReader(t.text);
				while (reader.Peek() > -1) {
						string tmp = reader.ReadLine();
						Debug.Log (tmp);
						//ignor to find "//"
						if(tmp.Substring (0, 2) == "//"){
								continue;
						}
						//Break to find blank
						else if(tmp == ""){
								break;
						}
						//make list
						lineData line = new lineData (tmp);
						formationData.Add (line);
				}
			
		}

		/// <summary>
		/// 削除
		/// </summary>
		void delete(){
				Destroy(gameObject);
		}
}
