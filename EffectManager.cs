using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour {

		public GameObject fadePrefab;
		public GameObject flashPrefab;
		public GameObject smallExplosion;
		public GameObject middleExplosion;
		public GameObject bigExplosion;
		public GameObject hitSperk;
		public GameObject fireExplosion;

		GameObject fadein;
		GameObject nextPrefab;
		string fadeType = "black";
		Vector3 prefabPos;

		// Use this for initialization
		void Start () {
	
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		//ゲームオブジェクトを作る
		public GameObject makeEffect(string effectType, Vector3 pos){
				GameObject obj = Instantiate (getPrefab(effectType), pos, this.transform.rotation) as GameObject;
				return obj;
		}

		//文字列できたタイプからプレファブを返す
		GameObject getPrefab(string effectType){
				switch(effectType){
				case "smallExplosion":
						return smallExplosion;
						break;

				case "middleExplosion":
						return middleExplosion;
						break;
				case "bigExplosion":
						return bigExplosion;
						break;
				case "hitSperk":
						return hitSperk;
						break;
				case "fireExplosion":
						return fireExplosion;
						break;
				default :
						return middleExplosion;
						break;
				}
		}

		/// <summary>
		/// Changes the scene.
		/// </summary>
		/// <param name="type">Fade type : black or white</param>
		/// <param name="nextScene">Next scene prefab</param>
		/// <param name="pos">view position</param>
		public void changeScene(string type, GameObject nextScene, Vector3 pos){
				fadeType = type;
				prefabPos = pos;
				if (fadeType == "white") {
						fadein = new Common ().makeFade (fadePrefab, this.gameObject, 1, 60, 255.0f, 255.0f, 255.0f);
				} else {
						fadein = new Common ().makeFade (fadePrefab, this.gameObject, 1, 60, 0.0f, 0.0f, 0.0f);
				}
				nextPrefab = nextScene;
				Invoke ("changeNextScene", 1f);

		}
		private void changeNextScene(){
				Destroy (fadein);
				GameObject scene = Instantiate (nextPrefab, prefabPos, this.transform.rotation) as GameObject;
				scene.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				scene.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
				if (fadeType == "white") {
						fadein = new Common ().makeFade (fadePrefab, this.gameObject, 0, 60, 255.0f, 255.0f, 255.0f);
				} else {
						fadein = new Common ().makeFade (fadePrefab, this.gameObject, 0, 60, 0.0f, 0.0f, 0.0f);
				}
		}

		/// <summary>
		/// フェード処理を作成
		/// </summary>
		/// <param name="fadePrefab">Fade prefab.</param>
		/// <param name="basePrefab">Base prefab.</param>
		/// <param name="type">Type.</param>
		/// <param name="time">Time.</param>
		/// <param name="r">The red component.</param>
		/// <param name="g">The green component.</param>
		/// <param name="b">The blue component.</param>
		public GameObject makeFade(string type, int time, float r, float g, float b){
				GameObject fadein = Instantiate (fadePrefab, this.transform.position, this.transform.rotation) as GameObject;
				int x = (type == "transToColor") ? 1 : 0;
				fadein.GetComponent<fade> ().Init (x, time, r, g, b);
				RectTransform rt = fadein.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(0, 0);	//位置変更
				return fadein;
		}

		//フラッシュ作成
		public void flash(){
				GameObject flash = Instantiate (flashPrefab, this.transform.position, this.transform.rotation) as GameObject;
		}
}
