using UnityEngine;
using System.Collections;

public class background : MonoBehaviour {
	private float speed = 0.01f;
	private float count = 0f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = this.transform.position;
		pos.y -= speed;
		this.transform.position = pos;
		count += speed;

		if(this.transform.position.y <= -9.6f){
			pos.y = 9.6f;
			this.transform.position = pos;
		}


	}
}
