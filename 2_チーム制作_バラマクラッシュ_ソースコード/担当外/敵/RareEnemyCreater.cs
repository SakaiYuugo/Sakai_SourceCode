using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RareEnemyCreater : MonoBehaviour
{
	[SerializeField] List<GameObject> rareEnemyPrefabs;
	static ObjectPool<GameObject> rareEnemyPool;



	private void Awake()
	{
		rareEnemyPool = new ObjectPool<GameObject>
		(
			PoolCreateObject, 
			PoolGetObject, 
			PoolReleaseObject, 
			PoolDestroyObject,
			true,
			500
		);
	}

	private void OnDestroy()
	{
		rareEnemyPool.Dispose();
	}


	public static GameObject Create(Vector3 position, Quaternion rotation)
	{
		GameObject obj = rareEnemyPool.Get();
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		return obj;
	}

	public static void Release(GameObject obj)
	{
		rareEnemyPool.Release(obj);
	}



	GameObject PoolCreateObject()
	{
		return Instantiate(rareEnemyPrefabs[Random.Range(0, rareEnemyPrefabs.Count - 1)]);
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
