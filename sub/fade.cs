using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class fade : MonoBehaviour {

		public int type = 0;	//0 : fadein / 1: fadeout / 2: keep
		public float v = 0.02f;

		int gameCount = 0;

		public float r = 0.0f;
		public float g = 0.0f;
		public float b = 0.0f;

		Image image;
		// Use this for initialization
		void Start () {
				this.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				image = GetComponent<Image> ();
		}

		//初期化
		public void Init(int fadeType, int fadeFrame, float red, float green, float blue){
				type = fadeType;
				v = 1.0f / fadeFrame;
				image = GetComponent<Image> ();
				r = red;
				g = green;
				b = blue;
				if (fadeType == 0) {
						GetComponent<Image> ().color = new Color (r, g, b, 1.0f);
				} else {
						GetComponent<Image> ().color = new Color (r, g, b, 0.0f);
				}

		}


		// Update is called once per frame
		void Update () {
				if(type == 0){
						float a = image.color.a;
						a -= v;
						if (a < 0.0f) {
								a = 0f;
								delete ();
						} else {
								image.color = new Color (r, g, b, a);
						}

				}
				else if(type == 1){
						float a = image.color.a;

						a += v;
						if (a > 1.0f) {
								a = 1.0f;
								type = 2;
						}
						image.color = new Color (r, g, b, a);
				}
				gameCount++;
		}

		public void delete(){
				Destroy(gameObject);
		}
}
