using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {

		const int POWER_MAX = 5;
		public int HP = 1;
		public bool isDirAnime = false;
		public float startBulletTime = 0.0f;
		public GameObject bulletPrefab;
		public GameObject explosion01;  //Big explosion
		public GameObject explosion04;  //very small explosion
		public GameObject itemPrefab;
		public GameObject subItemPrefab;

		public FormationDataBean data;

		public Bezier myBezier;
		private float t = 0.0f;


		GameManager gameManager;
		SoundManager soundManager;

		Vector3 prevPos = new Vector3 (0,0,0);	//前回位置
		public string direction = "triggerFront";

		// Use this for initialization
		void Start () {
			
				//initEnemy (MoveMode);
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
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
						vec = new Vector2 (data.bezierParam[0], data.bezierParam[1]).normalized;	//単位ベクトル
						this.GetComponent<Rigidbody2D>().AddForce(vec * data.speed);
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
				}

				if(isDirAnime){
						float x = this.transform.position.x - prevPos.x;
						float y = this.transform.position.y - prevPos.y;
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
						Debug.Log ("ここきちゃだめ");
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
				if(c.gameObject.tag == "p_bullet"){
						HP--;
						if(HP <= 0) deleteEnemy();
						Destroy (c.gameObject);
						GameObject e = Instantiate (explosion04, this.transform.position, this.transform.rotation) as GameObject;
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
				soundManager.playSE ("exp_small");
				makeItem (gameManager.power);
				GameObject e = Instantiate (explosion01, this.transform.position, this.transform.rotation) as GameObject;
				Destroy(gameObject);
		}

}
