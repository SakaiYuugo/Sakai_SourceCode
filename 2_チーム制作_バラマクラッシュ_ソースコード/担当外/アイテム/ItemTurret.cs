using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemTurret : ItemBase
{
	protected override void OnGetItem()
	{
		choiceObject.CollisionTurret();
	}
}
