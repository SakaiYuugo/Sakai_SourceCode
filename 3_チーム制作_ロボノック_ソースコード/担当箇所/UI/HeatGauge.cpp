#include "HeatGauge.h"
#include "Sprite.h"
#include "DirectXTex/Texture.h"

const float MinAngle = 0.0f;    // 針の最低角度
const float MaxAngle = 90.0f;   // 針の最大角度
const int MaxHeatItem = 6;      // ヒートアイテムの最大数

//--------------------
// コンストラクタ
//--------------------
HeatGauge::HeatGauge()
	:m_Size(330.0f, -330.0f), m_DisplayPos(1260.0f, 705.0f),
	 m_Scale(1.0f, 1.0f, 1.0f), m_Angle(0.0f, 0.0f, 0.0f),
	m_Meter(0.0f),  m_UINum(0)
{
	m_pSprite = new SpriteManager;
	LoadTextureFromFile("Assets/2D/針.png", &m_pHeatGauge);

	for (int i = 0; i < MaxHeatItem; i++)
	{
		m_HeatItem[i].m_Pos = { -500.0f, 800.0f };
		m_HeatItem[i].Size = { 100.0f, -100.0f };
		m_HeatItem[i].Angle = { 0.0f, 0.0f, 0.0f };
		m_HeatItem[i].m_StartPos = { 0.0f, 0.0f };
		m_HeatItem[i].m_EndPos = { 980.0f, 660.0f };
		m_HeatItem[i].CurrentCnt = 0;
		m_HeatItem[i].MaxCnt = 30;
		m_HeatItem[i].GetItem = false;
		m_HeatItem[i].Use = false;
		m_HeatItem[i].AnimeFlg = false;
		LoadTextureFromFile("Assets/2D/ヒートアイテム.png", &m_HeatItem[i].m_pHeatItem);
	}
}


//--------------------
// デストラクタ
//--------------------
HeatGauge::~HeatGauge()
{
	for (int i = 0; i < MaxHeatItem; i++)
	{
		m_HeatItem[i].m_pHeatItem->Release();
	}

	m_pHeatGauge->Release();
	delete m_pSprite;
}


//--------------------
// 更新
//--------------------
void HeatGauge::Update()
{	
	for (int i = 0; i < MaxHeatItem; i++)
	{
		// ヒートアイテムを取得していた場合
		if (m_HeatItem[i].GetItem && !m_HeatItem[i].AnimeFlg)
		{
			// 一度だけ行う処理
			if (!m_HeatItem[i].Use)
			{
				// プレイヤーの現在座標を取得
				m_HeatItem[i].m_Pos = m_pCameraManager->ChangeScreenPos(m_3DItemPos);

				// 開始座標セット
				m_HeatItem[i].m_StartPos = m_HeatItem[i].m_Pos;

				// フラグ切り替え
				m_HeatItem[i].Use = true;
			}

			// アニメーションカウンタの更新
			m_HeatItem[i].CurrentCnt++;

			// 座標の更新
			m_HeatItem[i].m_Pos.x = m_HeatItem[i].m_StartPos.x +
				(m_HeatItem[i].m_EndPos.x - m_HeatItem[i].m_StartPos.x) *
				((float)m_HeatItem[i].CurrentCnt / m_HeatItem[i].MaxCnt);
			m_HeatItem[i].m_Pos.y = m_HeatItem[i].m_StartPos.y +
				(m_HeatItem[i].m_EndPos.y - m_HeatItem[i].m_StartPos.y) *
				((float)m_HeatItem[i].CurrentCnt / m_HeatItem[i].MaxCnt);

			m_HeatItem[i].Angle.z += 1.5f;

			// 終了判定
			if (m_HeatItem[i].m_EndPos.x - 10.0f <= m_HeatItem[i].m_Pos.x &&
				m_HeatItem[i].m_EndPos.x + 10.0f >= m_HeatItem[i].m_Pos.x)
			{
				if (m_HeatItem[i].m_EndPos.y - 10.0f <= m_HeatItem[i].m_Pos.y &&
					m_HeatItem[i].m_EndPos.y + 10.0f >= m_HeatItem[i].m_Pos.y)
				{
					// UIを画面外へ移動
					m_HeatItem[i].m_Pos = { -500.0f, 800.0f };

					// 使わないUIのフラグ切り替え
					m_HeatItem[i].GetItem = false;
					m_HeatItem[i].AnimeFlg = true;
				}
			}
		}

		
		// 現在のアイテム数に応じてメーターの角度を変更
		if (m_Angle.z < m_Meter * 30.0f)
		{
			if (m_Meter <= 3)
			{
				m_Angle.z += 0.3f;
			}
		}
		else
		{
			m_Angle.z -= 0.5f;
		}	
	}
}


//--------------------
// ヒートメーター増加
//--------------------
void HeatGauge::IncreaseHeat(float gauge)
{
	// ヒートアイテム取得
	m_HeatItem[m_UINum].GetItem = true;
	m_UINum++;

	m_Meter++;

}


//--------------------
// ヒートメーター減少
//--------------------
void HeatGauge::DecreaseHeat(float gauge)
{
	m_Meter = gauge;

	for (int i = 0; i < MaxHeatItem; i++)
	{
		m_HeatItem[i].m_Pos = { -500.0f, 800.0f };
		m_HeatItem[i].m_StartPos = { 0.0f, 0.0f };
		m_HeatItem[i].CurrentCnt = 0;
		m_HeatItem[i].GetItem = false;
		m_HeatItem[i].Use = false;
		m_HeatItem[i].AnimeFlg = false;
	}

	m_UINum = 0;
}


//--------------------
// 描画
//--------------------
void HeatGauge::Draw()
{
	// ヒートメーター描画
	m_pSprite->Draw(m_DisplayPos, m_Size, m_Angle, { 0.0f,0.0f }, { 1.0f,1.0f }, { 1.0f,1.0f,1.0f,1.0f }, m_pHeatGauge);

	// ヒートアイテム描画
	for (int i = 0; i < MaxHeatItem; i++)
	{
		if (!m_HeatItem[i].GetItem)
		{
			continue;
		}

		m_pSprite->Draw(m_HeatItem[i].m_Pos, m_HeatItem[i].Size, m_HeatItem[i].Angle,
			{ 0.0f, 0.0f }, { 1.0f, 1.0f }, { 1.0f,1.0f,1.0f,1.0f },
			m_HeatItem[i].m_pHeatItem);
	}

}