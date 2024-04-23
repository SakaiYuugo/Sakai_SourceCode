using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE_ShockWaveState : EffectState
{
	private Vector3 hitPosition;

	// Start is called before the first frame update
	override protected void Start()
    {
		base.Start();

		// �^����Ray���΂����������ꏊ�ֈړ�
		RaycastHit raycastHit;

		if (Physics.Raycast(transform.position, -transform.up, out raycastHit))
		{
			// �n�ʂ̃^�O��"Ground"���܂܂�Ă����ꍇ
			if (raycastHit.collider.gameObject.tag.Contains("Ground"))
			{
				hitPosition = raycastHit.point;
			}

			// �G�t�F�N�g���g�p��Ԃ�
			gameObject.GetComponent<AoE_ShockWaveMove>().Set_EfkShockWaveFlg();

			hitPosition = raycastHit.point;
		}

		gameObject.transform.position = hitPosition;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}
}
