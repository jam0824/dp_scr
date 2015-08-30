using UnityEngine;
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
}
