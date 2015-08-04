using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
		public List<lineData> formationData = new List<lineData>();

		// Use this for initialization
		public void Create (string dataSource) {
			init (dataSource);
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

		private void init(string dataSource){
				string[] tmp = dataSource.Replace("¥n", "n").Split ("n"[0]);
				Debug.Log ("tmp=" + tmp.Length);
				for(int i = 0; i < tmp.Length; i++){
						Debug.Log ("i=" + i);
						//ignor to find "//"
						if(tmp [i].Substring (0, 2) == "//"){
								continue;
						}
						//Break to find blank
						else if(tmp[i] == ""){
								break;
						}
						//make list
						lineData line = new lineData (tmp[i]);
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
