using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;



public class ItemCreater : MonoBehaviour
{
	static List<GameObject> items = new List<GameObject>();
	

	private void Awake()
	{
		Random.InitState(System.DateTime.Now.Second * System.DateTime.Now.Millisecond);

		System_ObjectManager.itemParent = this.gameObject;

		foreach (GameObject item in Resources.LoadAll<GameObject>("Prefab/Item"))
		{
			items.Add(item);
		}
	}


	public static GameObject RandomItem(Vector3 position, Quaternion rotation)
	{
		GameObject obj = Instantiate(items[Random.Range(0, items.Count - 1)]);
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		
		foreach (RaycastHit hit in Physics.RaycastAll(position + (Vector3.up * 100f), Vector3.down))
		{
			if (hit.transform.tag == "Ground")
			{
				obj.transform.position = new Vector3(position.x, hit.point.y + (obj.transform.lossyScale.y / 2), position.z);
				break;
			}
		}

		return obj;
	}


	public static GameObject GetAllBombLevelup()
	{
		return Resources.Load<GameObject>("Prefab/Item/Item_BombLevelUP");
	}
}
