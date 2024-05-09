#pragma once
#include "Main.h"
#include "Component.h"

class Shader : public Component
{
public:
	using Component::Component;

	void Load(const char* VertexShader, const char* PixelShader);
	void Uninit() override;
	void Draw() override;

private:
	ID3D11VertexShader*		m_VertexShader{};
	ID3D11PixelShader*		m_PixelShader{};
	ID3D11InputLayout*		m_VertexLayout{};
};
