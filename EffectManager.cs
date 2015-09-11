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
				default :
						return middleExplosion;
						break;
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
