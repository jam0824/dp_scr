using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager2 : MonoBehaviour {

	public GameObject etama;
	private GameObject player;

	private string[] command;
	private int flag = 0;			//exec line number in bulletml
	private int wait_time = 0;		//it uses wait time in bulletml
	private bool continue_flag = true;	//if continue = ture, read next line.

	private float angle = 0.0f;
	private float speed = 0.8f;
	private float direction = 0.0f;
	private List<int> stack_flag = new List<int>();
	private List<int> stack_num = new List<int>();

	private int NEXT_BULLET_WAIT = 300;


	// Use this for initialization
	public void startBulletml (string bulletml) {
		//Application.targetFrameRate = 30;
		player = GameObject.Find ("player");
		command = bulletmlCompiler (bulletml);
		execLine ();
	}
	
	// Update is called once per frame
	void Update () {
	
		//wait time
		if (wait_time > 0) {
			wait_time--;
			//print(wait_time);
			if(wait_time == 0) execLine();
		}
	}


	//Execute bulletml lines
	void execLine(){
		string one_cmd = command [flag];

		if (one_cmd.IndexOf ("<bulletml") != -1) {
			flag++;
			continue_flag = true;
		} 
		else if (one_cmd.IndexOf ("<action") != -1) {
		    flag++;
			continue_flag = true;
		} 
		else if (one_cmd.IndexOf ("<fire") != -1) {
		    flag++;
			continue_flag = true;
		} 
		else if (one_cmd.IndexOf ("<direction") != -1) {
			caseDirection (one_cmd);
			flag++;
		} 
		else if (one_cmd.IndexOf ("</fire>") != -1) {
			caseBullet(one_cmd);
		} 
		else if (one_cmd.IndexOf ("<wait>") != -1) {
			caseWait(one_cmd);
		} 
		else if (one_cmd.IndexOf ("<repeat>") != -1) {
			caseRepeat(one_cmd);
		}
		else if (one_cmd.IndexOf ("</repeat>") != -1) {
			caseEndOfRepeat(one_cmd);
		}
		else if (one_cmd.IndexOf ("<speed") != -1) {
			caseSpeed(one_cmd);
		}
		else if (one_cmd.IndexOf ("</bulletml>") != -1) {
			wait_time = NEXT_BULLET_WAIT;
			flag = 0;
			continue_flag = false;
		} 
		else {
			//print (one_cmd);
			flag++;
			continue_flag = true;
			//print ("flag = " + flag);
		}


		//print (one_cmd);
		if (flag > command.Length - 1)
						continue_flag = false;
		if (continue_flag)
						execLine ();

	}


	/// <summary>
	/// Cases the speed.
	/// </summary>
	/// <param name="one_cmd">One_cmd.</param>
	void caseSpeed(string one_cmd){
		flag++;
		float s = float.Parse (command [flag]);
		//get player position.

		//case of sequence
		if (one_cmd.IndexOf ("sequence") != -1) {
			speed += s;
		}
		else {
			speed = s;
		}
		flag++;
		continue_flag = true;
	}

	/// <summary>
	/// Cases the bullet.
	/// </summary>
	/// <param name="one_cmd">One_cmd.</param>
	void caseBullet(string one_cmd){
		transform.rotation = Quaternion.Euler(0, 0, direction);
		makeEnemyShot(direction, speed, false);
		flag++;

	}


	/// <summary>
	/// Cases the end of repeat.
	/// </summary>
	/// <param name="one_cmd">One_cmd.</param>
	void caseEndOfRepeat(string one_cmd){
		stack_num [0]--;  //dec repeat number.
		if (stack_num[0] > 0) {
			flag = stack_flag[0];
		}

		else {
			//remove first stacks.
			stack_flag.RemoveAt(0);
			stack_num.RemoveAt(0);

			flag++;
		}
		continue_flag = true;
		return;
	}

	/// <summary>
	/// Cases the repeat.
	/// </summary>
	/// <param name="one_cmd">One_cmd.</param>
	void caseRepeat(string one_cmd){
		flag += 2;
		int num = int.Parse(command[flag]);
		flag++;
		stack_flag.Insert (0, flag);
		stack_num.Insert (0, num);
		continue_flag = true;
		return;
	}

	/// <summary>
	/// /////////////////////
	/// </summary>
	/// <param name="one_cmd">One_cmd.</param>
	//bulletml <wait>
	void caseWait(string one_cmd){
		flag++;
		wait_time = int.Parse(command[flag]);
		continue_flag = false;
		flag++;
		return;
	}


	void caseDirection(string one_cmd){
		flag++;
		float ang = float.Parse (command [flag]);
		//get player position.
		angle = getAngle (player.transform.position, this.transform.position) - 90f;


		//case of sequence
		if (one_cmd.IndexOf ("sequence") != -1) {
			direction += ang;
		}
		else if (one_cmd.IndexOf ("aim") != -1) {
			direction = angle + ang;
		}
		else if (one_cmd.IndexOf ("absolute") != -1) {
			direction = ang;
		}
		else {
			direction = angle + ang;
		}

		continue_flag = true;
		//print("direction = " + direction);
		return;
	}



	// It is return direction
	// @param goal : player position
	// @param start : enemy position
	private float getAngle(Vector3 goal, Vector3 start){
		float rad = Mathf.Atan2(goal.y - start.y, goal.x - start.x);
		return rad * Mathf.Rad2Deg;
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
		//print (str);
		command = str.Split (kugiri,System.StringSplitOptions.RemoveEmptyEntries);


		foreach(string s in command){
			//print (s);
		}


		return command;
	}




}
