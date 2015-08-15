﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ScenarioLoader : MonoBehaviour {


		//**************************************************************
		//ボスの行動データを読み込む
		public List<scenarioBean> fileRead(TextAsset t){
				List<scenarioBean> scenarioLine = new List<scenarioBean>();
				StringReader reader = new StringReader(t.text);

				while (reader.Peek() > -1) {
						string line = reader.ReadLine();
						string[] values = line.Split(',');
						if (values [0] == "")
								continue;

						scenarioBean data = new scenarioBean ();
						data.setTime (int.Parse(values[0]));
						data.setCommand (values[1]);

						if(values.Length > 2){
								data.setParam (arrayToList(values));
						}
								
						scenarioLine.Add (data);
				}

				return scenarioLine;
		}

		/// <summary>
		/// Arraies to list.
		/// </summary>
		/// <returns>The to list.</returns>
		/// <param name="array">Array.</param>
		List<string> arrayToList(string[] array){
				List<string> stringList = new List<string> ();
				stringList.AddRange (array);
				stringList.RemoveRange (0,2);
				return stringList;
		}
}
