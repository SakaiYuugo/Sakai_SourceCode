using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemRecovery : ItemBase
{
	protected override void OnGetItem()
	{
		playerState.Heal(1);
	}
}
