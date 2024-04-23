using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemBeacon : ItemBase
{
	protected override void OnGetItem()
	{
		choiceObject.CollisionBeacon();
	}
}
