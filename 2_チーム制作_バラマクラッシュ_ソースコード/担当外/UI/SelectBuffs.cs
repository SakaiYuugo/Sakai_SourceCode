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

		//ミッションコンプリート数
		allComplete.text = "ミッションコンプリート数　" + ReleaseSkilManager.instance.GetSkilState().PerfectClearNum;

		//Easy全ミッションコンプリート
		easyComplete.color	 = ReleaseSkilManager.instance.GetSkilState().Easy_PerfectClear ?
								Color.white : Color.white * 0.5f;

		//Normal全ミッションコンプリート
		normalComplete.color = ReleaseSkilManager.instance.GetSkilState().Normal_PerfectClear ?
								Color.white : Color.white * 0.5f;

		//Hard全クリア
		hardClear.color		 = ReleaseSkilManager.instance.GetSkilState().Hard_UsuallyClear ?
								Color.white : Color.white * 0.5f;

		//Hard全ミッションコンプリート
		hardComplete.color	 = ReleaseSkilManager.instance.GetSkilState().Hard_PerfectClear ?
								Color.white : Color.white * 0.5f;


		//追加ボム数
		bombNum.text = "　　追加ボム数　+" + (ReleaseSkilManager.instance.GetSkilState().PerfectClearNum * 3).ToString() + "個";

		//初期ボムレベル
		int level = 1;
		level = level + (ReleaseSkilManager.instance.GetSkilState().Easy_PerfectClear ? 1 : 0);
		level = level + (ReleaseSkilManager.instance.GetSkilState().Normal_PerfectClear ? 1 : 0);
		bombLevel.text = "初期ボムレベル　" + level.ToString();

		//無限ばらまき
		isInfiniity.color = ReleaseSkilManager.instance.GetSkilState().Hard_PerfectClear ? Color.white : Color.white * 0.5f;

		//エンディング
		isEnding.color = System_SaveManager.isEndingEnable ? Color.white : Color.white * 0.5f;
	}
}
