#include "Manager.h"
#include "Renderer.h"
#include "Shader.h"



void Shader::Load(const char* vertexshader, const char* pixelshader)
{
	Renderer::CreateVertexShader(&m_VertexShader, &m_VertexLayout, vertexshader);

	Renderer::CreatePixelShader(&m_PixelShader, pixelshader);
}

void Shader::Uninit()
{
	m_VertexLayout->Release();
	m_VertexShader->Release();
	m_PixelShader->Release();
}


void Shader::Draw()
{
	// 入力レイアウト設定
	Renderer::GetDeviceContext()->IASetInputLayout(m_VertexLayout);

	// シェーダ設定
	Renderer::GetDeviceContext()->VSSetShader(m_VertexShader, nullptr, 0);
	Renderer::GetDeviceContext()->PSSetShader(m_PixelShader, nullptr, 0);

}
