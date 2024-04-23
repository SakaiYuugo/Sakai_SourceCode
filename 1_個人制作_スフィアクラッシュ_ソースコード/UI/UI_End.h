#pragma once
#include "GameObject.h"

class UI_End : public GameObject
{
public:
	/***************************************************************************
	[概要]
	初期化処理

	[戻り値]
	void
	***************************************************************************/
	void Init() override;

	/***************************************************************************
	[概要]
	メモリの解放処理

	[戻り値]
	void
	***************************************************************************/
	void Uninit() override;

	/***************************************************************************
	[概要]
	更新処理

	[戻り値]
	void
	***************************************************************************/
	void Update() override;

	/***************************************************************************
	[概要]
	描画処理

	[戻り値]
	void
	***************************************************************************/
	void Draw() override;

	/***************************************************************************
	[概要]
	デフォルトサイズで描画

	[戻り値]
	void
	***************************************************************************/
	void DefaultDraw();

public:
	static bool m_Select;

	/***************************************************************************
	[概要]
	タイトルを選択するか切り替える
	
	[戻り値]
	void
	***************************************************************************/
	static void SetSelectFlg(bool flg) { m_Select = flg; }

	static bool GetSelectFlg() { return m_Select; }

private:
	struct ConstantBuffer
	{
		DirectX::XMMATRIX WorldViewProjection;
		DirectX::XMFLOAT4 FlashColor;
	};
	ConstantBuffer buffer;

private:
	ID3D11Buffer* m_VertexBuffer{};
	ID3D11ShaderResourceView* m_texture{};
	ID3D11ShaderResourceView* m_texture1{};

	const DirectX::XMFLOAT2 m_Enlarge = {30.0f, 20.0f}; // UIの拡大値
	float m_Count;
};