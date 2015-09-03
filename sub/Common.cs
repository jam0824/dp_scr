using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
}
