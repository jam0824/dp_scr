using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {

		const int POWER_MAX = 5;
		public int HP = 1;
		public GameObject explosion01;  //Big explosion
		public GameObject explosion04;  //very small explosion
		public GameObject itemPrefab;
		public GameObject subItemPrefab;


		public FormationDataBean data;

		public Bezier myBezier;
		private float t = 0.0f;


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

			
				movement ();
		}


		public void initEnemy(FormationDataBean d){
				data = d;
				Vector2 vec;
				switch(data.movement){
				case "GoDown":
						vec = new Vector2 (0f, -1f).normalized;	//単位ベクトル

						this.GetComponent<Rigidbody2D>().AddForce(vec * data.speed);	//下に力を加える
						break;
				case "GoRight":

						vec = new Vector2 (1f, 0f).normalized;	//単位ベクトル
						this.GetComponent<Rigidbody2D>().AddForce(vec * data.speed);
						break;
				case "GoLeft":
						vec = new Vector2 (-1f, 0f).normalized;	//単位ベクトル
						this.GetComponent<Rigidbody2D>().AddForce(vec * data.speed);
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
