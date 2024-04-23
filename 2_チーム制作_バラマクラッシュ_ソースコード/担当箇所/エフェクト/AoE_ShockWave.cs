using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE_ShockWave : MonoBehaviour
{
	[SerializeField, Header("AoE�G�t�F�N�g")] private GameObject AoEEffect;
	[SerializeField, Header("�G�t�F�N�g�����ꏊ")] private GameObject EffectPos;
	private GameObject effect;
	private int FinishFrame; // �G�t�F�N�g�̗\�����I���t���[��
	private int Frame = 0;
	private bool AoEFlg;

	// AoE�G�t�F�N�g���ŏ��͖��g�p���
	void Start()
    {
		AoEFlg = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
		// AoE�G�t�F�N�g�����݂���ꍇ
		if (AoEFlg)
		{
			Frame++;
		}

		// ���݂̃t���[�����ő�t���[���ȏ�̏ꍇ & AoE�G�t�F�N�g�����݂��Ă���Ƃ�
		if (FinishFrame <= Frame && AoEFlg)
		{
			Frame = 0;        // �t���[���̃��Z�b�g
			AoEFlg = false;   // ���g�p��Ԃ�
			Destroy(effect);  // AoE�G�t�F�N�g���폜
		}
    }

	//---------------------------------------
	// ���̃X�N���v�g�ŌĂяo��
	//---------------------------------------
	public void AoEShockWave()
	{
		// ��x�������s
		if (Frame <= 0)
		{
			effect = Instantiate(AoEEffect, EffectPos.transform.position, Quaternion.identity);
			Debug.Log("AoE �V���b�N�E�F�[�u" + effect);

			// �G�t�F�N�g�̗\�����I���t���[���擾
			FinishFrame = gameObject.GetComponent<ShockWaveAttack>().GetFinishFrame;

			AoEFlg = true; // AoE�����݂��Ă���
		}
	}
}
