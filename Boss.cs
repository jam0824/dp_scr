﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Boss : MonoBehaviour {

		const float START_POSISION_Y = 1.0f;
		const int gameFPS = 60;

		public List<GameObject> gurdSkills;
		public List<GameObject> normalSkills;
		public GameObject gurdSkillBG;
		public int MAX_HP = 0;
		public int HP = 0;
		public int gameFrame = 0;
		public GameObject lifebarPrefab;
		public TextAsset textAset;

		bool setStartPosition = false;
		bool isDefeated = false;

		int sessionNo = -1;
		RectTransform lifebar;
		GameObject activeGurdSkill;
		GameManager gameManager;
		List<BossData> bossData = new List<BossData>();

		//ボス行動パターンデータ
		class BossData{
				public int sessionNo = 0;
				public int startHP = 90;
				public string patternName = "";
				public int bombNo = 0;
		}



		//*******************************************************************************
		// Use this for initialization
		void Start () {
				bossData = fileRead (textAset);
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				makeLifeBar ();
		}
		
		// Update is called once per frame
		void Update () {
				//最初の登場シーン
				if(!setStartPosition){
						startingMoving ();

				}else{
						if (HP > 0) {
								movement (gameFrame);
						}
				}
				if((setStartPosition) && (!isDefeated)) checkHP ();
				drawLifeBar ();
				gameFrame++;

		}

		//******************************************************************************
		//最初の移動
		void startingMoving(){
				transform.Translate (0, -0.01f, 0);
				if(transform.position.y <= START_POSISION_Y){

						setStartPosition = true;
				}
		}

		//ふわふわ移動
		void movement(int frame){
				float floatTime = 0.01f;
				float swingTime = 6f;
				float sideFloatTime = 0.005f;
				float sideSwingTime = 3f;
				transform.position = new Vector3(-0.4f + Mathf.Cos (frame * sideFloatTime )/ sideSwingTime
						, 1 + Mathf.Sin (frame * floatTime )/ swingTime,transform.position.z);
		}
		//*********************************************************
		/// <summary>
		/// ヒットポイントチェック
		/// </summary>
		void checkHP(){
				//HPがゼロでクリア
				if (HP <= 0) {
						defeatedBoss ();
						return;
				}
				//ボム切り替えトリガー
				//Debug.Log ("sessionNo = " + bossData[0].sessionNo);
				//Debug.Log ("Hp = " + bossData[0].startHP);
				if(((HP * 100 / MAX_HP) <= bossData[0].startHP) && (sessionNo < bossData[0].sessionNo)){
						sessionNo = bossData [0].sessionNo;
						changeBomb (bossData[0]);
						bossData.RemoveAt (0);	//上から順に実行。実行された行は削除
				}
		}

		/// <summary>
		/// データからボム作成
		/// </summary>
		/// <param name="data">Data.</param>
		void changeBomb(BossData data){
				//弾幕発動中なら削除
				if (activeGurdSkill != null) {
						if (activeGurdSkill.tag == "gurdSkill") {
								deleteGurdSkill (activeGurdSkill);
								gameManager.makeBG ();	//通常背景復帰
								deleteGurdSkillBG ();	//弾幕背景削除
						} else {
								deleteGurdSkill (activeGurdSkill);
						}
				}

				switch(data.patternName){
				//弾幕時
				case "bomb":
						activeGurdSkill = makeGurdSkill (data.bombNo);
						makeGurdSkillBG ();
						gameManager.deleteBG ();
						break;
				//通常
				case "normal":
						activeGurdSkill = makeNormalSkill (data.bombNo);
						break;
				}
		}


		/// <summary>
		/// ボスを倒した後
		/// </summary>
		void defeatedBoss(){
				isDefeated = true;
				deleteGurdSkill (activeGurdSkill);
				deleteGurdSkillBG ();
				gameManager.finishGame ();
		}

		//******************************************************************************
		/// <summary>
		/// Makes the gurd skill.
		/// </summary>
		/// <returns>The gurd skill.</returns>
		/// <param name="gurdSkillNumber">Gurd skill number.</param>
		GameObject makeGurdSkill(int gurdSkillNumber){
				Vector3 pos = this.transform.position;
				pos.x += 0.6f;
				pos.y += 0.5f;
				GameObject gs = Instantiate (gurdSkills[gurdSkillNumber], pos, this.transform.rotation) as GameObject;
				gs.transform.parent = this.transform;	//Bossオブジェクト親にする

				return gs;
		}
		//******************************************************************************
		/// <summary>
		/// Makes the gurd skill.
		/// </summary>
		/// <returns>The gurd skill.</returns>
		/// <param name="gurdSkillNumber">Gurd skill number.</param>
		GameObject makeNormalSkill(int normalSkillNumber){
				Vector3 pos = this.transform.position;
				pos.x += 0.6f;
				pos.y += 0.5f;
				GameObject gs = Instantiate (normalSkills[normalSkillNumber], pos, this.transform.rotation) as GameObject;
				gs.transform.parent = this.transform;	//Bossオブジェクト親にする

				return gs;
		}

		/// <summary>
		/// Deletes the gurd skill.
		/// </summary>
		/// <param name="gs">Gs.</param>
		void deleteGurdSkill(GameObject gs){
				Destroy (gs);
		}

		// /////////////////////////////////////////////
		/// <summary>
		/// 弾幕時背景作成。
		/// </summary>
		void makeGurdSkillBG(){
				float y = 0.0f;
				float v = 30.0f;
				int num = 2;
				Vector3 pos = new Vector3 (0.0f, y, 0.0f);

				for(int i = 0; i < num; i++){
						GameObject prefab = Instantiate (gurdSkillBG) as GameObject;
						prefab.transform.position = pos;
						if (i % 2 == 1) {
								prefab.transform.Rotate (180.0f,0.0f,0.0f);
						}
						pos.y += v;
				}

		}

		void deleteGurdSkillBG(){
				GameObject[] bg = GameObject.FindGameObjectsWithTag ("gurdSkillBG");
				foreach(GameObject x in bg){
						Destroy (x);
				}
		}




		//**********************************************************
		//UI系
		void makeLifeBar(){
				int x = 0;
				int y = -40;
				GameObject prefab = Instantiate (lifebarPrefab, this.transform.position, this.transform.rotation) as GameObject;
				prefab.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				lifebar = prefab.GetComponent<RectTransform>();
				lifebar.localScale = new Vector3 (2, 2, 1);	//スケールを元に戻す
				lifebar.anchoredPosition = new Vector2(x, y);	//位置変更
		}

		void drawLifeBar(){
				int w = 400;
				int h = 23;
				lifebar.sizeDelta = new Vector2 ((int)Mathf.Floor(w * HP / MAX_HP), h);
		}

		//**************************************************************
		//ボスの行動データを読み込む
		List<BossData> fileRead(TextAsset t){
				List<BossData> returnValue = new List<BossData>();
				StringReader reader = new StringReader(t.text);

				while (reader.Peek() > -1) {
						string line = reader.ReadLine();
						string[] values = line.Split(',');
						BossData data = new BossData ();
						data.sessionNo = int.Parse (values[0]);
						data.startHP = int.Parse (values[1]);
						data.patternName = values [2];
						data.bombNo = int.Parse (values[3]);
						returnValue.Add (data);
				}

				return returnValue;
		}
}
