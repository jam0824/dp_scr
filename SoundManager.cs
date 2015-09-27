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
		const int BGM02 = 2;
		const int BGM03 = 3;
		const int BGM04 = 4;

		const int EXP_SMALL = 0;
		const int EXP_BIG = 1;
		const int GRAZE = 2;
		const int P_BULLET = 3;
		const int E_BULLET = 4;
		const int P_DAMAGE = 5;
		const int DANMAKU = 6;
		const int CAST_OFF = 7;
		const int SE_OK = 8;
		const int SE_GET_ITEM = 9;
		const int SE_MISSILE = 10;
		const int SE_MISSILE_EX = 11;
		const int SE_POWER_UP = 12;
		const int SE_RESULT_DRAW = 13;
		const int SE_BOMB = 14;

	

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
					case "opening":
						bgmPlayScr.bgmPlay(BGM02, this);
					break;
					case "ending":
						bgmPlayScr.bgmPlay(BGM03, this);
						break;
					case "result":
						bgmPlayScr.bgmPlay(BGM04, this);
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
		/// <summary>
		/// Fades the out background.
		/// </summary>
		/// <param name="bgmPlayScr">Bgm play scr.</param>
		/// <param name="v">減量分</param>
		/// <param name="waitTime">Wait time.</param>
		public void fadeOutBGM(BGMplay bgmPlayScr, float v, float waitTime){
				bgmPlayScr.bgmFadeOut (v, waitTime);
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
					case "castOff":
						sePlayScr.sePlay(CAST_OFF, this, volume);
						break;
					case "OK":
						sePlayScr.sePlay(SE_OK, this, volume);
						break;
					case "get_item":
						sePlayScr.sePlay(SE_GET_ITEM, this, 0.2f);
						break;
					case "missile":
						sePlayScr.sePlay(SE_MISSILE, this, volume);
						break;
					case "exp_missile":
						sePlayScr.sePlay(SE_MISSILE_EX, this, volume);
						break;
				case "power_up":
						sePlayScr.sePlay(SE_POWER_UP, this, volume);
						break;
				case "result_draw":
						sePlayScr.sePlay(SE_RESULT_DRAW, this, volume);
						break;
				case "bomb":
						sePlayScr.sePlay(SE_BOMB, this, volume);
						break;
				
				}

		}
}
