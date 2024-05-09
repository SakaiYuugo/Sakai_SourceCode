#include "GameObject.h"

//----- �q�b�g�X�g�b�v�p�̐ÓI�ϐ�
int GameObject::m_HitStopTagBit = 0x00;
int GameObject::m_HitStopTime = 0;

/***************************************************************************
[�T�v]
�������̃x�[�X

[�߂�l]
void
***************************************************************************/
void GameObject::InitBase()
{
	Init();
}

/***************************************************************************
[�T�v]
����̃x�[�X

[�߂�l]
void
***************************************************************************/
void GameObject::UninitBase()
{
	Uninit();

	for (Component* component : m_Component)
	{
		component->Uninit();
		delete component;
	}
	m_Component.clear();
}

/***************************************************************************
[�T�v]
�X�V�̃x�[�X

[�߂�l]
void
***************************************************************************/
void GameObject::UpdateBase()
{

	for (Component* component : m_Component)
	{
		component->Update();
	}

	Update();

}

/***************************************************************************
[�T�v]
�q�b�g�X�g�b�v�p�̍X�V�̃x�[�X

[�߂�l]
void
***************************************************************************/
void GameObject::HitStopUpdateBase()
{
	HitStopUpdate();
}

/***************************************************************************
[�T�v]
�`��̃x�[�X

[�߂�l]
void
***************************************************************************/
void GameObject::DrawBase(DirectX::XMFLOAT4X4 ParentMatrix)
{
	PreDraw();

	// �}�g���N�X�ݒ�
	DirectX::XMFLOAT4X4 world;
	DirectX::XMMATRIX scale, rot, trans;
	scale = DirectX::XMMatrixScaling(m_Scale.x, m_Scale.y, m_Scale.z);

	if (m_UseQuaternion)
	{
		// �N�H�[�^�j�I���̉�]�s��
		rot = DirectX::XMMatrixRotationQuaternion(m_Quaternion.v);
	}
	else
	{
		rot = DirectX::XMMatrixRotationRollPitchYaw(m_Rotation.x, m_Rotation.y, m_Rotation.z);
	}

	trans = DirectX::XMMatrixTranslation(m_Position.x, m_Position.y, m_Position.z);
	DirectX::XMStoreFloat4x4(&world, scale * rot * trans * DirectX::XMLoadFloat4x4(&ParentMatrix));

	for (GameObject* child : m_ChildGameObject)
	{
		child->DrawBase(world);
	}

	Renderer::SetWorldMatrix(&world);

	for (Component* component : m_Component)
	{
		component->Draw();
	}

	Draw();
}