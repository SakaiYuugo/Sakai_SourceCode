using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMove : EnemyAssaultmove
{
	[SerializeField, Header("�U���͈�")] float AtkLength = 20.0f;
	[SerializeField, Header("�U���̃N�[���^�C��")] int Cooltime = 120;
    private float Distance;
    private float Speed;
	private int AtkCnt;

    override protected void Start()
    {
		base.Start();

		// ����������
		EnemyState = this.transform.GetComponent<EnemyZakoState>();
		NowState = EnemyState.GetEnemyState();
		Distance = 1000.0f;
		Speed = base.MoveSpeed;
		AtkCnt = Cooltime;
    }

	
	override protected void FixedUpdate()
    {
		NowState = EnemyState.GetEnemyState();

		if (NowState == EnemyZakoState.ZakoState.Move)
		{
			// �G�̃X�s�[�h��ݒ�
			base.SetEnemySpeed(Speed);
			base.FixedUpdate();

			// �v���C���[�Ƃ̋������v�Z
			Distance = Vector3.Distance(gameObject.transform.position,
										base.GetHomingObj().transform.position);

			// ���͈͓̔��Ƀv���C���[�����݂����ꍇ
			if (Distance <= AtkLength)
			{
				// �G���~������
				base.SetEnemySpeed(0.0f);

				// �U����Ԃֈڍs
				EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
			}

		}
    }
}
