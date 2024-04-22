//----- �V�[���֌W
#include "Scene_Title.h"
#include "Scene_Game.h"
#include "Transition.h"
//----- �R���|�[�l���g�֌W
#include "Audio.h"
//-----  �V�X�e���֌W
#include "Manager.h"
#include "Input.h"
//-----  UI
#include "UI_BackGround.h"
#include "UI_TitleLogo.h"
#include "UI_Start.h"
#include "UI_End.h"

/***************************************************************************
[�T�v]
�V�[���̏�����

[�߂�l]
void
***************************************************************************/
void Scene_Title::Init()
{
	///----- UI��\��
	UI_BackGround* UI_background = AddGameObject<UI_BackGround>(3);
	UI_TitleLogo* UI_titlelogo   = AddGameObject<UI_TitleLogo>(3);
	UI_Start* UI_start           = AddGameObject<UI_Start>(3);
	UI_End* UI_end               = AddGameObject<UI_End>(3);

	//----- BGM
	m_TitleBGM = AddGameObject<GameObject>(1)->AddComponent<Audio>();
	m_TitleBGM->Load("asset/audio/BGM/BGM_Title.wav");
	m_TitleBGM->Play(true, 1.0f);

	//----- �t�F�[�h
	m_Transition = AddGameObject<Transition>(4);
	m_Transition->FadeIn();
}

/***************************************************************************
[�T�v]
�V�[���̍X�V

[�߂�l]
void
***************************************************************************/
void Scene_Title::Update()
{	
	// ���͏���
	KeyPress();

	// �I�𒆂�UI���g�k
	UI_Scaling();

	// �V�[���J��
	SceneTransition();
}

/***************************************************************************
[�T�v]
���͏���

[�߂�l]
void
***************************************************************************/
void Scene_Title::KeyPress()
{
	//----- ���Ԍv��
	m_Time += m_Count;

	//----- �L�[���͂ŁA�Q�[�����[�h�I��
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

	//----- ��莞�Ԍo�߂ŁAUI�̐؂�ւ��\
	if (m_ExchangeTime <= m_Time)
	{
		m_Time = 0.0f;
		m_SelectFlg = false;
	}
}

/***************************************************************************
[�T�v]
�I�𒆂�UI���g�k������

[�߂�l]
void
***************************************************************************/
void Scene_Title::UI_Scaling()
{
	//----- �I������Ă���UI���g�k������
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
[�T�v]
�V�[���J��

[�߂�l]
void
***************************************************************************/
void Scene_Title::SceneTransition()
{
	if (m_Transition->GetState() == Transition::State::Stop)
	{
		if (Input::GetKeyTrigger(VK_SPACE) && UI_Start::GetSelectFlg())
		{
			//----- �Q�[���J�n
			m_Transition->FadeOut();
		}
		else if (Input::GetKeyTrigger(VK_SPACE) && UI_End::GetSelectFlg())
		{
			//----- �Q�[���I��
			HWND hWnd = GetWindow();
			DestroyWindow(hWnd);
		}
	}

	//----- �t�B�[�h�I����A�V�[���J��
	if (m_Transition->GetState() == Transition::State::Finish)
	{
		Manager::SetScene<Scene_Game>();
	}
}
