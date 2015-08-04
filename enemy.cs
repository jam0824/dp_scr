using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {

		const int POWER_MAX = 5;
		public int HP = 1;
		public GameObject explosion01;  //Big explosion
		public GameObject explosion04;  //very small explosion
		public string MoveMode = "Go right";
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
			
				initEnemy (MoveMode);
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
				soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		}
		
		// Update is called once per frame
		void Update () {

			
			
		}


		private void initEnemy(string mode){
				Vector2 vec;
				float speed = 20.0f;
				Debug.Log (mode);
				switch(mode){
				case "Go down":
						vec = new Vector2 (0f, -1f).normalized;	//単位ベクトル

						this.GetComponent<Rigidbody2D>().AddForce(vec * speed);	//下に力を加える
						break;
				case "Go right":

						vec = new Vector2 (1f, 0f).normalized;	//単位ベクトル
						this.GetComponent<Rigidbody2D>().AddForce(vec * speed);
						break;
				case "Go left":
						vec = new Vector2 (-1f, 0f).normalized;	//単位ベクトル
						this.GetComponent<Rigidbody2D>().AddForce(vec * speed);
						break;
				case "Bezier" :
						myBezier = new Bezier( new Vector3( this.transform.position.x, this.transform.position.y, this.transform.position.z), 
								new Vector3( this.transform.position.x + 1f, this.transform.position.y - 4.0f, 0f ), 
								new Vector3( this.transform.position.x + 2f, this.transform.position.y - 4.0f, 0f ), 
								new Vector3( this.transform.position.x + 3f, this.transform.position.y, 0f ) );
						break;


				}

		}

		private void movement(string mode){
				switch(mode){

				case "Bezier" :
						Vector3 vec = myBezier.GetPointAtTime( t );
						transform.position = vec;

						t += 0.005f;
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
