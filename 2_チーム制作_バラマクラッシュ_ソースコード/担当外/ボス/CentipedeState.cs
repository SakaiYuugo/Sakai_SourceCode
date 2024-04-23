using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CentipedeState : MonoBehaviour
{
	//ムカデのステート一覧
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


	//現在のステート
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

		//ステート初期化
		nowState = STATE.None;
		isChangeState = false;

		//移動から開始
		StateChange(STATE.Move);
	}


	private void FixedUpdate()
	{
		//ステート変更要求を受けたら実行
		if (isChangeState)
		{
			//前のステートで使っていた機能をオフにする
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


			//次のステートを決定
			if (nextState is STATE.None)
			{
				//前のステートと別のステートになるまでランダムで回す
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
				//指定されたステートに変更
				nowState = nextState;
			}


			//次のステートに必要な機能をオンにする
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

			//フラグを消す
			isChangeState = false;
		}
	}


	//ステートを変化させる (引数が無ければランダムで選ばれる)
	public void StateChange(STATE nextState = STATE.None)
	{
		this.nextState = nextState;
		isChangeState = true;
	}
}
