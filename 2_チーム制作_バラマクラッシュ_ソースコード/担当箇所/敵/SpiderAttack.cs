using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttack : EnemyAttack
{
	[Header("SE")]
	[SerializeField] AudioSource SE_SpiderAtk;

	[SerializeField, Header("吐き出すオブジェクト")] private GameObject Net;
	[SerializeField, Header("吐き出す力")]          private float Power = 10f;
	[SerializeField, Header("攻撃前の待機時間")]     private int   RigidityTime = 60;
	[SerializeField, Header("攻撃時間")]            private int   AttackTime = 60;
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
	// 攻撃の状態遷移
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

		// 敵が攻撃状態の場合
		if (NowState == EnemyZakoState.ZakoState.Attack)
		{
			switch (NowAtkState)
			{
				//----------------------
				// 攻撃準備
				//----------------------
				case AtkState.STOP: 	
					// 攻撃フレーム増加
					AtkFrame++;

					// プレイヤーとの距離
					float PlayerDis = Vector3.Distance(transform.position,
													   Target.transform.position);

					// 攻撃準備時間中,常にプレイヤーの方を向く
					this.transform.LookAt(Target.transform.position);
					Vector3 Rot = this.transform.rotation.eulerAngles;
					Rot.x = 0f;
					Rot.z = 0f;
					this.transform.rotation = Quaternion.Euler(Rot.x, Rot.y, Rot.z);

					// 止まってから一定時間経過で攻撃
					if (RigidityTime <= AtkFrame)
					{
						AtkFrame = 0;
						AtkFlg = false;
						NowAtkState = AtkState.ATTACK;
					}
					break;
				//----------------------
				// 攻撃
				//----------------------
				case AtkState.ATTACK:
					if (!AtkFlg)
					{
						AtkFlg = true;

						// 吐き出すオブジェクトを生成
						SpiderNet = Instantiate(Net,
							this.transform.position + new Vector3(0f, 1.5f, 0f),
							Quaternion.identity);

						NetAttack();
					}

					AtkFrame++;

					if (AttackTime <= AtkFrame)
					{
						AtkFrame = 0;
						// 攻撃終了状態へ
						NowAtkState = AtkState.END;
					}

					break;
				//----------------------
				// 攻撃終了
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
	// 糸発射
	//------------------------
	public void NetAttack()
	{
		SE_SpiderAtk.Play();

		// 攻撃準備時間中,常にプレイヤーの方を向く
		this.transform.LookAt(Target.transform.position);
		Vector3 Rot = this.transform.rotation.eulerAngles;
		Rot.x = 0f;
		Rot.z = 0f;
		this.transform.rotation = Quaternion.Euler(Rot.x, Rot.y, Rot.z);

		// ネットのリジットボディ取得
		Rigidbody rid = SpiderNet.GetComponent<Rigidbody>();

		// 距離の計算
		rid.AddForce(this.transform.forward * Power, ForceMode.Impulse);
	}
}
