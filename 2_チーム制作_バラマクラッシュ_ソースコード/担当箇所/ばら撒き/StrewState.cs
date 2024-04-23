using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrewState : MonoBehaviour
{
    //***** �T�E���h�֌W *****
    [Header("SE")]
	[SerializeField] AudioSource SE_Strew;
	[SerializeField] AudioSource SE_Charge;
	[SerializeField] AudioSource SE_ChargeUP;

	//***** �΂�T���֌W *****
	[SerializeField, Range(1, 50), Header("�ő�΂�T����")] public int MaxObjectNum = 10;
	[SerializeField, Header("�ő咷��������(�b)")] public int MaxPushCnt = 3;
	[System.NonSerialized] public int PushCnt;     // ���b�L�[��������Ă��邩�v���p
    private Vector3 StrewPos;    // �ǂ�����΂�T����
    private int     StrewCnt;    // ���݃I�u�W�F�N�g�����΂�T���Ă��邩 
	private bool    StartFlg;    // �ŏ��Ɉ�񂾂���鏈��

	//***** �t���[���֌W *****
	[SerializeField, Header("�N�[���^�C��(�b)")]     private int CoolTime    = 2;
	[SerializeField, Header("�΂�T���ɂ����鎞��")]  private int EndStrewCnt = 30;
	private int EndFrameCnt = 30;

	//***** �t���[���Ǘ��p *****
	private float DeltaTime;    // ���݂̎��Ԃ��i�[����ϐ�
	private bool  PushKeyFlg;   // �{�^����������Ă��邩����p
	private int   FrameTime;    // �t���[���̎��Ԍv���p

	//***** �A�C�e���֌W *****
	[SerializeField, Header("�����΂�T������")] private int InfinityMaxCnt = 180;
	private int InfinityNowCnt;
	private bool InfinityFlg;

	//***** �΂�T���̏�ԑJ�� *****
	public enum NowState
	{
		WAIT = 0,
		PUSHKEY,
		PULLKEY,
		STREW,
		INFINITYSTREW,
		COOLTIME,
		MAX
	};
	NowState State;
	bool InfinityState;

	//***** ���̃X�N���v�g���Q�� *****
	ChoiceObject    ChoiceObj;
	StrewMove       MoveObj;
	StrewCharge     Charge;
	UI_CoolTime     cooltime;
	UI_PowerCharge  PowerCharge;
    IconMove        IconObj;


	void Start()
	{
		// ���̃X�N���v�g���Q��
		ChoiceObj = GetComponent<ChoiceObject>();
		MoveObj = GetComponent<StrewMove>();
		Charge = GameObject.Find("VisualEffect").GetComponentInChildren<StrewCharge>();
		cooltime = GetComponent<UI_CoolTime>();
		PowerCharge = GameObject.Find("PlayerUI").GetComponentInChildren<UI_PowerCharge>();
		IconObj = System_ObjectManager.BuffDebuffIconUI.GetComponent<IconMove>();


		// ������
		State = NowState.WAIT;   // �ŏ��͑ҋ@��Ԃɂ���
		DeltaTime = 0.0f;        // ���Ԍv���p�ϐ�
		FrameTime = 0;           // �t���[���̎��Ԍv���p�ϐ� 
		PushCnt = 0;             // �L�[��������Ă��鎞��
		StrewCnt = 0;            // ���݂̉��΂�T���̐�
		PushKeyFlg = false;      // �L�[��������Ă��邩�ǂ���

		// �����΂�T���֌W
		InfinityFlg    = false;    // ���΂�T���A�C�e�����擾���Ă��邩
		InfinityNowCnt = 0;
		InfinityState  = false;    // �펞�����΂�T����Ԃ�����

		// �ŏ��̈�񂾂����s����ׂ̃t���O
		StartFlg = false;

		// �X�e�[�W�̃N���A�󋵂ł΂�T�����ω�
		ClearStatus();
	}

   
    private void FixedUpdate()
    {
		//----- �I�u�W�F�N�g�𐶐�����ꏊ��ݒ� -----
		StrewPos = (transform.forward * 0.5f) + transform.position;

		switch (State)
		{
			case NowState.WAIT:      // �ҋ@���
				if (InputOrder.SPACE_Key() && !PushKeyFlg)
				{
					State = NowState.PUSHKEY;         // ��Ԃ��L�[��������Ă����Ԃ�
				    PowerCharge.InstPowerChargeUI();  // �΃p���[�Q�[�W�𐶐�
					SE_Charge.Play();

					// �ŏ��̈�񂾂����鏈��
					if (!StartFlg)
					{
						StartFlg = true;
						ChoiceObj.InitStrewObject(); // ���ɂ΂�T���I�u�W�F�N�g��ݒ�
					}
				}
			break; 

			case NowState.PUSHKEY:   // �L�[��������Ă�����
				PushKeyState();
			break;

			case NowState.PULLKEY:   // �L�[�������ꂽ���
				PullKeyState();
			break;

			case NowState.STREW:     // �΂�T�����
				Strew();
				break;

			case NowState.INFINITYSTREW:   // �펞�����΂�T�����
				InfinityStrewState();
				break;

			case NowState.COOLTIME:  // �N�[���^�C�����
				CooltimeState();
				break;
		}
	}

	/// <summary>
	/// �L�[��������Ă���Ԃ̏���
	/// </summary>
	private void PushKeyState()
	{
		PushKeyFlg = true;             // �L�[��������Ă�����					   
		DeltaTime += Time.deltaTime;   // �L�[��������Ă���ԁA���Ԃ��v��

		// �L�[��������Ă��鎞�Ԃ�����𒴂����ꍇ�A����l����
		if (MaxPushCnt <= DeltaTime) { PushCnt = MaxPushCnt; }

		PowerCharge.PowerChargeUIMove();   // �`���[�WUI�𓮂���

		// ��莞�ԃL�[�������Ă���ƃ`���[�W���x�����オ��
		if (1.0f <= DeltaTime)
		{
			// �`���[�W���x�����オ�邲�Ƃ�SE��炷
			if (PushCnt < MaxPushCnt) { SE_ChargeUP.Play(); }			

			DeltaTime = 0.0f;

			PushCnt++;
			if (MaxPushCnt < PushCnt) { PushCnt = MaxPushCnt; }

			
			PowerCharge.ChargeUILevelUI();   // �`���[�W�̃��x���𑝂₷
			Charge.LevelChange(PushCnt);     // �`���[�W�G�t�F�N�g���X�V	
		}

		// �L�[�������ꂽ���ǂ���
		if (!InputOrder.SPACE_Key() && PushKeyFlg) { State = NowState.PULLKEY; } // �{�^���𗣂�����ԂɑJ��

			// ���̃V�[�����`���[�g���A���̏ꍇ
		if (TutorialManager.TutorialNow)
		{
			if (null != GetComponent<Tutorial_BombStrew>())
			{
				GetComponent<Tutorial_BombStrew>().StrewAddCount();
			}
		}
	}

	/// <summary>
	/// �L�[��������Ă����� 
	/// </summary>
	private void PullKeyState()
	{
		// �`���[�WSE���~�߂�
		SE_Charge.Stop();

		if (!InfinityFlg) { cooltime.SetCoolTime(); }  // �N�[���^�C��UI�̕\��

		PowerCharge.DestroyChargeUI();
		Charge.LevelChange(0);          // �`���[�W�G�t�F�N�g������ 
		DeltaTime = 0.0f;               // �N�[���^�C�����v�����邽�ߏ�����

		// �����΂�T����Ԃ�����
		if (InfinityFlg)
        {
            State = NowState.INFINITYSTREW;
        }
		else
        {
            State = NowState.STREW;
        }
	}

	/// <summary>
	/// �΂�T�����
	/// </summary>
	private void Strew()
	{
		// ���̃V�[�����`���[�g���A���̏ꍇ
		if (TutorialManager.TutorialNow)
		{
			if (null != GetComponent<Tutorial_BombLongStrew>())
			{
				GetComponent<Tutorial_BombLongStrew>().LongStrewAddCount();
			}
		}

		// 1��̂΂�T���ɂ����鎞��
		if (FrameTime <= EndStrewCnt)
		{
			// ���݃t���[�����P�O�Ŋ������]�肪�A�O�̏ꍇ
			if (FrameTime % 10 == 0)
			{
				// �ő�΂�T�������獡�΂�T���Ă��鐔�ň��������𐶐�
				int SpownNum = Random.Range(1, MaxObjectNum - StrewCnt);
			
				for (int i = 0; i < SpownNum; i++)
				{
					StrewCnt++;   // �΂�T�����I�u�W�F�N�g�̐����J�E���g
					GameObject tempObj = ChoiceObj.GetStrewObject();   // �΂�T���I�u�W�F�N�g�̎�ނ��擾

					if (tempObj == null) { return; }

                    // �I�u�W�F�N�g���΂�T��
					GameObject StrewObject = Instantiate(tempObj, StrewPos, Quaternion.identity);
					MoveObj.Strew(StrewObject);
					SE_Strew.PlayOneShot(SE_Strew.clip);

					// �΂�T�����I�u�W�F�N�g�����ő吔�𒴂����ꍇ
					if (MaxObjectNum <= StrewCnt)
					{
						FrameTime = EndStrewCnt;
						break;
					}
				}

			}
			FrameTime++;      // �t���[���J�E���g�A�b�v

		}

		// ���ݎ��Ԃ��P��̂΂�T���ɂ����鎞�Ԉȏ�ɂȂ����ꍇ
		if (EndStrewCnt <= FrameTime)
		{
			ChoiceObj.ObjectListDestroy();  // �΂�T���I�u�W�F�N�g��UI���폜
			DeltaTime = 0.0f;           �@  // �N�[���^�C���v���̂��ߏ�����
			State = NowState.COOLTIME;  �@  // �N�[���^�C����Ԃ�
		}
	}

	/// <summary>
	/// �����΂�T�����
	/// </summary>
	private void InfinityStrewState()
	{	
		// 1��̂΂�T���ɂ����鎞��
		if (FrameTime <= EndStrewCnt)
		{
			// ���݃t���[�����P�O�Ŋ������]�肪�A�O�̏ꍇ
			if (FrameTime % 10 == 0)
			{
				// �ő�΂�T�������獡�΂�T���Ă��鐔�ň��������𐶐�
				int SpownNum = Random.Range(1, MaxObjectNum - StrewCnt);

				for (int i = 0; i < SpownNum; i++)
				{
					StrewCnt++;   // ���΂�T�����J�E���g

					GameObject tempObj = ChoiceObj.GetStrewObject();   // �����΂�T�����擾
					if (tempObj == null) { return; }

					GameObject StrewObject = Instantiate(tempObj, StrewPos, Quaternion.identity);
					MoveObj.Strew(StrewObject);   // �΂�T���̓���
					SE_Strew.Play();

					// �΂�T�������ő吔�𒴂����ꍇ
					if (MaxObjectNum <= StrewCnt)
					{
						FrameTime = EndStrewCnt;
						ChoiceObj.ObjectInfinityDestroy();   // �I�u�W�F�N�g��UI�폜
						State      = NowState.WAIT; // �ҋ@��Ԃɂ���
						DeltaTime  = 0.0f;          // ���Ԍv���p�ϐ�
						FrameTime  = 0;             // �t���[��
						PushCnt    = 0;             // �L�[��������Ă��鎞��
						StrewCnt   = 0;             // ���݂̉��΂�T���̐�
						PushKeyFlg = false;         // �L�[��������Ă��邩�ǂ���
						ChoiceObj.InitStrewObject();
						break;
					}
				}

			}
			FrameTime++;  // �t���[���J�E���g�A�b�v
		}

		// �����΂�T���̎�������
		InfinityNowCnt++;

		// �����΂�T����Ԃ̎��Ԍo�߁@���@�펞�����΂�T����ԂłȂ��ꍇ
		if (InfinityMaxCnt < InfinityNowCnt && !InfinityState)
		{
			// �����΂�T����Ԃ�����
			ChoiceObj.ObjectListDestroy();  // �΂�T���I�u�W�F�N�g��UI���폜
			InfinityFlg = false;            // ���΂�T����ԉ���
			FrameTime = EndStrewCnt;        // �t���[���ɍő�l����
			cooltime.SetCoolTime();         // �N�[���^�C�����Z�b�g 
			DeltaTime = 0.0f;          �@�@ // ���Ԍv���p�ϐ�
			State     = NowState.COOLTIME;
		}
	}

	/// <summary>
	/// �N�[���^�C����
	/// </summary>
	private void CooltimeState()
	{
		DeltaTime += Time.deltaTime;

		cooltime.CoolTimeUIMove();   // �N�[���^�C��UI�𓮂���

		if (UI_StrewManager.DestroyCheck()) { ChoiceObj.InitStrewObject(); }

		// �A�C�e�����擾�����ꍇ
		if (ChoiceObj.GetItemFlg() && UI_StrewManager.DestroyCheck()) { ChoiceObj.InitStrewObject(); }


		if (CoolTime <= (int)DeltaTime)
		{
			// ������
			State      = NowState.WAIT; // �ҋ@��Ԃɂ���
			DeltaTime  = 0.0f;          // ���Ԍv���p�ϐ�
			FrameTime  = 0;             // �t���[��
			PushCnt    = 0;             // �L�[��������Ă��鎞��
			StrewCnt   = 0;             // ���݂̉��΂�T���̐�
			PushKeyFlg = false;         // �L�[��������Ă��邩�ǂ���
		}
	}

	/// <summary>
	/// �X�e�[�W�̃N���A�󋵂ɂ���Ă΂�T�����̏���ύX
	/// </summary>
	private void ClearStatus()
	{
		SaveData data = System_SaveManager.savedata;
		SystemLevelManager.LEVELS level = SystemLevelManager.GetLevel_enum();

		// ----- �e�X�e�[�W���p�[�t�F�N�g�N���A�����ꍇ -----
		// �n�`
		for (int i = 0; i < (int)SystemLevelManager.LEVELS.MAX; i++)
		{
			if (System_SaveManager.savedata.BeeIsPerfectCleared[i])
			{
				MaxObjectNum += 3;
			}
		}
		// ���J�f
		for (int i = 0; i < (int)SystemLevelManager.LEVELS.MAX; i++)
		{
			if (data.CentipedeIsPerfectCleared[i])
			{
				MaxObjectNum += 3;
			}
		}
		// �t���R���K�V
		for (int i = 0; i < (int)SystemLevelManager.LEVELS.MAX; i++)
		{
			if (data.DungBeetleIsPerfectCleared[i])
			{
				MaxObjectNum += 3;
			}
		}

		// ----- �e�X�e�[�W�̃C�[�W�[�E�m�[�}���E�n�[�h���N���A�����ꍇ -----
		if (data.BeeIsPerfectCleared[(int)(int)SystemLevelManager.LEVELS.EASY]       &&
			data.CentipedeIsPerfectCleared[(int)(int)SystemLevelManager.LEVELS.EASY] &&
			data.DungBeetleIsPerfectCleared[(int)(int)SystemLevelManager.LEVELS.EASY])
		{
			ChoiceObj.SetLevelUP(ChoiceObject.StrewType.SPHERE);
			ChoiceObj.SetLevelUP(ChoiceObject.StrewType.CYLINDER);
			ChoiceObj.SetLevelUP(ChoiceObject.StrewType.TRIANGLE);
			ChoiceObj.SetLevelUP(ChoiceObject.StrewType.CUBE);
		}
		if (data.BeeIsPerfectCleared[(int)(int)SystemLevelManager.LEVELS.NORMAL]       &&
			data.CentipedeIsPerfectCleared[(int)(int)SystemLevelManager.LEVELS.NORMAL] &&
			data.DungBeetleIsPerfectCleared[(int)(int)SystemLevelManager.LEVELS.NORMAL])
		{
			ChoiceObj.SetLevelUP(ChoiceObject.StrewType.SPHERE);
			ChoiceObj.SetLevelUP(ChoiceObject.StrewType.CYLINDER);
			ChoiceObj.SetLevelUP(ChoiceObject.StrewType.TRIANGLE);
			ChoiceObj.SetLevelUP(ChoiceObject.StrewType.CUBE);
		}
		if (data.BeeIsPerfectCleared[(int)(int)SystemLevelManager.LEVELS.HARD] &&
		data.CentipedeIsPerfectCleared[(int)(int)SystemLevelManager.LEVELS.HARD] &&
		data.DungBeetleIsPerfectCleared[(int)(int)SystemLevelManager.LEVELS.HARD])
		{
			InfinityState = true;
		}

	}

	//------  �Q�b�^�[�E�Z�b�^�[ -----
	/// <summary>�@
	/// ���b�ԃL�[��������Ă�����
	/// </summary>
	public int GetPushTime()
	{
		return PushCnt;
	}

	/// <summary>�@
	/// ���I�u�W�F�N�g���΂�T���邩�擾
	/// </summary>
	public int GetStrewObjectNum()
	{
		return MaxObjectNum;
	}

	/// <summary>
	/// �ő咷�������Ԏ擾
	/// </summary>
	public int GetMaxPushTime()
	{
		return MaxPushCnt;
	}

	/// <summary>�@
	/// �΂�T�����̑���
	/// </summary>
	public void IncreaseStrew()
	{
		MaxObjectNum++;
	}

	/// <summary>
	/// �N�[���^�C���̍ő厞�Ԃ��擾
	/// </summary>
	public float GetMaxCoolTime()
	{
		return CoolTime;
	}

	/// <summary>�@
	/// ���΂�T��
	/// </summary>
	public void InfinityStrew()  
	{
		// �A�C�R���\��
		IconObj.SetIcon(IconMove.IconType.Infinitely, InfinityMaxCnt / 60);
        InfinityFlg = true;
		cooltime.CoolTimeReset();  // �N�[���^�C�������Z�b�g
		State = NowState.WAIT;     // �ҋ@���

		// ������
		DeltaTime = 0.0f;      // ���Ԍv���p�ϐ�
		FrameTime = 0;         // �t���[��
		PushCnt = 0;           // �L�[��������Ă��鎞��
		StrewCnt = 0;          // ���݂̉��΂�T���̐�
		PushKeyFlg = false;    // �L�[��������Ă��邩�ǂ���
		InfinityNowCnt = 0;    // �����΂�T���̎�������
	}

	/// <summary>
	/// �����΂�T����Ԃ��ǂ���
	/// </summary>
	public bool GetInfinityFlg()
	{
		return InfinityFlg;
	}

	/// <summary>�@
	/// ���e�̃��x���A�b�v
	/// </summary>
	public void BombSphereLevelUp()     // ����
	{
		ChoiceObj.SetLevelUP(ChoiceObject.StrewType.SPHERE);
	}
	public void BombCylinderLevelUp()   // �~��
	{
		ChoiceObj.SetLevelUP(ChoiceObject.StrewType.CYLINDER);
	}
	public void BombTriangleLevelUp()   // �O�p
	{
		ChoiceObj.SetLevelUP(ChoiceObject.StrewType.TRIANGLE);
	}
	public void BombCubeLevelUp()       // �l�p
	{
		ChoiceObj.SetLevelUP(ChoiceObject.StrewType.CUBE);
	}
	public void AllBombLevelUP()
	{
		ChoiceObj.SetAllBombLevelUP();
	}


	/// <summary>�@
	/// ���e�̃��x�����Z�b�g
	/// </summary>
	public void BombLevelReset()
	{
		ChoiceObj.SetLevelDown();
	}
}


