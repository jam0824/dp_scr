using UnityEngine;
using System.Collections;

public class DeleteArea : MonoBehaviour {

		//if bullet go out of screen, delete it
		void OnTriggerEnter2D(Collider2D c){
				if(c.gameObject.tag == "delete_area"){
						Destroy(gameObject);
				}
		}
}
