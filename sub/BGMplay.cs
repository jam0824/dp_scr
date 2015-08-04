using UnityEngine;
using System.Collections;

public class BGMplay : MonoBehaviour {
		public AudioClip audioClip;
		public AudioSource audioSource;

		//******************************再生メイン
		//Sound Manager側に登録されているSE NOを使って再生を行う
		public void bgmPlay(int audioNo, SoundManager soundManager){
					audioSource = GetComponent<AudioSource>();
					audioClip = soundManager.bgm[audioNo];

					audioSource.clip = audioClip;
					audioSource.Play ();
		}

		/// <summary>
		/// Bgms the stop.
		/// </summary>
		public void bgmStop(){
				audioSource.Stop ();
		}

		public void bgmDelete(){
				Destroy (this.gameObject);
		}

}
