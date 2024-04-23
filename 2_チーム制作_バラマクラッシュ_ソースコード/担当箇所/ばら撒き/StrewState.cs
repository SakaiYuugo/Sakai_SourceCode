using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrewState : MonoBehaviour
{
    //***** サウンド関係 *****
    [Header("SE")]
	[SerializeField] AudioSource SE_Strew;
	[SerializeField] AudioSource SE_Charge;
	[SerializeField] AudioSource SE_ChargeUP;

	//***** ばら撒き関係 *****
	[SerializeField, Range(1, 50), Header("最大ばら撒き数")] public int MaxObjectNum = 10;
	[SerializeField, Header("最大長押し時間(秒)")] public int MaxPushCnt = 3;
	[System.NonSerialized] public int PushCnt;     // 何秒キーが押されているか計測用
    private Vector3 StrewPos;    // どこからばら撒くか
    private int     StrewCnt;    // 現在オブジェクトを何個ばら撒いているか 
	private bool    StartFlg;    // 最初に一回だけやる処理

	//***** フレーム関係 *****
	[SerializeField, Header("クールタイム(秒)")]     private int CoolTime    = 2;
	[SerializeField, Header("ばら撒きにかかる時間")]  private int EndStrewCnt = 30;
	private int EndFrameCnt = 30;

	//***** フレーム管理用 *****
	private float DeltaTime;    // 現在の時間を格納する変数
	private bool  PushKeyFlg;   // ボタンが押されているか判定用
	private int   FrameTime;    // フレームの時間計測用

	//***** アイテム関係 *****
	[SerializeField, Header("無限ばら撒き時間")] private int InfinityMaxCnt = 180;
	private int InfinityNowCnt;
	private bool InfinityFlg;

	//***** ばら撒きの状態遷移 *****
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

	//***** 他のスクリプトを参照 *****
	ChoiceObject    ChoiceObj;
	StrewMove       MoveObj;
	StrewCharge     Charge;
	UI_CoolTime     cooltime;
	UI_PowerCharge  PowerCharge;
    IconMove        IconObj;


	void Start()
	{
		// 他のスクリプトを参照
		ChoiceObj = GetComponent<ChoiceObject>();
		MoveObj = GetComponent<StrewMove>();
		Charge = GameObject.Find("VisualEffect").GetComponentInChildren<StrewCharge>();
		cooltime = GetComponent<UI_CoolTime>();
		PowerCharge = GameObject.Find("PlayerUI").GetComponentInChildren<UI_PowerCharge>();
		IconObj = System_ObjectManager.BuffDebuffIconUI.GetComponent<IconMove>();


		// 初期化
		State = NowState.WAIT;   // 最初は待機状態にする
		DeltaTime = 0.0f;        // 時間計測用変数
		FrameTime = 0;           // フレームの時間計測用変数 
		PushCnt = 0;             // キーが押されている時間
		StrewCnt = 0;            // 現在の何個ばら撒きの数
		PushKeyFlg = false;      // キーが押されているかどうか

		// 無限ばら撒き関係
		InfinityFlg    = false;    // ∞ばら撒きアイテムを取得しているか
		InfinityNowCnt = 0;
		InfinityState  = false;    // 常時無限ばら撒き状態か判定

		// 最初の一回だけ実行する為のフラグ
		StartFlg = false;

		// ステージのクリア状況でばら撒き数変化
		ClearStatus();
	}

   
    private void FixedUpdate()
    {
		//----- オブジェクトを生成する場所を設定 -----
		StrewPos = (transform.forward * 0.5f) + transform.position;

		switch (State)
		{
			case NowState.WAIT:      // 待機状態
				if (InputOrder.SPACE_Key() && !PushKeyFlg)
				{
					State = NowState.PUSHKEY;         // 状態をキーが押されている状態に
				    PowerCharge.InstPowerChargeUI();  // 緑パワーゲージを生成
					SE_Charge.Play();

					// 最初の一回だけする処理
					if (!StartFlg)
					{
						StartFlg = true;
						ChoiceObj.InitStrewObject(); // 次にばら撒くオブジェクトを設定
					}
				}
			break; 

			case NowState.PUSHKEY:   // キーが押されている状態
				PushKeyState();
			break;

			case NowState.PULLKEY:   // キーが離された状態
				PullKeyState();
			break;

			case NowState.STREW:     // ばら撒き状態
				Strew();
				break;

			case NowState.INFINITYSTREW:   // 常時無限ばら撒き状態
				InfinityStrewState();
				break;

			case NowState.COOLTIME:  // クールタイム状態
				CooltimeState();
				break;
		}
	}

	/// <summary>
	/// キーが押されている間の処理
	/// </summary>
	private void PushKeyState()
	{
		PushKeyFlg = true;             // キーが押されている状態					   
		DeltaTime += Time.deltaTime;   // キーが押されている間、時間を計測

		// キーが押されている時間が上限を超えた場合、上限値を代入
		if (MaxPushCnt <= DeltaTime) { PushCnt = MaxPushCnt; }

		PowerCharge.PowerChargeUIMove();   // チャージUIを動かす

		// 一定時間キーを押しているとチャージレベルが上がる
		if (1.0f <= DeltaTime)
		{
			// チャージレベルが上がるごとにSEを鳴らす
			if (PushCnt < MaxPushCnt) { SE_ChargeUP.Play(); }			

			DeltaTime = 0.0f;

			PushCnt++;
			if (MaxPushCnt < PushCnt) { PushCnt = MaxPushCnt; }

			
			PowerCharge.ChargeUILevelUI();   // チャージのレベルを増やす
			Charge.LevelChange(PushCnt);     // チャージエフェクトを更新	
		}

		// キーが離されたかどうか
		if (!InputOrder.SPACE_Key() && PushKeyFlg) { State = NowState.PULLKEY; } // ボタンを離した状態に遷移

			// 今のシーンがチュートリアルの場合
		if (TutorialManager.TutorialNow)
		{
			if (null != GetComponent<Tutorial_BombStrew>())
			{
				GetComponent<Tutorial_BombStrew>().StrewAddCount();
			}
		}
	}

	/// <summary>
	/// キーが離されている状態 
	/// </summary>
	private void PullKeyState()
	{
		// チャージSEを止める
		SE_Charge.Stop();

		if (!InfinityFlg) { cooltime.SetCoolTime(); }  // クールタイムUIの表示

		PowerCharge.DestroyChargeUI();
		Charge.LevelChange(0);          // チャージエフェクトを消す 
		DeltaTime = 0.0f;               // クールタイムを計測するため初期化

		// 無限ばら撒き状態か判定
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
	/// ばら撒き状態
	/// </summary>
	private void Strew()
	{
		// 今のシーンがチュートリアルの場合
		if (TutorialManager.TutorialNow)
		{
			if (null != GetComponent<Tutorial_BombLongStrew>())
			{
				GetComponent<Tutorial_BombLongStrew>().LongStrewAddCount();
			}
		}

		// 1回のばら撒きにかかる時間
		if (FrameTime <= EndStrewCnt)
		{
			// 現在フレームを１０で割った余りが、０の場合
			if (FrameTime % 10 == 0)
			{
				// 最大ばら撒き数から今ばら撒いている数で引いた数を生成
				int SpownNum = Random.Range(1, MaxObjectNum - StrewCnt);
			
				for (int i = 0; i < SpownNum; i++)
				{
					StrewCnt++;   // ばら撒いたオブジェクトの数をカウント
					GameObject tempObj = ChoiceObj.GetStrewObject();   // ばら撒くオブジェクトの種類を取得

					if (tempObj == null) { return; }

                    // オブジェクトをばら撒く
					GameObject StrewObject = Instantiate(tempObj, StrewPos, Quaternion.identity);
					MoveObj.Strew(StrewObject);
					SE_Strew.PlayOneShot(SE_Strew.clip);

					// ばら撒いたオブジェクト数が最大数を超えた場合
					if (MaxObjectNum <= StrewCnt)
					{
						FrameTime = EndStrewCnt;
						break;
					}
				}

			}
			FrameTime++;      // フレームカウントアップ

		}

		// 現在時間が１回のばら撒きにかかる時間以上になった場合
		if (EndStrewCnt <= FrameTime)
		{
			ChoiceObj.ObjectListDestroy();  // ばら撒くオブジェクトとUIを削除
			DeltaTime = 0.0f;           　  // クールタイム計測のため初期化
			State = NowState.COOLTIME;  　  // クールタイム状態に
		}
	}

	/// <summary>
	/// 無限ばら撒き状態
	/// </summary>
	private void InfinityStrewState()
	{	
		// 1回のばら撒きにかかる時間
		if (FrameTime <= EndStrewCnt)
		{
			// 現在フレームを１０で割った余りが、０の場合
			if (FrameTime % 10 == 0)
			{
				// 最大ばら撒き数から今ばら撒いている数で引いた数を生成
				int SpownNum = Random.Range(1, MaxObjectNum - StrewCnt);

				for (int i = 0; i < SpownNum; i++)
				{
					StrewCnt++;   // 何個ばら撒きかカウント

					GameObject tempObj = ChoiceObj.GetStrewObject();   // 何をばら撒くか取得
					if (tempObj == null) { return; }

					GameObject StrewObject = Instantiate(tempObj, StrewPos, Quaternion.identity);
					MoveObj.Strew(StrewObject);   // ばら撒きの動き
					SE_Strew.Play();

					// ばら撒く数が最大数を超えた場合
					if (MaxObjectNum <= StrewCnt)
					{
						FrameTime = EndStrewCnt;
						ChoiceObj.ObjectInfinityDestroy();   // オブジェクトとUI削除
						State      = NowState.WAIT; // 待機状態にする
						DeltaTime  = 0.0f;          // 時間計測用変数
						FrameTime  = 0;             // フレーム
						PushCnt    = 0;             // キーが押されている時間
						StrewCnt   = 0;             // 現在の何個ばら撒きの数
						PushKeyFlg = false;         // キーが押されているかどうか
						ChoiceObj.InitStrewObject();
						break;
					}
				}

			}
			FrameTime++;  // フレームカウントアップ
		}

		// 無限ばら撒きの持続時間
		InfinityNowCnt++;

		// 無限ばら撒き状態の時間経過　＆　常時無限ばら撒き状態でない場合
		if (InfinityMaxCnt < InfinityNowCnt && !InfinityState)
		{
			// 無限ばら撒き状態を解除
			ChoiceObj.ObjectListDestroy();  // ばら撒くオブジェクトとUIを削除
			InfinityFlg = false;            // ∞ばら撒き状態解除
			FrameTime = EndStrewCnt;        // フレームに最大値を代入
			cooltime.SetCoolTime();         // クールタイムをセット 
			DeltaTime = 0.0f;          　　 // 時間計測用変数
			State     = NowState.COOLTIME;
		}
	}

	/// <summary>
	/// クールタイム中
	/// </summary>
	private void CooltimeState()
	{
		DeltaTime += Time.deltaTime;

		cooltime.CoolTimeUIMove();   // クールタイムUIを動かす

		if (UI_StrewManager.DestroyCheck()) { ChoiceObj.InitStrewObject(); }

		// アイテムを取得した場合
		if (ChoiceObj.GetItemFlg() && UI_StrewManager.DestroyCheck()) { ChoiceObj.InitStrewObject(); }


		if (CoolTime <= (int)DeltaTime)
		{
			// 初期化
			State      = NowState.WAIT; // 待機状態にする
			DeltaTime  = 0.0f;          // 時間計測用変数
			FrameTime  = 0;             // フレーム
			PushCnt    = 0;             // キーが押されている時間
			StrewCnt   = 0;             // 現在の何個ばら撒きの数
			PushKeyFlg = false;         // キーが押されているかどうか
		}
	}

	/// <summary>
	/// ステージのクリア状況によってばら撒き数の上限変更
	/// </summary>
	private void ClearStatus()
	{
		SaveData data = System_SaveManager.savedata;
		SystemLevelManager.LEVELS level = SystemLevelManager.GetLevel_enum();

		// ----- 各ステージをパーフェクトクリアした場合 -----
		// ハチ
		for (int i = 0; i < (int)SystemLevelManager.LEVELS.MAX; i++)
		{
			if (System_SaveManager.savedata.BeeIsPerfectCleared[i])
			{
				MaxObjectNum += 3;
			}
		}
		// ムカデ
		for (int i = 0; i < (int)SystemLevelManager.LEVELS.MAX; i++)
		{
			if (data.CentipedeIsPerfectCleared[i])
			{
				MaxObjectNum += 3;
			}
		}
		// フンコロガシ
		for (int i = 0; i < (int)SystemLevelManager.LEVELS.MAX; i++)
		{
			if (data.DungBeetleIsPerfectCleared[i])
			{
				MaxObjectNum += 3;
			}
		}

		// ----- 各ステージのイージー・ノーマル・ハードをクリアした場合 -----
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

	//------  ゲッター・セッター -----
	/// <summary>　
	/// 何秒間キーが押されていたか
	/// </summary>
	public int GetPushTime()
	{
		return PushCnt;
	}

	/// <summary>　
	/// 何個オブジェクトをばら撒けるか取得
	/// </summary>
	public int GetStrewObjectNum()
	{
		return MaxObjectNum;
	}

	/// <summary>
	/// 最大長押し時間取得
	/// </summary>
	public int GetMaxPushTime()
	{
		return MaxPushCnt;
	}

	/// <summary>　
	/// ばら撒き数の増加
	/// </summary>
	public void IncreaseStrew()
	{
		MaxObjectNum++;
	}

	/// <summary>
	/// クールタイムの最大時間を取得
	/// </summary>
	public float GetMaxCoolTime()
	{
		return CoolTime;
	}

	/// <summary>　
	/// ∞ばら撒き
	/// </summary>
	public void InfinityStrew()  
	{
		// アイコン表示
		IconObj.SetIcon(IconMove.IconType.Infinitely, InfinityMaxCnt / 60);
        InfinityFlg = true;
		cooltime.CoolTimeReset();  // クールタイムをリセット
		State = NowState.WAIT;     // 待機状態

		// 初期化
		DeltaTime = 0.0f;      // 時間計測用変数
		FrameTime = 0;         // フレーム
		PushCnt = 0;           // キーが押されている時間
		StrewCnt = 0;          // 現在の何個ばら撒きの数
		PushKeyFlg = false;    // キーが押されているかどうか
		InfinityNowCnt = 0;    // 無限ばら撒きの持続時間
	}

	/// <summary>
	/// 無限ばら撒き状態かどうか
	/// </summary>
	public bool GetInfinityFlg()
	{
		return InfinityFlg;
	}

	/// <summary>　
	/// 爆弾のレベルアップ
	/// </summary>
	public void BombSphereLevelUp()     // 球体
	{
		ChoiceObj.SetLevelUP(ChoiceObject.StrewType.SPHERE);
	}
	public void BombCylinderLevelUp()   // 円柱
	{
		ChoiceObj.SetLevelUP(ChoiceObject.StrewType.CYLINDER);
	}
	public void BombTriangleLevelUp()   // 三角
	{
		ChoiceObj.SetLevelUP(ChoiceObject.StrewType.TRIANGLE);
	}
	public void BombCubeLevelUp()       // 四角
	{
		ChoiceObj.SetLevelUP(ChoiceObject.StrewType.CUBE);
	}
	public void AllBombLevelUP()
	{
		ChoiceObj.SetAllBombLevelUP();
	}


	/// <summary>　
	/// 爆弾のレベルリセット
	/// </summary>
	public void BombLevelReset()
	{
		ChoiceObj.SetLevelDown();
	}
}


