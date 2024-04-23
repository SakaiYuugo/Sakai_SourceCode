#pragma once
#include "GameObject.h"

class UI_End : public GameObject
{
public:
	/***************************************************************************
	[�T�v]
	����������

	[�߂�l]
	void
	***************************************************************************/
	void Init() override;

	/***************************************************************************
	[�T�v]
	�������̉������

	[�߂�l]
	void
	***************************************************************************/
	void Uninit() override;

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
	�f�t�H���g�T�C�Y�ŕ`��

	[�߂�l]
	void
	***************************************************************************/
	void DefaultDraw();

public:
	static bool m_Select;

	/***************************************************************************
	[�T�v]
	�^�C�g����I�����邩�؂�ւ���
	
	[�߂�l]
	void
	***************************************************************************/
	static void SetSelectFlg(bool flg) { m_Select = flg; }

	static bool GetSelectFlg() { return m_Select; }

private:
	struct ConstantBuffer
	{
		DirectX::XMMATRIX WorldViewProjection;
		DirectX::XMFLOAT4 FlashColor;
	};
	ConstantBuffer buffer;

private:
	ID3D11Buffer* m_VertexBuffer{};
	ID3D11ShaderResourceView* m_texture{};
	ID3D11ShaderResourceView* m_texture1{};

	const DirectX::XMFLOAT2 m_Enlarge = {30.0f, 20.0f}; // UI�̊g��l
	float m_Count;
};