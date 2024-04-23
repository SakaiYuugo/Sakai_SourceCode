//----- �Q�[���I�u�W�F�N�g
#include "Sky.h"
#include "Camera.h"
//----- �R���|�[�l���g�֌W
#include "Renderer.h"
#include "Scene.h"
#include "ModelRenderer.h"
#include "Shader.h"
//----- �V�X�e���֌W
#include "Manager.h"

/***************************************************************************
[�T�v]
����������

[�߂�l]
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
[�T�v]
�X�V����

[�߂�l]
void
***************************************************************************/
void Sky::Update()
{
	Scene* scene = Manager::GetScene();
	Camera* camera = scene->GetGameObject<Camera>();

	DirectX::XMFLOAT3 cameraPosition = camera->GetPosition();
	m_Position = cameraPosition;
}
