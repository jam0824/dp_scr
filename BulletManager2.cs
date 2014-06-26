using UnityEngine;
using System.Collections;

public class BulletManager2 : MonoBehaviour {

	public GameObject etama;
	private GameObject player;

	private string str = "<bulletml><action label='top'><fire><direction>-15</direction><bullet label='spade' /></fire><fire><direction>-5</direction><bullet label='heart' /></fire><fire><direction>5</direction><bullet label='clover' /></fire><fire><direction>15</direction><bullet label='diamond' /></fire></action></bulletml>";

	private string[] command;
	private int flag = 0;
	private bool continue_flag = true;

	private float speed = 0.8f;
	private float direction = 0.0f;


	// Use this for initialization
	void Start () {
		player = GameObject.Find ("player");
		command = bulletmlCompiler (str);
		execLine ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	//
	void execLine(){
		string one_cmd = command [flag];

		if(one_cmd.IndexOf ("<bulletml") != -1){
			print ("exec bulletml");
			continue_flag = true;
		}
		else if(one_cmd.IndexOf ("<action") != -1){
			print ("exec action");
			continue_flag = true;
		}
		else if(one_cmd.IndexOf ("<fire") != -1){
			print ("exec fire");
			continue_flag = true;
		}
		else if(one_cmd.IndexOf ("<direction") != -1){
			caseDirection(one_cmd);
		}
		else if(one_cmd.IndexOf ("<bullet") != -1){
			makeShot();
		}
		else if(one_cmd.IndexOf ("</bulletml>") != -1){
			print ("exit");
			continue_flag = false;
		}


		flag++;
		print ("flag = " + flag);
		if (flag > command.Length - 1)
						continue_flag = false;
		if (continue_flag)
						execLine ();

	}


	void caseDirection(string one_cmd){
		//if(one_cmd.IndexOf("</") != -1){
			flag++;
			direction = float.Parse(command[flag]);
			//get player position.
			float angle = getAngle(player.transform.position, this.transform.position) - 90f;
			direction += angle;
			continue_flag = true;
			print("direction = " + direction);
		//}

		return;
	}



	// It is return direction
	// @param goal : player position
	// @param start : enemy position
	private float getAngle(Vector3 goal, Vector3 start){
		float rad = Mathf.Atan2(goal.y - start.y, goal.x - start.x);
		return rad * Mathf.Rad2Deg;
	}


	private void makeShot(){
		transform.rotation = Quaternion.Euler(0, 0, direction);
		makeEnemyShot(direction, speed, false);
		
	}

	//Making one bullet object
	// @param direction : angle to shot bullet
	// @param speed : speed per sec
	// @param rotationTrigger : It's switch to rotate image
	private void makeEnemyShot(float direction, float speed, bool rotationTrigger){
		GameObject shot = (GameObject)Instantiate (etama, this.transform.position, this.transform.rotation);
		bullet s = shot.GetComponent<bullet>();
		
		s.Create (transform.up, direction, speed, rotationTrigger);
		
	}

	
	// It splits bulletml codes
	// @param str : bulletML
	// @return command : splited bulletML codes.
	string[] bulletmlCompiler(string str){
		string[] kugiri = {"¥r", "¥n"};
		string[] command;

		str = str.Replace ("<", "¥n<");
		str = str.Replace (">", ">¥n");
		print (str);
		command = str.Split (kugiri,System.StringSplitOptions.RemoveEmptyEntries);


		foreach(string s in command){
			print (s);
		}


		return command;
	}
}
