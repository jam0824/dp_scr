using UnityEngine;
using System.Collections;

public class BGMplay : MonoBehaviour {
		public AudioSource audioSource;

		//******************************再生メイン
		//Sound Manager側に登録されているSE NOを使って再生を行う
		public void bgmPlay(AudioClip bgm){
				audioSource = GetComponent<AudioSource>();
				audioSource.clip = bgm;
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

		/// <summary>
		/// Bgms the fade out.
		/// </summary>
		/// <param name="v">減分</param>
		/// <param name="waitTime">Wait time.</param>
		public void bgmFadeOut(float v, float waitTime){
				StartCoroutine (fadeOut(v, waitTime)); //コールチン
		}

		//コールチンでフェードアウト
		private IEnumerator fadeOut(float v, float waitTime){
				//無限ループ防止
				for (int i = 0; i < 1000; i++) {
						audioSource.volume -= v;
						yield return new WaitForSeconds (waitTime);
						if (audioSource.volume <= 0) {
							yield break;
						}

							

				}
				Destroy (this.gameObject);
				yield break;
		}
}
