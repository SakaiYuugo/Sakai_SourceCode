using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;


public class CentipedeAttackAround : MonoBehaviour
{
	[Header("Paramater")]
	[SerializeField] float moveSpeed;
	[SerializeField] float rotateSpeed;
	[SerializeField] float rotateTime;
	[SerializeField] float distance;

	[Header("SE")]
	[SerializeField] AudioClip seAround;
	[SerializeField] float seInterval;
	[SerializeField] AudioClip seAroundEnd;

	enum STATE
	{
		Approach,
		Around
	}

	STATE state;

	CentipedeState centipedeState;

	GameObject player;
	Vector3 targetPos;
	int rotateDirection;

	Timer timer = new Timer();

	Vector3 playerLeft;
	Vector3 playerRight;

	GameObject tail;
	Timer seTimer = new Timer();
	bool isSePlayed;



	private void Start()
	{
		centipedeState = this.transform.parent.GetComponent<CentipedeState>();
		tail = this.transform.parent.Find("Tail").gameObject;
	}


	private void OnEnable()
	{
		if (player is null) { player = System_ObjectManager.playerObject; }
		timer.Set(rotateTime);

		//計算
		Vector3 playerToThis = this.transform.position - player.transform.position;
		playerLeft = player.transform.position + (Quaternion.Euler(0f, 120f, 0f) * playerToThis).normalized * distance;
		playerRight = player.transform.position + (Quaternion.Euler(0f, -120f, 0f) * playerToThis).normalized * distance;

		//近い方の座標を取得
		if ((this.transform.position - playerLeft).magnitude < (this.transform.position - playerRight).magnitude)
		{
			targetPos = playerLeft;
		}
		else
		{
			targetPos = playerRight;
		}
		targetPos.y = this.transform.position.y;

		//SE関係
		seTimer.Set(seInterval);
		isSePlayed = false;
		AudioSource audioSource = this.GetComponent<AudioSource>();
		audioSource.PlayOneShot(seAround, 0.5f);
	}

	
	private void FixedUpdate()
	{
		switch (state)
		{
		case STATE.Approach:
		{
			//SEを鳴らす
			if (seTimer.ScaledUpdate())
			{
				AudioSource audioSource = this.GetComponent<AudioSource>();
				audioSource.PlayOneShot(seAround, 0.5f);
				seTimer.Reset();
			}


			//移動
			Vector3 targetToThis = this.transform.position - targetPos;
			this.transform.position += this.transform.forward * moveSpeed;
			this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(-targetToThis), rotateSpeed);
			this.transform.rotation = Quaternion.Euler(0f, this.transform.eulerAngles.y, this.transform.eulerAngles.z);


			//ターゲット座標近くに来たら
			if ((targetToThis.magnitude < 30f) & (player.transform.position - this.transform.position).magnitude > 50f)
			{
				targetPos = player.transform.position;
				targetPos.y = this.transform.position.y;

				state = STATE.Around;
			}
		}
		break;
		case STATE.Around:
		{
			//囲み完了したらSEを鳴らす
			if (!isSePlayed )
			{
				if (Vector3.Distance(this.transform.position, tail.transform.position) < 50f)
				{
					isSePlayed = true;

					//囲み完了音
					AudioSource audioSource = this.GetComponent<AudioSource>();
					audioSource.Stop();
					audioSource.PlayOneShot(seAroundEnd);
				}
				else
				if (seTimer.ScaledUpdate())
				{
					AudioSource audioSource = this.GetComponent<AudioSource>();
					audioSource.PlayOneShot(seAround, 0.5f);
					seTimer.Reset();
				}
			}

			//移動
			this.transform.position += this.transform.forward * moveSpeed;
			this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(targetPos - this.transform.position), rotateSpeed);
			this.transform.rotation = Quaternion.Euler(0f, this.transform.eulerAngles.y, this.transform.eulerAngles.z);

			//時間が経ったら終わる
			if (timer.ScaledUpdate())
			{
				centipedeState.StateChange(CentipedeState.STATE.Move);
			}
		}
		break;
		}
	}
}
