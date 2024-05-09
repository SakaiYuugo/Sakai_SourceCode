#include "GameObject.h"

//----- ヒットストップ用の静的変数
int GameObject::m_HitStopTagBit = 0x00;
int GameObject::m_HitStopTime = 0;

/***************************************************************************
[概要]
初期化のベース

[戻り値]
void
***************************************************************************/
void GameObject::InitBase()
{
	Init();
}

/***************************************************************************
[概要]
解放のベース

[戻り値]
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
[概要]
更新のベース

[戻り値]
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
[概要]
ヒットストップ用の更新のベース

[戻り値]
void
***************************************************************************/
void GameObject::HitStopUpdateBase()
{
	HitStopUpdate();
}

/***************************************************************************
[概要]
描画のベース

[戻り値]
void
***************************************************************************/
void GameObject::DrawBase(DirectX::XMFLOAT4X4 ParentMatrix)
{
	PreDraw();

	// マトリクス設定
	DirectX::XMFLOAT4X4 world;
	DirectX::XMMATRIX scale, rot, trans;
	scale = DirectX::XMMatrixScaling(m_Scale.x, m_Scale.y, m_Scale.z);

	if (m_UseQuaternion)
	{
		// クォータニオンの回転行列
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