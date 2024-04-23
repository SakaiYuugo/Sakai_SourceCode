using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PowerCharge : MonoBehaviour
{
    // UIの色管理用
    public enum PowerState
    {
        GREEN = 0,
        YELOW,
        RED,
        MAX
    };
    PowerState state;

    [SerializeField, Header("UIオブジェクト")]  GameObject[] UI_Charge;
	[SerializeField, Header("UIのfillAmount")] float MaxFillAmount = 0.67f;

	GameObject[] UI_Object = new GameObject[(int)PowerState.MAX];
	Image[] ImageObj       = new Image[(int)PowerState.MAX];

	List<GameObject> UI_Obj;
	StrewState strewState;
	int CountType;
	

	void Start()
	{
		strewState = GameObject.Find("Player").GetComponentInChildren<StrewState>();
		UI_Obj = new List<GameObject>();
		CountType = 0;
	}

	/// <summary>
	/// チャージUIを生成
	/// </summary>
	public void InstPowerChargeUI()
	{
		// 緑ゲージを生成
		UI_Object[CountType] = Instantiate(UI_Charge[CountType], this.transform);
		// オブジェクトのImageコンポーネント取得
		ImageObj[CountType] = UI_Object[CountType].GetComponent<Image>();
		// fillAmountを初期化
		ImageObj[CountType].fillAmount = 0.0f;
		// リストに追加
		UI_Obj.Add(UI_Charge[CountType]);
	}
	

	/// <summary>
	/// チャージUIの動き
	/// </summary>
	public void PowerChargeUIMove()
	{
		// 1秒で最大サイズまで変化
		ImageObj[CountType].fillAmount += MaxFillAmount / 1.0f * 0.02f;

		if (MaxFillAmount <= ImageObj[CountType].fillAmount)  { return; }
	}

	/// <summary>
	/// チャージレベルを上げる
	/// </summary>
	public void ChargeUILevelUI()
	{
		if ((int)PowerState.RED <= CountType) { return; }

        // レベルが上がるごとにUIの色を変更
		CountType++;
		UI_Object[CountType] = Instantiate(UI_Charge[CountType], this.transform);
		ImageObj[CountType] = UI_Object[CountType].GetComponent<Image>();
		ImageObj[CountType].fillAmount = 0.0f;
		UI_Obj.Add(UI_Charge[CountType]);
	}

	/// <summary>
	/// 生成したUIを削除
	/// </summary>
	public void DestroyChargeUI()
	{
		// 生成したUI分だけ削除
		for (int i = 0; i <= CountType; i++)
		{
			Destroy(UI_Object[i]);
		}
		
		UI_Obj.Clear();
		CountType = 0;
	}

}
