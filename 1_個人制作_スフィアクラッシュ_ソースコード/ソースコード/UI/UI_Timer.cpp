//----- UI
#include "UI_Timer.h"
#include "Time.h"
//----- コンポーネント関係
#include "Audio.h"
#include "Scene.h"
#include "Shader.h"
#include "Sprite.h"
#include "CreateTexture.h"

//----- 静的変数
int UI_Timer::m_TimeLimit;
int UI_Timer::m_CurrentTime;
int UI_Timer::m_ElapsedTime;

/***************************************************************************
[概要]
初期化処理

[戻り値]
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

	// 頂点バッファ生成
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

	// テクスチャ読み込み
	CreateTextureFromFile(Renderer::GetDevice(),
		"asset/texture/UI/null-1.png", &m_texture);
	assert(m_texture);

	m_Count = 0;
	m_ElapsedTime = 0;
}

/***************************************************************************
[概要]
メモリの解放処理

[戻り値]
void
***************************************************************************/
void UI_Timer::Uninit()
{
	m_VertexBuffer->Release();
	m_texture->Release();
}

/***************************************************************************
[概要]
更新処理

[戻り値]
void
***************************************************************************/
void UI_Timer::Update()
{
	//----- 現在の制限時間を計算
	m_ElapsedTime = m_TimeLimit - m_CurrentTime;
}

/***************************************************************************
[概要]
描画処理

[戻り値]
void
***************************************************************************/
void UI_Timer::Draw()
{
	// マトリクス生成
	Renderer::SetWorldViewProjection2D();

	// 頂点バッファ設定
	UINT stride = sizeof(VERTEX_3D);
	UINT offset = 0;
	Renderer::GetDeviceContext()->IASetVertexBuffers(0, 1, &m_VertexBuffer, &stride, &offset);

	// マテリアル設定
	MATERIAL material;
	ZeroMemory(&material, sizeof(material));
	material.Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	material.TextureEnable = TRUE;
	Renderer::SetMaterial(material);

	// テクスチャ設定
	Renderer::GetDeviceContext()->PSSetShaderResources(0, 1, &m_texture);

	// プリミティブトポロジ設定
	Renderer::GetDeviceContext()->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP);

	float tx;

	int TensPlace = (m_ElapsedTime / 10) % 10;  // 10の位の値を割り出す
	int OnesPlace = m_ElapsedTime % 10;         //  1の位の値を割り出す

	//----- 2桁描画
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

		//----- 頂点データ書き換え
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

		// ポリゴン描画
		Renderer::GetDeviceContext()->Draw(4, 0);
	}
}
