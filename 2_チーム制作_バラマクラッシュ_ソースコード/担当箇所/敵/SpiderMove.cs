using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMove : EnemyAssaultmove
{
	[SerializeField, Header("攻撃範囲")] float AtkLength = 20.0f;
	[SerializeField, Header("攻撃のクールタイム")] int Cooltime = 120;
    private float Distance;
    private float Speed;
	private int AtkCnt;

    override protected void Start()
    {
		base.Start();

		// 初期化処理
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
			// 敵のスピードを設定
			base.SetEnemySpeed(Speed);
			base.FixedUpdate();

			// プレイヤーとの距離を計算
			Distance = Vector3.Distance(gameObject.transform.position,
										base.GetHomingObj().transform.position);

			// 一定の範囲内にプレイヤーが存在した場合
			if (Distance <= AtkLength)
			{
				// 敵を停止させる
				base.SetEnemySpeed(0.0f);

				// 攻撃状態へ移行
				EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
			}

		}
    }
}
