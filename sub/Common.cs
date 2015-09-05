using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class Common : MonoBehaviour {

		/// <summary>
		/// 文字配列を文字リストにして返す
		/// </summary>
		/// <returns>The to list.</returns>
		/// <param name="array">Array.</param>
		/// <param name="count">Count.</param>
		/// <return> List<string>
		public List<string> arrayToList(string[] array, int count){
				List<string> stringList = new List<string> ();
				stringList.AddRange (array);
				stringList.RemoveRange (0,count);
				return stringList;
		}

		/// <summary>
		/// 単位ベクトルから角度を返す。0~180 / 0~ -180
		/// </summary>
		/// <returns>angle.</returns>
		/// <param name="vec">Vector2</param>
		public float vectorToAngle(Vector2 vec){
				float rot = Mathf.Atan2 (vec.y, vec.x) * 180 / Mathf.PI;
				if(rot > 180) rot-= 360;
				if(rot <-180) rot+= 360;
				return rot;
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
		public GameObject makeFade(GameObject fadePrefab, GameObject basePrefab, int type, int time, float r, float g, float b){
				GameObject fadein = Instantiate (fadePrefab, basePrefab.transform.position, basePrefab.transform.rotation) as GameObject;
				fadein.GetComponent<fade> ().Init (type, time, r, g, b);

				RectTransform rt = fadein.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(0, 0);	//位置変更
				return fadein;
		}

		/// <summary>
		/// Gets time.
		/// </summary>
		/// <returns>date string</returns>
		public string getDate(){
				DateTime thisDay = DateTime.Now;
				return thisDay.ToString("yyyy/MM/dd (ddd) HH:mm:ss");
		}

		// **************************json系
		//Jsonを受け取ってdictionary<string,string>のListで返す
		public List<Dictionary<string, string>> decodeJson(string source){
				IList json = decodeJsonString (source);
				return decodeIListToDictionary(json);
		}

		/// <summary>
		/// jsonをIList形式にする
		/// </summary>
		/// <returns>The json string.</returns>
		/// <param name="source">Source.</param>
		public IList decodeJsonString(string source){
				return (IList)Json.Deserialize (source);
		}

		/// <summary>
		/// IListからDictionaryに変換する
		/// </summary>
		/// <returns>List<dictionary<string,string>></returns>
		/// <param name="json">Json.</param>
		public List<Dictionary<string, string>> decodeIListToDictionary(IList json){
				List<Dictionary<string, string>> dic = new List<Dictionary<string, string>> ();
				foreach(IDictionary item in json){
						Dictionary<string, string> d = new Dictionary<string, string> ();
						//キーをdictionaryに登録する
						foreach(string key in item.Keys){
								d.Add (key, (string)item[key]);
						}
						dic.Add (d);
				}
				return dic;
		}

		/// <summary>
		/// Dictionaryのリストをキーを比較してソートする
		/// </summary>
		/// <returns>The dictionary list.</returns>
		/// <param name="source">Source.</param>
		/// <param name="compairKey">To Compair key.</param>
		public List<Dictionary<string, string>> sortDictionaryList(List<Dictionary<string, string>> source, string compairKey){
				Dictionary<string, string> tmp = new Dictionary<string, string> ();

				for(int i = 0; i < source.Count; i++){
						for(int k = i + 1; k < source.Count; k++){
								if(int.Parse(source[i][compairKey]) < int.Parse(source[k][compairKey])){
										tmp = source [i];
										source [i] = source [k];
										source [k] = tmp;
								}
						}
				}
				return source;
		}

		/// <summary>
		/// dictionaryのlistをrankまでJSONの文字列に変換する
		/// </summary>
		/// <returns>json string</returns>
		/// <param name="dic">Dic.</param>
		/// <param name="rank">Rank.</param>
		public string encodeDictionaryToJson(List<Dictionary<string, string>> dic, int rank){
				string jsonStr = "[";
				for(int i = 0; i < rank; i++){
						jsonStr += Json.Serialize (dic[i]);
						if (i != rank - 1) {
								jsonStr += ",";
						}
				}
				jsonStr += "]";
				return jsonStr;
		}
}
