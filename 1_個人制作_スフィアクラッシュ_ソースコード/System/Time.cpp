#include "Time.h"
#include "UI_Timer.h"

//----- 静的変数
DWORD Time::m_dwExecLastTime;
DWORD Time::m_dwCurrentTime;
float Time::m_Frame;
bool  Time::m_HitStopFlg;
int   Time::m_GameCount;

/***************************************************************************
[概要]
初期化処理

[戻り値]
void
***************************************************************************/
void Time::Init()
{
	timeBeginPeriod(1);

	//----- プログラム開始時の時間を取得
	m_dwExecLastTime = timeGetTime();

	//----- 最初のアップデート用
	m_dwCurrentTime = 0;

	m_HitStopFlg = false;
}

/***************************************************************************
[概要]
メモリの解放処理

[戻り値]
void
***************************************************************************/
void Time::Uninit()
{
	timeEndPeriod(1);
}

/***************************************************************************
[概要]
更新処理

[戻り値]
bool 1秒経過しているか判定
***************************************************************************/
bool Time::Update()
{
	//----- 現在時間取得
	m_dwCurrentTime = timeGetTime();

	//----- UIに現在時間をセット(秒単位)
	UI_Timer::SetCurrentTime((m_dwCurrentTime - m_dwExecLastTime) / 1000);

	//----- 経過時間を計算(秒単位)
	if ((m_dwCurrentTime - m_dwExecLastTime) / 1000.0f  >= UI_Timer::GetTimeLimit()) 
	{
		m_dwExecLastTime = m_dwCurrentTime;
		return true;
	}

	return false;
}

