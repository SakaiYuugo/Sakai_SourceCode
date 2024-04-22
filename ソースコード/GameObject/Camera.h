#pragma once
#include "GameObject.h"
#include <DirectXMath.h>

class Camera : public GameObject
{
public:
	/***************************************************************************
	[概要]
	更新処理

	[戻り値]
	void
	***************************************************************************/
	void Update() override;

	/***************************************************************************
	[概要]
	描画処理

	[戻り値]
	void
	***************************************************************************/
	void Draw() override;

	/***************************************************************************
	[概要]
	カメラの初期位置設定

	[戻り値]
	void
	***************************************************************************/
	void SetCameraPos();

	/***************************************************************************
	[概要]
	ビュー座標取得

	[戻り値]
	XMFLOAT4X4　m_ViewMatrix
	***************************************************************************/
	DirectX::XMFLOAT4X4 GetViewMatrix() { return m_ViewMatrix; }

	/***************************************************************************
	[概要]
	プロジェクション座標取得

	[戻り値]
	XMFLOAT4X4　m_ProjectionMatrix
	***************************************************************************/
	DirectX::XMFLOAT4X4 GetProjectMatrix() { return m_ProjectionMatrix; }

private:
	DirectX::XMFLOAT3 m_Target = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);
	DirectX::XMFLOAT3 m_Forward = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);

	DirectX::XMFLOAT4X4	m_ViewMatrix{};
	DirectX::XMFLOAT4X4 m_ProjectionMatrix{};

	float m_Distance = -10.0f;
};
