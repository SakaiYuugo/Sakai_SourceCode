#include "jewelryUI.h"
#include "Sprite.h"
#include "DirectXTex/Texture.h"

#define MAX_JEWELRY     (3)
#define MAX_CUREECTCNT (55)

//---------------------------------
// コンストラクタ
//---------------------------------
JewelryUI::JewelryUI()
	: m_UINum(0), m_Count(0)
{
	m_pSprite = new SpriteManager;
	
	for (int i = 0; i < MAX_JEWELRY * 2; i++)
	{
		m_Jewelry[i].Pos         = { 0.0f, 0.0f};
		m_Jewelry[i].Size        = { 0.0f, 0.0f };
		m_Jewelry[i].Angle       = { 0.0f, 0.0f,  0.0f };
		m_Jewelry[i].StartPos    = { 0.0f, 0.0f };
		m_Jewelry[i].EndPos      = { 0.0f, 0.0f };
		m_Jewelry[i].ControlPos1 = { 0.0f, 0.0f };
		m_Jewelry[i].ControlPos2 = { 0.0f, 0.0f };
		m_Jewelry[i].CurrentCnt = 0;
		m_Jewelry[i].MaxCnt = 90;
		m_Jewelry[i].GetJewelry = false;
		m_Jewelry[i].Use = false;
		m_Jewelry[i].AnimeFlg = false;
	}

}


//--------------------
// デストラクタ
//--------------------
JewelryUI::~JewelryUI()
{
	for (int i = 0; i < m_UINum; i++)
	{
		m_Jewelry[i].pJewelryGet->Release();
		m_Jewelry[i].pJewelryNot->Release();
	}

	delete m_pSprite;
}


//----------------------------------------
// 宝石を作成
// 引数：int num 作成する宝石UIの数
//----------------------------------------
void JewelryUI::MakeJewelryUI(int Num)
{
	// 保存用変数に作成するUIの数を代入
	m_UINum = Num;

	for (int i = 0; i < Num * 2; i++)
	{
		if (i < Num)
		{
			m_Jewelry[i].Pos = { 1110.0f, 200.0f - (i * 75.0f) };
		}
		else
		{
			m_Jewelry[i].Pos = { -1500.0f, 800.0f };
		}

		m_Jewelry[i].Size  = { 80.0f, -70.0f };
		m_Jewelry[i].Angle = { 0.0f,   0.0f,  10.0f };
		m_Jewelry[i].GetJewelry = false;

		LoadTextureFromFile("Assets/2D/ダイヤ_未獲得.png", &m_Jewelry[i].pJewelryNot);
		LoadTextureFromFile("Assets/2D/ダイヤ_獲得.png", &m_Jewelry[i].pJewelryGet);
	}

}


//----------------------------------------
// 宝石のUIを切り替える
//----------------------------------------
void JewelryUI::GetJewelryUI()
{
	m_Jewelry[m_Count + m_UINum].GetJewelry = true;
	tmp++;
}


//----------------------------------------
// 宝石取得時のアニメーション更新
//----------------------------------------
void JewelryUI::GetMoveJewelry()
{
	for (int i = m_UINum; i < m_UINum * 2; i++)
	{
		// 宝石を取得していた場合
		if (m_Jewelry[i].GetJewelry && !m_Jewelry[i].AnimeFlg)
		{
			// 一度だけやる処理
			if (m_Jewelry[i].Use == false)
			{
				//引数の座標からスクリーン座標に変換する
				m_2DJewelyPos = m_pCameraManager->ChangeScreenPos(m_pPlayer->GetPos());
				// プレイヤーの現在座標を取得
				m_Jewelry[i].Pos = m_2DJewelyPos;

				// 開始地点の設定
				m_Jewelry[i].StartPos = m_2DJewelyPos;

				// 終了地点
				m_Jewelry[i].EndPos = m_Jewelry[i - m_UINum].Pos;

				// 制御点１
				m_Jewelry[i].ControlPos1 = {
					(m_Jewelry[i].EndPos.x - m_Jewelry[i].StartPos.x) * 0.4f,
					(m_Jewelry[i].EndPos.y - m_Jewelry[i].StartPos.y) * 0.15f
				};

				// 制御点２
				m_Jewelry[i].ControlPos2 = {
				(m_Jewelry[i].EndPos.x - m_Jewelry[i].StartPos.x) * 0.3f,
				(m_Jewelry[i].EndPos.y - m_Jewelry[i].StartPos.y) * 0.03f
				};

				// フラグ切り替え
				m_Jewelry[i].Use = true;
			}
		

			// カウンタの更新
			m_Jewelry[i].CurrentCnt++;

			float Bzier0 = pow(1 - (float)m_Jewelry[i].CurrentCnt / m_Jewelry[i].MaxCnt, 3);
			float Bzier1 = 
				 pow(3 * (float)m_Jewelry[i].CurrentCnt / m_Jewelry[i].MaxCnt * 
				(pow(1 - (float)m_Jewelry[i].CurrentCnt / m_Jewelry[i].MaxCnt, 2)), 3);

			float Bzier2 = 
				 pow(3 * (float)m_Jewelry[i].CurrentCnt / m_Jewelry[i].MaxCnt, 2) *
				(1 - (float)m_Jewelry[i].CurrentCnt / m_Jewelry[i].MaxCnt);
			float Bzier3 = pow((float)m_Jewelry[i].CurrentCnt / m_Jewelry[i].MaxCnt, 3);

			// 座標の更新
			m_Jewelry[i].Pos.x = (m_Jewelry[i].StartPos.x    * Bzier0) +
				                   (m_Jewelry[i].ControlPos1.x * Bzier1) +
								   (m_Jewelry[i].ControlPos2.x * Bzier2) +
				                   (m_Jewelry[i].EndPos.x      * Bzier3);

			m_Jewelry[i].Pos.y = (m_Jewelry[i].StartPos.y    * Bzier0) +
				                   (m_Jewelry[i].ControlPos1.y * Bzier1) +
							       (m_Jewelry[i].ControlPos2.y * Bzier2) +
				                   (m_Jewelry[i].EndPos.y      * Bzier3);

			// 終了判定
			if (m_Jewelry[i].EndPos.x - 25.0f <= m_Jewelry[i].Pos.x &&
				m_Jewelry[i].EndPos.x + 25.0f >= m_Jewelry[i].Pos.x)
			{
				if (m_Jewelry[i].EndPos.y - 25.0f <= m_Jewelry[i].Pos.y &&
					m_Jewelry[i].EndPos.y + 25.0f >= m_Jewelry[i].Pos.y)
				{
					// 移動時に使用したUIを画面外へ
					m_Jewelry[i].Size = { 0.0f, 0.0f };
					m_Jewelry[i].Pos = { 1500.0f, 800.0f };
					m_Jewelry[i].AnimeFlg = true;

					// 獲得時の宝石UIの表示
					m_Jewelry[i - m_UINum].GetJewelry = true;

				}
			}
		}
	}

}


//--------------------
// 描画
//--------------------
void JewelryUI::Draw()
{
	for (int i = 0; i < m_UINum * 2; i++)
	{
		// 宝石が未獲得の場合
		if (m_Jewelry[i].GetJewelry == false)
		{
			m_pSprite->Draw(m_Jewelry[i].Pos, m_Jewelry[i].Size, m_Jewelry[i].Angle,
							{ 0.0f,0.0f }, { 1.0f,1.0f }, { 1.0f,1.0f,1.0f,1.0f }, 
							m_Jewelry[i].pJewelryNot);
		}

		// 宝石を獲得している場合
		if (m_Jewelry[i].GetJewelry == true)
		{
			m_pSprite->Draw(m_Jewelry[i].Pos, m_Jewelry[i].Size, m_Jewelry[i].Angle,
							{ 0.0f,0.0f }, { 1.0f,1.0f }, { 1.0f,1.0f,1.0f,1.0f }, 
							m_Jewelry[i].pJewelryGet);
		}
	
	}
}