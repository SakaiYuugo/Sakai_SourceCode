using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemSpeedUp : ItemBase
{
	protected override void OnGetItem()
	{
		playerState.Dash();
	}
}
