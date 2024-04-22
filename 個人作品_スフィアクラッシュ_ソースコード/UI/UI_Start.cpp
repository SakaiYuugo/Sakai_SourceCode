//----- UI
#include "UI_Start.h"
#include "Time.h"
//----- コンポーネント関係
#include "Audio.h"
#include "Scene.h"
#include "Shader.h"
#include "Sprite.h"
#include "Manager.h"
#include "CreateTexture.h"

bool UI_Start::m_Select = false;

/***************************************************************************
[概要]
初期化処理

[戻り値]
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

	//----- 頂点バッファ生成
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

	//----- テクスチャ読み込み
	CreateTextureFromFile(Renderer::GetDevice(),
		"asset/texture/Title/UI_Start.png", &m_texture);
	assert(m_texture);

	m_Count = 0.0f;
}

/***************************************************************************
[概要]
メモリの解放処理

[戻り値]
void
***************************************************************************/
void UI_Start::Uninit()
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
void UI_Start::Update()
{
	//----- 一定時間経過で拡縮率を変更
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
[概要]
描画処理

[戻り値]
void
***************************************************************************/
void UI_Start::Draw()
{
	//----- マトリクス生成
	Renderer::SetWorldViewProjection2D();

	//----- 頂点バッファ設定
	UINT stride = sizeof(VERTEX_3D);
	UINT offset = 0;
	Renderer::GetDeviceContext()->IASetVertexBuffers(0, 1, &m_VertexBuffer, &stride, &offset);

	//----- マテリアル設定
	MATERIAL material;
	ZeroMemory(&material, sizeof(material));
	material.Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	material.TextureEnable = TRUE;
	Renderer::SetMaterial(material);

	//----- テクスチャ設定
	Renderer::GetDeviceContext()->PSSetShaderResources(0, 1, &m_texture);

	//----- プリミティブトポロジ設定
	Renderer::GetDeviceContext()->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP);

	//----- タイトルでUIが選択中なら、拡縮用の時間をカウント
	if (m_Select)
	{
		m_Time += m_Count;
	}
	else
	{
		DefaultDraw();
		return;
	}

	//----- 頂点データ書き換え
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

	//----- ポリゴン描画
	Renderer::GetDeviceContext()->Draw(4, 0);
}

/***************************************************************************
[概要]
デフォルトサイズで描画

[戻り値]
void
***************************************************************************/
void UI_Start::DefaultDraw()
{
	//----- 頂点データ書き換え
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

	//----- ポリゴン描画
	Renderer::GetDeviceContext()->Draw(4, 0);
}
