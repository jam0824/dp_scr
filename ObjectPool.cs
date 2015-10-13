using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
		private static ObjectPool _instance;

		// シングルトン
		public static ObjectPool instance {
				get {
						if (_instance == null) {

								// シーン上から取得する
								_instance = FindObjectOfType<ObjectPool> ();

								if (_instance == null) {

										// ゲームオブジェクトを作成しObjectPoolコンポーネントを追加する
										_instance = new GameObject ("ObjectPool").AddComponent<ObjectPool> ();
								}
						}
						return _instance;
				}
		}

		// ゲームオブジェクトのDictionary
		private Dictionary<int, List<GameObject>> pooledGameObjects = new Dictionary<int, List<GameObject>> ();

		// ゲームオブジェクトをpooledGameObjectsから取得する。必要であれば新たに生成する
		public GameObject GetGameObject (GameObject prefab, float direction, float speed, Vector2 position, Quaternion rotation)
		{
				// プレハブのインスタンスIDをkeyとする
				int key = prefab.GetInstanceID ();

				// Dictionaryにkeyが存在しなければ作成する
				if (pooledGameObjects.ContainsKey (key) == false) {

						pooledGameObjects.Add (key, new List<GameObject> ());
				}

				List<GameObject> gameObjects = pooledGameObjects [key];

				GameObject go = null;

				for (int i = 0; i < gameObjects.Count; i++) {

						go = gameObjects [i];

						// 現在非アクティブ（未使用）であれば
						if (go.activeInHierarchy == false) {

								go = setObjectStat (go, direction, speed, position, rotation);

								// これから使用するのでアクティブにする
								go.SetActive (true);

								return go;
						}
				}

				// 使用できるものがないので新たに生成する
				go = (GameObject)Instantiate (prefab, position, rotation);

				// ObjectPoolゲームオブジェクトの子要素にする
				go.transform.parent = transform;
				go = setObjectStat (go, direction, speed, position, rotation);

				// リストに追加
				gameObjects.Add (go);

				return go;
		}

		//オブジェクトのセッティング
		private GameObject setObjectStat(GameObject go, float direction, float speed, Vector2 position, Quaternion rotation){
				Vector2 v;
				go.transform.position = position;
				go.transform.rotation = rotation;
				go.transform.Rotate (0,0,direction - 90);
				//v.x = Mathf.Cos (Mathf.Deg2Rad * direction) * speed;
				//v.y = Mathf.Sin (Mathf.Deg2Rad * direction) * speed;
				//go.GetComponent<Rigidbody2D>().velocity = v;

				return go;
		}

		// ゲームオブジェクトを非アクティブにする。こうすることで再利用可能状態にする
		public void ReleaseGameObject (GameObject go)
		{
				// 非アクティブにする
				go.SetActive (false);
		}
}