#pragma once
#include "GameObject.h"
#include <DirectXMath.h>

class Camera : public GameObject
{
public:
	/***************************************************************************
	[�T�v]
	�X�V����

	[�߂�l]
	void
	***************************************************************************/
	void Update() override;

	/***************************************************************************
	[�T�v]
	�`�揈��

	[�߂�l]
	void
	***************************************************************************/
	void Draw() override;

	/***************************************************************************
	[�T�v]
	�J�����̏����ʒu�ݒ�

	[�߂�l]
	void
	***************************************************************************/
	void SetCameraPos();

	/***************************************************************************
	[�T�v]
	�r���[���W�擾

	[�߂�l]
	XMFLOAT4X4�@m_ViewMatrix
	***************************************************************************/
	DirectX::XMFLOAT4X4 GetViewMatrix() { return m_ViewMatrix; }

	/***************************************************************************
	[�T�v]
	�v���W�F�N�V�������W�擾

	[�߂�l]
	XMFLOAT4X4�@m_ProjectionMatrix
	***************************************************************************/
	DirectX::XMFLOAT4X4 GetProjectMatrix() { return m_ProjectionMatrix; }

private:
	DirectX::XMFLOAT3 m_Target = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);
	DirectX::XMFLOAT3 m_Forward = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);

	DirectX::XMFLOAT4X4	m_ViewMatrix{};
	DirectX::XMFLOAT4X4 m_ProjectionMatrix{};

	float m_Distance = -10.0f;
};
