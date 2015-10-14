using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

		public GameObject sePrefab;
		public GameObject bgmPrefab;

		public List<string> BGMLabels;
		public List<AudioClip> BGMObjects;
		public Dictionary<string, AudioClip> BGMDictionary;

		public List<string> SELabels;
		public List<AudioClip> SEObjects;
		public Dictionary<string, AudioClip> SEDictionary;
		public Dictionary<string, float> SEVolDictionary;



	
		void Awake(){
				//inspectorからdictionaryを作成
				BGMDictionary = makeBGMDictionary(BGMLabels, BGMObjects);
				makeSEDictionary(SELabels, SEObjects);
		}

		/// <summary>
		/// inspectorからBGMのdictionaryを作成
		/// </summary>
		/// <returns>The BGM dictionary.</returns>
		/// <param name="BGMLabels">BGM labels.</param>
		/// <param name="BGMObjects">BGM objects.</param>
		Dictionary<string, AudioClip> makeBGMDictionary(List<string> BGMLabels, List<AudioClip> BGMObjects){
				Dictionary<string, AudioClip> dic = new Dictionary<string, AudioClip> ();
				for(int i = 0; i < BGMLabels.Count; i++){
						dic.Add (BGMLabels[i], BGMObjects[i]);
				}
				return dic;
		}
		//SEのdictionaryを作る
		//ただし、labelはlabel名,ボリュームの並びである
		void makeSEDictionary(List<string> SELabels, List<AudioClip> SEObjects){
				Dictionary<string, AudioClip> dic = new Dictionary<string, AudioClip> ();
				Dictionary<string, float> volDic = new Dictionary<string, float> ();
				for(int i = 0; i < SELabels.Count; i++){
						string[] param = SELabels [i].Split (","[0]);
						dic.Add (param[0], SEObjects[i]);
						volDic.Add(param[0], float.Parse(param[1]));
				}
				SEDictionary = dic;
				SEVolDictionary = volDic;
		}

		//BGMを鳴らす
		public BGMplay playBGM(string kind){
			GameObject bgmPlay = Instantiate(bgmPrefab, transform.position, transform.rotation) as GameObject;
			BGMplay bgmPlayScr = bgmPlay.GetComponent<BGMplay>();
			bgmPlayScr.bgmPlay(BGMDictionary[kind]);
			
			return bgmPlayScr;
		}

		//SEを鳴らす
		public void playSE(string kind){
				GameObject sePlay = Instantiate(sePrefab, transform.position, transform.rotation) as GameObject;
				SEplay sePlayScr = sePlay.GetComponent<SEplay>();
				sePlayScr.sePlay(SEDictionary[kind], SEVolDictionary[kind]);
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



}
