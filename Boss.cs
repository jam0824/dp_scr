using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Boss : MonoBehaviour {

		const float BULLET_MARGIN = 2f;
		const float START_POSISION_Y = 1.0f;
		const int gameFPS = 60;

		public List<GameObject> gurdSkills;
		public List<GameObject> normalSkills;
		public List<GameObject> labelGuardSkillName;
		public GameObject gurdSkillBG;
		public int MAX_HP = 0;
		public int HP = 0;
		public int gameFrame = 0;
		public GameObject lifebarPrefab;
		public GameObject lifebarWakuPrefab;
		public TextAsset textAset;

		public bool setStartPosition = false;
		bool isDefeated = false;
		bool isChangeFace = false;

		int sessionNo = -1;	//現在どこのセッションの行動を行っているか
		int bulletMargin = 0;	//弾幕発生までのマージン。０以上が設定されていると待ち時間を作る
		RectTransform lifebar;
		GameObject activeGurdSkill = null;
		GameManager gameManager;
		SoundManager soundManager;
		EffectManager effectManager;
		Common common;
		List<BossData> bossData = new List<BossData>();


		//ボス行動パターンデータ
		class BossData{
				public int sessionNo = 0;
				public int startHP = 90;
				public string patternName = "";
				public int bombNo = 0;
		}


		void Awake(){
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
				common = GameObject.Find("Common").GetComponent<Common>();
		}


		//*******************************************************************************
		// Use this for initialization
		void Start () {
				bossData = fileRead (textAset);

				makeLifeBar ();
		}
		
		// Update is called once per frame
		void Update () {
				if((setStartPosition) && (!isDefeated)) checkHP ();
				drawLifeBar ();
				gameFrame++;

		}

		void FixedUpdate(){
				//最初の登場シーン
				if(!setStartPosition){
						startingMoving ();

				}else{
						if (HP > 0) {
								movement (gameFrame);
						}
						if((HP < (MAX_HP * 0.3f)) && (!isChangeFace)){
								changeFace ();	//てれ表情に変更
						}
				}

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

		//表情変え
		void changeFace(){
				this.GetComponent<Animator>().SetTrigger("tereTrigger");
				isChangeFace = true;
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
				if(bossData.Count > 0){
					if(((HP * 100 / MAX_HP) <= bossData[0].startHP) && (sessionNo < bossData[0].sessionNo)){
							sessionNo = bossData [0].sessionNo;
							changeBomb (bossData[0]);
							bossData.RemoveAt (0);	//上から順に実行。実行された行は削除
					}
				}
		}

		/// <summary>
		/// データからボム作成
		/// </summary>
		/// <param name="data">Data.</param>
		void changeBomb(BossData data){
				//弾幕発動中なら削除
				if (activeGurdSkill != null) {
						deleteGurdSkill (activeGurdSkill);
						if (activeGurdSkill.tag == "gurdSkill") {
								gameManager.makeBG ();	//通常背景復帰
								deleteGurdSkillBG ();	//弾幕背景削除
								soundManager.playSE ("exp_big");
						} 
				}

				switch(data.patternName){
				//弾幕時
				case "bomb":
						activeGurdSkill = gurdSkills [data.bombNo];	//弾幕予約
						makeGurdSkillBG ();
						gameManager.deleteBG ();
						soundManager.playSE ("danmaku");
						makeLabelGuardSkillName (data.bombNo);
						Invoke ("invokeBomb", BULLET_MARGIN);
						break;
				//通常
				case "normal":
						activeGurdSkill = normalSkills[data.bombNo];//弾幕予約
						Invoke ("invokeBomb", BULLET_MARGIN);
						break;
				}
		}

		//*******
		//次の弾幕までのマージン計算
		void invokeBomb(){
				activeGurdSkill = makeGurdSkill (activeGurdSkill);
		}

		/// <summary>
		/// ガードスキルのラベル表示
		/// </summary>
		/// <param name="guardSkillNo">Guard skill no.</param>
		void makeLabelGuardSkillName(int guardSkillNo){
				GameObject label = Instantiate (labelGuardSkillName[guardSkillNo]) as GameObject;
				label.transform.parent = GameObject.Find("statusView").transform;
				label.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
		}

		/// <summary>
		/// ボスを倒した後
		/// </summary>
		void defeatedBoss(){
				isDefeated = true;
				deleteGurdSkill (activeGurdSkill);
				//R18モードと区別
				if (gameManager.isR18Mode) {
						Invoke ("waitFinish",3f);	//ちょっと見せ時間を儲ける
				} else {
						Invoke ("finishGame",2f);	//ちょっと見せ時間を儲ける
				}

		}

		//コールちんを呼び出すためだけ
		void waitFinish(){
				StartCoroutine (makeSmall (0.003f));
		}

		//コールチンで縮小
		public IEnumerator makeSmall(float v) {
				//無限ループ防止
				for (int i = 0; i < 1000; i++) {
						Vector3 scale = this.transform.localScale;
						scale.x -= v;
						scale.y -= v;
						this.transform.localScale = scale;
						if(scale.x < 0.1f){
								finishGame ();
								yield break;
						}
						yield return new WaitForSeconds (0.016f);

				}
				yield break;
		}

		//終了する
		void finishGame(){
				//deleteGurdSkillBG ();
				gameManager.finishGame ();
		}

		//******************************************************************************
		/// <summary>
		/// Makes the gurd skill.
		/// </summary>
		/// <returns>The gurd skill.</returns>
		/// <param name="gurdSkillNumber">Gurd skill number.</param>
		GameObject makeGurdSkill(GameObject bullet){
				Vector3 pos = this.transform.position;
				pos.x += 0.6f;
				pos.y += 0.5f;
				GameObject gs = Instantiate (bullet, pos, this.transform.rotation) as GameObject;
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
				float v = 9.0f;
				int num = 2;
				Vector3 pos = new Vector3 (0.0f, y, 0.0f);

				for(int i = 0; i < num; i++){
						GameObject prefab = Instantiate (gurdSkillBG) as GameObject;
						prefab.transform.position = pos;
						pos.y += v;
				}

		}

		void deleteGurdSkillBG(){
				GameObject[] bg = GameObject.FindGameObjectsWithTag ("gurdSkillBG");
				foreach(GameObject x in bg){
						effectManager.flash ();
						Destroy (x);
				}
		}




		//**********************************************************
		//UI系
		void makeLifeBar(){
				int x = 0;
				int y = 0;
				GameObject prefabWaku = Instantiate (lifebarWakuPrefab, this.transform.position, this.transform.rotation) as GameObject;
				prefabWaku.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				RectTransform waku = prefabWaku.GetComponent<RectTransform>();
				waku.localScale = new Vector3 (2, 2, 1);	//スケールを元に戻す
				waku.anchoredPosition = new Vector2(x, y);	//位置変更

				GameObject prefab = Instantiate (lifebarPrefab, this.transform.position, this.transform.rotation) as GameObject;
				prefab.transform.parent = prefabWaku.transform;	//枠を親にする
				lifebar = prefab.GetComponent<RectTransform>();
				lifebar.localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
				lifebar.anchoredPosition = new Vector2(0, 0);	//位置変更

				StartCoroutine (common.moveUI(waku, -1f, -40f, 0.01f));	//コールチンでバー移動
		}
				
		//バーを再描画
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
						if ((values [0] == "")||(line.Substring (0, 2) == "//"))
								continue;
						
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
