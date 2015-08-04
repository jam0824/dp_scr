using UnityEngine;
using System.Collections;

public class effectsController : MonoBehaviour {

		GameManager gameManager;

		// Use this for initialization
		void Start () {
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void bombFlagOn(){
				gameManager.bombOn ();
		}
		void bombFlagOff(){
				gameManager.bombOff ();
		}

		void OnAnimationFinish ()
		{
			Destroy (gameObject);
		}
}
