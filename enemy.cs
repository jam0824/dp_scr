using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {

	private int HP = 1;
	private float shotDelay = 5f;
	private float bullet_speed = 0.7f;
	public GameObject etama;
	private GameObject player;


	// Use this for initialization
	void Start () {
		player = GameObject.Find ("player");
		StartCoroutine("enemyShot");

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator enemyShot(){
		while (true) {

			//making bullets
			float angle = getAngle(player.transform.position, this.transform.position);
			makeEnemyShot(angle, bullet_speed, false);

			yield return new WaitForSeconds (shotDelay);
		}
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
		GameObject shot = Instantiate (etama, this.transform.position, this.transform.rotation) as GameObject;
		bullet s = shot.GetComponent<bullet>();
		s.Create(direction, speed, rotationTrigger);
	}
}
