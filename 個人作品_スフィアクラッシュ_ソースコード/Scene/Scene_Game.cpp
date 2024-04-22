//----- オブジェクト関係
#include "EnemyBalls.h"
#include "PlayerBall.h"
#include "Sky.h"
#include "Field.h"
#include "Camera.h"
//----- コンポーネント関係
#include "Audio.h"
//----- システム関係
#include "Collision.h"
#include "Manager.h"
#include "Input.h"
#include "Transition.h"
#include "Time.h"
//----- シーン関係
#include "Scene_Title.h"
#include "Scene_Game.h"
#include "Scene_GameOver.h"
#include "Scene_GameClear.h"
//----- UI関係
#include "UI_Timer.h"
//----- エフェクト関係
#include "Effect_Collision.h"
#include "Effect_Charge.h"

/***************************************************************************
[概要]
シーンの初期化

[戻り値]
void
***************************************************************************/
void Scene_Game::Init()
{
	//----- 予めデータを読み込む
	EnemyBalls::Load();
	Effect_Collision::Load();
	Effect_Charge::Load();

	//----- カメラ
	Camera* camera = AddGameCamera<Camera>();

	//----- プレイヤーと敵を円状に配置
	PlayerBall* playerball = AddGameObject<PlayerBall>();
	playerball->SetPosition(DirectX::XMFLOAT3((Field::m_Radius - 5.0f) * cosf(0.0f), 1.0f, (Field::m_Radius - 5.0f) * sinf(0.0f)));

	for (int i = 1; i < EnemyBalls::GetEnemyBallNum(); i++)
	{
		float rad = i / (float)EnemyBalls::GetEnemyBallNum() * 3.14159265f * 2;
		EnemyBalls* enemy = AddGameObject<EnemyBalls>();
		enemy->SetPosition(DirectX::XMFLOAT3((Field::m_Radius - 5.0f) * cosf(rad), 1.0f, (Field::m_Radius - 5.0f) * sinf(rad)));
	}

	//----- カメラの初期位置を設定
	camera->SetCameraPos();

	//----- ステージ関係
	AddGameObject<Sky>();
	AddGameObject<Field>();

	//----- 制限時間設定(秒
	Time::Init();
	AddGameObject<UI_Timer>(4);
	UI_Timer::SetTimeLimit(60);

	//----- BGM
	m_BGM = AddGameObject<GameObject>(1)->AddComponent<Audio>();
	m_BGM->Load("asset/audio/BGM/BGM_Game.wav");
	m_BGM->Play(true, 0.3f);

	//----- フェードイン
	m_Transition = AddGameObject<Transition>(4);
	m_Transition->FadeIn();
}

/***************************************************************************
[概要]
シーンの解放

[戻り値]
void
***************************************************************************/
void Scene_Game::Uninit()
{
	//----- 時間更新終了
	Time::Uninit();
}

/***************************************************************************
[概要]
シーンの更新

[戻り値]
void
***************************************************************************/
void Scene_Game::Update()
{
	//----- 制限時間更新
	UpdateTimeLimit();

	//----- 敵同士の当たり判定
	EnemyCollision();

	//----- プレイヤー死亡判定
	PlayerDied();

	//----- シーン遷移
	SceneTransition();
}

/***************************************************************************
[概要]
制限時間更新

[戻り値]
void
***************************************************************************/
void Scene_Game::UpdateTimeLimit()
{	
	if (!m_TimeLimit && Time::Update())
	{
		//----- 制限時間経過
		m_TimeLimit = true;
		m_Transition->FadeOut();
	}
}

/***************************************************************************
[概要]
敵同士の当たり判定

[戻り値]
void
***************************************************************************/
void Scene_Game::EnemyCollision()
{
	//----- 敵がいない場合、当たり判定を取らない
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
			//----- 敵同士の当たり判定
			m_Collision->ColliderWithNumberBall();
		}
	}
}

/***************************************************************************
[概要]
プレイヤーの死亡判定

[戻り値]
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
[概要]
シーン遷移

[戻り値]
void
***************************************************************************/
void Scene_Game::SceneTransition()
{
	if (m_Transition->GetState() == Transition::State::Finish)
	{
		//----- プレイヤー死亡で、ゲームオーバーに遷移
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

		//----- 敵全滅でゲームクリアに遷移
		if (m_EnemyDead)
		{
			Manager::SetScene<Scene_GameClear>();
			return;
		}
	}
}
