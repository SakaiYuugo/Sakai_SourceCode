using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportSpiderMove : EnemySupportmove
{
	[SerializeField, Header("�G�Ƃ̋���")] float Distance = 15.0f;

	
	override protected void Start()
    {
		// �I�u�W�F�N�g�̏�Ԃ��擾
		EnemyState = this.transform.GetComponent<EnemyZakoState>();
		NowState   = EnemyState.GetEnemyState();

		// �G�Ƃ̋������i�[
		EnemyDistance = Distance;

		base.Start();
    }


	override protected void FixedUpdate()
    {
		// ���݂̏�Ԃ��擾
		NowState = EnemyState.GetEnemyState();

		// ���݂̏�Ԃ��ړ����̏ꍇ
		if (NowState == EnemyZakoState.ZakoState.Move)
		{
			base.FixedUpdate();

			// �^�[�Q�b�g�I�u�W�F�N�g�����ł��Ă����ꍇ�A�V���ɋ߂��ɂ���G���^�[�Q�b�g�ɐݒ�
			if (HomingTarget == null)
			{
				HomingTarget = SerchNearEnemy("Enemy");
			}

			// �^�[�Q�b�g(Enemy)�����݂��Ă����ꍇ
			if (!(HomingTarget == null))
			{
				// �^�[�Q�b�g(Enemy)�Ƃ̋���
				float enemyDis = Vector3.Distance(transform.position,
												  HomingTarget.transform.position);

				// ��Ƀ^�[�Q�b�g(Enemy)�̕���������
				this.transform.LookAt(HomingTarget.transform.position);
				Vector3 tmo = this.transform.rotation.eulerAngles;
				tmo.x = 0f;
				tmo.z = 0f;
				this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);

				// �^�[�Q�b�g(Enemy)����苗���ȉ��Ȃ��~	
				if ((enemyDis < EnemyDistance) && (HomingTarget != PlayerObj))
				{
					transform.Translate(0f, 0f, 0f);
				}
				else
				{
					// �^�[�Q�b�g(Enemy)�̕���Z�����ʂňړ�
					transform.Translate(0f, 0f, MoveSpeed);
				}
			}
			else
			{
				// �^�[�Q�b�g���v���C���[�ɐݒ�
				HomingTarget = PlayerObj;

				// ��Ƀ^�[�Q�b�g(Player)�̕���������
				this.transform.LookAt(HomingTarget.transform.position);
				Vector3 tmo = this.transform.rotation.eulerAngles;
				tmo.x = 0f;
				tmo.z = 0f;
				this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);

				transform.Translate(0f, 0f, MoveSpeed);
			}


		}
    }
}
