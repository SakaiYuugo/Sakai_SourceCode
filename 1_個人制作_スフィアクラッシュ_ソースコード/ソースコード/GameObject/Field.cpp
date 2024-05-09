#include "Manager.h"
#include "ModelRenderer.h"
#include "Field.h"
#include "Shader.h"

//----- 静的メンバ変数初期化
const float Field::m_Radius = 28.0f;
const DirectX::XMFLOAT3 Field::CenterPoint = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);

/***************************************************************************
[概要]
プレイヤー初期化

[戻り値]
void
***************************************************************************/
void Field::Init()
{
	AddComponent<ModelRenderer>()->Load("asset/model/Field/Field.obj");
	SetPosition(DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f));
	SetScale(DirectX::XMFLOAT3(Field::m_Radius, 20.0f, Field::m_Radius));
}