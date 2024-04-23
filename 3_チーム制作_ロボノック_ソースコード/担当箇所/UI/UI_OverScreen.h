//#pragma once
#ifndef OVER_SCREEN_H
#define OVER_SCREEN_H

#include <DirectXMath.h>
#include "SpriteManager.h"
#include "Image2D.h"

class UI_OverScreen
{
private:
	enum UI_Over
	{
		GAMEOVER = 0,
		RETRY,
		STAGESELECT,
		MAX
	};
public:
	enum STATE_OVER
	{
		STATE_INIT = 0,
		STATE_IN,
		STATE_STAY,
		STATE_OUT,
		STATE_END,
		STATE_MAX
	};

public:
	UI_OverScreen();
	~UI_OverScreen();
	void ScreenInit();
	bool Update();
	void ScreenDraw();
	void GameOverDraw();
	void RetryDraw(bool Select);
	void StageSelectDraw(bool Select);

	STATE_OVER GetState() { return m_NowState; }
	void SetState(STATE_OVER NextState) { m_NowState = NextState; }
private:

	typedef struct
	{
		DirectX::XMFLOAT2 Pos;		// 座標
		DirectX::XMFLOAT2 Size;		// 画像の大きさ
		DirectX::XMFLOAT3 Scale;	// 大きさ
		DirectX::XMFLOAT3 Angle;	// 角度

	}UItransform;

private:

	SpriteManager* m_pSprite;
	UItransform m_ScreenTransform;
	ID3D11ShaderResourceView* m_pScreenTexture;
	UItransform m_Transform[UI_Over::MAX];
	ID3D11ShaderResourceView* m_pTexture[UI_Over::MAX];    // テクスチャ
	ID3D11ShaderResourceView* m_pChoseTexture[UI_Over::MAX];    // テクスチャ
	STATE_OVER m_NowState;

	Image2D* m_pArm;
	DirectX::XMFLOAT2 m_ArmAddPos;
};

#endif // !OVER_SCREEN_H

