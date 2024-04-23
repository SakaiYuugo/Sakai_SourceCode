using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBossDefeat : CameraBossBase
{
	public float bossDistance;
	public float bossHeight;
	[Space]
	public float playerDistance;
	public float playerHeight;

	Timer cameraChangeTimer = new Timer();

	Vector3 basePos;
	Quaternion baseRotation;

	Vector3 targetPos;
	Quaternion targetRotate;


	bool bossAnimationEnd;

	

    private new void Start()
	{
		base.Start();

		bossAnimationEnd = false;

        Vector3 bossToPlayer = player.transform.position - boss.transform.position;

		this.transform.position = boss.transform.position + bossToPlayer.normalized * bossDistance;
		this.transform.Translate(0f, bossHeight, 0f);
		this.transform.LookAt(boss.transform.position);
		basePos = this.transform.position;
		baseRotation = this.transform.rotation;

		
		targetPos = player.transform.position + (bossToPlayer.normalized * playerDistance);
		targetPos.y = player.transform.position.y + playerHeight;

		cameraChangeTimer.Set(5f);
	}


	private void OnDestroy()
	{
		InputOrder.inputEnable = true;
	}


	public void BossAnimationEnd()
	{
		bossAnimationEnd = true;
		GameObject.Find("GameEventManager").GetComponent<GameEventManager>().GameClear();
	}
}
