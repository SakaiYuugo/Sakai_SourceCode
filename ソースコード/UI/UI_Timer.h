#pragma once
#include "GameObject.h"

class UI_Timer : public GameObject
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
	制限時間を設定(秒単位)
	
	[戻り値]
	void
	***************************************************************************/
	static void SetTimeLimit(int time) { m_TimeLimit = time; }

	/***************************************************************************
	[概要]
	制限時間を取得(秒単位)
	
	[戻り値]
	void
	***************************************************************************/
	static int GetTimeLimit() { return m_TimeLimit; }

	/***************************************************************************
	[概要]
	現在経過時間を設定(秒単位)

	[戻り値]
	void
	***************************************************************************/
	static void SetCurrentTime(int time) { m_CurrentTime = time; }

	/***************************************************************************
	[概要]
	現在の経過時間を取得(秒単位)

	[戻り値]
	void
	***************************************************************************/
	static int GetCurrentTimer() { return m_CurrentTime; }

	/***************************************************************************
	[概要]
	現在の経過時間を取得(秒単位)
	
	[戻り値]
	void
	***************************************************************************/
	static int GetElapsedTime() { return m_ElapsedTime; }

private:
	struct ConstantBuffer
	{
		DirectX::XMMATRIX WorldViewProjection;
		DirectX::XMFLOAT4 FlashColor;
	};
	ConstantBuffer buffer;
private:
	static int m_TimeLimit;
	static int m_CurrentTime;
	static int m_ElapsedTime;   // 経過時間

	ID3D11Buffer* m_VertexBuffer{};
	ID3D11ShaderResourceView* m_texture{};

	int m_Count;
	
};