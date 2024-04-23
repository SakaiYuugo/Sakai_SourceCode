using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBombCubeLevelUp : ItemBase
{
	protected override void OnGetItem()
	{
		strewState.BombCubeLevelUp();
	}
}
