using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class Common : MonoBehaviour {

		bool debugMode = true;

		//***************************************************************数学系
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

		// p2からp1への角度を求める
		// @param p1 自分の座標
		// @param p2 相手の座標
		// @return 2点の角度(Degree)
		public float getAim(Vector2 p1, Vector2 p2) {
				float dx = p2.x - p1.x;
				float dy = p2.y - p1.y;
				float rad = Mathf.Atan2(dy, dx);
				return rad * Mathf.Rad2Deg;
		}

		//***************************************************************エフェクト系
		/// <summary>
		/// 元のポジションを中心にランダムなポジションを返す
		/// </summary>
		/// <returns>Changed position</returns>
		/// <param name="pos">Original Position</param>
		/// <param name="width">Width.</param>
		public Vector3 randomPos(Vector3 pos, float width){
				pos.x += (UnityEngine.Random.value * width) - width / 2;
				pos.y += (UnityEngine.Random.value * width) - width / 2;
				return pos;
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

		//コールチンでステータスアニメーション
		/// <summary>
		/// UIを上下に移動する
		/// </summary>
		/// <returns></returns>
		/// <param name="rt">RectTransform</param>
		/// <param name="v">増分</param>
		/// <param name="maxY">ストップ位置</param>
		/// <param name="waitTime">待ち時間（sec）</param>
		public IEnumerator moveUI(RectTransform rt, float v, float maxY, float waitTime) {
				//無限ループ防止
				for (int i = 0; i < 1000; i++) {

						Vector2 pos = rt.anchoredPosition;
						pos.y += v;
						rt.anchoredPosition = pos;
						yield return new WaitForSeconds (waitTime);
						//maxでコルーチン終了
						if(v < 0){
								if (pos.y <= maxY) {
										yield break;
								}
						}else{
								if (pos.y >= maxY) {
										yield break;
								}
						}

				}
				yield break;
		}
		//***************************************************************データ処理系
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
		/// Gets time.
		/// </summary>
		/// <returns>date string</returns>
		public string getDate(){
				DateTime thisDay = DateTime.Now;
				return thisDay.ToString("yyyy/MM/dd (ddd) HH:mm:ss");
		}

		/// <summary>
		/// Gets the day.
		/// </summary>
		/// <returns>The day.</returns>
		public int getDay(){
				DateTime thisDay = DateTime.Now;
				return thisDay.Day;
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

		//--------------------------------------------------------------------
		/// <summary>  指定された文字列をMD5でハッシュ化し、文字列として返す
		/// </summary>
		/// <param name="srcStr">入力文字列</param>
		/// <returns>入力文字列のMD5ハッシュ値</returns>
		//--------------------------------------------------------------------
		public string calcMd5( string srcStr ) {

				System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

				// md5ハッシュ値を求める
				byte[] srcBytes = System.Text.Encoding.UTF8.GetBytes(srcStr);
				byte[] destBytes = md5.ComputeHash(srcBytes);

				// 求めたmd5値を文字列に変換する
				System.Text.StringBuilder destStrBuilder;
				destStrBuilder = new System.Text.StringBuilder();
				foreach (byte curByte in destBytes) {
						destStrBuilder.Append(curByte.ToString("x2"));
				}

				// 変換後の文字列を返す
				return destStrBuilder.ToString();
		}

		/// <summary>
		/// ビュー座標からワールド座標を返す
		/// </summary>
		/// <returns>The world position from view position.</returns>
		/// <param name="worldPos">World position.</param>
		public Vector3 getWorldPosFromViewPos(Vector3 worldPos){
				Vector3 tmpPos = Camera.main.ViewportToWorldPoint (new Vector3(
						worldPos.x, 
						worldPos.y,
						-1 * Camera.main.transform.position.z)	//カメラの高さを合わせないとずれる
				);
				return tmpPos;
		}

		/// <summary>
		/// デバッグメッセージ表示
		/// </summary>
		/// <param name="s">S.</param>
		/// <typeparam name="Type">The 1st type parameter.</typeparam>
		public void debug<Type>(Type s){
				if(debugMode == true)
				Debug.Log (s);
		}

}
