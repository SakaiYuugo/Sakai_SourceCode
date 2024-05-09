//----- オブジェクト関係
#include "Camera.h"
#include "PlayerBall.h"
//----- システム関係
#include "Manager.h"
#include "Renderer.h"
#include "Input.h"
//----- コンポーネント関係
#include "Scene.h"
//----- 計算関係
#include "XMFLOAT_Calculation.h"
#include "Random.h"
#include <math.h>
#include <DirectXMath.h>

/***************************************************************************
[概要]
更新処理

[戻り値]
void
***************************************************************************/
void Camera::Update()
{
	Scene* scene = Manager::GetScene();
	PlayerBall* player = scene->GetGameObject<PlayerBall>();

	if (player == nullptr) { return; }

	DirectX::XMFLOAT3 playerPos = player->GetPosition();

	if (Input::GetKeyPress('A'))
	{
		m_Rotation.y -= DirectX::XMConvertToRadians(2.0f);
	}
	if (Input::GetKeyPress('D'))
	{
		m_Rotation.y += DirectX::XMConvertToRadians(2.0f);
	}

	//----- 注視点の更新
	m_Target.x = playerPos.x;
	m_Target.y = playerPos.y + 5.0f;
	m_Target.z = playerPos.z;

	m_Position.x = playerPos.x + m_Distance * sinf(m_Rotation.y);
	m_Position.y = playerPos.y - m_Distance;
	m_Position.z = playerPos.z + m_Distance * cosf(m_Rotation.y);

	//----- カメラの回転値をプレイヤーに設定
	player->SetRotation(m_Rotation);
}

/***************************************************************************
[概要]
描画処理

[戻り値]
void
***************************************************************************/
void Camera::Draw()
{
	// ビューマトリクス設定
	DirectX::XMFLOAT3 up = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	DirectX::XMStoreFloat4x4(&m_ViewMatrix,
		DirectX::XMMatrixLookAtLH(
			XMLoadFloat3(&m_Position), 
			XMLoadFloat3(&m_Target),
			XMLoadFloat3(&up))
	);
	Renderer::SetViewMatrix(&m_ViewMatrix);

	// プロジェクションマトリクス設定
	DirectX::XMStoreFloat4x4(&m_ProjectionMatrix,
		DirectX::XMMatrixPerspectiveFovLH(DirectX::XMConvertToRadians(60),
			(float)SCREEN_WIDTH / SCREEN_HEIGHT, 0.2f, 1000.0f)
	);
	Renderer::SetProjectionMatrix(&m_ProjectionMatrix);
}

/***************************************************************************
[概要]
カメラの初期位置設定

[戻り値]
void
***************************************************************************/
void Camera::SetCameraPos()
{
	Scene* scene = Manager::GetScene();
	PlayerBall* player = scene->GetGameObject<PlayerBall>();
	DirectX::XMFLOAT3 playerpos = player->GetPosition();
	DirectX::XMFLOAT3 playerforward = player->GetForward();

	m_Rotation.y = player->GetRotation().y;

	// カメラ位置の設定
	m_Position.x = playerpos.x - m_Distance * playerforward.x;
	m_Position.y = playerpos.y - m_Distance * playerforward.y + 3.0f;
	m_Position.z = playerpos.z - m_Distance * playerforward.z;

}
