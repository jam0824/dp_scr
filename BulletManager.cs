using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {
	public GameObject etama;
	private GameObject player;
	private int count = 0;

	public int bullet_num = 5;
	public float bullet_speed = 0.7f;
	public float bullet_space = 30.0f;
	public int INTERVAL = 200;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("player");

	}
	
	// Update is called once per frame
	void Update () {
	
		count++;
		if(count == INTERVAL){
			count = 0;
			enemyShot(bullet_num, bullet_space, bullet_speed);
		}
	}


	//Making enemy shots.
	// @param bullet_num : Number of bullets
	// @param bullet_space : Angle between bullet and bullet
	// @param bullet_speed : Speed of bullet per sec
	private void enemyShot(int bullet_num, float bullet_space, float bullet_speed){

		float angle = getAngle(player.transform.position, this.transform.position) - 90f;
		float start_pos = angle;

		if(bullet_num % 2 == 0){
			start_pos = angle - (bullet_space / 2) - (bullet_space * (bullet_num / 2 - 1));

		}
		else if(bullet_num % 2 == 1){
			start_pos = angle - (bullet_space * Mathf.Floor(bullet_num / 2));
		}
		makeBullets(angle, start_pos, bullet_num, bullet_space, bullet_speed);
			
	}

	
	//Case odd bullets
	// @param angle : player's direction
	// @param start_pos : 
	// @param bullet_num : Number of bullets
	// @param bullet_space : Angle between bullet and bullet
	// @param bullet_speed : Bullet speed per sec
	private void makeBullets(float angle, float start_pos, int bullet_num, float bullet_space, float bullet_speed){
		//making bullets



		for(int i = 0; i < bullet_num; i++){
			transform.rotation = Quaternion.Euler(0, 0, start_pos);
			makeEnemyShot(angle, bullet_speed, false);
			start_pos += bullet_space;
		}

		return;
	}

	// It is return direction
	// @param goal : player position
	// @param start : enemy position
	private float getAngle(Vector3 goal, Vector3 start){
		float rad = Mathf.Atan2(goal.y - start.y, goal.x - start.x);
		return rad * Mathf.Rad2Deg;
	}
	
	//Making one bullet object
	// @param direction : angle to shot bullet
	// @param speed : speed per sec
	// @param rotationTrigger : It's switch to rotate image
	private void makeEnemyShot(float direction, float speed, bool rotationTrigger){
		GameObject shot = (GameObject)Instantiate (etama, this.transform.position, this.transform.rotation);
		bullet s = shot.GetComponent<bullet>();

		s.Create (transform.up, direction, speed, rotationTrigger);

	}

}
