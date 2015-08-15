using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {

		const int POWER_MAX = 5;
		public int HP = 1;
		public GameObject explosion01;  //Big explosion
		public GameObject explosion04;  //very small explosion
		public string MoveMode = "GoDown";
		public GameObject itemPrefab;
		public GameObject subItemPrefab;

		
		public Bezier myBezier;
		private float t = 0.0f;

		public Vector3 p1;
		public Vector3 p2;
		public Vector3 p3;

		GameManager gameManager;
		SoundManager soundManager;

		// Use this for initialization
		void Start () {
			
				//initEnemy (MoveMode);
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		}
		
		// Update is called once per frame
		void Update () {

			
				movement (MoveMode);
		}


		public void initEnemy(string mode){
				Vector2 vec;
				float speed = 20.0f;
				switch(mode){
				case "GoDown":
						vec = new Vector2 (0f, -1f).normalized;	//単位ベクトル

						this.GetComponent<Rigidbody2D>().AddForce(vec * speed);	//下に力を加える
						break;
				case "GoRight":

						vec = new Vector2 (1f, 0f).normalized;	//単位ベクトル
						this.GetComponent<Rigidbody2D>().AddForce(vec * speed);
						break;
				case "GoLeft":
						vec = new Vector2 (-1f, 0f).normalized;	//単位ベクトル
						this.GetComponent<Rigidbody2D>().AddForce(vec * speed);
						break;
				case "StopAndGo":
						Debug.Log ("bezier_x=" + transform.position.x);
						myBezier = new Bezier( new Vector3(transform.position.x, transform.position.y, transform.position.z), 
								new Vector3( transform.position.x, transform.position.y - 6.0f, 0f ), 
								new Vector3( transform.position.x, transform.position.y - 3.0f, 0f ), 
								new Vector3( transform.position.x, transform.position.y, 0f ) );
						break;


				}
				MoveMode = mode;
		}

		private void movement(string mode){
				switch(mode){

				case "StopAndGo":
						float v = 0.002f;
						Vector3 vec = myBezier.GetPointAtTime( t );
						transform.position = vec;

						t += v;
						if( t > 2f ) t = 0f;
						break;


				}

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
