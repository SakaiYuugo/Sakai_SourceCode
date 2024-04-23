using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StrewObj : MonoBehaviour
{
    // 表示するテクスチャの番号
	private enum Number
	{
		ONE = 0,
		TWO,
		THREE,
		FOUR,
		FIVE,
		SIX,
		SEVEN,
		EIGHT,
		NINE,
		TEN,
		MAX
	};
	Number NumberType;

	[SerializeField, Header("ばら撒き数")]
	GameObject[] CountNumUI = new GameObject[(int)Number.TEN]; 

	GameObject CountUI;


	/// <summary>
	/// レベルとばら撒き数を表示
	/// </summary>
	public void InitStrewObjcetUI(ChoiceObject.StrewParameter StrewObjectParameter)
    {
        // アイテムの場合何もしない
        if (CountNumUI == null)
        {
            return;
        }

		// ばら撒く数だけUIを表示
		for (int i = 0; i < CountNumUI.Length; i++)
		{
			if (StrewObjectParameter.StrewNum == i + 1)
			{
				CountUI = Instantiate(CountNumUI[i], this.transform);
				RectTransform Strew = CountUI.GetComponent<RectTransform>();
				Strew.anchoredPosition += new Vector2(30.0f, -30f);
				break;
			}
		}
    }
}

