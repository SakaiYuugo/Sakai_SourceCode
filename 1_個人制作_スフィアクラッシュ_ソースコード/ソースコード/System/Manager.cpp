//----- コンポーネント関係
#include "Audio.h"
#include "Renderer.h"
//----- システム関係
#include "Input.h"
#include "Main.h"
#include "Manager.h"
#include "Collision.h"
#include "Random.h"
//----- シーン
#include "Scene_Title.h"
#include "Scene_Game.h"

Scene* Manager::m_Scene{};

/***************************************************************************
[概要]
初期化処理

[戻り値]
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
[概要]
メモリの解放処理

[戻り値]
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
[概要]
更新処理

[戻り値]
void
***************************************************************************/
void Manager::Update()
{
	Input::Update();

	m_Scene->UpdateBase();
}

/***************************************************************************
[概要]
描画処理

[戻り値]
void
***************************************************************************/
void Manager::Draw()
{
	Renderer::Begin();

	m_Scene->DrawBase();

	Renderer::End();
}