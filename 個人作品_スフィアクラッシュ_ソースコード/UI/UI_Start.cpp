//----- UI
#include "UI_Start.h"
#include "Time.h"
//----- �R���|�[�l���g�֌W
#include "Audio.h"
#include "Scene.h"
#include "Shader.h"
#include "Sprite.h"
#include "Manager.h"
#include "CreateTexture.h"

bool UI_Start::m_Select = false;

/***************************************************************************
[�T�v]
����������

[�߂�l]
void
***************************************************************************/
void UI_Start::Init()
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

	//----- ���_�o�b�t�@����
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

	//----- �e�N�X�`���ǂݍ���
	CreateTextureFromFile(Renderer::GetDevice(),
		"asset/texture/Title/UI_Start.png", &m_texture);
	assert(m_texture);

	m_Count = 0.0f;
}

/***************************************************************************
[�T�v]
�������̉������

[�߂�l]
void
***************************************************************************/
void UI_Start::Uninit()
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
void UI_Start::Update()
{
	//----- ��莞�Ԍo�߂Ŋg�k����ύX
	if (m_Select)
	{
		if (1.0f <= m_Time)
		{
			m_Count = -0.02f;
		}
		else if (m_Time <= 0.0f)
		{
			m_Count = 0.02f;
		}
	}

}

/***************************************************************************
[�T�v]
�`�揈��

[�߂�l]
void
***************************************************************************/
void UI_Start::Draw()
{
	//----- �}�g���N�X����
	Renderer::SetWorldViewProjection2D();

	//----- ���_�o�b�t�@�ݒ�
	UINT stride = sizeof(VERTEX_3D);
	UINT offset = 0;
	Renderer::GetDeviceContext()->IASetVertexBuffers(0, 1, &m_VertexBuffer, &stride, &offset);

	//----- �}�e���A���ݒ�
	MATERIAL material;
	ZeroMemory(&material, sizeof(material));
	material.Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	material.TextureEnable = TRUE;
	Renderer::SetMaterial(material);

	//----- �e�N�X�`���ݒ�
	Renderer::GetDeviceContext()->PSSetShaderResources(0, 1, &m_texture);

	//----- �v���~�e�B�u�g�|���W�ݒ�
	Renderer::GetDeviceContext()->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP);

	//----- �^�C�g����UI���I�𒆂Ȃ�A�g�k�p�̎��Ԃ��J�E���g
	if (m_Select)
	{
		m_Time += m_Count;
	}
	else
	{
		DefaultDraw();
		return;
	}

	//----- ���_�f�[�^��������
	D3D11_MAPPED_SUBRESOURCE msr;
	Renderer::GetDeviceContext()->Map(m_VertexBuffer, 0, D3D11_MAP_WRITE_DISCARD, 0, &msr);

	VERTEX_3D* vertex = (VERTEX_3D*)msr.pData;

	vertex[0].Position = DirectX::XMFLOAT3((SCREEN_WIDTH_HALF - 200.0f) - (m_Time * m_Enlarge.x), (SCREEN_HEIGHT - 300.0f) - (m_Time * m_Enlarge.y), 0.0f);
	vertex[0].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[0].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[0].TexCoord = DirectX::XMFLOAT2(0.0f, 0.0f);

	vertex[1].Position =DirectX::XMFLOAT3((SCREEN_WIDTH_HALF + 200.0f) + (m_Time * m_Enlarge.x), (SCREEN_HEIGHT - 300.0f) - (m_Time * m_Enlarge.y), 0.0f);
	vertex[1].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[1].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[1].TexCoord = DirectX::XMFLOAT2(1.0f, 0.0f);

	vertex[2].Position = DirectX::XMFLOAT3((SCREEN_WIDTH_HALF - 200.0f) - (m_Time * m_Enlarge.x), (SCREEN_HEIGHT - 150.0f) + (m_Time * m_Enlarge.y), 0.0f);
	vertex[2].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[2].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[2].TexCoord = DirectX::XMFLOAT2(0.0f, 1.0f);

	vertex[3].Position = DirectX::XMFLOAT3((SCREEN_WIDTH_HALF + 200.0f) + (m_Time * m_Enlarge.x), (SCREEN_HEIGHT - 150.0f) + (m_Time * m_Enlarge.y), 0.0f);
	vertex[3].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[3].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[3].TexCoord = DirectX::XMFLOAT2(1.0f, 1.0f);

	Renderer::GetDeviceContext()->Unmap(m_VertexBuffer, 0);

	//----- �|���S���`��
	Renderer::GetDeviceContext()->Draw(4, 0);
}

/***************************************************************************
[�T�v]
�f�t�H���g�T�C�Y�ŕ`��

[�߂�l]
void
***************************************************************************/
void UI_Start::DefaultDraw()
{
	//----- ���_�f�[�^��������
	D3D11_MAPPED_SUBRESOURCE msr;
	Renderer::GetDeviceContext()->Map(m_VertexBuffer, 0, D3D11_MAP_WRITE_DISCARD, 0, &msr);


	VERTEX_3D* vertex = (VERTEX_3D*)msr.pData;

	vertex[0].Position = DirectX::XMFLOAT3((SCREEN_WIDTH_HALF - 200.0f), SCREEN_HEIGHT - 300.0f, 0.0f);
	vertex[0].Normal = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[0].Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[0].TexCoord = DirectX::XMFLOAT2(0.0f, 0.0f);

	vertex[1].Position = DirectX::XMFLOAT3((SCREEN_WIDTH_HALF + 200.0f), SCREEN_HEIGHT - 300.0f, 0.0f);
	vertex[1].Normal = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[1].Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[1].TexCoord = DirectX::XMFLOAT2(1.0f, 0.0f);

	vertex[2].Position = DirectX::XMFLOAT3((SCREEN_WIDTH_HALF - 200.0f), SCREEN_HEIGHT - 150.0f, 0.0f);
	vertex[2].Normal = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[2].Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[2].TexCoord = DirectX::XMFLOAT2(0.0f, 1.0f);

	vertex[3].Position = DirectX::XMFLOAT3((SCREEN_WIDTH_HALF + 200.0f), SCREEN_HEIGHT - 150.0f, 0.0f);
	vertex[3].Normal = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[3].Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[3].TexCoord = DirectX::XMFLOAT2(1.0f, 1.0f);

	Renderer::GetDeviceContext()->Unmap(m_VertexBuffer, 0);

	//----- �|���S���`��
	Renderer::GetDeviceContext()->Draw(4, 0);
}
