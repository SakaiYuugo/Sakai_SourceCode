using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBombTriangleLevelUp : ItemBase
{
	protected override void OnGetItem()
	{
		strewState.BombTriangleLevelUp();
	}
}
