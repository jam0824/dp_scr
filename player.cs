using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

	public GameObject prefab;
	private int speed = 200;
	private float moveOffset = 0.04f;
	private float shotDelay = 0.05f;


	// Use this for initialization
	void Start () {
		StartCoroutine("Shot");
	}
	
	// Update is called once per frame
	void Update () {
		movePlayerToTouch ();


	}

	//player movement to use touching
	private void movePlayerToTouch(){
		if (Input.GetMouseButton(0)){
			Vector3 screen_vec = Input.mousePosition;	//Get toch points (screen)
			screen_vec.z = 4.0f;
			
			//Exchange screenPotison to world position
			Vector3 world_vec = Camera.main.ScreenToWorldPoint(screen_vec);
			Vector3 nVec = Vector3.Normalize(world_vec - this.transform.position);

			//if distance so close between mouse and player, don't move it;
			if((Mathf.Abs(world_vec.x - this.transform.position.x) < moveOffset) && 
			   (Mathf.Abs(world_vec.y - this.transform.position.y) < moveOffset)){
			}
			else{
				this.rigidbody2D.AddForce(nVec * speed);
			}
			
		}
		return;
	}


	private IEnumerator Shot(){
		while (true) {
			makeShot (90.0f, 10.0f);
			yield return new WaitForSeconds (shotDelay);
		}
	}


	//Making one bullet object
	private void makeShot(float direction, float speed){
		GameObject shot = Instantiate (prefab, this.transform.position, this.transform.rotation) as GameObject;
		player_wepon01 s = shot.GetComponent<player_wepon01>();
		s.Create(direction, speed);
	}
}
