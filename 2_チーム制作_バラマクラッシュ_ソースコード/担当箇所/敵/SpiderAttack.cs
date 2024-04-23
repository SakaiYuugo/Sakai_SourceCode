using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttack : EnemyAttack
{
	[Header("SE")]
	[SerializeField] AudioSource SE_SpiderAtk;

	[SerializeField, Header("�f���o���I�u�W�F�N�g")] private GameObject Net;
	[SerializeField, Header("�f���o����")]          private float Power = 10f;
	[SerializeField, Header("�U���O�̑ҋ@����")]     private int   RigidityTime = 60;
	[SerializeField, Header("�U������")]            private int   AttackTime = 60;
	private GameObject SpiderNet;
	private GameObject Target;
	private int AtkFrame = 0;
	private bool AtkFlg = false;
	private SpiderState SpiderState;
	private Vector3 AtkPos;

	enum AtkState
	{
		STOP,
		ATTACK,
		END,
		MAX
	};
	// �U���̏�ԑJ��
	private AtkState NowAtkState;


    override protected void Start()
    {
		base.Start();

		NowAtkState = AtkState.STOP;
		AtkFrame = 0;
		Target = GameObject.Find("Player");
		SpiderState = gameObject.GetComponent<SpiderState>();
    }

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		AtkPos += this.transform.position;

		// �G���U����Ԃ̏ꍇ
		if (NowState == EnemyZakoState.ZakoState.Attack)
		{
			switch (NowAtkState)
			{
				//----------------------
				// �U������
				//----------------------
				case AtkState.STOP: 	
					// �U���t���[������
					AtkFrame++;

					// �v���C���[�Ƃ̋���
					float PlayerDis = Vector3.Distance(transform.position,
													   Target.transform.position);

					// �U���������Ԓ�,��Ƀv���C���[�̕�������
					this.transform.LookAt(Target.transform.position);
					Vector3 Rot = this.transform.rotation.eulerAngles;
					Rot.x = 0f;
					Rot.z = 0f;
					this.transform.rotation = Quaternion.Euler(Rot.x, Rot.y, Rot.z);

					// �~�܂��Ă����莞�Ԍo�߂ōU��
					if (RigidityTime <= AtkFrame)
					{
						AtkFrame = 0;
						AtkFlg = false;
						NowAtkState = AtkState.ATTACK;
					}
					break;
				//----------------------
				// �U��
				//----------------------
				case AtkState.ATTACK:
					if (!AtkFlg)
					{
						AtkFlg = true;

						// �f���o���I�u�W�F�N�g�𐶐�
						SpiderNet = Instantiate(Net,
							this.transform.position + new Vector3(0f, 1.5f, 0f),
							Quaternion.identity);

						NetAttack();
					}

					AtkFrame++;

					if (AttackTime <= AtkFrame)
					{
						AtkFrame = 0;
						// �U���I����Ԃ�
						NowAtkState = AtkState.END;
					}

					break;
				//----------------------
				// �U���I��
				//----------------------
				case AtkState.END:
					NowAtkState = AtkState.STOP;
					SpiderState.SetEnemyState(EnemyZakoState.ZakoState.Move);
					break;

				default:
					break;
			}

		}
	}

	//------------------------
	// ������
	//------------------------
	public void NetAttack()
	{
		SE_SpiderAtk.Play();

		// �U���������Ԓ�,��Ƀv���C���[�̕�������
		this.transform.LookAt(Target.transform.position);
		Vector3 Rot = this.transform.rotation.eulerAngles;
		Rot.x = 0f;
		Rot.z = 0f;
		this.transform.rotation = Quaternion.Euler(Rot.x, Rot.y, Rot.z);

		// �l�b�g�̃��W�b�g�{�f�B�擾
		Rigidbody rid = SpiderNet.GetComponent<Rigidbody>();

		// �����̌v�Z
		rid.AddForce(this.transform.forward * Power, ForceMode.Impulse);
	}
}
