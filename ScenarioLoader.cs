using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ScenarioLoader{



		//**************************************************************
		//データを読み込む
		public List<scenarioBean> fileRead(TextAsset t){
				bool isSkip = false;

				List<scenarioBean> scenarioLine = new List<scenarioBean>();
				StringReader reader = new StringReader(t.text);

				while (reader.Peek() > -1) {
						string line = reader.ReadLine();
						//空白行ブロック
						if (line == "")
								continue;

						string[] values = line.Split(',');

						if (line.Substring (0, 2) == "/*") {
								isSkip = true;
								continue;
						}
						if (line.Substring (0, 2) == "*/") {
								isSkip = false;
								continue;
						}
						if ((isSkip)||(line.Substring (0, 2) == "//")) continue;

						scenarioBean data = new scenarioBean ();
						data.setTime (int.Parse(values[0]));
						data.setCommand (values[1]);

						if(values.Length > 2){
								data.setParam (new Common().arrayToList(values,2));
						}
								
						scenarioLine.Add (data);
				}

				return scenarioLine;
		}
}
