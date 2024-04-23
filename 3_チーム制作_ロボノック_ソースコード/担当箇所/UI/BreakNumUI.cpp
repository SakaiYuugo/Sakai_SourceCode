#include "BreakNumUI.h"
#include "Sprite.h"
#include "DirectXTex/Texture.h"

//********** 定数定義 **********
#define MAX_BREAK_UI        (2)   // UIの桁数
#define ANIM_BREAK_FRAME    (2)   // アニメーションの1コマのフレーム数
#define ANIM_BREAK_SPLIT_X (11)	  // 画像の分割数（横）
#define ANIM_BREAK_SPLIT_Y  (1)   // 画像の分割数（縦）


//--------------------
// コンストラクタ
//--------------------
BreakNumUI::BreakNumUI()
{
	m_pSprite = new SpriteManager;

	for (int i = 0; i < MAX_BREAK_UI; i++)
	{
		// 画像の位置
		m_BreakUI[i].Pos = { 130.0f + (i * 60.0f), 657.0f };

		// 画像のサイズ
		m_BreakUI[i].Size = { 65.0f, -70.0f };

		// 画像の角度
		m_BreakUI[i].Angle = { 0.0f, 0.0f, 0.0f };

		// 画像の拡縮
		m_BreakUI[i].Scale = { 1.0f, 1.0f, 1.0f };

		// 画像の左上のUV座標
		m_BreakUI[i].posTexCoord = { 0.0f, 0.0f };

		// 画像の分割数
		m_BreakUI[i].sizeTexCoord.x = 1.0f / (float)ANIM_BREAK_SPLIT_X;
		m_BreakUI[i].sizeTexCoord.y = 1.0f;

		// 画像の使用状態
		m_BreakUI[i].use = true;

		// フレーム数
		m_BreakUI[i].frame = 0;

		// 画像の何コマ目か
		m_BreakUI[i].currentAnimNo = 0;

		// テクスチャ読み込み
		LoadTextureFromFile("Assets/2D/number_blue.png", &m_BreakUI[i].pBreakUI);
	}

	// 打ち付け回数初期化
	m_BreakCnt = 0;
}


//--------------------
// デストラクタ
//--------------------
BreakNumUI::~BreakNumUI()
{
	for (int i = 0; i < MAX_BREAK_UI; i++)
	{
		m_BreakUI[i].pBreakUI->Release();
	}

	delete m_pSprite;
}


//--------------------
// 描画
//--------------------
void BreakNumUI::Draw()
{
	for (int i = 0; i < MAX_BREAK_UI; i++)
	{
		m_pSprite->Draw(
			m_BreakUI[i].Pos, m_BreakUI[i].Size, m_BreakUI[i].Angle,
			m_BreakUI[i].posTexCoord, m_BreakUI[i].sizeTexCoord,
			{ 1.4f,1.4f,1.4f,1.0f },
			m_BreakUI[i].pBreakUI);
	}

	Sprite::SetUVPos({ 0.0f, 0.0f });
	Sprite::SetUVScale({ 1.0f, 1.0f });
}


//--------------------------------------
// 破壊数増加
// 引数：int num  破壊したブロックの数
//--------------------------------------
void BreakNumUI::AddNum(int num)
{

		m_BreakCnt += 1;

		// 上限補正
		if (99 < m_BreakCnt)
		{
			m_BreakCnt = 99;
		}

		// テクスチャ座標更新
		BreakNumUI::UpdateTexCoord();

}



//----------------------------------
// 画像のUV座標更新
//----------------------------------
void BreakNumUI::UpdateTexCoord()
{
	int temp = m_BreakCnt;

	for (int i = MAX_BREAK_UI - 1; 0 <= i; i--)
	{
		// tmpの値の下一桁取得
		m_BreakUI[i].currentAnimNo = temp % 10;

		// テクスチャ座標更新
		m_BreakUI[i].posTexCoord.x = m_BreakUI[i].sizeTexCoord.x *
			(m_BreakUI[i].currentAnimNo % ANIM_BREAK_SPLIT_X);

		m_BreakUI[i].posTexCoord.y = m_BreakUI[i].sizeTexCoord.y *
			(m_BreakUI[i].currentAnimNo / ANIM_BREAK_SPLIT_X);

		// 桁落とし
		temp /= 10;
	}
}