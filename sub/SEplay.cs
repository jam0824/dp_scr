using UnityEngine;
using System.Collections;

public class SEplay : MonoBehaviour {
	public AudioSource audioSource;
	public int destroyFrame = 0;
	public int frameRate = 60;
	// Update is called once per frame
	void Update () {
		destroyFrame--;
		if(destroyFrame < 0){
			Destroy(gameObject);	//audio再生が終わったら自動で破棄
		}
	}

	//******************************再生メイン
	//Sound Manager側に登録されているSE NOを使って再生を行う
		public void sePlay(AudioClip se, float volume){
			audioSource = GetComponent<AudioSource>();

				destroyFrame = (int)Mathf.Ceil(se.length * frameRate);
				audioSource.clip = se;
				audioSource.volume = volume;
				audioSource.PlayOneShot(se);
		}
}
