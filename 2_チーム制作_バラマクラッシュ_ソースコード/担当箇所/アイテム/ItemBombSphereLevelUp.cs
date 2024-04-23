using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBombSphereLevelUp : ItemBase
{
    protected override void OnGetItem()
	{
		strewState.BombSphereLevelUp();
	}
}
