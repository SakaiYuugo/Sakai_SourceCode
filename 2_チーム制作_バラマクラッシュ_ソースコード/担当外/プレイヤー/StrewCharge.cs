using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class StrewCharge : MonoBehaviour
{
	[SerializeField] float chargeSpeed;
	
	VisualEffect effect;

	int level;
	float scale;
	float maxScale;
	bool isPlay;
	


	private void Start()
	{
		effect = this.GetComponent<VisualEffect>();
		effect.SendEvent("OnStop");
		isPlay = false;
	}

	private void FixedUpdate()
	{
		if (!isPlay) return;

		if (scale < maxScale)
		{
			scale += chargeSpeed;
		}
		else
		{
			scale = maxScale;
		}

		effect.SetFloat("Scale", scale);
	}



	/// <summary>
	/// �`���[�W�c��ύX���ɌĂяo��
	/// 0�`3 �z��
	/// 0�Ȃ牽���\������Ȃ�
	/// </summary>
	/// <param name="chargeLevel"></param>
	public void LevelChange(int chargeLevel)
	{
		if (chargeLevel == 0)
		{
			isPlay = false;
			effect.SendEvent("OnStop");
			effect.SetInt("ChargeLevel", 0);
			return;
		}

		switch (chargeLevel)
		{
		case 1: { maxScale = 0.5f; } break;
		case 2:	{ maxScale = 1.0f; } break;
		case 3:	{ maxScale = 1.5f; } break;
		}

		effect.SendEvent("OnPlay");
		level = chargeLevel;
		effect.SetInt("ChargeLevel", level);
		isPlay = true;
	}
}
