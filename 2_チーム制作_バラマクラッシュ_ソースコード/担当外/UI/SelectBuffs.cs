using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectBuffs : MonoBehaviour
{
    private void Awake()
    {
		this.gameObject.SetActive(false);
    }


    private void Start()
	{
		Text allComplete	= this.transform.Find("CompleteNum").	GetComponent<Text>();
		Text easyComplete	= this.transform.Find("EasyComplete").	GetComponent<Text>();
		Text normalComplete = this.transform.Find("NormalComplete").GetComponent<Text>();
		Text hardClear		= this.transform.Find("HardClear").		GetComponent<Text>();
		Text hardComplete	= this.transform.Find("HardComplete").	GetComponent<Text>();
		Text bombNum	 = this.transform.Find("BombNum").	 GetComponent<Text>();
		Text bombLevel	 = this.transform.Find("BombLevel"). GetComponent<Text>();
		Text isInfiniity = this.transform.Find("IsInfinity").GetComponent<Text>();
		Text isEnding	 = this.transform.Find("IsEnding").	 GetComponent<Text>();

		//�~�b�V�����R���v���[�g��
		allComplete.text = "�~�b�V�����R���v���[�g���@" + ReleaseSkilManager.instance.GetSkilState().PerfectClearNum;

		//Easy�S�~�b�V�����R���v���[�g
		easyComplete.color	 = ReleaseSkilManager.instance.GetSkilState().Easy_PerfectClear ?
								Color.white : Color.white * 0.5f;

		//Normal�S�~�b�V�����R���v���[�g
		normalComplete.color = ReleaseSkilManager.instance.GetSkilState().Normal_PerfectClear ?
								Color.white : Color.white * 0.5f;

		//Hard�S�N���A
		hardClear.color		 = ReleaseSkilManager.instance.GetSkilState().Hard_UsuallyClear ?
								Color.white : Color.white * 0.5f;

		//Hard�S�~�b�V�����R���v���[�g
		hardComplete.color	 = ReleaseSkilManager.instance.GetSkilState().Hard_PerfectClear ?
								Color.white : Color.white * 0.5f;


		//�ǉ��{����
		bombNum.text = "�@�@�ǉ��{�����@+" + (ReleaseSkilManager.instance.GetSkilState().PerfectClearNum * 3).ToString() + "��";

		//�����{�����x��
		int level = 1;
		level = level + (ReleaseSkilManager.instance.GetSkilState().Easy_PerfectClear ? 1 : 0);
		level = level + (ReleaseSkilManager.instance.GetSkilState().Normal_PerfectClear ? 1 : 0);
		bombLevel.text = "�����{�����x���@" + level.ToString();

		//�����΂�܂�
		isInfiniity.color = ReleaseSkilManager.instance.GetSkilState().Hard_PerfectClear ? Color.white : Color.white * 0.5f;

		//�G���f�B���O
		isEnding.color = System_SaveManager.isEndingEnable ? Color.white : Color.white * 0.5f;
	}
}
