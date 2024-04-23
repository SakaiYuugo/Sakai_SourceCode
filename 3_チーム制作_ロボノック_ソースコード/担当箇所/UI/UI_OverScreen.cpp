#include "UI_OverScreen.h"
#include "Sprite.h"
#include "DirectXTex/Texture.h"

//----------------------------
// コンストラクタ
//----------------------------
UI_OverScreen::UI_OverScreen()
	:m_NowState(STATE_MAX)
{
	m_pSprite = new SpriteManager;

	DirectX::XMFLOAT2 WindowSize = { 1280.0f, 720.0f };

	//スクリーンのポジションの設定
	m_ScreenTransform.Pos = { WindowSize.x * 0.5f,-200.0f };
	m_ScreenTransform.Size = { 1200.0f,-600.0f };
	m_ScreenTransform.Scale = { 1.0f,1.0f,1.0f };
	m_ScreenTransform.Angle = { 0.0f,0.0f,0.0f };
	// テクスチャ読み込み
	LoadTextureFromFile("Assets/2D/screen.png", &m_pScreenTexture);

	const char* TextureName[UI_Over::MAX] =
	{
		"Assets/2D_Roma/letter/GameOver0.png",		//ゲームオーバー
		"Assets/2D_Roma/letter/Retry0.png",			//リトライ
		"Assets/2D_Roma/letter/StageSelect.png",	//ステージセレクト
	};

	const char* ChoseTextureName[UI_Over::MAX] =
	{
		"Assets/2D_Roma/letter/GameOver0.png",			//ゲームオーバー
		"Assets/2D_Roma/letter/Retry_Select.png",		//リトライ
		"Assets/2D_Roma/letter/StageSelect_Select.png",	//ステージセレクト
	};

	DirectX::XMFLOAT2 PosIndex[UI_Over::MAX] =
	{
		{0.0f,-110.0f},	//ゲームオーバー
		{0.0f,20.0f},	//リトライ
		{0.0f,90.0f},	//ステージセレクト
	};

	DirectX::XMFLOAT2 SizeIndex[UI_Over::MAX] =
	{
		{400.0f,-50.0f},
		{240.0f,-40.0f},
		{360.0f,-45.0f},
	};

	//座標の設定
	for (int i = 0; i < UI_Over::MAX; i++)
	{
		m_Transform[i].Pos = PosIndex[i];
		m_Transform[i].Size = SizeIndex[i];
		m_Transform[i].Scale = { 1.0f,1.0f,1.0f };
		m_Transform[i].Angle = { 0.0f,0.0f,0.0f };

		LoadTextureFromFile(TextureName[i], &m_pTexture[i]);
		LoadTextureFromFile(ChoseTextureName[i], &m_pChoseTexture[i]);
	}

	m_pArm = new Image2D("Assets/2D/screen_arm.png", m_pSprite);
	m_pArm->size = { 4890.0f * 0.15f,3135.0f * 0.15f };
	m_ArmAddPos = { 0.0f, -200.0f };
}

//--------------------
// デストラクタ
//--------------------
UI_OverScreen::~UI_OverScreen()
{
	delete m_pArm;
	for (int i = 0; i < UI_Over::MAX; i++)
	{
		m_pTexture[i]->Release();
		m_pChoseTexture[i]->Release();
	}
	m_pScreenTexture->Release();

	delete m_pSprite;
}

//--------------------
// 更新
//--------------------
bool UI_OverScreen::Update()
{
	const unsigned int SCREEN_CENTER_SIDE = 1280 * 0.5f;
	const unsigned int SCREEN_CENTER_LENGTH = 720 * 0.5f;

	bool End_exe = false;

	switch (m_NowState)
	{
	case UI_OverScreen::STATE_INIT:
		m_NowState = UI_OverScreen::STATE_IN;
		break;
	case UI_OverScreen::STATE_IN:	//スクリーンを入れる
		if (SCREEN_CENTER_LENGTH <= m_ScreenTransform.Pos.y)
		{
			m_ScreenTransform.Pos.y = SCREEN_CENTER_LENGTH;
			m_NowState = UI_OverScreen::STATE_STAY;
		}

		m_ScreenTransform.Pos.y += 15.0f;
		break;
	case UI_OverScreen::STATE_STAY:

		break;
	case UI_OverScreen::STATE_OUT:	//スクリーンを外に出す
		if (-250.0f >= m_ScreenTransform.Pos.y)
		{
			m_NowState = UI_OverScreen::STATE_END;
		}

		m_ScreenTransform.Pos.y -= 25.0f;

		break;
	case UI_OverScreen::STATE_END:
		End_exe = true;
		break;
	default:
		break;
	}

	return End_exe;
}

void UI_OverScreen::ScreenInit()
{
	m_NowState = UI_OverScreen::STATE_INIT;
}

void UI_OverScreen::ScreenDraw()
{
	m_pArm->pos.x = m_ScreenTransform.Pos.x + m_ArmAddPos.x;
	m_pArm->pos.y = m_ScreenTransform.Pos.y + m_ArmAddPos.y;
	m_pArm->Draw();

	m_pSprite->Draw(m_ScreenTransform.Pos, m_ScreenTransform.Size, m_ScreenTransform.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
		m_pScreenTexture);
}

void UI_OverScreen::GameOverDraw()
{
	UItransform TempPos = m_Transform[UI_Over::GAMEOVER];

	TempPos.Pos.x += m_ScreenTransform.Pos.x;
	TempPos.Pos.y += m_ScreenTransform.Pos.y;

	m_pSprite->Draw(TempPos.Pos, TempPos.Size, TempPos.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
		m_pChoseTexture[UI_Over::GAMEOVER]);
}

void UI_OverScreen::RetryDraw(bool Select)
{
	UItransform TempPos = m_Transform[UI_Over::RETRY];
	ID3D11ShaderResourceView* pTexture;
	if (Select)
	{
		pTexture = m_pChoseTexture[UI_Over::RETRY];
	}
	else
	{
		pTexture = m_pTexture[UI_Over::RETRY];
	}


	TempPos.Pos.x += m_ScreenTransform.Pos.x;
	TempPos.Pos.y += m_ScreenTransform.Pos.y;

	m_pSprite->Draw(TempPos.Pos, TempPos.Size, TempPos.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
		pTexture);
}


void UI_OverScreen::StageSelectDraw(bool Select)
{
	UItransform TempPos = m_Transform[UI_Over::STAGESELECT];
	ID3D11ShaderResourceView* pTexture;
	if (Select)
	{
		pTexture = m_pChoseTexture[UI_Over::STAGESELECT];
	}
	else
	{
		pTexture = m_pTexture[UI_Over::STAGESELECT];
	}


	TempPos.Pos.x += m_ScreenTransform.Pos.x;
	TempPos.Pos.y += m_ScreenTransform.Pos.y;

	m_pSprite->Draw(TempPos.Pos, TempPos.Size, TempPos.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
		pTexture);
}