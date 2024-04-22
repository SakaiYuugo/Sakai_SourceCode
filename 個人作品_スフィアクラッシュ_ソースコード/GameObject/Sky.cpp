//----- ゲームオブジェクト
#include "Sky.h"
#include "Camera.h"
//----- コンポーネント関係
#include "Renderer.h"
#include "Scene.h"
#include "ModelRenderer.h"
#include "Shader.h"
//----- システム関係
#include "Manager.h"

/***************************************************************************
[概要]
初期化処理

[戻り値]
void
***************************************************************************/
void Sky::Init()
{
	AddComponent<Shader>()->Load("shader/unlitTextureVS.cso", "shader/unlitTexturePS.cso");
	AddComponent<ModelRenderer>()->Load("asset/model/Sky/Sky.obj");
	m_Position = DirectX::XMFLOAT3(0.0f, -30.0f, 0.0f);
	m_Scale = DirectX::XMFLOAT3(100.0f, 100.0f, 100.0f);
}

/***************************************************************************
[概要]
更新処理

[戻り値]
void
***************************************************************************/
void Sky::Update()
{
	Scene* scene = Manager::GetScene();
	Camera* camera = scene->GetGameObject<Camera>();

	DirectX::XMFLOAT3 cameraPosition = camera->GetPosition();
	m_Position = cameraPosition;
}
