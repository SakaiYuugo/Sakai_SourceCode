using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBeetleDefeat : BossDefeatBase
{
    int Frame = 0;


	protected override void OnEnable()
	{
		base.OnEnable();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (Frame >= 600)
		{
			System_ObjectManager.mainCamera.GetComponent<CameraBossDefeat>().BossAnimationEnd();
			Destroy(this);
		}
        Frame++;
	}
}
