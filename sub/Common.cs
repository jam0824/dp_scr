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
}
