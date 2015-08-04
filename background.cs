using UnityEngine;
using System.Collections;

public class background : MonoBehaviour {

		const float DEFAULT_Y = 18.0f;
		const float DEFAULT_Z = 23.0f;

		const float RETURN_Y = -8.0f;
		const float RETURN_Z = -5.0f;


		private float speed = 0.005f;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
				Vector3 pos = this.transform.position;
				pos.y -= speed;
				pos.z -= speed;
				this.transform.position = pos;

				if(this.transform.position.y <= RETURN_Y){
						pos.y = DEFAULT_Y;
						pos.z = DEFAULT_Z;
						this.transform.position = pos;
				}


	}
}
