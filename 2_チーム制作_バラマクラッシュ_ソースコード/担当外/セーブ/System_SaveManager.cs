using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class System_SaveManager : MonoBehaviour
{
	bool JudgeINTtrigger = false;
    bool JudgeSTRtrigger = false;

	public static System_SaveManager instance = null;

	// �t�@�C���p�X
	public static string dataPath;

    // �Z�[�u�f�[�^
	public static SaveData savedata;

	private void Awake()
	{

		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}

		// �t�@�C���̃p�X���v�Z
		dataPath = Path.Combine(Application.persistentDataPath, "SaveData.json");
		InitData();
		Load();
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		bool InitTrigger = Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.T);
        bool CompTrigger = Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.R);
        if (InitTrigger && !JudgeINTtrigger)
		{
			InitData();
			Save();
		}
        if (CompTrigger && JudgeSTRtrigger)
        {
            InCompData();
            Save();
        }

		JudgeINTtrigger = InitTrigger;
        JudgeSTRtrigger = CompTrigger;
	}

	// ==================================
	// ��1����:�X�e�[�W
	// ��2����:��Փx
	// ��3����:�N���A�������ǂ���
	// ��4����:�N���A�^�C��
	// ��5����:����1�𖞂�������
	// ��6����:����2�𖞂�������
	// ��7����:����3�𖞂�������
	// ==================================
	public static void InData(StageSelect.E_STAGE type, SystemLevelManager.LEVELS level,
		bool isCleared, float CleardTime, bool Con1, bool Con2, bool Con3, bool Perfect = false)
	{
		int OldJudgeNum = 0;
		int NewJudgeNum = 0;

		// �Z�[�u���悤�Ƃ��Ă���N���A���������̌��𐔂���
		if (Con1)
			NewJudgeNum++;
		if (Con2)
			NewJudgeNum++;
		if (Con3)
			NewJudgeNum++;

		switch (type)
		{
		case StageSelect.E_STAGE.BEE_STAGE:        // --- �I�̃X�e�[�W ---
												   // --- ���łɊ��S�N���A���Ă���Ȃ� ---

		if (savedata.BeeIsPerfectCleared[(int)level])
		{
			// ����悤�Ƃ��Ă���N���A���Ԃ��Z�[�u���Ă��鎞�Ԃ�葁���Ȃ�
			if (savedata.BeeCrearedTime[(int)level] > CleardTime)
				savedata.BeeCrearedTime[(int)level] = CleardTime;
			break;
		}
		else
		{
			//  --- �Z�[�u���Ă���N���A���������̌��𐔂��� ---
			if (savedata.BeeCrealedCondition1[(int)level])
				OldJudgeNum++;
			if (savedata.BeeCrealedCondition2[(int)level])
				OldJudgeNum++;
			if (savedata.BeeCrealedCondition3[(int)level])
				OldJudgeNum++;

			// --- �N���A���������̌��������Ȃ������� ---
			if (OldJudgeNum == NewJudgeNum)
			{
				// ����悤�Ƃ��Ă���N���A���Ԃ��Z�[�u���Ă��鎞�Ԃ�葁���Ȃ�
				if (savedata.BeeCrearedTime[(int)level] > CleardTime || savedata.BeeCrearedTime[(int)level] == 0.0f)
					savedata.BeeCrearedTime[(int)level] = CleardTime;
			}
			else if (OldJudgeNum < NewJudgeNum) // ��������
			{
				savedata.BeeCrealedCondition1[(int)level] = Con1;
				savedata.BeeCrealedCondition2[(int)level] = Con2;
				savedata.BeeCrealedCondition3[(int)level] = Con3;
				savedata.BeeIsNormalCleared[(int)level] = isCleared;
				// ����悤�Ƃ��Ă���N���A���Ԃ��Z�[�u���Ă��鎞�Ԃ�葁���Ȃ�
				if (savedata.BeeCrearedTime[(int)level] > CleardTime || savedata.BeeCrearedTime[(int)level] == 0.0f)
					savedata.BeeCrearedTime[(int)level] = CleardTime;

				if (NewJudgeNum >= 3)   //�����N���A
					savedata.BeeIsPerfectCleared[(int)level] = true;
			}
		}
		break;
		case StageSelect.E_STAGE.CENTIPEDE_STAGE:
		// ���łɊ��S�N���A���Ă���Ȃ�
		if (savedata.CentipedeIsPerfectCleared[(int)level])
		{
                    Debug.Log("���S�N���A");
			// ����悤�Ƃ��Ă���N���A���Ԃ��Z�[�u���Ă��鎞�Ԃ�葁���Ȃ�
			if (savedata.CentipedeCrearedTime[(int)level] > CleardTime || savedata.CentipedeCrearedTime[(int)level] == 0.0f)
                    {
                        Debug.Log("���ԓ����");
                        savedata.CentipedeCrearedTime[(int)level] = CleardTime;
                    }
				
			break;
		}
		else
		{
			// �Z�[�u���Ă���N���A���������̌��𐔂���
			if (savedata.CentipedeCrealedCondition1[(int)level])
				OldJudgeNum++;
			if (savedata.CentipedeCrealedCondition2[(int)level])
				OldJudgeNum++;
			if (savedata.CentipedeCrealedCondition3[(int)level])
				OldJudgeNum++;

			// �N���A���������̌��������Ȃ�������
			if (OldJudgeNum == NewJudgeNum)
			{
				// ����悤�Ƃ��Ă���N���A���Ԃ��Z�[�u���Ă��鎞�Ԃ�葁���Ȃ�
				if (savedata.CentipedeCrearedTime[(int)level] > CleardTime || savedata.CentipedeCrearedTime[(int)level] == 0.0f)
                        {
                            savedata.CentipedeCrearedTime[(int)level] = CleardTime;
                            Debug.Log((int)level);
                            Debug.Log(CleardTime);
                            Debug.Log("���ԓ����");
                        }
					
			}
			else if (OldJudgeNum < NewJudgeNum) // ��������
			{
				savedata.CentipedeCrealedCondition1[(int)level] = Con1;
				savedata.CentipedeCrealedCondition2[(int)level] = Con2;
				savedata.CentipedeCrealedCondition3[(int)level] = Con3;
				savedata.CentipedeIsNormalCleared[(int)level] = isCleared;
				// ����悤�Ƃ��Ă���N���A���Ԃ��Z�[�u���Ă��鎞�Ԃ�葁���Ȃ�
				if (savedata.CentipedeCrearedTime[(int)level] > CleardTime || savedata.CentipedeCrearedTime[(int)level] == 0.0f)
                        {
                            savedata.CentipedeCrearedTime[(int)level] = CleardTime;
                            Debug.Log((int)level);
                            Debug.Log(CleardTime);
                            Debug.Log("���ԓ����");
                        }
					

				if (NewJudgeNum >= 3)
					savedata.CentipedeIsPerfectCleared[(int)level] = true;
			}
                    Debug.Log("�f�[�^�����");
		}
		break;

		case StageSelect.E_STAGE.DUNGBEETLE_STAGE:
		// ���łɊ��S�N���A���Ă���Ȃ�
		if (savedata.DungBeetleIsPerfectCleared[(int)level])
		{
			// ����悤�Ƃ��Ă���N���A���Ԃ��Z�[�u���Ă��鎞�Ԃ�葁���Ȃ�
			if (savedata.DungBeetleCrearedTime[(int)level] > CleardTime || savedata.DungBeetleCrearedTime[(int)level] == 0.0f)
				savedata.DungBeetleCrearedTime[(int)level] = CleardTime;
			break;
		}
		else
		{
			// �Z�[�u���Ă���N���A���������̌��𐔂���
			if (savedata.DungBeetleCrealedCondition1[(int)level])
				OldJudgeNum++;
			if (savedata.DungBeetleCrealedCondition2[(int)level])
				OldJudgeNum++;
			if (savedata.DungBeetleCrealedCondition3[(int)level])
				OldJudgeNum++;

			// �N���A���������̌��������Ȃ�������
			if (OldJudgeNum == NewJudgeNum)
			{
				// ����悤�Ƃ��Ă���N���A���Ԃ��Z�[�u���Ă��鎞�Ԃ�葁���Ȃ�
				if (savedata.DungBeetleCrearedTime[(int)level] > CleardTime || savedata.DungBeetleCrearedTime[(int)level] == 0.0f)
					savedata.DungBeetleCrearedTime[(int)level] = CleardTime;
			}
			else if (OldJudgeNum < NewJudgeNum) // ��������
			{
				savedata.DungBeetleCrealedCondition1[(int)level] = Con1;
				savedata.DungBeetleCrealedCondition2[(int)level] = Con2;
				savedata.DungBeetleCrealedCondition3[(int)level] = Con3;
				savedata.DungBeetleIsNormalCleared[(int)level] = isCleared;
				// ����悤�Ƃ��Ă���N���A���Ԃ��Z�[�u���Ă��鎞�Ԃ�葁���Ȃ�
				if (savedata.DungBeetleCrearedTime[(int)level] > CleardTime || savedata.DungBeetleCrearedTime[(int)level] == 0.0f)
					savedata.DungBeetleCrearedTime[(int)level] = CleardTime;

				if (NewJudgeNum >= 3)
					savedata.DungBeetleIsPerfectCleared[(int)level] = true;
			}
		}
		break;
		}
	}

	public static void Save()
	{
		// JSON�`���ɃV���A���C�Y
		var json = JsonUtility.ToJson(savedata, false);

		// JSON�f�[�^���t�@�C���ɕۑ�
		File.WriteAllText(dataPath, json);

        Debug.Log("�Z�[�u");
	}

	public static void Load()
	{
		// �O�̂��߃t�@�C���̑��݃`�F�b�N
		if (!File.Exists(dataPath)) return;

		// JSON�f�[�^�Ƃ��ăf�[�^��ǂݍ���
		var json = File.ReadAllText(dataPath);

		// JSON�`������I�u�W�F�N�g�Ƀf�V���A���C�Y
		var obj = JsonUtility.FromJson<SaveData>(json);

		// Transform�ɃI�u�W�F�N�g�̃f�[�^���Z�b�g
		savedata = obj;
	}

	// ������
	public static void InitData()
	{
		savedata = new SaveData();

		Debug.Log("�Z�[�u������");
	}

	public static void InData(bool isCleared, float CleardTime, bool Con1, bool Con2, bool Con3, bool Perfect)
	{
		InData(StageSelect.E_GetNowSelect(), SystemLevelManager.GetLevel_enum(), isCleared, CleardTime, Con1, Con2, Con3, Perfect);
	}

	public static bool isEndingEnable
	{
		get
		{
			return 
			savedata.BeeIsNormalCleared[(int)SystemLevelManager.LEVELS.HARD] &&
			savedata.CentipedeIsNormalCleared[(int)SystemLevelManager.LEVELS.HARD] &&
			savedata.DungBeetleIsNormalCleared[(int)SystemLevelManager.LEVELS.HARD];
		}
	}

    public void InCompData()
    {
        for (int i = 0; i < (int)SystemLevelManager.LEVELS.MAX;i++)
        {
            savedata.BeeCrealedCondition1[i] = true;
            savedata.BeeCrealedCondition2[i] = true;
            savedata.BeeCrealedCondition3[i] = true;
            savedata.BeeCrearedTime[i] = 0.0f;
            savedata.BeeIsNormalCleared[i] = true;
            savedata.BeeIsPerfectCleared[i] = true;
            savedata.CentipedeCrealedCondition1[i] = true;
            savedata.CentipedeCrealedCondition2[i] = true;
            savedata.CentipedeCrealedCondition3[i] = true;
            savedata.CentipedeCrearedTime[i] = 0.0f;
            savedata.CentipedeIsNormalCleared[i] = true;
            savedata.CentipedeIsPerfectCleared[i] = true;
            savedata.DungBeetleCrealedCondition1[i] = true;
            savedata.DungBeetleCrealedCondition2[i] = true;
            savedata.DungBeetleCrealedCondition3[i] = true;
            savedata.DungBeetleCrearedTime[i] = 0.0f;
            savedata.DungBeetleIsNormalCleared[i] = true;
            savedata.DungBeetleIsPerfectCleared[i] = true;
        }
    }
}
