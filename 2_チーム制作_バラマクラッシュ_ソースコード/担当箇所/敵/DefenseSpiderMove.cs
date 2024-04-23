using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseSpiderMove : EnemyDefmove
{
	[Header("SE")]
	[SerializeField] AudioSource SE_SpiderAtk;

	[SerializeField, Header("�{�X�Ƃ̋���")]       float Distance;
	[SerializeField, Header("�U������")]		      float AtkDistance;
	[SerializeField, Header("��]���x")]	          float RotSpeed;
	[SerializeField, Header("�{�X�I�u�W�F�N�g")]    GameObject BossObject;


    override protected void Start()
    {
		// �l���i�[
		BossDistance   = Distance;       // �{�X�Ƃ̋���
		AttackDistance = AtkDistance;    // �U������
		MinRotateSpeed = RotSpeed;       // ��]����
		BossObj        = BossObject;     // �{�X�I�u�W�F�N�g

		// �I�u�W�F�N�g�̏�Ԃ��擾
		EnemyState = this.transform.GetComponent<EnemyZakoState>();
		NowState   = EnemyState.GetEnemyState();

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

			// �����ƃ{�X�̋������擾
			float dis = Vector3.Distance(this.gameObject.transform.position,
										 BossObj.transform.position);

			// ��苗���{�X��藣��Ă����ꍇ
			if (BossDistance < dis)
			{
				// �{�X�̎���ɖ߂�
				TargetNears();
			}
			else
			{
				// �{�X�̎��������
				TargetRotate();
			}

		}
    }
}
