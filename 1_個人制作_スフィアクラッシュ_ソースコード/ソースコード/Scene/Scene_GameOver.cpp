//----- システム関係
#include "Manager.h"
#include "Renderer.h"
#include "Input.h"
#include "Transition.h"
//-----コンポーネント関係
#include "Audio.h"
#include "Shader.h"
#include "Sprite.h"
//----- シーン関係
#include "Scene_GameOver.h"
#include "Scene_Title.h"
//----- UI
#include "UI_GameOver.h"

/***************************************************************************
[概要]
シーンの初期化

[戻り値]
void
***************************************************************************/
void Scene_GameOver::Init()
{
	//----- テクスチャ
	UI_GameOver* UI_gameover = AddGameObject<UI_GameOver>(3);

	//-----フェード (Layerを4番(最前面)に設定)
	m_Transition = AddGameObject<Transition>(4);
	m_Transition->FadeIn();

	//----- SE
	m_SE = AddGameObject<GameObject>(1)->AddComponent<Audio>();
	m_SE->Load("asset/audio/SE/SE_GameOver.wav");
	m_SE->Play(false, 1.0f);
}

/***************************************************************************
[概要]
シーンの更新

[戻り値]
void
***************************************************************************/
void Scene_GameOver::Update()
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