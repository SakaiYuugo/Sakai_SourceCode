using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StrewManager : MonoBehaviour
{
	// UI�\���֌W 
	[SerializeField, Header("UI�I�u�W�F�N�g")]     GameObject[] UI_StrewObjectPrefab;
    [SerializeField, Header("�\�����鏉���ʒu")]   Vector3 InstPos = Vector3.zero;
    [SerializeField, Header("UI���m�̊Ԋu")]      float BombDistance;
	[SerializeField, Header("UI��\������P�[�W")] GameObject BombCage = null;
	GameObject       Cage;
	UI_StrewObj      UI_Obj;
	GameObject       InstUI;
	List<GameObject> InstUIObjectList;
	int UI_StrewCnt;
	
	//---------- �P�[�W�̈ړ��֌W ----------
	private enum CageState
	{
		INST = 0,
		WAIT,
		DESTROY,
		MAX
	};
	CageState state;   // �P�[�W�̏��
	[SerializeField, Header("�ړ��l")]          float MovemectY = 400.0f;
	[SerializeField, Header("�ړ��ɂ����鎞��")] float deltaTime = 0.05f;
	RectTransform RectCage;

	float MoveTime;
	float moveValue;
	float EndPos;
	static private bool Destroyflg;

	/// <summary>
	/// ������ 
	/// </summary>
	void Start()
    {
		InstUIObjectList = new List<GameObject>();
		UI_StrewCnt = 0;

		state = CageState.INST;
		EndPos = 0.0f;
		Destroyflg = false;

		// �ړ��ʂ̌v�Z
		moveValue = MovemectY * deltaTime;   // �ړ��ʂɎ��Ԃ��|����					 		 
		MoveTime  = MovemectY / moveValue;   // �ړ��ɂ����鎞��
	}

	private void FixedUpdate()
	{
		// �ҋ@��Ԃ̏ꍇ
		if (state == CageState.WAIT�@|| Cage == null) { return; }

		// �I���n�_�ɓ��B����܂ŁA�P�[�W���ړ�
		if (RectCage.anchoredPosition.y <= EndPos)
		{
			RectCage.anchoredPosition += new Vector2(0f, moveValue);
		}

		switch (state)
		{
			case CageState.INST:
				if (EndPos <= RectCage.anchoredPosition.y)
				{   // �ҋ@��ԂɑJ�ڂ��A���Ԃ�������
					state = CageState.WAIT;
					EndPos = 0.0f;   
				}
				break;
			case CageState.DESTROY:
				if (EndPos <= RectCage.anchoredPosition.y)
				{   // �ҋ@��ԂɑJ�ڂ��A���Ԃ�������
					state = CageState.WAIT;
					EndPos = 0.0f;
					Destroyflg = true;  // �폜����
					Destroy(Cage);
				}
				break;
		}
	}

	/// <summary>
	/// �{��UI�Ȃǂ̐����Ȃǂ�����(�����)
	/// </summary>
	public void InputBombUI(ChoiceObject.StrewParameter StrewObjParameter)
    {
		// UI�𐶐�����
		InstUI = Instantiate(�@UI_StrewObjectPrefab[(int)StrewObjParameter.type], 
            Cage.transform.position, Quaternion.identity, Cage.transform);

        // UI�𐶐����������X�g�ɃI�u�W�F�N�g��ǉ�
        InstUIObjectList.Add(InstUI);

		//�g�̒��ɓ����
		Cage.transform.SetParent(InstUI.transform, false);

		// ���X�g�ɂ��鐔���A�ʒu�����炵�ĕ\��
		for (int i = 0; i < InstUIObjectList.Count; i++)
        {
            RectTransform rectTrans = InstUIObjectList[i].GetComponent<RectTransform>();
			rectTrans.anchoredPosition = new Vector2(InstPos.x, InstPos.y + (BombDistance * i));
		}
	

		// �΂�T������\��
		UI_Obj = InstUIObjectList[UI_StrewCnt].GetComponent<UI_StrewObj>();
		UI_Obj.InitStrewObjcetUI(StrewObjParameter);
		// �\������UI���J�E���g
		UI_StrewCnt++;
	}


	/// <summary>
	/// �\������Ă���I�u�W�F�N�g��S�č폜����
	/// </summary>
    public void UninitBombUI()
    {
		if (Cage == null) { return; }

		UI_StrewCnt = 0;

		// ���X�g���̗v�f��S�폜
		InstUIObjectList.Clear();

		// �P�[�W���w��̈ʒu�܂ňړ������A���Ԍo�߂ŃP�[�W���폜
		state  = CageState.DESTROY;
		EndPos = MovemectY; // �I���n�_
	}

	/// <summary>
	/// �����΂�T����Ԃ̍폜�֐�
	/// </summary>
	public void UninitInfinityBombUI()
	{
		if (Cage == null) { return; }

		UI_StrewCnt = 0;

		// ���X�g���̗v�f��S�폜
		InstUIObjectList.Clear();

		state = CageState.WAIT;
		EndPos = 0.0f;
		Destroyflg = true;  // �폜����
		Destroy(Cage);
	}

	/// <summary>
	/// �P�[�W���폜����Ă��邩�ǂ�������
	/// </summary>
	static public bool DestroyCheck()
	{
		return Destroyflg;
	}

	/// <summary>
	/// �P�[�W���쐬
	/// </summary>
	public void CreateBombCage()
	{
		// �폜����
		Destroyflg = false;

		// �P�[�W�������ɐ�����A���S�܂�UI�𓮂���
		Cage = Instantiate(BombCage, this.transform);
		RectCage = Cage.GetComponent<RectTransform>();
		// �I���n�_�ݒ�
		EndPos = RectCage.anchoredPosition.y + MovemectY;

		state = CageState.INST;
	}

    /// <summary>
    /// �����΂�T����Ԃ̃P�[�W���쐬
    /// </summary>
    public void CreateInfinityBombCage()
	{
		// �폜����
		Destroyflg = false;

		// �P�[�W�𐶐�
		Cage = Instantiate(BombCage, this.transform);
		RectCage = Cage.GetComponent<RectTransform>();
		RectCage.anchoredPosition += new Vector2(0f, MovemectY);
		state = CageState.WAIT;
	}
}
