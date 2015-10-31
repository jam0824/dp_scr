using UnityEngine;
using System.Collections;

public class GlobalConfig : MonoBehaviour {

		const int GAME_FPS = 60;
	
		void Awake () {
				// ターゲットフレームレートを60に設定
				Application.targetFrameRate = GAME_FPS; 

				//Androidで戻るキーでアプリを終了させる
				if(Input.GetKey(KeyCode.Escape)){
						Application.Quit();
				}
		}
	
	
}
