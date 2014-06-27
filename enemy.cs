using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {

	private int HP = 1;

	private GameObject shotposition;
	private BulletManager2 bulletmanager;
	public string bulletml = "<bulletml><action label='top'><fire><direction>-20</direction><bullet/></fire><repeat><times>4</times><action><fire><direction type='sequence'>10</direction><bullet/></fire></action></repeat></action></bulletml>";


	// Use this for initialization
	void Start () {
		shotposition = GameObject.Find ("shotposition");
		bulletmanager = shotposition.GetComponent<BulletManager2>();
		bulletmanager.startBulletml (bulletml);

	}
	
	// Update is called once per frame
	void Update () {
	}




}
