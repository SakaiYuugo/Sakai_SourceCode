#include "Manager.h"
#include "ModelRenderer.h"
#include "Field.h"
#include "Shader.h"

//----- �ÓI�����o�ϐ�������
const float Field::m_Radius = 28.0f;
const DirectX::XMFLOAT3 Field::CenterPoint = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);

/***************************************************************************
[�T�v]
�v���C���[������

[�߂�l]
void
***************************************************************************/
void Field::Init()
{
	AddComponent<ModelRenderer>()->Load("asset/model/Field/Field.obj");
	SetPosition(DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f));
	SetScale(DirectX::XMFLOAT3(Field::m_Radius, 20.0f, Field::m_Radius));
}