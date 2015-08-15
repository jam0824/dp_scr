using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

		public GameObject sePrefab;
		public GameObject bgmPrefab;
		public AudioClip[] audioClip;
		public AudioClip[] bgm;
		public AudioSource audioSource;

		const int BGM00 = 0;
		const int BGM01 = 1;

		const int EXP_SMALL = 0;
		const int EXP_BIG = 1;
		const int GRAZE = 2;
		const int P_BULLET = 3;
		const int E_BULLET = 4;
		const int P_DAMAGE = 5;
		const int DANMAKU = 6;

	

		public BGMplay playBGM(string kind){
			GameObject bgmPlay = Instantiate(bgmPrefab, transform.position, transform.rotation) as GameObject;
			BGMplay bgmPlayScr = bgmPlay.GetComponent<BGMplay>();
			switch(kind){
					case "bgm00":
						bgmPlayScr.bgmPlay(BGM00, this);
					break;
					case "bgm01":
						bgmPlayScr.bgmPlay(BGM01, this);
					break;

			}
			return bgmPlayScr;
		}

		public void stopBGM(BGMplay bgmPlayScr){
				bgmPlayScr.bgmStop ();
		}
		public void deleteBGM(BGMplay bgmPlayScr){
				bgmPlayScr.bgmDelete ();
		}

		public void playSE(string kind){
				GameObject sePlay = Instantiate(sePrefab, transform.position, transform.rotation) as GameObject;
				SEplay sePlayScr = sePlay.GetComponent<SEplay>();
					float volume = 0.6f;
				switch(kind){
					case "exp_small":
							sePlayScr.sePlay(EXP_SMALL, this, volume);
					break;
					case "exp_big":
							sePlayScr.sePlay(EXP_BIG, this, volume);
					break;
					case "graze":
							sePlayScr.sePlay(GRAZE, this, volume);
					break;
					case "playerBullet":
							sePlayScr.sePlay(P_BULLET, this, 0.2f);
					break;
					case "enemy_bullet":
							sePlayScr.sePlay(E_BULLET, this, volume);
							break;
					case "playerDamage":
							sePlayScr.sePlay(P_DAMAGE, this, volume);
							break;
					case "danmaku":
						sePlayScr.sePlay(DANMAKU, this, volume);
						break;
				}

		}
}
