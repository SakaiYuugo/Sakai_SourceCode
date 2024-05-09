#include "Manager.h"
#include "Sprite.h"
#include "CreateTexture.h"


void Sprite::Init(int x, int y, int Width, int Height, const char* TextureName)
{
	m_Vertex[0].Position = DirectX::XMFLOAT3((float)x, (float)y, 0.0f);
	m_Vertex[0].Normal = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);
	m_Vertex[0].Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	m_Vertex[0].TexCoord = DirectX::XMFLOAT2(0.0f, 0.0f);

	m_Vertex[1].Position = DirectX::XMFLOAT3((float)Width, (float)y, 0.0f);
	m_Vertex[1].Normal = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);
	m_Vertex[1].Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	m_Vertex[1].TexCoord = DirectX::XMFLOAT2(1.0f, 0.0f);

	m_Vertex[2].Position = DirectX::XMFLOAT3((float)x, (float)Height, 0.0f);
	m_Vertex[2].Normal = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);
	m_Vertex[2].Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	m_Vertex[2].TexCoord = DirectX::XMFLOAT2(0.0f, 1.0f);

	m_Vertex[3].Position = DirectX::XMFLOAT3((float)Width, (float)Height, 0.0f);
	m_Vertex[3].Normal = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);
	m_Vertex[3].Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
	m_Vertex[3].TexCoord = DirectX::XMFLOAT2(1.0f, 1.0f);


	// 頂点バッファ生成
	D3D11_BUFFER_DESC bd{};
	bd.Usage = D3D11_USAGE_DEFAULT;
	bd.ByteWidth = sizeof(VERTEX_3D) * 4;
	bd.BindFlags = D3D11_BIND_VERTEX_BUFFER;
	bd.CPUAccessFlags = 0;

	D3D11_SUBRESOURCE_DATA sd{};
	sd.pSysMem = m_Vertex;

	Renderer::GetDevice()->CreateBuffer(&bd, &sd, &m_VertexBuffer);

	// テクスチャ読み込み
	if (TextureName)
	{
		CreateTextureFromFile(Renderer::GetDevice(), TextureName, &m_Texture);
		assert(m_Texture);
		m_Material.TextureEnable = TRUE;
	}
	else
	{
		m_Texture = nullptr;
		m_Material.TextureEnable = FALSE;
	}
	m_Material.Diffuse = DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f);
}



void Sprite::Uninit()
{
	m_VertexBuffer->Release();
	if (m_Texture)
	{
		m_Texture->Release();
	}
}



void Sprite::Draw()
{
	// マトリクス設定
	Renderer::SetWorldViewProjection2D();

	// 頂点バッファ設定
	UINT stride = sizeof(VERTEX_3D);
	UINT offset = 0;
	Renderer::GetDeviceContext()->IASetVertexBuffers(0, 1, &m_VertexBuffer, &stride, &offset);

	// テクスチャ設定
	if (m_Texture)
	{
		Renderer::GetDeviceContext()->PSSetShaderResources(0, 1, &m_Texture);
	}
	

	// プリミティブトポロジ設定
	Renderer::GetDeviceContext()->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP);

	// マテリアル設定
	Renderer::SetMaterial(m_Material);

	// ポリゴン描画
	Renderer::GetDeviceContext()->Draw(4, 0);

}