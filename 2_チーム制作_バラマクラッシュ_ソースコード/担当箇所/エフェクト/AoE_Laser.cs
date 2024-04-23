using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE_Laser : MonoBehaviour
{
	[SerializeField, Header("AoE�G�t�F�N�g")] private GameObject AoEEffect;
	[SerializeField, Header("�G�t�F�N�g�����ꏊ")] private GameObject EffectPos;
	[SerializeField] private string AoEText;
	private GameObject effect;
	private int FinishFrame;   // �G�t�F�N�g�̗\�����I���t���[��
	private int Frame = 0;
	private bool AoEFlg;

	// AoE�G�t�F�N�g���ŏ��͖��g�p���
	void Start()
	{
		AoEFlg = false;
	}

	private void FixedUpdate()
	{
		// AoE���[�U�[�����݂�����
		if (AoEFlg)
		{
			Frame++;
		}

		// ���݂̃t���[�����ő�t���[���ȏ�̏ꍇ & AoE�G�t�F�N�g�����݂��Ă���Ƃ�
		if (FinishFrame <= Frame && AoEFlg)
		{
			Frame = 0;       // �t���[���̃��Z�b�g
			AoEFlg = false;  // ���g�p��Ԃ�
			Destroy(effect); // AoE�G�t�F�N�g���폜
		}
	}


	//---------------------------------------
	// ���̃X�N���v�g�ŌĂяo��
	//---------------------------------------
	public void AoELaser()
	{
		// ��x�������s
		if (Frame <= 0)
		{
			effect = Instantiate(AoEEffect, EffectPos.transform.position, Quaternion.identity);
			// �G�t�F�N�g�̗\�����I���t���[���擾
			FinishFrame = gameObject.GetComponent<LaserAttack>().GetFinishFrame;

			AoEFlg = true; // AoE�����݂��Ă���
		}
	}

}
