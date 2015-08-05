using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour {

		const float START_POSISION_Y = 1.0f;
		const int gameFPS = 60;

		public List<GameObject> gurdSkills;
		public GameObject gurdSkillBG;
		public int MAX_HP = 0;
		public int HP = 0;
		public int gameFrame = 0;
		public GameObject lifebarPrefab;
		bool setStartPosition = false;
		bool isDefeated = false;

		RectTransform lifebar;
		GameObject activeGurdSkill;
		GameManager gameManager;

		// Use this for initialization
		void Start () {
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				makeLifeBar ();
		}
		
		// Update is called once per frame
		void Update () {
				//最初の登場シーン
				if(!setStartPosition){
						startingMoving ();

				}else{
						if(HP > 0)
							movement (gameFrame);
				}
				if(!isDefeated) checkHP ();
				drawLifeBar ();
				gameFrame++;

		}

		//******************************************************************************
		//最初の移動
		void startingMoving(){
				transform.Translate (0, -0.01f, 0);
				if(transform.position.y <= START_POSISION_Y){
						activeGurdSkill = makeGurdSkill (0);
						makeGurdSkillBG ();
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
		//******************************************************************************
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

		//*********************************************************
		/// <summary>
		/// ヒットポイントチェック
		/// </summary>
		void checkHP(){
				if (HP <= 0) {
						defeatedBoss ();
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
}
