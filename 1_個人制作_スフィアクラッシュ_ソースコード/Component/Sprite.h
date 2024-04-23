#pragma once

#include "Renderer.h"
#include "Component.h"

class Sprite : public Component
{
private:
	ID3D11Buffer*				m_VertexBuffer{};
	ID3D11ShaderResourceView*	m_Texture{};
	
	VERTEX_3D m_Vertex[4];

	MATERIAL					m_Material{};

public:

	using Component::Component;

	void Init(int x, int y, int Width, int Height, const char* TextureName = nullptr);
	void Uninit() override;
	void Draw() override;

	void SetMaterial(MATERIAL Material) { m_Material = Material; }

};
