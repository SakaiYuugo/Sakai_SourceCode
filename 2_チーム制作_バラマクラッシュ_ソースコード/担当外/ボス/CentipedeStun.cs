using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CentipedeStun : MonoBehaviour
{
	[SerializeField] float stunTime;
	[SerializeField] AudioClip stunSE;

	CentipedeState state;

	Timer stunDelay = new Timer();
	Timer stunTimer = new Timer();

	bool stunStart;



	private void Start()
	{
		state = this.transform.parent.GetComponent<CentipedeState>();
		stunStart = false;
	}

	private void OnEnable()
	{
		//スタン時間を計測開始
		stunDelay.Set(1f);
		stunTimer.Set(stunTime);
	}

	private void FixedUpdate()
	{
		if (!stunStart && stunDelay.ScaledUpdate())
		{
			//カメラ変更
			System_ObjectManager.mainCameraManager.ChangeCamera("BossStunCamera");

			//SEを鳴らす
			System_ObjectManager.mainCameraAudioSource.PlayOneShot(stunSE);

			//スタン開始
			stunStart = true;
		}

		if (stunStart && stunTimer.ScaledUpdate())
		{
			state.StateChange(CentipedeState.STATE.Move);
		}
	}
}
