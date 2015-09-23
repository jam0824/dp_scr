using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButton : MonoBehaviour {


		public GameObject nextPrefab;
		EffectManager effectManager;
		bool isClick = false;

		// Use this for initialization
		void Start () {
				effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
		}
	
		// Update is called once per frame
		void Update () {

		}

		public void changeScene(){
				if (!isClick) {
						effectManager.changeScene ("black", nextPrefab, GameObject.Find("TJ_logo").transform.position);
						isClick = true;
				}
		}


				
}
