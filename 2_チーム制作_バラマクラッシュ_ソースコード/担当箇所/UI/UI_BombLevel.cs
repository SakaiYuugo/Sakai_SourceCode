using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BombLevel : MonoBehaviour
{
    // ���e�̃��x���Ǘ��p
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

	[SerializeField, Header("���x��UI")] GameObject[] level = new GameObject[(int)Level.FIVE];
	[SerializeField, Header("���x��UI�̐e")] GameObject[] Parent = new GameObject[(int)Level.FIVE];

	// ���e�̎�ނ��Ƃ̃��x���Ǘ��p
	public struct LevelUIParameter
	{
		public GameObject[] InstObj;
		public int NowLevel;
	}
	LevelUIParameter[] UIParameters = new LevelUIParameter[(int)Level.FIVE];

	GameObject[] GameObj = new GameObject[(int)Level.MAX];


	private void Awake()
	{
		// ���e�̎�ނ��ƂɁA�ő僌�x�����ǉ�
		for (int i = 0; i < (int)Level.FIVE; i++)
		{
			UIParameters[i].InstObj = new GameObject[(int)Level.MAX];
			UIParameters[i].NowLevel = 0;  // �������x��
		}
	}

	/// <summary>
	/// ���e�̃��x����\��
	/// </summary>
	public void InstLevelUI(ChoiceObject.StrewParameter Parameter)
	{
		if (UIParameters[(int)Parameter.type].NowLevel == (int)Level.MAX)
		{
			return;
		}

		// ���e�̎�ނ��ƂɑΉ������e�I�u�W�F�N�g�̎q�ɐݒ�
		UIParameters[(int)Parameter.type].InstObj[Parameter.Level] = 
			Instantiate( level[(int)Parameter.type], Parent[(int)Parameter.type].transform );

		Parent[(int)Parameter.type].transform.SetParent
			(UIParameters[(int)Parameter.type].InstObj[Parameter.Level].transform, false);

		// ���x�����オ�邲�ƂɈʒu���E�ɂ��炷
		RectTransform rect = UIParameters[(int)Parameter.type].InstObj[Parameter.Level].GetComponent<RectTransform>();
		rect.anchoredPosition += new Vector2( Mathf.Abs((Parameter.Level * (rect.anchoredPosition.x * 0.5f)) ), 
			                                rect.anchoredPosition.y);
	}

	/// <summary>
	/// UI�̃��x�����グ��
	/// </summary>
	public void LevelUPUI(ChoiceObject.StrewParameter Parameter)
	{
		if (UIParameters[(int)Parameter.type].NowLevel == (int)Level.FIVE)
		{
			return;
		}

		UIParameters[(int)Parameter.type].NowLevel++;

		// UI�𐶐�
		InstLevelUI(Parameter);
	}

	/// <summary>
	/// UI�̃��x�����P�Ƀ��Z�b�g
	/// </summary>
	public void LevelResetUI(ChoiceObject.StrewParameter Parameter)
	{
		// ���݂̃��x�����P���傫����
		for (int j = UIParameters[(int)Parameter.type].NowLevel; (int)Level.ONE < j; j--)
		{  
			// ���x�����P�̏ꍇ����ȏヌ�x���������Ȃ�
			if (UIParameters[(int)Parameter.type].NowLevel == 0)
			{
				UIParameters[(int)Parameter.type].NowLevel = 0;  // ���x����������
				return;
			}

			Destroy(UIParameters[(int)Parameter.type].InstObj[UIParameters[(int)Parameter.type].NowLevel]);
			UIParameters[(int)Parameter.type].NowLevel--;
		}
	}

}
