#include "UI_PauseScreen.h"
#include "GamePause.h"
#include "Sprite.h"
#include "DirectXTex/Texture.h"

//----------------------------
// �R���X�g���N�^
//----------------------------
UI_PauseScreen::UI_PauseScreen()
	:m_NowState(STATE_MAX)
{
	m_pSprite = new SpriteManager;

	DirectX::XMFLOAT2 WindowSize = { 1280.0f, 720.0f };

	//�X�N���[���̃|�W�V�����̐ݒ�
	m_ScreenTransform.Pos = { WindowSize.x * 0.5f,-200.0f };
	m_ScreenTransform.Size = { 1200.0f,-600.0f };
	m_ScreenTransform.Scale = { 1.0f,1.0f,1.0f };
	m_ScreenTransform.Angle = { 0.0f,0.0f,0.0f };
	// �e�N�X�`���ǂݍ���
	LoadTextureFromFile("Assets/2D/screen.png", &m_pScreenTexture);

	const char* TextureName[UI_Pause::MAX] =
	{
		"Assets/2D_Roma/letter/Pause0.png",				//�|�[�Y
		"Assets/2D_Roma/letter/Retry0.png",			//���g���C
		"Assets/2D_Roma/letter/StageSelect.png",	//�X�e�[�W�Z���N�g
		"Assets/2D_Roma/letter/Owaru.png"				//�G���h
	};

	const char* ChoseTextureName[UI_Pause::MAX] =
	{
		"Assets/2D_Roma/letter/Pause0.png",						//�|�[�Y
		"Assets/2D_Roma/letter/Retry_Select.png",			//���g���C
		"Assets/2D_Roma/letter/StageSelect_Select.png",	//�X�e�[�W�Z���N�g
		"Assets/2D_Roma/letter/Owaru_Select.png"				//�G���h
	};

	DirectX::XMFLOAT2 PosIndex[UI_Pause::MAX] =
	{
		{0.0f,-130.0f},	//�|�[�Y
		{0.0f,-15.0f},	//���g���C
		{0.0f,65.0f},	//�X�e�[�W�Z���N�g
		{0.0f,135.0f}	//�G���h
	};

	DirectX::XMFLOAT2 SizeIndex[UI_Pause::MAX] =
	{
		{200.0f,-60.0f},
		{200.0f,-50.0f},
		{340.0f,-45.0f},
		{150.0f,-40.0f}
	};

	//���W�̐ݒ�
	for (int i = 0; i < UI_Pause::MAX; i++)
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
// �f�X�g���N�^
//--------------------
UI_PauseScreen::~UI_PauseScreen()
{
	delete m_pArm;

	for (int i = 0; i < UI_Pause::MAX; i++)
	{
		m_pTexture[i]->Release();
		m_pChoseTexture[i]->Release();
	}
	m_pScreenTexture->Release();
	delete m_pSprite;
}

//--------------------
// �X�V
//--------------------
bool UI_PauseScreen::Update()
{
	const unsigned int SCREEN_CENTER_SIDE = 1280 * 0.5f;
	const unsigned int SCREEN_CENTER_LENGTH = 720 * 0.5f;

	bool End_exe = false;

	switch (m_NowState)
	{
	case UI_PauseScreen::STATE_INIT:
		m_NowState = UI_PauseScreen::STATE_IN;
		break;
	case UI_PauseScreen::STATE_IN:	//�X�N���[��������
		if (SCREEN_CENTER_LENGTH <= m_ScreenTransform.Pos.y)
		{
			m_ScreenTransform.Pos.y = SCREEN_CENTER_LENGTH;
			m_NowState = UI_PauseScreen::STATE_STAY;
		}

		m_ScreenTransform.Pos.y += 15.0f;
		break;
	case UI_PauseScreen::STATE_STAY:

		break;
	case UI_PauseScreen::STATE_OUT:	//�X�N���[�����O�ɏo��
		if (-250.0f >= m_ScreenTransform.Pos.y)
		{
			m_NowState = UI_PauseScreen::STATE_END;
		}

		m_ScreenTransform.Pos.y -= 25.0f;

		break;
	case UI_PauseScreen::STATE_END:
		End_exe = true;
		break;
	default:
		break;
	}

	return End_exe;
}

void UI_PauseScreen::ScreenInit()
{
	m_NowState = UI_PauseScreen::STATE_INIT;
}

void UI_PauseScreen::ScreenDraw()
{
	m_pArm->pos.x = m_ScreenTransform.Pos.x + m_ArmAddPos.x;
	m_pArm->pos.y = m_ScreenTransform.Pos.y + m_ArmAddPos.y;
	m_pArm->Draw();

	m_pSprite->Draw(m_ScreenTransform.Pos, m_ScreenTransform.Size, m_ScreenTransform.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f,1.0f), DirectX::XMFLOAT4(1.0f,1.0f,1.0f,1.0f),
					m_pScreenTexture);
}

void UI_PauseScreen::PauseDraw()
{
	UItransform TempPos = m_Transform[UI_Pause::PAUSE];

	TempPos.Pos.x += m_ScreenTransform.Pos.x;
	TempPos.Pos.y += m_ScreenTransform.Pos.y;
	
	m_pSprite->Draw(TempPos.Pos, TempPos.Size, TempPos.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f), 
		m_pChoseTexture[UI_Pause::PAUSE]);
}

void UI_PauseScreen::RetryDraw(bool Select)
{
	UItransform TempPos = m_Transform[UI_Pause::RETRY];
	ID3D11ShaderResourceView *pTexture;
	if (Select)
	{
		pTexture = m_pChoseTexture[UI_Pause::RETRY];
	}
	else
	{
		pTexture = m_pTexture[UI_Pause::RETRY];
	}

	TempPos.Pos.x += m_ScreenTransform.Pos.x;
	TempPos.Pos.y += m_ScreenTransform.Pos.y;

	m_pSprite->Draw(TempPos.Pos, TempPos.Size,TempPos.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
		pTexture);
}

void UI_PauseScreen::StageSelectDraw(bool Select)
{
	UItransform TempPos = m_Transform[UI_Pause::STAGESELECT];
	ID3D11ShaderResourceView *pTexture;
	if (Select)
	{
		pTexture = m_pChoseTexture[UI_Pause::STAGESELECT];
	}
	else
	{
		pTexture = m_pTexture[UI_Pause::STAGESELECT];
	}

	TempPos.Pos.x += m_ScreenTransform.Pos.x;
	TempPos.Pos.y += m_ScreenTransform.Pos.y;

	m_pSprite->Draw(TempPos.Pos, TempPos.Size,  TempPos.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
		pTexture);
}

void UI_PauseScreen::EndDraw(bool Select)
{
	UItransform TempPos = m_Transform[UI_Pause::END];
	ID3D11ShaderResourceView *pTexture;
	if (Select)
	{
		pTexture = m_pChoseTexture[UI_Pause::END];
	}
	else
	{
		pTexture = m_pTexture[UI_Pause::END];
	}


	TempPos.Pos.x += m_ScreenTransform.Pos.x;
	TempPos.Pos.y += m_ScreenTransform.Pos.y;

	m_pSprite->Draw(TempPos.Pos, TempPos.Size, TempPos.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
		pTexture);
}