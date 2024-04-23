//----- �I�u�W�F�N�g�֌W
#include "EnemyBalls.h"
#include "PlayerBall.h"
#include "Sky.h"
#include "Field.h"
#include "Camera.h"
//----- �R���|�[�l���g�֌W
#include "Audio.h"
//----- �V�X�e���֌W
#include "Collision.h"
#include "Manager.h"
#include "Input.h"
#include "Transition.h"
#include "Time.h"
//----- �V�[���֌W
#include "Scene_Title.h"
#include "Scene_Game.h"
#include "Scene_GameOver.h"
#include "Scene_GameClear.h"
//----- UI�֌W
#include "UI_Timer.h"
//----- �G�t�F�N�g�֌W
#include "Effect_Collision.h"
#include "Effect_Charge.h"

/***************************************************************************
[�T�v]
�V�[���̏�����

[�߂�l]
void
***************************************************************************/
void Scene_Game::Init()
{
	//----- �\�߃f�[�^��ǂݍ���
	EnemyBalls::Load();
	Effect_Collision::Load();
	Effect_Charge::Load();

	//----- �J����
	Camera* camera = AddGameCamera<Camera>();

	//----- �v���C���[�ƓG���~��ɔz�u
	PlayerBall* playerball = AddGameObject<PlayerBall>();
	playerball->SetPosition(DirectX::XMFLOAT3((Field::m_Radius - 5.0f) * cosf(0.0f), 1.0f, (Field::m_Radius - 5.0f) * sinf(0.0f)));

	for (int i = 1; i < EnemyBalls::GetEnemyBallNum(); i++)
	{
		float rad = i / (float)EnemyBalls::GetEnemyBallNum() * 3.14159265f * 2;
		EnemyBalls* enemy = AddGameObject<EnemyBalls>();
		enemy->SetPosition(DirectX::XMFLOAT3((Field::m_Radius - 5.0f) * cosf(rad), 1.0f, (Field::m_Radius - 5.0f) * sinf(rad)));
	}

	//----- �J�����̏����ʒu��ݒ�
	camera->SetCameraPos();

	//----- �X�e�[�W�֌W
	AddGameObject<Sky>();
	AddGameObject<Field>();

	//----- �������Ԑݒ�(�b
	Time::Init();
	AddGameObject<UI_Timer>(4);
	UI_Timer::SetTimeLimit(60);

	//----- BGM
	m_BGM = AddGameObject<GameObject>(1)->AddComponent<Audio>();
	m_BGM->Load("asset/audio/BGM/BGM_Game.wav");
	m_BGM->Play(true, 0.3f);

	//----- �t�F�[�h�C��
	m_Transition = AddGameObject<Transition>(4);
	m_Transition->FadeIn();
}

/***************************************************************************
[�T�v]
�V�[���̉��

[�߂�l]
void
***************************************************************************/
void Scene_Game::Uninit()
{
	//----- ���ԍX�V�I��
	Time::Uninit();
}

/***************************************************************************
[�T�v]
�V�[���̍X�V

[�߂�l]
void
***************************************************************************/
void Scene_Game::Update()
{
	//----- �������ԍX�V
	UpdateTimeLimit();

	//----- �G���m�̓����蔻��
	EnemyCollision();

	//----- �v���C���[���S����
	PlayerDied();

	//----- �V�[���J��
	SceneTransition();
}

/***************************************************************************
[�T�v]
�������ԍX�V

[�߂�l]
void
***************************************************************************/
void Scene_Game::UpdateTimeLimit()
{	
	if (!m_TimeLimit && Time::Update())
	{
		//----- �������Ԍo��
		m_TimeLimit = true;
		m_Transition->FadeOut();
	}
}

/***************************************************************************
[�T�v]
�G���m�̓����蔻��

[�߂�l]
void
***************************************************************************/
void Scene_Game::EnemyCollision()
{
	//----- �G�����Ȃ��ꍇ�A�����蔻������Ȃ�
	if (!m_EnemyDead)
	{
		std::vector<EnemyBalls*> ballList = GetGameObjects<EnemyBalls>();
		if (ballList.size() <= 0)
		{
			m_EnemyDead = true;
			m_Transition->FadeOut();
		}
		else
		{
			//----- �G���m�̓����蔻��
			m_Collision->ColliderWithNumberBall();
		}
	}
}

/***************************************************************************
[�T�v]
�v���C���[�̎��S����

[�߂�l]
void
***************************************************************************/
void Scene_Game::PlayerDied()
{
	if (!m_PlayerDied)
	{
		PlayerBall* player = GetGameObject<PlayerBall>();
		if (player == nullptr)
		{
			m_PlayerDied = true;
			m_Transition->FadeOut();
		}
	}
}

/***************************************************************************
[�T�v]
�V�[���J��

[�߂�l]
void
***************************************************************************/
void Scene_Game::SceneTransition()
{
	if (m_Transition->GetState() == Transition::State::Finish)
	{
		//----- �v���C���[���S�ŁA�Q�[���I�[�o�[�ɑJ��
		if (m_PlayerDied)
		{
			Manager::SetScene<Scene_GameOver>();
			return;
		}
		else if (m_TimeLimit)
		{
			Manager::SetScene<Scene_GameOver>();
			return;
		}

		//----- �G�S�łŃQ�[���N���A�ɑJ��
		if (m_EnemyDead)
		{
			Manager::SetScene<Scene_GameClear>();
			return;
		}
	}
}
