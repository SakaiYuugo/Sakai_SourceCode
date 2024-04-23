using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceObject : MonoBehaviour
{
	public enum StrewType
	{
		SPHERE = 0,
		CYLINDER,
		TRIANGLE,
		CUBE,
		TURRET,
		BEACON,
		MAX
	}

	public struct StrewParameter
	{
		public StrewType type;
		public int Level;
		public int StrewNum;
		public GameObject typeObject;
		public bool NextInst;
	}

	StrewParameter[] StrewObject = new StrewParameter[(int)StrewType.MAX];
	List<StrewType> ParameterList;   // �����鏇�̃��X�g
	List<StrewType> TempList;        // �ꎞ�ۑ��p�̓����鏇���X�g
	StrewType NowStrewType;          //���̓�����I�u�W�F�N�g�̎��

	[SerializeField] GameObject Bom_Sphere;
	[SerializeField] GameObject Bom_Cylinder;
	[SerializeField] GameObject Bom_Triangle;
	[SerializeField] GameObject Bom_Cube;
	[SerializeField] GameObject Item_Turret;
	[SerializeField] GameObject Item_Beacon;
	[SerializeField, Header("���e�̍ő僌�x��")] int MaxLevel = 5;
	[SerializeField, Header("���e�P��ނ̂΂�T����")] float LimitNum = 0.5f;

	StrewState      state;
	UI_StrewManager UI_Manager;
	UI_BombLevel    UI_Bomblevel;
	private int NextBombNumber;
	private bool GetItem;

	
	void Start()
    {
		//----- ���e�̏����� -----
		int StageLevel = SystemLevelManager.GetLevel();

		//----- ���̃R���|�[�l���g���擾 -----
		state = GetComponent<StrewState>();
		ParameterList = new List<StrewType>();
		TempList      = new List<StrewType>();
		UI_Manager    = GameObject.Find("StrewObjectList").GetComponentInChildren<UI_StrewManager>();
		UI_Bomblevel  = GameObject.Find("UI_BombLevel").GetComponent<UI_BombLevel>();


		// �ŏ��̓A�C�e�����擾���Ă��Ȃ����
		GetItem = false;

		// �������锚�e�̐ݒ�
		StrewObject[(int)StrewType.SPHERE].typeObject   = Bom_Sphere;
		StrewObject[(int)StrewType.CYLINDER].typeObject = Bom_Cylinder;
		StrewObject[(int)StrewType.TRIANGLE].typeObject = Bom_Triangle;
		StrewObject[(int)StrewType.CUBE].typeObject     = Bom_Cube;

	
		// �A�C�e���̏�����
		for (int i = (int)StrewType.MAX - 2; i < (int)StrewType.MAX; i++)
		{
			StrewObject[i].type = (StrewType)i;
		}

		// ��������A�C�e���̐ݒ�
		StrewObject[(int)StrewType.TURRET].typeObject = Item_Turret;
		StrewObject[(int)StrewType.BEACON].typeObject = Item_Beacon;

		// �e�ڍׂ̐ݒ�
		int temp = Random.Range((int)StrewType.SPHERE, (int)StrewType.CUBE + 1);
		NowStrewType = (StrewType)temp;

		// ���e�̃��x����\��
		for (int i = 0; i < (int)StrewType.MAX - 2; i++)
		{
			StrewObject[i].type = (StrewType)i;
			StrewObject[i].Level = 0;
			UI_Bomblevel.InstLevelUI(StrewObject[i]);
			// ���e�̃��x�����Z�b�g
			BombBase.SetLevel(StrewObject[i].type, StrewObject[i].Level);
		}
	}



	/// <summary>
	/// ���΂�T���I�u�W�F�N�g��������
	/// </summary>
	public void InitStrewObject()
	{
		// �΂�T�������A�C�e���������ꍇ
		if (GetItem)
		{
			UI_Manager.CreateBombCage();
			StrewObject[(int)NowStrewType].StrewNum = state.GetStrewObjectNum();  // ���΂�T����
			ParameterList.Add(StrewObject[(int)NowStrewType].type);   // ���X�g�ɂ΂�T���A�C�e���ǉ�
			UI_Manager.InputBombUI(StrewObject[(int)NowStrewType]);   // UI��\��
			return;
		}

		// ���΂�T�����̃J�E���g�p�ϐ�
		int NowObjectNum = 0;

		// UI�I�u�W�F�N�g�̐e�I�u�W�F�N�g�𐶐�
		if (state.GetInfinityFlg())
		{ UI_Manager.CreateInfinityBombCage(); Debug.Log("������������");�@}
		else
		{ UI_Manager.CreateBombCage(); }
			
		
		// �{�������΂�T����
		for (int i = 0; i < (int)StrewType.MAX - 2; i++)
		{
		    // ���݂̂΂�T����������𒴂��Ă��Ȃ��ꍇ
		    if (NowObjectNum < state.GetStrewObjectNum())
			{
				// �e�^�C�v�����΂�T�����ݒ�
				float tempValue = Random.Range(1, ((state.GetStrewObjectNum() - NowObjectNum) * LimitNum) + 1 );
				StrewObject[i].StrewNum = Mathf.CeilToInt(tempValue);  // �l��؂�グ
				NowObjectNum += StrewObject[i].StrewNum;   // ���v�ŉ��΂�T���ꂽ�����J�E���g

				if (state.GetStrewObjectNum() < NowObjectNum)
				{
					StrewObject[i].StrewNum = NowObjectNum - state.GetStrewObjectNum();
				}

				// �΂�T�����e�̃^�C�v�����X�g�ɒǉ�
				ParameterList.Add(StrewObject[i].type);
				// UI�\���̂��߃I�u�W�F�N�g�̏���n��
				UI_Manager.InputBombUI(StrewObject[i]);
			}
		}
	}


	/// <summary>
	/// UI�`��p
	/// </summary>
	public StrewParameter GetStrewParamater(StrewType type)
	{
		return StrewObject[(int)type];
	}


	/// <summary>
	/// �΂�T���I�u�W�F�N�g���擾
	/// </summary>
	public GameObject GetStrewObject()
	{
		if (ParameterList.Count == 0)
		{
			return null;
		}

		// �A�C�e���̏ꍇ
		if (GetItem)
		{
			StrewObject[(int)NowStrewType].StrewNum--;
			return StrewObject[(int)NowStrewType].typeObject;
		}

		// ���X�g�̒����烉���_���ŁA�΂�T���I�u�W�F�N�g��ݒ�
		StrewType TempType = (StrewType)Random.Range(0, ParameterList.Count);

		if (StrewObject[(int)TempType].StrewNum == 0)
		{
			ParameterList.RemoveAt((int)TempType);

			// �Ē��I
			TempType = (StrewType)Random.Range(0, TempList.Count);
		}

		StrewObject[(int)TempType].StrewNum--;
		return StrewObject[(int)TempType].typeObject;
	}


	/// <summary>
	/// �\������Ă���UI��S�č폜
	/// </summary>
	public void ObjectListDestroy()
	{
		if (GetItem) { GetItem = false; }  // �A�C�e�����擾���Ă�����

		UI_Manager.UninitBombUI();        // UI���폜
		ParameterList.Clear();            // ���e�̃^�C�v�����������X�g���폜
	}


	/// <summary>
	/// �����΂�T����Ԃ̍폜�֐�
	/// </summary>
	public void ObjectInfinityDestroy()
	{
		UI_Manager.UninitInfinityBombUI();
		ParameterList.Clear();
	}

	/// <summary>�@
	/// //���̓�����I�u�W�F�N�g�̎�ނ��擾
	/// </summary>
	public StrewType GetNowStrewType()
	{
		return NowStrewType;
	}


	/// <summary>�@
	/// ���e�̃��x���A�b�v
	/// </summary>
	public void SetLevelUP(ChoiceObject.StrewType type)
	{
		// ���e�̃��x�����ő�ȏ�̏ꍇ�@OR�@���݂̃^�C�v���A�C�e���������ꍇ
		if (MaxLevel - 1 <= StrewObject[(int)type].Level || (int)StrewType.MAX - 2 <= (int)type)
		{
			return;
		}

		// �{���̃��x���A�b�v
		StrewObject[(int)type].Level++;
		BombBase.SetLevel(type, StrewObject[(int)type].Level);
		UI_Bomblevel.LevelUPUI(StrewObject[(int)type]);
	}

	/// <summary>�@
	/// �S�Ă̔��e�̃��x���A�b�v
	/// </summary>
	public void SetAllBombLevelUP()
	{
		for (int i = 0; i < (int)StrewType.MAX - 2; i++)
		{
			// �{���̃��x���A�b�v
			if (StrewObject[i].Level < MaxLevel - 1) { StrewObject[i].Level++; }

			BombBase.SetLevel((StrewType)i, StrewObject[i].Level);
			UI_Bomblevel.LevelUPUI(StrewObject[i]);
		}
	}


	/// <summary>�@
	/// ���e�̃��x�����Z�b�g
	/// </summary>
	public void SetLevelDown()   
	{
		// ���e�̎�ޕ�
		for (int i = 0; i < (int)StrewType.MAX - 2; i++)
		{
			if (0 < StrewObject[i].Level) // ���e�̃��x�����P�ȏ�̏ꍇ
			{
				UI_Bomblevel.LevelResetUI(StrewObject[i]);
				StrewObject[i].Level = 0;  // ���e�̃��x����������
				BombBase.SetLevel(StrewObject[i].type, 1);
			}
		}
	}

	/// <summary>
	/// ���x���}�b�N�X�̔��e��n��
	/// </summary>
	public int GetMaxLevelBomb()
	{
		int MaxLevelCnt = 0;
		for (int i = 0; i < (int)StrewType.MAX - 2; i++)
		{
			// ���e�̃��x�����ő�̏ꍇ
			if (StrewObject[i].Level == MaxLevel - 1)
			{
				MaxLevelCnt++;
			}
		}

		return MaxLevelCnt;
	}

	/// <summary>�@
	/// ���e�̃��x�����擾
	/// </summary>
	public int GetLevel(ChoiceObject.StrewType type)
	{
		// �^�C�v���A�C�e���̏ꍇ
		if ((int)StrewType.MAX - 2�@<= (int)type)
		{
			return 0;
		}

		return StrewObject[(int)type].Level;
	}

	/// <summary>
	/// �A�C�e�����擾���Ă��邩�ǂ���
	/// </summary>
	public bool GetItemFlg()
	{
		return GetItem;
	}

	/// <summary>�@
	/// �A�C�e���^���b�g�ɓ��������ꍇ
	/// </summary>
	public void CollisionTurret() 
	{
		ObjectListDestroy();
		GetItem = true;
		NowStrewType = StrewType.TURRET;
		StrewObject[(int)StrewType.TURRET].NextInst = true;
		StrewObject[(int)StrewType.BEACON].NextInst = false;

	}

	/// <summary>�@
	/// �A�C�e���r�[�R���ɓ��������ꍇ
	/// </summary>
	public void CollisionBeacon()
	{
		ObjectListDestroy();
		GetItem = true;
		NowStrewType = StrewType.BEACON;
		StrewObject[(int)StrewType.BEACON].NextInst = true;
		StrewObject[(int)StrewType.TURRET].NextInst = false;
		
	}

}
