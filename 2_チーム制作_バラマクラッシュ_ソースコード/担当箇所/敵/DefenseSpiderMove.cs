using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseSpiderMove : EnemyDefmove
{
	[Header("SE")]
	[SerializeField] AudioSource SE_SpiderAtk;

	[SerializeField, Header("ボスとの距離")]       float Distance;
	[SerializeField, Header("攻撃距離")]		      float AtkDistance;
	[SerializeField, Header("回転速度")]	          float RotSpeed;
	[SerializeField, Header("ボスオブジェクト")]    GameObject BossObject;


    override protected void Start()
    {
		// 値を格納
		BossDistance   = Distance;       // ボスとの距離
		AttackDistance = AtkDistance;    // 攻撃距離
		MinRotateSpeed = RotSpeed;       // 回転距離
		BossObj        = BossObject;     // ボスオブジェクト

		// オブジェクトの状態を取得
		EnemyState = this.transform.GetComponent<EnemyZakoState>();
		NowState   = EnemyState.GetEnemyState();

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

			// 自分とボスの距離を取得
			float dis = Vector3.Distance(this.gameObject.transform.position,
										 BossObj.transform.position);

			// 一定距離ボスより離れていた場合
			if (BossDistance < dis)
			{
				// ボスの周りに戻る
				TargetNears();
			}
			else
			{
				// ボスの周りを巡回
				TargetRotate();
			}

		}
    }
}
