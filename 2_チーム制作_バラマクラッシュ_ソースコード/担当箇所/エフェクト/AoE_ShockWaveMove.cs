using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE_ShockWaveMove : MonoBehaviour
{
	[SerializeField, Header("�ő�g��l")] private float MaxSize;
	private float FinishFrame = 180f;
	private float Expansion;
	private bool  EffectFlg = false;

	// �Z�b�^�[
	public void Set_EfkShockWaveFlg()
	{
		EffectFlg = true;
	}
}
