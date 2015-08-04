using UnityEngine;
using System.Collections;

public class SEplay : MonoBehaviour {
	public AudioSource audioSource;
	public SoundManager soundManager;
	public AudioClip[] clip;
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
		public void sePlay(int audioNo, SoundManager soundManager, float volume){
			audioSource = GetComponent<AudioSource>();

			AudioClip audioClip = soundManager.audioClip[audioNo];
			destroyFrame = (int)Mathf.Ceil(audioClip.length * frameRate);
			audioSource.clip = audioClip;
			audioSource.volume = volume;
			audioSource.PlayOneShot(audioClip);
		}
}
