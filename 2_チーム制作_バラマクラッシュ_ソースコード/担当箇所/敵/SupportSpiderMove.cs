using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportSpiderMove : EnemySupportmove
{
	[SerializeField, Header("敵との距離")] float Distance = 15.0f;

	
	override protected void Start()
    {
		// オブジェクトの状態を取得
		EnemyState = this.transform.GetComponent<EnemyZakoState>();
		NowState   = EnemyState.GetEnemyState();

		// 敵との距離を格納
		EnemyDistance = Distance;

		base.Start();
    }


	override protected void FixedUpdate()
    {
		// 現在の状態を取得
		NowState = EnemyState.GetEnemyState();

		// 現在の状態が移動中の場合
		if (NowState == EnemyZakoState.ZakoState.Move)
		{
			base.FixedUpdate();

			// ターゲットオブジェクトが消滅していた場合、新たに近くにいる敵をターゲットに設定
			if (HomingTarget == null)
			{
				HomingTarget = SerchNearEnemy("Enemy");
			}

			// ターゲット(Enemy)が存在していた場合
			if (!(HomingTarget == null))
			{
				// ターゲット(Enemy)との距離
				float enemyDis = Vector3.Distance(transform.position,
												  HomingTarget.transform.position);

				// 常にターゲット(Enemy)の方向を向く
				this.transform.LookAt(HomingTarget.transform.position);
				Vector3 tmo = this.transform.rotation.eulerAngles;
				tmo.x = 0f;
				tmo.z = 0f;
				this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);

				// ターゲット(Enemy)が一定距離以下なら停止	
				if ((enemyDis < EnemyDistance) && (HomingTarget != PlayerObj))
				{
					transform.Translate(0f, 0f, 0f);
				}
				else
				{
					// ターゲット(Enemy)の方にZ軸正面で移動
					transform.Translate(0f, 0f, MoveSpeed);
				}
			}
			else
			{
				// ターゲットをプレイヤーに設定
				HomingTarget = PlayerObj;

				// 常にターゲット(Player)の方向を向く
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
