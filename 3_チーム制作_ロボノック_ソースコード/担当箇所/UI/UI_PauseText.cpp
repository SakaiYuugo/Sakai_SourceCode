#include "UI_PauseText.h"
#include "Sprite.h"
#include "DirectXTex/Texture.h"

//--------------------
// •`‰æ
//--------------------
void UI_Pause_Text::Draw()
{
	if (m_pSprite == nullptr || m_pTexture == nullptr)
	{
		return;
	}

	m_pSprite->Draw(m_Pos, m_Size, m_Angle,
		{ 0.0f,0.0f }, { 1.0f,1.0f }, { 1.0f,1.0f,1.0f,1.0f },
		m_pTexture);
}

void UI_Pause_Text::SetTexture(const char* pTextureName)
{
	LoadTextureFromFile(pTextureName, &m_pTexture);
}

void UI_Pause_Text::SetTransform(DirectX::XMFLOAT2 pos, DirectX::XMFLOAT2 size)
{
	m_Pos = pos;
	m_Size = size;
}