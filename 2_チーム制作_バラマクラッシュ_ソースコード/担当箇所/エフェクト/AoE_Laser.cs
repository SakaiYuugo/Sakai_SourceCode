using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE_Laser : MonoBehaviour
{
	[SerializeField, Header("AoEエフェクト")] private GameObject AoEEffect;
	[SerializeField, Header("エフェクト生成場所")] private GameObject EffectPos;
	[SerializeField] private string AoEText;
	private GameObject effect;
	private int FinishFrame;   // エフェクトの予兆が終わるフレーム
	private int Frame = 0;
	private bool AoEFlg;

	// AoEエフェクトを最初は未使用状態
	void Start()
	{
		AoEFlg = false;
	}

	private void FixedUpdate()
	{
		// AoEレーザーが存在したら
		if (AoEFlg)
		{
			Frame++;
		}

		// 現在のフレームが最大フレーム以上の場合 & AoEエフェクトが存在しているとき
		if (FinishFrame <= Frame && AoEFlg)
		{
			Frame = 0;       // フレームのリセット
			AoEFlg = false;  // 未使用状態に
			Destroy(effect); // AoEエフェクトを削除
		}
	}


	//---------------------------------------
	// 他のスクリプトで呼び出す
	//---------------------------------------
	public void AoELaser()
	{
		// 一度だけ実行
		if (Frame <= 0)
		{
			effect = Instantiate(AoEEffect, EffectPos.transform.position, Quaternion.identity);
			// エフェクトの予兆が終わるフレーム取得
			FinishFrame = gameObject.GetComponent<LaserAttack>().GetFinishFrame;

			AoEFlg = true; // AoEが実在している
		}
	}

}
