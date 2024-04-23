using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CentipedeAttackStand : MonoBehaviour
{
	BossCentipede_BodyManager bodyManager;

	
	private void OnEnable()
	{
		if (bodyManager is null) { bodyManager = this.transform.parent.Find("BodyManager").GetComponent<BossCentipede_BodyManager>(); }

		bodyManager.ChangeHeadGetUPMode();
	}
}
