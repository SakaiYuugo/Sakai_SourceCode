using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBombLevelReset : ItemBase
{
	protected override void OnGetItem()
	{
		strewState.BombLevelReset();
	}    
}