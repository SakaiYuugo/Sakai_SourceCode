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
	List<StrewType> ParameterList;   // 投げる順のリスト
	List<StrewType> TempList;        // 一時保存用の投げる順リスト
	StrewType NowStrewType;          //今の投げるオブジェクトの種類

	[SerializeField] GameObject Bom_Sphere;
	[SerializeField] GameObject Bom_Cylinder;
	[SerializeField] GameObject Bom_Triangle;
	[SerializeField] GameObject Bom_Cube;
	[SerializeField] GameObject Item_Turret;
	[SerializeField] GameObject Item_Beacon;
	[SerializeField, Header("爆弾の最大レベル")] int MaxLevel = 5;
	[SerializeField, Header("爆弾１種類のばら撒き率")] float LimitNum = 0.5f;

	StrewState      state;
	UI_StrewManager UI_Manager;
	UI_BombLevel    UI_Bomblevel;
	private int NextBombNumber;
	private bool GetItem;

	
	void Start()
    {
		//----- 爆弾の初期化 -----
		int StageLevel = SystemLevelManager.GetLevel();

		//----- 他のコンポーネントを取得 -----
		state = GetComponent<StrewState>();
		ParameterList = new List<StrewType>();
		TempList      = new List<StrewType>();
		UI_Manager    = GameObject.Find("StrewObjectList").GetComponentInChildren<UI_StrewManager>();
		UI_Bomblevel  = GameObject.Find("UI_BombLevel").GetComponent<UI_BombLevel>();


		// 最初はアイテムを取得していない状態
		GetItem = false;

		// 生成する爆弾の設定
		StrewObject[(int)StrewType.SPHERE].typeObject   = Bom_Sphere;
		StrewObject[(int)StrewType.CYLINDER].typeObject = Bom_Cylinder;
		StrewObject[(int)StrewType.TRIANGLE].typeObject = Bom_Triangle;
		StrewObject[(int)StrewType.CUBE].typeObject     = Bom_Cube;

	
		// アイテムの初期化
		for (int i = (int)StrewType.MAX - 2; i < (int)StrewType.MAX; i++)
		{
			StrewObject[i].type = (StrewType)i;
		}

		// 生成するアイテムの設定
		StrewObject[(int)StrewType.TURRET].typeObject = Item_Turret;
		StrewObject[(int)StrewType.BEACON].typeObject = Item_Beacon;

		// 各詳細の設定
		int temp = Random.Range((int)StrewType.SPHERE, (int)StrewType.CUBE + 1);
		NowStrewType = (StrewType)temp;

		// 爆弾のレベルを表示
		for (int i = 0; i < (int)StrewType.MAX - 2; i++)
		{
			StrewObject[i].type = (StrewType)i;
			StrewObject[i].Level = 0;
			UI_Bomblevel.InstLevelUI(StrewObject[i]);
			// 爆弾のレベルをセット
			BombBase.SetLevel(StrewObject[i].type, StrewObject[i].Level);
		}
	}



	/// <summary>
	/// 今ばら撒くオブジェクトを初期化
	/// </summary>
	public void InitStrewObject()
	{
		// ばら撒く物がアイテムだった場合
		if (GetItem)
		{
			UI_Manager.CreateBombCage();
			StrewObject[(int)NowStrewType].StrewNum = state.GetStrewObjectNum();  // 何個ばら撒くか
			ParameterList.Add(StrewObject[(int)NowStrewType].type);   // リストにばら撒くアイテム追加
			UI_Manager.InputBombUI(StrewObject[(int)NowStrewType]);   // UIを表示
			return;
		}

		// 何個ばら撒くかのカウント用変数
		int NowObjectNum = 0;

		// UIオブジェクトの親オブジェクトを生成
		if (state.GetInfinityFlg())
		{ UI_Manager.CreateInfinityBombCage(); Debug.Log("無限ｄｓｄｓ");　}
		else
		{ UI_Manager.CreateBombCage(); }
			
		
		// ボムを何個ばら撒くか
		for (int i = 0; i < (int)StrewType.MAX - 2; i++)
		{
		    // 現在のばら撒き数が上限を超えていない場合
		    if (NowObjectNum < state.GetStrewObjectNum())
			{
				// 各タイプが何個ばら撒くか設定
				float tempValue = Random.Range(1, ((state.GetStrewObjectNum() - NowObjectNum) * LimitNum) + 1 );
				StrewObject[i].StrewNum = Mathf.CeilToInt(tempValue);  // 値を切り上げ
				NowObjectNum += StrewObject[i].StrewNum;   // 合計で何個ばら撒かれたかをカウント

				if (state.GetStrewObjectNum() < NowObjectNum)
				{
					StrewObject[i].StrewNum = NowObjectNum - state.GetStrewObjectNum();
				}

				// ばら撒く爆弾のタイプをリストに追加
				ParameterList.Add(StrewObject[i].type);
				// UI表示のためオブジェクトの情報を渡す
				UI_Manager.InputBombUI(StrewObject[i]);
			}
		}
	}


	/// <summary>
	/// UI描画用
	/// </summary>
	public StrewParameter GetStrewParamater(StrewType type)
	{
		return StrewObject[(int)type];
	}


	/// <summary>
	/// ばら撒くオブジェクトを取得
	/// </summary>
	public GameObject GetStrewObject()
	{
		if (ParameterList.Count == 0)
		{
			return null;
		}

		// アイテムの場合
		if (GetItem)
		{
			StrewObject[(int)NowStrewType].StrewNum--;
			return StrewObject[(int)NowStrewType].typeObject;
		}

		// リストの中からランダムで、ばら撒くオブジェクトを設定
		StrewType TempType = (StrewType)Random.Range(0, ParameterList.Count);

		if (StrewObject[(int)TempType].StrewNum == 0)
		{
			ParameterList.RemoveAt((int)TempType);

			// 再抽選
			TempType = (StrewType)Random.Range(0, TempList.Count);
		}

		StrewObject[(int)TempType].StrewNum--;
		return StrewObject[(int)TempType].typeObject;
	}


	/// <summary>
	/// 表示されているUIを全て削除
	/// </summary>
	public void ObjectListDestroy()
	{
		if (GetItem) { GetItem = false; }  // アイテムを取得していたら

		UI_Manager.UninitBombUI();        // UIを削除
		ParameterList.Clear();            // 爆弾のタイプが入ったリストを削除
	}


	/// <summary>
	/// 無限ばら撒き状態の削除関数
	/// </summary>
	public void ObjectInfinityDestroy()
	{
		UI_Manager.UninitInfinityBombUI();
		ParameterList.Clear();
	}

	/// <summary>　
	/// //今の投げるオブジェクトの種類を取得
	/// </summary>
	public StrewType GetNowStrewType()
	{
		return NowStrewType;
	}


	/// <summary>　
	/// 爆弾のレベルアップ
	/// </summary>
	public void SetLevelUP(ChoiceObject.StrewType type)
	{
		// 爆弾のレベルが最大以上の場合　OR　現在のタイプがアイテムだった場合
		if (MaxLevel - 1 <= StrewObject[(int)type].Level || (int)StrewType.MAX - 2 <= (int)type)
		{
			return;
		}

		// ボムのレベルアップ
		StrewObject[(int)type].Level++;
		BombBase.SetLevel(type, StrewObject[(int)type].Level);
		UI_Bomblevel.LevelUPUI(StrewObject[(int)type]);
	}

	/// <summary>　
	/// 全ての爆弾のレベルアップ
	/// </summary>
	public void SetAllBombLevelUP()
	{
		for (int i = 0; i < (int)StrewType.MAX - 2; i++)
		{
			// ボムのレベルアップ
			if (StrewObject[i].Level < MaxLevel - 1) { StrewObject[i].Level++; }

			BombBase.SetLevel((StrewType)i, StrewObject[i].Level);
			UI_Bomblevel.LevelUPUI(StrewObject[i]);
		}
	}


	/// <summary>　
	/// 爆弾のレベルリセット
	/// </summary>
	public void SetLevelDown()   
	{
		// 爆弾の種類分
		for (int i = 0; i < (int)StrewType.MAX - 2; i++)
		{
			if (0 < StrewObject[i].Level) // 爆弾のレベルが１以上の場合
			{
				UI_Bomblevel.LevelResetUI(StrewObject[i]);
				StrewObject[i].Level = 0;  // 爆弾のレベルを初期化
				BombBase.SetLevel(StrewObject[i].type, 1);
			}
		}
	}

	/// <summary>
	/// レベルマックスの爆弾を渡す
	/// </summary>
	public int GetMaxLevelBomb()
	{
		int MaxLevelCnt = 0;
		for (int i = 0; i < (int)StrewType.MAX - 2; i++)
		{
			// 爆弾のレベルが最大の場合
			if (StrewObject[i].Level == MaxLevel - 1)
			{
				MaxLevelCnt++;
			}
		}

		return MaxLevelCnt;
	}

	/// <summary>　
	/// 爆弾のレベルを取得
	/// </summary>
	public int GetLevel(ChoiceObject.StrewType type)
	{
		// タイプがアイテムの場合
		if ((int)StrewType.MAX - 2　<= (int)type)
		{
			return 0;
		}

		return StrewObject[(int)type].Level;
	}

	/// <summary>
	/// アイテムを取得しているかどうか
	/// </summary>
	public bool GetItemFlg()
	{
		return GetItem;
	}

	/// <summary>　
	/// アイテムタレットに当たった場合
	/// </summary>
	public void CollisionTurret() 
	{
		ObjectListDestroy();
		GetItem = true;
		NowStrewType = StrewType.TURRET;
		StrewObject[(int)StrewType.TURRET].NextInst = true;
		StrewObject[(int)StrewType.BEACON].NextInst = false;

	}

	/// <summary>　
	/// アイテムビーコンに当たった場合
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
