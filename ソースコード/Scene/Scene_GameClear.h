#pragma once
#include "Scene.h"

class Scene_GameClear : public Scene
{
public:
	/***************************************************************************
	[�T�v]
	�V�[���̏�����

	[�߂�l]
	void
	***************************************************************************/
	void Init() override;

	/***************************************************************************
	[�T�v]
	�V�[���̍X�V

	[�߂�l]
	void
	***************************************************************************/
	void Update() override;

private:
	class Transition* m_Transition{};
	class Audio* m_BGM{};
	class Audio* m_SE{};
};