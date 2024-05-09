//----- �I�u�W�F�N�g�֌W
#include "Camera.h"
#include "PlayerBall.h"
//----- �V�X�e���֌W
#include "Manager.h"
#include "Renderer.h"
#include "Input.h"
//----- �R���|�[�l���g�֌W
#include "Scene.h"
//----- �v�Z�֌W
#include "XMFLOAT_Calculation.h"
#include "Random.h"
#include <math.h>
#include <DirectXMath.h>

/***************************************************************************
[�T�v]
�X�V����

[�߂�l]
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

	//----- �����_�̍X�V
	m_Target.x = playerPos.x;
	m_Target.y = playerPos.y + 5.0f;
	m_Target.z = playerPos.z;

	m_Position.x = playerPos.x + m_Distance * sinf(m_Rotation.y);
	m_Position.y = playerPos.y - m_Distance;
	m_Position.z = playerPos.z + m_Distance * cosf(m_Rotation.y);

	//----- �J�����̉�]�l���v���C���[�ɐݒ�
	player->SetRotation(m_Rotation);
}

/***************************************************************************
[�T�v]
�`�揈��

[�߂�l]
void
***************************************************************************/
void Camera::Draw()
{
	// �r���[�}�g���N�X�ݒ�
	DirectX::XMFLOAT3 up = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	DirectX::XMStoreFloat4x4(&m_ViewMatrix,
		DirectX::XMMatrixLookAtLH(
			XMLoadFloat3(&m_Position), 
			XMLoadFloat3(&m_Target),
			XMLoadFloat3(&up))
	);
	Renderer::SetViewMatrix(&m_ViewMatrix);

	// �v���W�F�N�V�����}�g���N�X�ݒ�
	DirectX::XMStoreFloat4x4(&m_ProjectionMatrix,
		DirectX::XMMatrixPerspectiveFovLH(DirectX::XMConvertToRadians(60),
			(float)SCREEN_WIDTH / SCREEN_HEIGHT, 0.2f, 1000.0f)
	);
	Renderer::SetProjectionMatrix(&m_ProjectionMatrix);
}

/***************************************************************************
[�T�v]
�J�����̏����ʒu�ݒ�

[�߂�l]
void
***************************************************************************/
void Camera::SetCameraPos()
{
	Scene* scene = Manager::GetScene();
	PlayerBall* player = scene->GetGameObject<PlayerBall>();
	DirectX::XMFLOAT3 playerpos = player->GetPosition();
	DirectX::XMFLOAT3 playerforward = player->GetForward();

	m_Rotation.y = player->GetRotation().y;

	// �J�����ʒu�̐ݒ�
	m_Position.x = playerpos.x - m_Distance * playerforward.x;
	m_Position.y = playerpos.y - m_Distance * playerforward.y + 3.0f;
	m_Position.z = playerpos.z - m_Distance * playerforward.z;

}
