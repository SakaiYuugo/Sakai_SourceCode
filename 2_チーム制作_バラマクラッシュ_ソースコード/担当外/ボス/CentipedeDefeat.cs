using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CentipedeDefeat : BossDefeatBase
{
	[SerializeField] AudioClip seDefeat;

	Timer endTimer = new Timer();

	protected override void OnEnable()
	{
		base.OnEnable();

		this.GetComponent<AudioSource>().PlayOneShot(seDefeat);

		endTimer.Set(3f);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (endTimer.UnscaledUpdate())
		{
			System_ObjectManager.mainCamera.GetComponent<CameraBossDefeat>().BossAnimationEnd();
			Destroy(this);
		}
	}
}
