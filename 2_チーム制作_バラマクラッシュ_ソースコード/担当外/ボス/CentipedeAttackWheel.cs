using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CentipedeAttackWheel : MonoBehaviour
{
	CentipedeState state;
	BossCentipede_BodyManager bodyManager;


	private void Start()
	{
		state = this.GetComponent<CentipedeState>();
	}

	private void OnEnable()
	{
		if (bodyManager is null) { bodyManager = this.transform.parent.Find("BodyManager").GetComponent<BossCentipede_BodyManager>(); }
		bodyManager.CentipedeJump(100f);
	}

	private void FixedUpdate()
	{
	}
}
