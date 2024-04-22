//----- �V�X�e���֌W
#include "Manager.h"
#include "Renderer.h"
#include "Input.h"
#include "Transition.h"
//-----�R���|�[�l���g�֌W
#include "Audio.h"
#include "Shader.h"
#include "Sprite.h"
//----- �V�[���֌W
#include "Scene_GameClear.h"
#include "Scene_Title.h"
//----- UI
#include "UI_GameClear.h"

/***************************************************************************
[�T�v]
�V�[���̏�����

[�߂�l]
void
***************************************************************************/
void Scene_GameClear::Init()
{
	//----- �e�N�X�`��
	UI_GameClear* UI_gameclear = AddGameObject<UI_GameClear>(3);

	//-----�t�F�[�h (Layer��4��(�őO��)�ɐݒ�)
	m_Transition = AddGameObject<Transition>(4);
	m_Transition->FadeIn();

	//----- SE
	m_SE = AddGameObject<GameObject>(1)->AddComponent<Audio>();
	m_SE->Load("asset/audio/SE/SE_GameClear.wav");
	m_SE->Play(false, 1.0f);
}

/***************************************************************************
[�T�v]
�V�[���̍X�V

[�߂�l]
void
***************************************************************************/
void Scene_GameClear::Update()
{
	if (m_Transition->GetState() == Transition::State::Stop)
	{
		if (Input::GetKeyTrigger(VK_SPACE))
		{
			m_Transition->FadeOut();
		}
	}

	if (m_Transition->GetState() == Transition::State::Finish)
	{
		Manager::SetScene<Scene_Title>();
	}
}