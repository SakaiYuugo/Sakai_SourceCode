using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PowerCharge : MonoBehaviour
{
    // UI�̐F�Ǘ��p
    public enum PowerState
    {
        GREEN = 0,
        YELOW,
        RED,
        MAX
    };
    PowerState state;

    [SerializeField, Header("UI�I�u�W�F�N�g")]  GameObject[] UI_Charge;
	[SerializeField, Header("UI��fillAmount")] float MaxFillAmount = 0.67f;

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
	/// �`���[�WUI�𐶐�
	/// </summary>
	public void InstPowerChargeUI()
	{
		// �΃Q�[�W�𐶐�
		UI_Object[CountType] = Instantiate(UI_Charge[CountType], this.transform);
		// �I�u�W�F�N�g��Image�R���|�[�l���g�擾
		ImageObj[CountType] = UI_Object[CountType].GetComponent<Image>();
		// fillAmount��������
		ImageObj[CountType].fillAmount = 0.0f;
		// ���X�g�ɒǉ�
		UI_Obj.Add(UI_Charge[CountType]);
	}
	

	/// <summary>
	/// �`���[�WUI�̓���
	/// </summary>
	public void PowerChargeUIMove()
	{
		// 1�b�ōő�T�C�Y�܂ŕω�
		ImageObj[CountType].fillAmount += MaxFillAmount / 1.0f * 0.02f;

		if (MaxFillAmount <= ImageObj[CountType].fillAmount)  { return; }
	}

	/// <summary>
	/// �`���[�W���x�����グ��
	/// </summary>
	public void ChargeUILevelUI()
	{
		if ((int)PowerState.RED <= CountType) { return; }

        // ���x�����オ�邲�Ƃ�UI�̐F��ύX
		CountType++;
		UI_Object[CountType] = Instantiate(UI_Charge[CountType], this.transform);
		ImageObj[CountType] = UI_Object[CountType].GetComponent<Image>();
		ImageObj[CountType].fillAmount = 0.0f;
		UI_Obj.Add(UI_Charge[CountType]);
	}

	/// <summary>
	/// ��������UI���폜
	/// </summary>
	public void DestroyChargeUI()
	{
		// ��������UI�������폜
		for (int i = 0; i <= CountType; i++)
		{
			Destroy(UI_Object[i]);
		}
		
		UI_Obj.Clear();
		CountType = 0;
	}

}
