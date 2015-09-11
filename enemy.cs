using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {

		const int POWER_MAX = 5;
		public int HP = 1;
		public bool isDirAnime = false;
		public float startBulletTime = 0.0f;
		public GameObject bulletPrefab;
		public GameObject itemPrefab;
		public GameObject subItemPrefab;

		public FormationDataBean data;

		public Bezier myBezier;
		private float t = 0.0f;


		GameManager gameManager;
		SoundManager soundManager;
		EffectManager effectManager;

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
				//弾を打つ場合指定時間で発射
				if(startBulletTime != 0){
						Invoke ("makeBullet", startBulletTime);
				}
		}
		
		// Update is called once per frame
		void Update () {

			
				movement ();
		}


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
								stopY = data.bezierParam [3];
						}
						break;
				case "GravityRandom":
						Vector3 pos = transform.position;
						pos.x = (Random.value * 5) - 2.5f;
						this.transform.position = pos;
						break;
				case "Bezier":
						//Debug.Log ("bezier_x=" + transform.position.x);
						myBezier = new Bezier( new Vector3(transform.position.x, transform.position.y, transform.position.z), 
								new Vector3( transform.position.x + data.bezierParam[0], transform.position.y + data.bezierParam[1], 0f ), 
								new Vector3( transform.position.x + data.bezierParam[2], transform.position.y + data.bezierParam[3], 0f ), 
								new Vector3( transform.position.x + data.bezierParam[4], transform.position.y + data.bezierParam[5], 0f ) );
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
								//this.GetComponent<Rigidbody2D> ().angularVelocity = Vector3.zero;
								stopY = 0;
						}
						break;
				}

				if(isDirAnime){
						float x = this.transform.position.x - prevPos.x; //xの差分
						float y = this.transform.position.y - prevPos.y; //yの差分
						setAnimeTrigger (x, y);
						prevPos = this.transform.position;
				}
		}

		void setAnimeTrigger(float x, float y){
				if((x == 0) && (y == 0)){
						return;
				}
				//単位ベクトルからの角度計算
				Vector2 vec = new Vector2 (x, y).normalized;
				float rot = new Common ().vectorToAngle (vec);

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
				if(power < POWER_MAX){

						GameObject item = Instantiate (itemPrefab, this.transform.position, this.transform.rotation) as GameObject;
				}else{
						GameObject item = Instantiate (subItemPrefab, this.transform.position, this.transform.rotation) as GameObject;
				}
		}


		//if bullet go out of screen, delete it
		void OnTriggerEnter2D(Collider2D c){
				if((c.gameObject.tag == "p_bullet") || (c.gameObject.tag == "bomb")){
						HP--;
						if(HP <= 0) deleteEnemy();
						if(c.gameObject.tag == "p_bullet"){
								Destroy (c.gameObject);
								Vector3 pos = c.transform.position;
								float v = 0.5f;
								pos.x += (Random.value * v) - v / 2;
								pos.y += (Random.value * v) - v / 2;
								effectManager.makeEffect ("middleExplosion", pos);

						}
				}
				//デリートエリア到着で削除
				if(c.gameObject.tag == "delete_area"){
						Destroy(gameObject);
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
