using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalObjectBehaviour : MonoBehaviour
{
	[Range(0,1)] public float itemGeneratePercent = 0.5f;

	static List<GameObject> items = new List<GameObject>();
	GameObject itemParent;


	private void Awake()
	{
		if (items.Count != 0) { return; }
		
		Random.InitState(System.DateTime.Now.Second * System.DateTime.Now.Millisecond);
		
		foreach (GameObject obj in Resources.LoadAll<GameObject>("Prefab/Item"))
		{
			if (!obj.name.Contains("Item_")) { continue; }
			items.Add(obj);
		}
	}


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Boss")
		{
			DestroyAndCreateItem();
		}
	}


	public virtual void DestroyAndCreateItem()
	{
		if(TutorialManager.TutorialNow)
        {
			//��΂ɃI�[���A�b�v�A�C�e�����o��
			Tutorial_ObjectDestroy T_ObDsTemp = transform.parent.gameObject.GetComponent<Tutorial_ObjectDestroy>();

			if(T_ObDsTemp != null)
            {
				T_ObDsTemp.DestroyThingObject();
            }

			//�K������̃A�C�e���𐶐�
			Instantiate(ItemCreater.GetAllBombLevelup(), transform.position, Quaternion.identity);
			Destroy(this.gameObject);
			return;
        }


		if (Random.Range(0f, 1f) <= itemGeneratePercent)
		{
			ItemCreater.RandomItem(this.transform.position, Quaternion.identity);
		}

		Destroy(this.gameObject);
	}
}
