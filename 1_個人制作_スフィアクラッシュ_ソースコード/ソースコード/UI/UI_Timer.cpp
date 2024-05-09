//----- UI
#include "UI_Timer.h"
#include "Time.h"
//----- �R���|�[�l���g�֌W
#include "Audio.h"
#include "Scene.h"
#include "Shader.h"
#include "Sprite.h"
#include "CreateTexture.h"

//----- �ÓI�ϐ�
int UI_Timer::m_TimeLimit;
int UI_Timer::m_CurrentTime;
int UI_Timer::m_ElapsedTime;

/***************************************************************************
[�T�v]
����������

[�߂�l]
void
***************************************************************************/
void UI_Timer::Init()
{
	AddComponent<Shader>()->Load("shader/unlitTextureVS.cso",
		"shader/unlitTexturePS.cso");

	VERTEX_3D vertex[4];

	vertex[0].Position = DirectX::XMFLOAT3(-1.0f, 1.0f, 0.0f);
	vertex[0].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[0].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[0].TexCoord = DirectX::XMFLOAT2(0.0f, 0.0f);

	vertex[1].Position = DirectX::XMFLOAT3(1.0f, 1.0f, 0.0f);
	vertex[1].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[1].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[1].TexCoord = DirectX::XMFLOAT2(1.0f, 0.0f);

	vertex[2].Position = DirectX::XMFLOAT3(-1.0f, -1.0f, 0.0f);
	vertex[2].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[2].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[2].TexCoord = DirectX::XMFLOAT2(0.0f, 1.0f);

	vertex[3].Position = DirectX::XMFLOAT3(1.0f, -1.0f, 0.0f);
	vertex[3].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[3].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[3].TexCoord = DirectX::XMFLOAT2(1.0f, 1.0f);

	// ���_�o�b�t�@����
	D3D11_BUFFER_DESC bd;
	ZeroMemory(&bd, sizeof(bd));
	bd.Usage = D3D11_USAGE_DYNAMIC;
	bd.ByteWidth = sizeof(VERTEX_3D) * 4;
	bd.BindFlags = D3D11_BIND_VERTEX_BUFFER;
	bd.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;

	D3D11_SUBRESOURCE_DATA sd;
	ZeroMemory(&sd, sizeof(sd));
	sd.pSysMem = vertex;

	Renderer::GetDevice()->CreateBuffer(&bd, &sd, &m_VertexBuffer);

	// �e�N�X�`���ǂݍ���
	CreateTextureFromFile(Renderer::GetDevice(),
		"asset/texture/UI/null-1.png", &m_texture);
	assert(m_texture);

	m_Count = 0;
	m_ElapsedTime = 0;
}

/***************************************************************************
[�T�v]
�������̉������

[�߂�l]
void
***************************************************************************/
void UI_Timer::Uninit()
{
	m_VertexBuffer->Release();
	m_texture->Release();
}

/***************************************************************************
[�T�v]
�X�V����

[�߂�l]
void
***************************************************************************/
void UI_Timer::Update()
{
	//----- ���݂̐������Ԃ��v�Z
	m_ElapsedTime = m_TimeLimit - m_CurrentTime;
}

/***************************************************************************
[�T�v]
�`�揈��

[�߂�l]
void
***************************************************************************/
void UI_Timer::Draw()
{
	// �}�g���N�X����
	Renderer::SetWorldViewProjection2D();

	// ���_�o�b�t�@�ݒ�
	UINT stride = sizeof(VERTEX_3D);
	UINT offset = 0;
	Renderer::GetDeviceContext()->IASetVertexBuffers(0, 1, &m_VertexBuffer, &stride, &offset);

	// �}�e���A���ݒ�
	MATERIAL material;
	ZeroMemory(&material, sizeof(material));
	material.Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	material.TextureEnable = TRUE;
	Renderer::SetMaterial(material);

	// �e�N�X�`���ݒ�
	Renderer::GetDeviceContext()->PSSetShaderResources(0, 1, &m_texture);

	// �v���~�e�B�u�g�|���W�ݒ�
	Renderer::GetDeviceContext()->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP);

	float tx;

	int TensPlace = (m_ElapsedTime / 10) % 10;  // 10�̈ʂ̒l������o��
	int OnesPlace = m_ElapsedTime % 10;         //  1�̈ʂ̒l������o��

	//----- 2���`��
	for (int i = 0; i < 2; i++)
	{
		if (i == 0)
		{
			tx = TensPlace % 10 * (1.0f / 10);
		}
		else
		{
			tx = OnesPlace % 10 * (1.0f / 10);
		}
		
		float ty = 0.0f;

		//----- ���_�f�[�^��������
		D3D11_MAPPED_SUBRESOURCE msr;
		Renderer::GetDeviceContext()->Map(m_VertexBuffer, 0,
			D3D11_MAP_WRITE_DISCARD, 0, &msr);

		VERTEX_3D* vertex = (VERTEX_3D*)msr.pData;

		vertex[0].Position = DirectX::XMFLOAT3((SCREEN_WIDTH_HALF - 50.0f) + (50.0f * i), SCREEN_HEIGHT_HALF - 350.0f, 0.0f);
		vertex[0].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
		vertex[0].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
		vertex[0].TexCoord = DirectX::XMFLOAT2(tx, 0.0f);

		vertex[1].Position = DirectX::XMFLOAT3((SCREEN_WIDTH_HALF) + (50.0f * i), SCREEN_HEIGHT_HALF - 350.0f, 0.0f);
		vertex[1].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
		vertex[1].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
		vertex[1].TexCoord = DirectX::XMFLOAT2(tx + (1.0f / 10), 0.0f);

		vertex[2].Position = DirectX::XMFLOAT3((SCREEN_WIDTH_HALF - 50.0f) + (50.0f * i), SCREEN_HEIGHT_HALF - 300.0f, 0.0f);
		vertex[2].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
		vertex[2].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
		vertex[2].TexCoord = DirectX::XMFLOAT2(tx, 1.0f);

		vertex[3].Position = DirectX::XMFLOAT3((SCREEN_WIDTH_HALF) + (50.0f * i), SCREEN_HEIGHT_HALF - 300.0f, 0.0f);
		vertex[3].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
		vertex[3].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
		vertex[3].TexCoord = DirectX::XMFLOAT2(tx + (1.0f / 10), 1.0f);

		Renderer::GetDeviceContext()->Unmap(m_VertexBuffer, 0);

		// �|���S���`��
		Renderer::GetDeviceContext()->Draw(4, 0);
	}
}
