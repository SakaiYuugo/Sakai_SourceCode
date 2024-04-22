#pragma once
#include "Scene.h"

class Scene_GameClear : public Scene
{
public:
	/***************************************************************************
	[概要]
	シーンの初期化

	[戻り値]
	void
	***************************************************************************/
	void Init() override;

	/***************************************************************************
	[概要]
	シーンの更新

	[戻り値]
	void
	***************************************************************************/
	void Update() override;

private:
	class Transition* m_Transition{};
	class Audio* m_BGM{};
	class Audio* m_SE{};
};