using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BombLevel : MonoBehaviour
{
    // 爆弾のレベル管理用
	private enum Level
	{
		ONE = 0,
		TWO,
		THREE,
		FOUR,
		FIVE,
		MAX
	};
	Level LevelNum;

	[SerializeField, Header("レベルUI")] GameObject[] level = new GameObject[(int)Level.FIVE];
	[SerializeField, Header("レベルUIの親")] GameObject[] Parent = new GameObject[(int)Level.FIVE];

	// 爆弾の種類ごとのレベル管理用
	public struct LevelUIParameter
	{
		public GameObject[] InstObj;
		public int NowLevel;
	}
	LevelUIParameter[] UIParameters = new LevelUIParameter[(int)Level.FIVE];

	GameObject[] GameObj = new GameObject[(int)Level.MAX];


	private void Awake()
	{
		// 爆弾の種類ごとに、最大レベル分追加
		for (int i = 0; i < (int)Level.FIVE; i++)
		{
			UIParameters[i].InstObj = new GameObject[(int)Level.MAX];
			UIParameters[i].NowLevel = 0;  // 初期レベル
		}
	}

	/// <summary>
	/// 爆弾のレベルを表示
	/// </summary>
	public void InstLevelUI(ChoiceObject.StrewParameter Parameter)
	{
		if (UIParameters[(int)Parameter.type].NowLevel == (int)Level.MAX)
		{
			return;
		}

		// 爆弾の種類ごとに対応した親オブジェクトの子に設定
		UIParameters[(int)Parameter.type].InstObj[Parameter.Level] = 
			Instantiate( level[(int)Parameter.type], Parent[(int)Parameter.type].transform );

		Parent[(int)Parameter.type].transform.SetParent
			(UIParameters[(int)Parameter.type].InstObj[Parameter.Level].transform, false);

		// レベルが上がるごとに位置を右にずらす
		RectTransform rect = UIParameters[(int)Parameter.type].InstObj[Parameter.Level].GetComponent<RectTransform>();
		rect.anchoredPosition += new Vector2( Mathf.Abs((Parameter.Level * (rect.anchoredPosition.x * 0.5f)) ), 
			                                rect.anchoredPosition.y);
	}

	/// <summary>
	/// UIのレベルを上げる
	/// </summary>
	public void LevelUPUI(ChoiceObject.StrewParameter Parameter)
	{
		if (UIParameters[(int)Parameter.type].NowLevel == (int)Level.FIVE)
		{
			return;
		}

		UIParameters[(int)Parameter.type].NowLevel++;

		// UIを生成
		InstLevelUI(Parameter);
	}

	/// <summary>
	/// UIのレベルを１にリセット
	/// </summary>
	public void LevelResetUI(ChoiceObject.StrewParameter Parameter)
	{
		// 現在のレベルが１より大きい分
		for (int j = UIParameters[(int)Parameter.type].NowLevel; (int)Level.ONE < j; j--)
		{  
			// レベルが１の場合それ以上レベルを下げない
			if (UIParameters[(int)Parameter.type].NowLevel == 0)
			{
				UIParameters[(int)Parameter.type].NowLevel = 0;  // レベルを初期化
				return;
			}

			Destroy(UIParameters[(int)Parameter.type].InstObj[UIParameters[(int)Parameter.type].NowLevel]);
			UIParameters[(int)Parameter.type].NowLevel--;
		}
	}

}
