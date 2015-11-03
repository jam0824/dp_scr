using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormationDataBean{

		const int COLUMN_FRAME = 0;
		const int COLUMN_X = 1;
		const int COLUMN_Y = 2;
		const int COLUMN_PREFAB = 3;
		const int COLUMN_MOVEMENT = 4;
		const int COLUMN_SPEED = 5;

		public int triggerFrame;
		public float x;
		public float y;
		public int enemyNo;
		public string movement;
		public float speed;
		public List<float> bezierParam = new List<float> ();

		//コンストラクタ
		public FormationDataBean(string dataSource){
				//Debug.Log("source=" + dataSource);
				string[] data = dataSource.Split (","[0]);
				triggerFrame = int.Parse(data[COLUMN_FRAME]);
				x = float.Parse(data[COLUMN_X]);
				y = float.Parse(data[COLUMN_Y]);
				enemyNo = int.Parse(data[COLUMN_PREFAB]);
				movement = data [COLUMN_MOVEMENT];
				speed = float.Parse(data[COLUMN_SPEED]);

				//Bezierのパラメータを読み込む
				List<string> tmp = arrayToList (data, 6);
				for(int i = 0; i < tmp.Count; i++){
						float p = float.Parse (tmp [i]);
						bezierParam.Add (p);
				}

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
				
}
