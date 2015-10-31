using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {

		public int HP = 1;
		public bool isDirAnime = false;
		public float startBulletTime = 0.0f;
		public GameObject bulletPrefab;
		public GameObject itemPrefab;

		public FormationDataBean data;

		public Bezier myBezier;
		private float t = 0.0f;


		GameManager gameManager;
		SoundManager soundManager;
		EffectManager effectManager;
		Common common;

		int maxHP = 1;

		Vector3 prevPos = new Vector3 (0,0,0);	//前回位置
		float stopY = 0;

		// Use this for initialization
		void Start () {
				maxHP = HP;
				//initEnemy (MoveMode);
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
				common = GameObject.Find("Common").GetComponent<Common>();

				//弾を打つ場合指定時間で発射
				if(startBulletTime != 0){
						Invoke ("makeBullet", startBulletTime);
				}
		}
		
		// Update is called once per frame
		void Update () {
				if(isDirAnime){
						enemyAnimation ();
				}
		}
		//固定時間呼び出し
		void FixedUpdate(){
				movement ();
		}
		//**********************************************************
		//初期化
		public void initEnemy(FormationDataBean d){
				data = d;
				Vector2 vec;
				switch(data.movement){

				case "Vector":
						//1番目のx,yのセットをベクトルの方向としてセット
						vec = new Vector2 (data.bezierParam[0], data.bezierParam[1]).normalized;	//単位ベクトル
						this.GetComponent<Rigidbody2D>().AddForce(vec * data.speed);
						//２番目のx,yのyをストップ座標としてセットする。
						if(data.bezierParam[3] != 0){
								//viewport座標で指定
								Vector3 tmpPos = Camera.main.ViewportToWorldPoint (new Vector3(
										0f, 
										data.bezierParam [3] / 100, 
										-1 * Camera.main.transform.position.z)	//カメラの高さを合わせないとずれる
								);
								stopY = tmpPos.y;
						}
						break;
				case "GravityRandom":
						Vector3 pos = transform.position;
						pos.x = (Random.value * 5) - 2.5f;
						this.transform.position = pos;
						break;
				case "Bezier":
						//Debug.Log ("bezier_x=" + transform.position.x);

						myBezier = new Bezier( new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), 
								new Vector3( this.transform.position.x + data.bezierParam[0], this.transform.position.y + data.bezierParam[1], 0f ), 
								new Vector3( this.transform.position.x + data.bezierParam[2], this.transform.position.y + data.bezierParam[3], 0f ), 
								new Vector3( this.transform.position.x + data.bezierParam[4], this.transform.position.y + data.bezierParam[5], 0f ) );

						break;


				}
		}

		private void movement(){
				switch(data.movement){

				case "Bezier":
						Vector3 vec = myBezier.GetPointAtTime( t );
						transform.position = vec;

						t += data.speed;
						if( t > 2f ) t = 0f;
						break;

				case "Vector":
						//stopYが設定している場合、座標が基準を超えたらストップさせる
						if((stopY != 0)&&(this.transform.position.y < stopY)){
								this.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
								stopY = 0;
						}
						break;
				}


		}

		private void enemyAnimation(){
				float x = this.transform.position.x - prevPos.x; //xの差分
				float y = this.transform.position.y - prevPos.y; //yの差分
				setAnimeTrigger (x, y);
				prevPos = this.transform.position;
		}

		//***********************************************
		//アニメーション
		void setAnimeTrigger(float x, float y){
				if((x == 0) && (y == 0)){
						return;
				}
				//単位ベクトルからの角度計算
				Vector2 vec = new Vector2 (x, y).normalized;
				float rot = common.vectorToAngle (vec);

				if((rot >= -45 ) && (rot < 45)){
						this.GetComponent<Animator>().SetTrigger("rightTrigger");
				}
				else if((rot >= 45 ) && (rot < 135)){
						this.GetComponent<Animator>().SetTrigger("backTrigger");
				}

				else if((rot >= 135 ) && (rot <= 180)){
						this.GetComponent<Animator>().SetTrigger("leftTrigger");
				}
				else if((rot <= -135 ) && (rot >= -180)){
						this.GetComponent<Animator>().SetTrigger("leftTrigger");
				}
				else if((rot > -135 ) && (rot < -45)){
						this.GetComponent<Animator>().SetTrigger("frontTrigger");
				}
		}

		//弾作成
		void makeBullet(){
				GameObject gs = Instantiate (bulletPrefab, this.transform.position, this.transform.rotation) as GameObject;
				gs.transform.parent = this.transform;	//親にする
		}

		//アイテム作成
		private void makeItem(int power){
				GameObject item = Instantiate (itemPrefab, this.transform.position, this.transform.rotation) as GameObject;

		}

		//bulletヒット時
		public void hitBullet(int damage){
				HP -= damage;
				checkHP ();
		}
		void checkHP(){
				if (HP <= 0) {
						//敵を倒す
						deleteEnemy ();
				}
		}
		//エフェクトは敵側で出す。
		public void makeEffect(string tag, Vector3 bulletPos){

				if (tag == "p_bullet"){
						//エフェクトはたまにだけにする
						if(HP > 5){
								int r = Random.Range (0,5);
								if (r != 0)
										return;
						}
						Vector3 pos = common.randomPos (bulletPos, 0.5f);
						effectManager.makeEffect ("middleExplosion", pos);
				}
				else if (tag == "missile") {
						Vector3 pos = common.randomPos (bulletPos, 0.5f);
						effectManager.makeEffect ("fireExplosion", pos);
						soundManager.playSE ("exp_missile");
				}
		}

		//if bullet go out of screen, delete it
		void OnTriggerEnter2D(Collider2D c){
				//デリートエリア到着で削除
				if(c.gameObject.tag == "delete_area"){
						Destroy(gameObject);
				}
		}

		void OnTriggerStay2D (Collider2D c){
				if(c.gameObject.tag == "bomb"){
						HP--;
						if(HP <= 0) deleteEnemy();

						Vector3 pos = common.randomPos (this.transform.position, 0.5f);
						effectManager.makeEffect ("middleExplosion", pos);

				}

		}



		/// <summary>
		/// Deletes the enemy.
		/// </summary>
		void deleteEnemy(){
				if (maxHP < 20) {
						soundManager.playSE ("exp_small");
						effectManager.makeEffect ("middleExplosion", this.transform.position);
				} else {
						soundManager.playSE ("exp_big");
						effectManager.makeEffect ("bigExplosion", this.transform.position);

				}
				makeItem (gameManager.power);
				Destroy(gameObject);
		}

}
