using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StrewObj : MonoBehaviour
{
    // �\������e�N�X�`���̔ԍ�
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

	[SerializeField, Header("�΂�T����")]
	GameObject[] CountNumUI = new GameObject[(int)Number.TEN]; 

	GameObject CountUI;


	/// <summary>
	/// ���x���Ƃ΂�T������\��
	/// </summary>
	public void InitStrewObjcetUI(ChoiceObject.StrewParameter StrewObjectParameter)
    {
        // �A�C�e���̏ꍇ�������Ȃ�
        if (CountNumUI == null)
        {
            return;
        }

		// �΂�T��������UI��\��
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

