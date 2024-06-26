#pragma once

#include "Scene.h"

class Scene_Game : public Scene
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
	シーンの解放
	
	[戻り値]
	void
	***************************************************************************/
	void Uninit() override;

	/***************************************************************************
	[概要]
	シーンの更新
	
	[戻り値]
	void
	***************************************************************************/
	void Update() override;

	/***************************************************************************
	[概要]
	制限時間更新

	[戻り値]
	void
	***************************************************************************/
	void UpdateTimeLimit();

	/***************************************************************************
	[概要]
	敵同士の当たり判定

	[戻り値]
	void
	***************************************************************************/
	void EnemyCollision();

	/***************************************************************************
	[概要]
	プレイヤーの死亡判定

	[戻り値]
	void
	***************************************************************************/
	void PlayerDied();

	/***************************************************************************
	[概要]
	シーン遷移
	
	[戻り値]
	void
	***************************************************************************/
	void SceneTransition();

private:
	class Audio* m_BGM{};
	class Audio* m_SE{};
	class Transition* m_Transition{};
	class Collision* m_Collision{};

	bool m_TutorialFlg = false;  // チュートリアルが終わっているか判定
	bool m_PlayerDied  = false;  // プレイヤーの死亡判定
	bool m_EnemyDead   = false;  // 敵の全滅判定
	bool m_TimeLimit   = false;  // 時間時間を経過しているか 
};