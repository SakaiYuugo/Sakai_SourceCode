using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class System_SaveManager : MonoBehaviour
{
	bool JudgeINTtrigger = false;
    bool JudgeSTRtrigger = false;

	public static System_SaveManager instance = null;

	// ファイルパス
	public static string dataPath;

    // セーブデータ
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

		// ファイルのパスを計算
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
	// 第1引数:ステージ
	// 第2引数:難易度
	// 第3引数:クリアしたかどうか
	// 第4引数:クリアタイム
	// 第5引数:条件1を満たしたか
	// 第6引数:条件2を満たしたか
	// 第7引数:条件3を満たしたか
	// ==================================
	public static void InData(StageSelect.E_STAGE type, SystemLevelManager.LEVELS level,
		bool isCleared, float CleardTime, bool Con1, bool Con2, bool Con3, bool Perfect = false)
	{
		int OldJudgeNum = 0;
		int NewJudgeNum = 0;

		// セーブしようとしているクリアした条件の個数を数える
		if (Con1)
			NewJudgeNum++;
		if (Con2)
			NewJudgeNum++;
		if (Con3)
			NewJudgeNum++;

		switch (type)
		{
		case StageSelect.E_STAGE.BEE_STAGE:        // --- 蜂のステージ ---
												   // --- すでに完全クリアしているなら ---

		if (savedata.BeeIsPerfectCleared[(int)level])
		{
			// 入れようとしているクリア時間がセーブしている時間より早いなら
			if (savedata.BeeCrearedTime[(int)level] > CleardTime)
				savedata.BeeCrearedTime[(int)level] = CleardTime;
			break;
		}
		else
		{
			//  --- セーブしているクリアした条件の個数を数える ---
			if (savedata.BeeCrealedCondition1[(int)level])
				OldJudgeNum++;
			if (savedata.BeeCrealedCondition2[(int)level])
				OldJudgeNum++;
			if (savedata.BeeCrealedCondition3[(int)level])
				OldJudgeNum++;

			// --- クリアした条件の個数が増えなかったら ---
			if (OldJudgeNum == NewJudgeNum)
			{
				// 入れようとしているクリア時間がセーブしている時間より早いなら
				if (savedata.BeeCrearedTime[(int)level] > CleardTime || savedata.BeeCrearedTime[(int)level] == 0.0f)
					savedata.BeeCrearedTime[(int)level] = CleardTime;
			}
			else if (OldJudgeNum < NewJudgeNum) // 増えたら
			{
				savedata.BeeCrealedCondition1[(int)level] = Con1;
				savedata.BeeCrealedCondition2[(int)level] = Con2;
				savedata.BeeCrealedCondition3[(int)level] = Con3;
				savedata.BeeIsNormalCleared[(int)level] = isCleared;
				// 入れようとしているクリア時間がセーブしている時間より早いなら
				if (savedata.BeeCrearedTime[(int)level] > CleardTime || savedata.BeeCrearedTime[(int)level] == 0.0f)
					savedata.BeeCrearedTime[(int)level] = CleardTime;

				if (NewJudgeNum >= 3)   //完璧クリア
					savedata.BeeIsPerfectCleared[(int)level] = true;
			}
		}
		break;
		case StageSelect.E_STAGE.CENTIPEDE_STAGE:
		// すでに完全クリアしているなら
		if (savedata.CentipedeIsPerfectCleared[(int)level])
		{
                    Debug.Log("完全クリア");
			// 入れようとしているクリア時間がセーブしている時間より早いなら
			if (savedata.CentipedeCrearedTime[(int)level] > CleardTime || savedata.CentipedeCrearedTime[(int)level] == 0.0f)
                    {
                        Debug.Log("時間入れる");
                        savedata.CentipedeCrearedTime[(int)level] = CleardTime;
                    }
				
			break;
		}
		else
		{
			// セーブしているクリアした条件の個数を数える
			if (savedata.CentipedeCrealedCondition1[(int)level])
				OldJudgeNum++;
			if (savedata.CentipedeCrealedCondition2[(int)level])
				OldJudgeNum++;
			if (savedata.CentipedeCrealedCondition3[(int)level])
				OldJudgeNum++;

			// クリアした条件の個数が増えなかったら
			if (OldJudgeNum == NewJudgeNum)
			{
				// 入れようとしているクリア時間がセーブしている時間より早いなら
				if (savedata.CentipedeCrearedTime[(int)level] > CleardTime || savedata.CentipedeCrearedTime[(int)level] == 0.0f)
                        {
                            savedata.CentipedeCrearedTime[(int)level] = CleardTime;
                            Debug.Log((int)level);
                            Debug.Log(CleardTime);
                            Debug.Log("時間入れる");
                        }
					
			}
			else if (OldJudgeNum < NewJudgeNum) // 増えたら
			{
				savedata.CentipedeCrealedCondition1[(int)level] = Con1;
				savedata.CentipedeCrealedCondition2[(int)level] = Con2;
				savedata.CentipedeCrealedCondition3[(int)level] = Con3;
				savedata.CentipedeIsNormalCleared[(int)level] = isCleared;
				// 入れようとしているクリア時間がセーブしている時間より早いなら
				if (savedata.CentipedeCrearedTime[(int)level] > CleardTime || savedata.CentipedeCrearedTime[(int)level] == 0.0f)
                        {
                            savedata.CentipedeCrearedTime[(int)level] = CleardTime;
                            Debug.Log((int)level);
                            Debug.Log(CleardTime);
                            Debug.Log("時間入れる");
                        }
					

				if (NewJudgeNum >= 3)
					savedata.CentipedeIsPerfectCleared[(int)level] = true;
			}
                    Debug.Log("データ入れる");
		}
		break;

		case StageSelect.E_STAGE.DUNGBEETLE_STAGE:
		// すでに完全クリアしているなら
		if (savedata.DungBeetleIsPerfectCleared[(int)level])
		{
			// 入れようとしているクリア時間がセーブしている時間より早いなら
			if (savedata.DungBeetleCrearedTime[(int)level] > CleardTime || savedata.DungBeetleCrearedTime[(int)level] == 0.0f)
				savedata.DungBeetleCrearedTime[(int)level] = CleardTime;
			break;
		}
		else
		{
			// セーブしているクリアした条件の個数を数える
			if (savedata.DungBeetleCrealedCondition1[(int)level])
				OldJudgeNum++;
			if (savedata.DungBeetleCrealedCondition2[(int)level])
				OldJudgeNum++;
			if (savedata.DungBeetleCrealedCondition3[(int)level])
				OldJudgeNum++;

			// クリアした条件の個数が増えなかったら
			if (OldJudgeNum == NewJudgeNum)
			{
				// 入れようとしているクリア時間がセーブしている時間より早いなら
				if (savedata.DungBeetleCrearedTime[(int)level] > CleardTime || savedata.DungBeetleCrearedTime[(int)level] == 0.0f)
					savedata.DungBeetleCrearedTime[(int)level] = CleardTime;
			}
			else if (OldJudgeNum < NewJudgeNum) // 増えたら
			{
				savedata.DungBeetleCrealedCondition1[(int)level] = Con1;
				savedata.DungBeetleCrealedCondition2[(int)level] = Con2;
				savedata.DungBeetleCrealedCondition3[(int)level] = Con3;
				savedata.DungBeetleIsNormalCleared[(int)level] = isCleared;
				// 入れようとしているクリア時間がセーブしている時間より早いなら
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
		// JSON形式にシリアライズ
		var json = JsonUtility.ToJson(savedata, false);

		// JSONデータをファイルに保存
		File.WriteAllText(dataPath, json);

        Debug.Log("セーブ");
	}

	public static void Load()
	{
		// 念のためファイルの存在チェック
		if (!File.Exists(dataPath)) return;

		// JSONデータとしてデータを読み込む
		var json = File.ReadAllText(dataPath);

		// JSON形式からオブジェクトにデシリアライズ
		var obj = JsonUtility.FromJson<SaveData>(json);

		// Transformにオブジェクトのデータをセット
		savedata = obj;
	}

	// 初期化
	public static void InitData()
	{
		savedata = new SaveData();

		Debug.Log("セーブ初期化");
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
