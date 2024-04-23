using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBombCylinderLevelUp : ItemBase
{
	protected override void OnGetItem()
	{
		strewState.BombCylinderLevelUp();
	}
}
