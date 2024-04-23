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
		//�X�^�����Ԃ��v���J�n
		stunDelay.Set(1f);
		stunTimer.Set(stunTime);
	}

	private void FixedUpdate()
	{
		if (!stunStart && stunDelay.ScaledUpdate())
		{
			//�J�����ύX
			System_ObjectManager.mainCameraManager.ChangeCamera("BossStunCamera");

			//SE��炷
			System_ObjectManager.mainCameraAudioSource.PlayOneShot(stunSE);

			//�X�^���J�n
			stunStart = true;
		}

		if (stunStart && stunTimer.ScaledUpdate())
		{
			state.StateChange(CentipedeState.STATE.Move);
		}
	}
}
