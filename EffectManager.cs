using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour {

		public GameObject fadePrefab;
		public GameObject flashPrefab;
		public GameObject debugPrefab;

		public List<string> labels;
		public List<GameObject> effects;
		Dictionary<string,GameObject> effectDictionary;

		GameObject fadein;
		GameObject nextPrefab;
		string fadeType = "black";
		Vector3 prefabPos;

		Common common;

		// Use this for initialization
		void Start () {
				//inspectorのデータからdictionaryを作る
				effectDictionary = makeEffectDictionary(labels, effects);
				common = GameObject.Find("Common").GetComponent<Common>();
		}

		/// <summary>
		/// inspectorのデータからLabelとgameobjectを対応させたdictionaryを作る
		/// </summary>
		/// <returns>The effect dictionary.</returns>
		/// <param name="labels">Labels.</param>
		/// <param name="effects">Effects.</param>
		Dictionary<string,GameObject> makeEffectDictionary(List<string> labels, List<GameObject> effects){
				Dictionary<string, GameObject> dic = new Dictionary<string, GameObject> ();
				for(int i = 0; i < labels.Count; i++){
						dic.Add (labels[i], effects[i]);
				}
				return dic;
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		//ゲームオブジェクトを作る
		public GameObject makeEffect(string effectType, Vector3 pos){
				GameObject obj = Instantiate (effectDictionary [effectType], pos, this.transform.rotation) as GameObject;
				return obj;
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
						fadein = common.makeFade (fadePrefab, this.gameObject, 1, 60, 255.0f, 255.0f, 255.0f);
				} else {
						fadein = common.makeFade (fadePrefab, this.gameObject, 1, 60, 0.0f, 0.0f, 0.0f);
				}
				nextPrefab = nextScene;
				Invoke ("changeNextScene", 1f);

		}
		//遷移実体
		private void changeNextScene(){
				Destroy (fadein);
				GameObject scene = Instantiate (nextPrefab, prefabPos, this.transform.rotation) as GameObject;
				scene.transform.parent = GameObject.Find ("Canvas").transform;	//Canvasを親にする
				scene.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
				if (fadeType == "white") {
						fadein = common.makeFade (fadePrefab, this.gameObject, 0, 60, 255.0f, 255.0f, 255.0f);
				} else {
						fadein = common.makeFade (fadePrefab, this.gameObject, 0, 60, 0.0f, 0.0f, 0.0f);
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

		//デバッグメッセージを書く
		public void drawDebug(string msg){
				GameObject debugWindow = Instantiate (debugPrefab, this.transform.position, this.transform.rotation) as GameObject;
				debugWindow.transform.parent = GameObject.Find ("Canvas").transform;
				RectTransform pos = debugWindow.GetComponent<RectTransform>();
				pos.localScale = new Vector3 (1, 1, 1);	//スケールを元に戻す
				//pos.anchoredPosition = new Vector2(0, 0);	//位置変更
				debugPrefab.GetComponent<Text> ().text = msg;
		}
}
