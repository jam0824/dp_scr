using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class FormationManager : MonoBehaviour {

		const int OBJECT_LIFE = 3600;	//オブジェクトの寿命
		public int objectFrame = 0;
		public GameObject[] enemyPrefab;
		public TextAsset formationFile;
		public List<FormationDataBean> formationData = new List<FormationDataBean>();

		// Use this for initialization
		void Start () {
				formationData = init (formationFile);
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

		private void makeEnemy(FormationDataBean enemyData){
				//Debug.Log ("enemyNo = " + enemyData.enemyNo);
				//9999を見つけたら削除
				if (enemyData.enemyNo == 9999) {
						delete ();
				} else {
						Vector3 pos = this.transform.position;
						pos.x += enemyData.x;
						pos.y += enemyData.y;
						GameObject enemy = Instantiate (enemyPrefab[enemyData.enemyNo], pos, this.transform.rotation) as GameObject;
						enemy.GetComponent<enemy> ().initEnemy (enemyData);
				}
		}

		/// <summary>
		/// 初期化。フォーメーションファイルを読み込む
		/// </summary>
		/// <param name="t">T.</param>
		List<FormationDataBean> init(TextAsset t){
				List<FormationDataBean> data = new List<FormationDataBean>();
				StringReader reader = new StringReader(t.text);
				while (reader.Peek() > -1) {
						string tmp = reader.ReadLine();
						//ignor to find "//"
						if(tmp.Substring (0, 2) == "//"){
								continue;
						}
						//Break to find blank
						else if(tmp == ""){
								break;
						}
						//make list
						FormationDataBean line = new FormationDataBean (tmp);
						data.Add (line);
				}
				return data;
		}

		/// <summary>
		/// 削除
		/// </summary>
		void delete(){
				Destroy(gameObject);
		}
}
