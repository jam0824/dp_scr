using UnityEngine;
using System.Collections;

public class Option : MonoBehaviour {

		public GameObject optionViewPrefab;
		GameObject optionView;

		/// <summary>
		/// 停止/再生
		/// </summary>
		public void onClickStopButton(){
				if (Time.timeScale != 0) {
						Time.timeScale = 0;	//一時停止
						optionView = Instantiate (optionViewPrefab) as GameObject;
						optionView.name = optionViewPrefab.name;	//(clone)付加問題対応
						Transform canvasTransform = GameObject.Find ("Canvas").transform;	//Canvasを親にする
						optionView.transform.parent = canvasTransform;
						RectTransform rt = optionView.GetComponent<RectTransform>();
						rt.localScale = new Vector3 (1, 1, 1);
						optionView.transform.position = canvasTransform.position;
				} else {
						Time.timeScale = 1;	//復旧
						Destroy (GameObject.Find("OptionView"));
				}
		}

		/// <summary>
		/// リトライ
		/// </summary>
		public void onClickRetry(){
				destroyAllObject ();
				Time.timeScale = 1;	//復旧
				Application.LoadLevel(Application.loadedLevel);
		}

		/// <summary>
		/// タイトルに戻る
		/// </summary>
		public void onClickGoToTheTitle(){
				Destroy (GameObject.Find("GameManager"));
				Time.timeScale = 1;	//復旧
				Application.LoadLevel ("title");
		}

		/// <summary>
		/// 終了
		/// </summary>
		public void onClickExit(){
				Application.Quit();
		}

		void destroyAllObject(){
				// typeで指定した型の全てのオブジェクトを配列で取得し,その要素数分繰り返す.
				foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
				{
						// シーン上に存在するオブジェクトならば処理.
						if (obj.activeInHierarchy)
						{
								Destroy (obj);
						}
				}
		}
}
