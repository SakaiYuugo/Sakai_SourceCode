using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CoolTime : MonoBehaviour
{
	[SerializeField, Header("UI��fillAmount(�ő�l)")] float FillAmount = 0.64f;
	GameObject UI_Obj;
    GameObject Ui_CoolTime;
    Image ImageObj;
	StrewState strewState;
	private bool MoveFlg;
	float Cooltime;

	void Start()
	{
		// ���̃X�N���v�g�Q��
		strewState = GetComponent<StrewState>();

		// UI�I�u�W�F�N�g���擾
		Ui_CoolTime = GameObject.Find("UI_CoolTime");
		ImageObj = Ui_CoolTime.GetComponent<Image>();

		// �����̓e�N�X�`���������Ȃ��悤�ɂ���
		ImageObj.fillAmount = 0.0f;
		MoveFlg = false;
		Cooltime = 0.0f;
	}

	/// <summary>
	/// �N�[���^�C��UI��\��
	/// </summary>
	public void SetCoolTime()
	{
		ImageObj.fillAmount = 0.64f;
		MoveFlg = true;
		Cooltime = strewState.GetMaxCoolTime();	
	}


	/// <summary>
	/// �N�[���^�C��UI�̓���
	/// </summary>
	public void CoolTimeUIMove()
	{
		if (MoveFlg)
		{
			ImageObj.fillAmount -= FillAmount / Cooltime * 0.02f;

			if (ImageObj.fillAmount <= 0.0f)
			{
				MoveFlg = false;
				return;
			}
		}
	}

	/// <summary>
	/// �N�[���^�C���̒l�����Z�b�g
	/// </summary>
	public void CoolTimeReset()
	{
		ImageObj.fillAmount = 0.0f;
		MoveFlg = false;
		Cooltime = 0.0f;
	}
}
