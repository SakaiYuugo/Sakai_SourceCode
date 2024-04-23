using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StrewManager : MonoBehaviour
{
	// UI表示関係 
	[SerializeField, Header("UIオブジェクト")]     GameObject[] UI_StrewObjectPrefab;
    [SerializeField, Header("表示する初期位置")]   Vector3 InstPos = Vector3.zero;
    [SerializeField, Header("UI同士の間隔")]      float BombDistance;
	[SerializeField, Header("UIを表示するケージ")] GameObject BombCage = null;
	GameObject       Cage;
	UI_StrewObj      UI_Obj;
	GameObject       InstUI;
	List<GameObject> InstUIObjectList;
	int UI_StrewCnt;
	
	//---------- ケージの移動関係 ----------
	private enum CageState
	{
		INST = 0,
		WAIT,
		DESTROY,
		MAX
	};
	CageState state;   // ケージの状態
	[SerializeField, Header("移動値")]          float MovemectY = 400.0f;
	[SerializeField, Header("移動にかかる時間")] float deltaTime = 0.05f;
	RectTransform RectCage;

	float MoveTime;
	float moveValue;
	float EndPos;
	static private bool Destroyflg;

	/// <summary>
	/// 初期化 
	/// </summary>
	void Start()
    {
		InstUIObjectList = new List<GameObject>();
		UI_StrewCnt = 0;

		state = CageState.INST;
		EndPos = 0.0f;
		Destroyflg = false;

		// 移動量の計算
		moveValue = MovemectY * deltaTime;   // 移動量に時間を掛ける					 		 
		MoveTime  = MovemectY / moveValue;   // 移動にかかる時間
	}

	private void FixedUpdate()
	{
		// 待機状態の場合
		if (state == CageState.WAIT　|| Cage == null) { return; }

		// 終了地点に到達するまで、ケージを移動
		if (RectCage.anchoredPosition.y <= EndPos)
		{
			RectCage.anchoredPosition += new Vector2(0f, moveValue);
		}

		switch (state)
		{
			case CageState.INST:
				if (EndPos <= RectCage.anchoredPosition.y)
				{   // 待機状態に遷移し、時間を初期化
					state = CageState.WAIT;
					EndPos = 0.0f;   
				}
				break;
			case CageState.DESTROY:
				if (EndPos <= RectCage.anchoredPosition.y)
				{   // 待機状態に遷移し、時間を初期化
					state = CageState.WAIT;
					EndPos = 0.0f;
					Destroyflg = true;  // 削除判定
					Destroy(Cage);
				}
				break;
		}
	}

	/// <summary>
	/// ボムUIなどの生成などをする(一個ずつ)
	/// </summary>
	public void InputBombUI(ChoiceObject.StrewParameter StrewObjParameter)
    {
		// UIを生成する
		InstUI = Instantiate(　UI_StrewObjectPrefab[(int)StrewObjParameter.type], 
            Cage.transform.position, Quaternion.identity, Cage.transform);

        // UIを生成した分リストにオブジェクトを追加
        InstUIObjectList.Add(InstUI);

		//枠の中に入れる
		Cage.transform.SetParent(InstUI.transform, false);

		// リストにある数分、位置をずらして表示
		for (int i = 0; i < InstUIObjectList.Count; i++)
        {
            RectTransform rectTrans = InstUIObjectList[i].GetComponent<RectTransform>();
			rectTrans.anchoredPosition = new Vector2(InstPos.x, InstPos.y + (BombDistance * i));
		}
	

		// ばら撒き数を表示
		UI_Obj = InstUIObjectList[UI_StrewCnt].GetComponent<UI_StrewObj>();
		UI_Obj.InitStrewObjcetUI(StrewObjParameter);
		// 表示したUIをカウント
		UI_StrewCnt++;
	}


	/// <summary>
	/// 表示されているオブジェクトを全て削除する
	/// </summary>
    public void UninitBombUI()
    {
		if (Cage == null) { return; }

		UI_StrewCnt = 0;

		// リスト内の要素を全削除
		InstUIObjectList.Clear();

		// ケージを指定の位置まで移動させ、時間経過でケージを削除
		state  = CageState.DESTROY;
		EndPos = MovemectY; // 終了地点
	}

	/// <summary>
	/// 無限ばら撒き状態の削除関数
	/// </summary>
	public void UninitInfinityBombUI()
	{
		if (Cage == null) { return; }

		UI_StrewCnt = 0;

		// リスト内の要素を全削除
		InstUIObjectList.Clear();

		state = CageState.WAIT;
		EndPos = 0.0f;
		Destroyflg = true;  // 削除判定
		Destroy(Cage);
	}

	/// <summary>
	/// ケージが削除されているかどうか判定
	/// </summary>
	static public bool DestroyCheck()
	{
		return Destroyflg;
	}

	/// <summary>
	/// ケージを作成
	/// </summary>
	public void CreateBombCage()
	{
		// 削除判定
		Destroyflg = false;

		// ケージを下部に生成後、中心までUIを動かす
		Cage = Instantiate(BombCage, this.transform);
		RectCage = Cage.GetComponent<RectTransform>();
		// 終了地点設定
		EndPos = RectCage.anchoredPosition.y + MovemectY;

		state = CageState.INST;
	}

    /// <summary>
    /// 無限ばら撒き状態のケージを作成
    /// </summary>
    public void CreateInfinityBombCage()
	{
		// 削除判定
		Destroyflg = false;

		// ケージを生成
		Cage = Instantiate(BombCage, this.transform);
		RectCage = Cage.GetComponent<RectTransform>();
		RectCage.anchoredPosition += new Vector2(0f, MovemectY);
		state = CageState.WAIT;
	}
}
