//----- �G�t�F�N�g
#include "Effect_Collision.h"
//----- �J����
#include "Camera.h"
//----- �R���|�[�l���g�֌W
#include "CreateTexture.h"
#include "Scene.h"
#include "Shader.h"
#include "Sprite.h"
#include "Manager.h"

//----- �ÓI�ϐ�
ID3D11Buffer* Effect_Collision::m_VertexBuffer{};
ID3D11ShaderResourceView* Effect_Collision::m_Texture{};

/***************************************************************************
[�T�v]
���O�Ƀe�N�X�`����ǂݍ���

[�߂�l]
void
***************************************************************************/
void Effect_Collision::Load()
{
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
		"asset/texture/Effect/Effect_Collision.png",
		&m_Texture);
	assert(m_Texture);
}

/***************************************************************************
[�T�v]
�e�N�X�`���̉��

[�߂�l]
void
***************************************************************************/
void Effect_Collision::Unload()
{
	m_VertexBuffer->Release();
	m_Texture->Release();
}

/***************************************************************************
[�T�v]
����������

[�߂�l]
void
***************************************************************************/
void Effect_Collision::Init()
{
	AddComponent<Shader>()->Load("shader/unlitTextureVS.cso","shader/unlitTexturePS.cso");
}

/***************************************************************************
[�T�v]
�X�V����

[�߂�l]
void
***************************************************************************/
void Effect_Collision::Update()
{
	m_Count++;

	//----- ��莞�Ԍo�߂Ńe�N�X�`�����폜
	if (m_Count >= 10)
	{
		SetDestroy();
		return;
	}
}

/***************************************************************************
[�T�v]
�`�揈��

[�߂�l]
void
***************************************************************************/
void Effect_Collision::Draw()
{
	//----- �e�N�X�`�����W�Z�o
	float x = m_Count % 5 * (1.0f * 5);

	//----- ���_�f�[�^��������
	D3D11_MAPPED_SUBRESOURCE msr;
	Renderer::GetDeviceContext()->Map(m_VertexBuffer, 0, D3D11_MAP_WRITE_DISCARD, 0, &msr);

	VERTEX_3D* vertex = (VERTEX_3D*)msr.pData;

	vertex[0].Position = DirectX::XMFLOAT3(-1.0f, 1.0f, 0.0f);
	vertex[0].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[0].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[0].TexCoord = DirectX::XMFLOAT2(x, 0.0f);

	vertex[1].Position = DirectX::XMFLOAT3(1.0f, 1.0f, 0.0f);
	vertex[1].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[1].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[1].TexCoord = DirectX::XMFLOAT2(x + (1.0f / 5), 0.0f);

	vertex[2].Position = DirectX::XMFLOAT3(-1.0f, -1.0f, 0.0f);
	vertex[2].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[2].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[2].TexCoord = DirectX::XMFLOAT2(x, 1.0f);

	vertex[3].Position = DirectX::XMFLOAT3(1.0f, -1.0f, 0.0f);
	vertex[3].Normal   = DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f);
	vertex[3].Diffuse  = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	vertex[3].TexCoord = DirectX::XMFLOAT2(x + (1.0f / 5), 1.0f);

	Renderer::GetDeviceContext()->Unmap(m_VertexBuffer, 0);

	//----- �J�����̃r���[�}�g���b�N�X�擾
	Scene* scene = Manager::GetScene();
	Camera* camera = scene->GetGameObject<Camera>();
	DirectX::XMFLOAT4X4 view = camera->GetViewMatrix();

	//----- �r���[�̋t�s�� (�]�u�s��)
	DirectX::XMFLOAT4X4 invView;
	invView._11 = view._11;
	invView._12 = view._21;
	invView._13 = view._31;
	invView._14 = 0.0f;
	invView._21 = view._12;
	invView._22 = view._22;
	invView._23 = view._32;
	invView._24 = 0.0f;
	invView._31 = view._13;
	invView._32 = view._23;
	invView._33 = view._33;
	invView._34 = 0.0f;
	invView._41 = 0.0f;
	invView._42 = 0.0f;
	invView._43 = 0.0f;
	invView._44 = 1.0f;

	//----- ���[���h�}�g���N�X�ݒ�
	DirectX::XMFLOAT4X4 world;
	DirectX::XMMATRIX scale, trans;
	scale = DirectX::XMMatrixScaling(m_Scale.x, m_Scale.y, m_Scale.z);
	trans = DirectX::XMMatrixTranslation(m_Position.x, m_Position.y, m_Position.z);
	DirectX::XMStoreFloat4x4(&world, scale * DirectX::XMLoadFloat4x4(&invView) * trans);
	Renderer::SetWorldMatrix(&world);

	//----- ���_�o�b�t�@�ݒ�
	UINT stride = sizeof(VERTEX_3D);
	UINT offset = 0;
	Renderer::GetDeviceContext()->IASetVertexBuffers(0, 1, &m_VertexBuffer, &stride, &offset);

	//----- �}�e���A���ݒ�
	MATERIAL material;
	ZeroMemory(&material, sizeof(material));
	material.Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	material.TextureEnable = true;
	Renderer::SetMaterial(material);

	//----- �e�N�X�`���ݒ�
	Renderer::GetDeviceContext()->PSSetShaderResources(0, 1, &m_Texture);

	//----- �v���~�e�B�u�g�|���W�ݒ�
	Renderer::GetDeviceContext()->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP);

	//----- �|���S���`��
	Renderer::GetDeviceContext()->Draw(4, 0);
}
