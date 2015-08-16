using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormationDataBean : MonoBehaviour {

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
				List<string> tmp = new Common ().arrayToList (data, 6);
				for(int i = 0; i < tmp.Count; i++){
						float p = float.Parse (tmp [i]);
						bezierParam.Add (p);
				}

		}
				
}
