using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE_ShockWaveMove : MonoBehaviour
{
	[SerializeField, Header("最大拡大値")] private float MaxSize;
	private float FinishFrame = 180f;
	private float Expansion;
	private bool  EffectFlg = false;

	// セッター
	public void Set_EfkShockWaveFlg()
	{
		EffectFlg = true;
	}
}
