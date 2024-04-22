//----- シーン関係
#include "Scene_Title.h"
#include "Scene_Game.h"
#include "Transition.h"
//----- コンポーネント関係
#include "Audio.h"
//-----  システム関係
#include "Manager.h"
#include "Input.h"
//-----  UI
#include "UI_BackGround.h"
#include "UI_TitleLogo.h"
#include "UI_Start.h"
#include "UI_End.h"

/***************************************************************************
[概要]
シーンの初期化

[戻り値]
void
***************************************************************************/
void Scene_Title::Init()
{
	///----- UIを表示
	UI_BackGround* UI_background = AddGameObject<UI_BackGround>(3);
	UI_TitleLogo* UI_titlelogo   = AddGameObject<UI_TitleLogo>(3);
	UI_Start* UI_start           = AddGameObject<UI_Start>(3);
	UI_End* UI_end               = AddGameObject<UI_End>(3);

	//----- BGM
	m_TitleBGM = AddGameObject<GameObject>(1)->AddComponent<Audio>();
	m_TitleBGM->Load("asset/audio/BGM/BGM_Title.wav");
	m_TitleBGM->Play(true, 1.0f);

	//----- フェード
	m_Transition = AddGameObject<Transition>(4);
	m_Transition->FadeIn();
}

/***************************************************************************
[概要]
シーンの更新

[戻り値]
void
***************************************************************************/
void Scene_Title::Update()
{	
	// 入力処理
	KeyPress();

	// 選択中のUIを拡縮
	UI_Scaling();

	// シーン遷移
	SceneTransition();
}

/***************************************************************************
[概要]
入力処理

[戻り値]
void
***************************************************************************/
void Scene_Title::KeyPress()
{
	//----- 時間計測
	m_Time += m_Count;

	//----- キー入力で、ゲームモード選択
	if (Input::GetKeyPress('W') && !m_SelectFlg)
	{
		m_SelectFlg = true;
		UI_select ^= 1;
	}
	if (Input::GetKeyPress('S') && !m_SelectFlg)
	{
		m_SelectFlg = true;
		UI_select ^= 1;
	}

	//----- 一定時間経過で、UIの切り替え可能
	if (m_ExchangeTime <= m_Time)
	{
		m_Time = 0.0f;
		m_SelectFlg = false;
	}
}

/***************************************************************************
[概要]
選択中のUIを拡縮させる

[戻り値]
void
***************************************************************************/
void Scene_Title::UI_Scaling()
{
	//----- 選択されているUIを拡縮させる
	switch (UI_select)
	{
	case 0:
		// Start
		UI_Start::SetSelectFlg(true);
		UI_End::SetSelectFlg(false);
		break;

	case 1:
		// End
		UI_End::SetSelectFlg(true);
		UI_Start::SetSelectFlg(false);
		break;
	}
}

/***************************************************************************
[概要]
シーン遷移

[戻り値]
void
***************************************************************************/
void Scene_Title::SceneTransition()
{
	if (m_Transition->GetState() == Transition::State::Stop)
	{
		if (Input::GetKeyTrigger(VK_SPACE) && UI_Start::GetSelectFlg())
		{
			//----- ゲーム開始
			m_Transition->FadeOut();
		}
		else if (Input::GetKeyTrigger(VK_SPACE) && UI_End::GetSelectFlg())
		{
			//----- ゲーム終了
			HWND hWnd = GetWindow();
			DestroyWindow(hWnd);
		}
	}

	//----- フィード終了後、シーン遷移
	if (m_Transition->GetState() == Transition::State::Finish)
	{
		Manager::SetScene<Scene_Game>();
	}
}
