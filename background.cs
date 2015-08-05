using UnityEngine;
using System.Collections;

public class background : MonoBehaviour {

		public float DEFAULT_Y = 18.0f;
		public float DEFAULT_Z = 23.0f;

		public float RETURN_Y = -8.0f;
		public float RETURN_Z = -5.0f;


		public float speedY = 0.005f;
		public float speedZ = 0.005f;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
				Vector3 pos = this.transform.position;
				pos.y -= speedY;
				pos.z -= speedZ;
				this.transform.position = pos;

				if(this.transform.position.y <= RETURN_Y){
						pos.y = DEFAULT_Y;
						pos.z = DEFAULT_Z;
						this.transform.position = pos;
				}


	}
}
