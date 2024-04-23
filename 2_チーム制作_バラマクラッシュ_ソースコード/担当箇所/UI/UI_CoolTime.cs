using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CoolTime : MonoBehaviour
{
	[SerializeField, Header("UIのfillAmount(最大値)")] float FillAmount = 0.64f;
	GameObject UI_Obj;
    GameObject Ui_CoolTime;
    Image ImageObj;
	StrewState strewState;
	private bool MoveFlg;
	float Cooltime;

	void Start()
	{
		// 他のスクリプト参照
		strewState = GetComponent<StrewState>();

		// UIオブジェクトを取得
		Ui_CoolTime = GameObject.Find("UI_CoolTime");
		ImageObj = Ui_CoolTime.GetComponent<Image>();

		// 初期はテクスチャを見えないようにする
		ImageObj.fillAmount = 0.0f;
		MoveFlg = false;
		Cooltime = 0.0f;
	}

	/// <summary>
	/// クールタイムUIを表示
	/// </summary>
	public void SetCoolTime()
	{
		ImageObj.fillAmount = 0.64f;
		MoveFlg = true;
		Cooltime = strewState.GetMaxCoolTime();	
	}


	/// <summary>
	/// クールタイムUIの動き
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
	/// クールタイムの値をリセット
	/// </summary>
	public void CoolTimeReset()
	{
		ImageObj.fillAmount = 0.0f;
		MoveFlg = false;
		Cooltime = 0.0f;
	}
}
