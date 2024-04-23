using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemShield : ItemBase
{
	protected override void OnGetItem()
	{
		playerState.InvisibleTimeCoroutine();
	}
}
