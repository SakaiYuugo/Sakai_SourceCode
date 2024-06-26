//#pragma once
#ifndef UI_PAUSE_TEXT_H
#define UI_PAUSE_TEXT_H
#include <DirectXMath.h>
#include "DirectX.h"
#include "SpriteManager.h"

class UI_Pause_Text
{
public:
	void Draw();

	void SetTexture(const char* pTextureName);
	void SetTransform(DirectX::XMFLOAT2 pos, DirectX::XMFLOAT2 size);

private:
	DirectX::XMFLOAT2 m_Size;               // 画像の大きさ
	DirectX::XMFLOAT2 m_Pos;                // 画像の表示位置
	DirectX::XMFLOAT3 m_Scale;	            // 大きさ
	DirectX::XMFLOAT3 m_Angle;	            // 角度
	ID3D11ShaderResourceView* m_pTexture;   // 未獲得UIの情報
	SpriteManager* m_pSprite;
};

#endif // !UI_PAUSE_TEXT_H
