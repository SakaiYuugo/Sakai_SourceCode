using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemScatterUp : ItemBase
{
	protected override void OnGetItem()
	{
		strewState.IncreaseStrew();
	}
}
