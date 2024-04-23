using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CentipedeState : MonoBehaviour
{
	//���J�f�̃X�e�[�g�ꗗ
	public enum STATE
	{
		Move,
		StandAttack,
		AroundAttack,
		WheelAttack,
		Stun,
		Defeat,

		None
	}


	//���݂̃X�e�[�g
	protected STATE nowState { get; private set; }

	private STATE nextState;
	private bool isChangeState;
	
	Animator animator;
	GameObject head;

	CentipedeMove move;
	CentipedeAttackStand attackStand;
	CentipedeAttackAround attackAround;
	CentipedeAttackWheel attackWheel;
	CentipedeStun stun;
	CentipedeDefeat defeat;

	

	private void Start()
	{
		head = this.transform.Find("Head").gameObject;

		move = head.GetComponent<CentipedeMove>();
		attackStand = head.GetComponent<CentipedeAttackStand>();
		attackAround = head.GetComponent<CentipedeAttackAround>();
		attackWheel = head.GetComponent<CentipedeAttackWheel>();
		stun = head.GetComponent<CentipedeStun>();
		defeat = head.GetComponent<CentipedeDefeat>();

		//�X�e�[�g������
		nowState = STATE.None;
		isChangeState = false;

		//�ړ�����J�n
		StateChange(STATE.Move);
	}


	private void FixedUpdate()
	{
		//�X�e�[�g�ύX�v�����󂯂�����s
		if (isChangeState)
		{
			//�O�̃X�e�[�g�Ŏg���Ă����@�\���I�t�ɂ���
			switch (nowState)
			{
			default:
			case STATE.Move:		{ move.enabled = false; }			break;
			case STATE.StandAttack:	{ attackStand.enabled = false; }	break;
			case STATE.AroundAttack:{ attackAround.enabled = false; }	break;
			case STATE.WheelAttack: { attackWheel.enabled = false; }	break;
			case STATE.Stun:		{ stun.enabled = false; }			break;
			case STATE.Defeat:		{ defeat.enabled = false; }			break;
			}


			//���̃X�e�[�g������
			if (nextState is STATE.None)
			{
				//�O�̃X�e�[�g�ƕʂ̃X�e�[�g�ɂȂ�܂Ń����_���ŉ�
				while (true)
				{
					nextState = (STATE)Random.Range(0, (int)STATE.None);

					if (nowState != nextState		&&
						nextState != STATE.None		&&
						nextState != STATE.Stun		&&
						nextState != STATE.Defeat	)
					{
						nowState = nextState;
						break;
					}
				}
			}
			else
			{
				//�w�肳�ꂽ�X�e�[�g�ɕύX
				nowState = nextState;
			}


			//���̃X�e�[�g�ɕK�v�ȋ@�\���I���ɂ���
			switch (nowState)
			{
			default:
			case STATE.Move:		{ move.enabled = true; }			break;
			case STATE.StandAttack: { attackStand.enabled = true; }		break;
			case STATE.AroundAttack:{ attackAround.enabled = true; }	break;
			case STATE.WheelAttack: { attackWheel.enabled = true; }		break;
			case STATE.Stun:		{ stun.enabled = true; }			break;
			case STATE.Defeat:		{ defeat.enabled = true; }			break;
			}

			//�t���O������
			isChangeState = false;
		}
	}


	//�X�e�[�g��ω������� (������������΃����_���őI�΂��)
	public void StateChange(STATE nextState = STATE.None)
	{
		this.nextState = nextState;
		isChangeState = true;
	}
}
