//----- �R���|�[�l���g�֌W
#include "Audio.h"
#include "Renderer.h"
//----- �V�X�e���֌W
#include "Input.h"
#include "Main.h"
#include "Manager.h"
#include "Collision.h"
#include "Random.h"
//----- �V�[��
#include "Scene_Title.h"
#include "Scene_Game.h"

Scene* Manager::m_Scene{};

/***************************************************************************
[�T�v]
����������

[�߂�l]
void
***************************************************************************/
void Manager::Init()
{
	Renderer::Init();
	Input::Init();
	Audio::InitMaster();
	Random::Init();

	SetScene<Scene_Title>();
}

/***************************************************************************
[�T�v]
�������̉������

[�߂�l]
void
***************************************************************************/
void Manager::Uninit()
{
	m_Scene->UninitBase();
	delete m_Scene;

	Audio::UninitMaster();
	Renderer::Uninit();
}

/***************************************************************************
[�T�v]
�X�V����

[�߂�l]
void
***************************************************************************/
void Manager::Update()
{
	Input::Update();

	m_Scene->UpdateBase();
}

/***************************************************************************
[�T�v]
�`�揈��

[�߂�l]
void
***************************************************************************/
void Manager::Draw()
{
	Renderer::Begin();

	m_Scene->DrawBase();

	Renderer::End();
}