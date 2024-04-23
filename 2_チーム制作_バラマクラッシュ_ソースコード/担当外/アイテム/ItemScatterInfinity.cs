using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemScatterInfinity : ItemBase
{
	protected override void OnGetItem()
	{
		strewState.InfinityStrew();
	}
}
