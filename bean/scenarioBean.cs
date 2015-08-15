using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scenarioBean : MonoBehaviour {

		int startTime = 0;
		string command = "";
		List<string> param = new List<string>();

		public void setTime(int time){
				this.startTime = time;
		}
		public int getTime(){
				return this.startTime;
		}

		public void setCommand(string c){
				this.command = c;
		}
		public string getCommand(){
				return this.command;
		}

		public void setParam(List<string> p){
				this.param = p;
		}
		public List<string> getParam(){
				return this.param;
		}
	
}
