using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WhiteOut : MonoBehaviour {

		Image image;
		float v = 0.01f;

		// Use this for initialization
		void Start () {
				image = GetComponent<Image> ();
		}
		
		// Update is called once per frame
		void Update () {
				float a = image.color.a;
				a += v;
				if (a <= 255) {
						image.color = new Color (255.0f, 255.0f, 255.0f, a);
				}
		
		}
}
