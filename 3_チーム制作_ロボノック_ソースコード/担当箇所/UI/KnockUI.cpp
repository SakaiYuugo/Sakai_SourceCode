#include "knockUI.h"
#include "Sprite.h"
#include "DirectXTex/Texture.h"

//********** 定数定義 **********
#define MAX_KNOCK_UI  (2)   // UIの桁数
#define ANIM_FRAME    (2)   // アニメーションの1コマのフレーム数
#define ANIM_SPLIT_X (11)	// 画像の分割数（横）
#define ANIM_SPLIT_Y  (1)   // 画像の分割数（縦）


//--------------------
// コンストラクタ
//--------------------
KnockUI::KnockUI()
{
	m_pSprite = new SpriteManager;

	for (int i = 0; i < MAX_KNOCK_UI; i++)
	{
		// 画像の位置
		m_KnockUI[i].Pos = { 340.0f + (i * 60.0f), 657.0f };

		// 画像のサイズ
		m_KnockUI[i].Size = { 65.0f, -70.0f };

		// 画像の角度
		m_KnockUI[i].Angle = { 0.0f, 0.0f, 0.0f };

		// 画像の拡縮
		m_KnockUI[i].Scale = { 1.0f, 1.0f, 1.0f };

		// 画像の左上のUV座標
		m_KnockUI[i].posTexCoord = { 0.0f, 0.0f };

		// 画像の分割数
		m_KnockUI[i].sizeTexCoord.x = 1.0f / (float)ANIM_SPLIT_X;
		m_KnockUI[i].sizeTexCoord.y = 1.0f;

		// 画像の使用状態
		m_KnockUI[i].use = true;

		// フレーム数
		m_KnockUI[i].frame = 0;

		// 画像の何コマ目か
		m_KnockUI[i].currentAnimNo = 0;

		// テクスチャ読み込み
		LoadTextureFromFile("Assets/2D/number_blue.png", &m_KnockUI[i].pKnockUI);
	}

	// 打ち付け回数初期化
	m_KnockCnt = 0;


}


//--------------------
// デストラクタ
//--------------------
KnockUI::~KnockUI()
{
	for (int i = 0; i < MAX_KNOCK_UI; i++)
	{
		m_KnockUI[i].pKnockUI->Release();
	}
	
	delete m_pSprite;
}


//--------------------
// 描画
//--------------------
void KnockUI::Draw()
{
	for (int i = 0; i < MAX_KNOCK_UI; i++)
	{
		// SpriteにUV情報を送る
		Sprite::SetUVPos(m_KnockUI[i].posTexCoord);
		Sprite::SetUVScale(m_KnockUI[i].sizeTexCoord);

		m_pSprite->Draw(
			m_KnockUI[i].Pos, m_KnockUI[i].Size, m_KnockUI[i].Angle,
			m_KnockUI[i].posTexCoord, m_KnockUI[i].sizeTexCoord, { 1.5f,1.5f,1.5f,1.0f },
			m_KnockUI[i].pKnockUI);
	}

	Sprite::SetUVPos({ 0.0f, 0.0f });
	Sprite::SetUVScale({1.0f, 1.0f});
}


//----------------------------------
// 打ち付け回数増加
// 引数：int num  打ち付けた回数
//----------------------------------
void KnockUI::AddKnock(int num)
{
	m_KnockCnt += num;

	// 上限補正
	if (99 < m_KnockCnt)
	{
		m_KnockCnt = 99;
	}

	// テクスチャ座標更新
	KnockUI::UpdateTexCoord();
}


//----------------------------------
// 画像のUV座標更新
//----------------------------------
void KnockUI::UpdateTexCoord()
{
	int temp = m_KnockCnt;

	for (int i = MAX_KNOCK_UI - 1; 0 <= i; i--)
	{
		// tmpの値の下一桁取得
		m_KnockUI[i].currentAnimNo = temp % 10;

		// テクスチャ座標更新
		m_KnockUI[i].posTexCoord.x = m_KnockUI[i].sizeTexCoord.x *
			(m_KnockUI[i].currentAnimNo % ANIM_SPLIT_X);

		m_KnockUI[i].posTexCoord.y = m_KnockUI[i].sizeTexCoord.y *
			(m_KnockUI[i].currentAnimNo / ANIM_SPLIT_X);

		// 桁落とし
		temp /= 10;
	}
}