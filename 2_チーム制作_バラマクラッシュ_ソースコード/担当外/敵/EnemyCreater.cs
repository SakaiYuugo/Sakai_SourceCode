using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyCreater : MonoBehaviour
{
	[SerializeField] List<GameObject> enemyPrefabs;
	static ObjectPool<GameObject> enemyPool;
    static private int enemyNum;



	private void Awake()
	{        
		Random.InitState(System.DateTime.Now.Second * System.DateTime.Now.Millisecond);

		enemyPool = new ObjectPool<GameObject>
		(
			PoolCreateObject, 
			PoolGetObject, 
			PoolReleaseObject,
			PoolDestroyObject,
			true,
			100
		);
	}

	private void OnDestroy()
	{
		enemyPool.Dispose();
	}


	public static GameObject Create(int x ,Vector3 position, Quaternion rotation)
	{
        enemyNum = x;
		GameObject obj = enemyPool.Get();
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		return obj;
	}
	
	public static void Release(GameObject obj)
	{
		enemyPool.Release(obj);
	}



	GameObject PoolCreateObject()
	{
		return Instantiate(enemyPrefabs[enemyNum]);
	}

	void PoolGetObject(GameObject obj)
	{
		obj.SetActive(true);
	}

	void PoolReleaseObject(GameObject obj)
	{
		obj.SetActive(false);
	}

	void PoolDestroyObject(GameObject obj)
	{
		Destroy(obj);
	}
}
