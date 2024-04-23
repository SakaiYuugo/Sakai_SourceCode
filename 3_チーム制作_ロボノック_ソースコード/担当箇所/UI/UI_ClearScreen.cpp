#include "UI_ClearScreen.h"
#include "Sprite.h"
#include "DirectXTex/Texture.h"

//----------------------------
// �R���X�g���N�^
//----------------------------
UI_ClearScreen::UI_ClearScreen()
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

	const char* TextureName[UI_Clear::MAX] =
	{
		"Assets/2D_Roma/letter/StageClear.png",		//�X�e�[�W�N���A
		"Assets/2D_Roma/letter/Tugie.png",			//����
		"Assets/2D_Roma/letter/Retry0.png",			//���g���C
		"Assets/2D_Roma/letter/StageSelect.png"		//�X�e�[�W�Z���N�g
	};

	const char* ChoseTextureName[UI_Clear::MAX] =
	{
		"Assets/2D_Roma/letter/StageClear.png",					//�X�e�[�W�N���A
		"Assets/2D_Roma/letter/Tugie_Select.png",		//����
		"Assets/2D_Roma/letter/Retry_Select.png",			//���g���C
		"Assets/2D_Roma/letter/StageSelect_Select.png"			//�X�e�[�W�Z���N�g
	};

	DirectX::XMFLOAT2 PosIndex[UI_Clear::MAX] =
	{
		{0.0f,-120.0f},	//�X�e�[�W�N���A
		{0.0f,50.0f},	//����
		{0.0f,110.0f},	//���g���C
		{0.0f,160.0f}	//�X�e�[�W�Z���N�g
	};

	DirectX::XMFLOAT2 SizeIndex[UI_Clear::MAX] =
	{
		{420.0f,-60.0f},
		{135.0f,-45.0f},
		{210.0f,-35.0f},
		{320.0f,-40.0f}
	};

	//���W�̐ݒ�
	for (int i = 0; i < UI_Clear::MAX; i++)
	{
		m_Transform[i].Pos = PosIndex[i];
		m_Transform[i].Size = SizeIndex[i];
		m_Transform[i].Scale = { 1.0f,1.0f,1.0f };
		m_Transform[i].Angle = { 0.0f,0.0f,0.0f };

		LoadTextureFromFile(TextureName[i], &m_pTexture[i]);
		LoadTextureFromFile(ChoseTextureName[i], &m_pChoseTexture[i]);
	}

	LoadTextureFromFile("Assets/2D_Roma/letter/HAKAISUU.png",&m_pDestroyNum_Mozi);
	LoadTextureFromFile("Assets/2D_Roma/letter/UTITUKESUU.png",&m_pNockNum_Mozi);

	m_pScore = new UI_ClearScore();
	m_pArm = new Image2D("Assets/2D/screen_arm.png", m_pSprite);
	m_pArm->size = { 4890.0f * 0.15f,3135.0f * 0.15f };
	m_ArmAddPos = { 0.0f, -200.0f};

}

//--------------------
// �f�X�g���N�^
//--------------------
UI_ClearScreen::~UI_ClearScreen()
{
	delete m_pArm;

	m_pNockNum_Mozi->Release();
	m_pDestroyNum_Mozi->Release();
	delete m_pScore;
	for (int i = 0; i < UI_Clear::MAX; i++)
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
bool UI_ClearScreen::Update()
{
	const unsigned int SCREEN_CENTER_SIDE = 1280 * 0.5f;
	const unsigned int SCREEN_CENTER_LENGTH = 720 * 0.5f;

	bool End_exe = false;

	switch (m_NowState)
	{
	case UI_ClearScreen::STATE_INIT:
		m_NowState = UI_ClearScreen::STATE_IN;
		break;
	case UI_ClearScreen::STATE_IN:	//�X�N���[��������
		if (SCREEN_CENTER_LENGTH <= m_ScreenTransform.Pos.y)
		{
			m_ScreenTransform.Pos.y = SCREEN_CENTER_LENGTH;
			m_NowState = UI_ClearScreen::STATE_STAY;
		}

		m_ScreenTransform.Pos.y += 15.0f;
		break;
	case UI_ClearScreen::STATE_STAY:

		break;
	case UI_ClearScreen::STATE_OUT:	//�X�N���[�����O�ɏo��
		if (-250.0f >= m_ScreenTransform.Pos.y)
		{
			m_NowState = UI_ClearScreen::STATE_END;
		}

		m_ScreenTransform.Pos.y -= 25.0f;

		break;
	case UI_ClearScreen::STATE_END:
		End_exe = true;
		break;
	default:
		break;
	}

	return End_exe;
}

void UI_ClearScreen::ScreenInit()
{
	m_NowState = UI_ClearScreen::STATE_INIT;
}

void UI_ClearScreen::ScreenDraw()
{
	m_pArm->pos.x = m_ScreenTransform.Pos.x + m_ArmAddPos.x;
	m_pArm->pos.y = m_ScreenTransform.Pos.y + m_ArmAddPos.y;
	m_pArm->Draw();

	m_pSprite->Draw(m_ScreenTransform.Pos, m_ScreenTransform.Size, m_ScreenTransform.Angle,
		{ 0.0f,0.0f }, { 1.0f,1.0f }, { 1.0f,1.0f,1.0f,1.0f },
		m_pScreenTexture);
}

void UI_ClearScreen::StageClearDraw()
{
	UItransform TempPos = m_Transform[UI_Clear::STAGECLEAR];

	TempPos.Pos.x += m_ScreenTransform.Pos.x;
	TempPos.Pos.y += m_ScreenTransform.Pos.y;

	m_pSprite->Draw(TempPos.Pos, TempPos.Size, TempPos.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
					m_pChoseTexture[UI_Clear::STAGECLEAR]);
}

void UI_ClearScreen::HakaiScoreDraw(int Score)
{
	DirectX::XMFLOAT2 AddPos = {0.0f,-15.0f};

	AddPos.y += m_ScreenTransform.Pos.y;
	AddPos.x += m_ScreenTransform.Pos.x;

	//�j�󕶎��̃|�W�V�����ݒ�
	DirectX::XMFLOAT2 HakaiMoziPos = AddPos;
	HakaiMoziPos.x -= 55.0f;

	m_pSprite->Draw(HakaiMoziPos, DirectX::XMFLOAT2(230.0f,-35.0f), DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f),
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
		m_pDestroyNum_Mozi);
	
	//�X�R�A�̃|�W�V�����ݒ�
	DirectX::XMFLOAT2 ScorePos = AddPos;
	ScorePos.x += 125.0f;

	m_pScore->SetPos(ScorePos);
	m_pScore->Draw(Score);
}

void UI_ClearScreen::UtitukeScoreDraw(int Score)
{
	DirectX::XMFLOAT2 AddPos = { 0.0f,-55.0f };

	AddPos.x += m_ScreenTransform.Pos.x;
	AddPos.y += m_ScreenTransform.Pos.y;

	//�j�󕶎��̃|�W�V�����ݒ�
	DirectX::XMFLOAT2 HakaiMoziPos = AddPos;
	HakaiMoziPos.x -= 55.0f;

	m_pSprite->Draw(HakaiMoziPos, DirectX::XMFLOAT2(230.0f, -35.0f), DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f),
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
		m_pNockNum_Mozi);

	//�X�R�A�̃|�W�V�����ݒ�
	DirectX::XMFLOAT2 ScorePos = AddPos;
	ScorePos.x += 125.0f;

	m_pScore->SetPos(ScorePos);
	m_pScore->Draw(Score);
}
void UI_ClearScreen::RetryDraw(bool Select)
{
	UItransform TempPos = m_Transform[UI_Clear::RETRY];
	ID3D11ShaderResourceView *pTexture;
	if (Select)
	{
		pTexture = m_pChoseTexture[UI_Clear::RETRY];
	}
	else
	{
		pTexture = m_pTexture[UI_Clear::RETRY];
	}
	TempPos.Pos.x += m_ScreenTransform.Pos.x;
	TempPos.Pos.y += m_ScreenTransform.Pos.y;

	m_pSprite->Draw(TempPos.Pos, TempPos.Size, TempPos.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
		pTexture);
}
void UI_ClearScreen::NextDraw(bool Select)
{
	UItransform TempPos = m_Transform[UI_Clear::NEXT];
	ID3D11ShaderResourceView *pTexture;
	if (Select)
	{
		pTexture = m_pChoseTexture[UI_Clear::NEXT];
	}
	else
	{
		pTexture = m_pTexture[UI_Clear::NEXT];
	}

	TempPos.Pos.x += m_ScreenTransform.Pos.x;
	TempPos.Pos.y += m_ScreenTransform.Pos.y;

	m_pSprite->Draw(TempPos.Pos, TempPos.Size,TempPos.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
		pTexture);
}
void UI_ClearScreen::StageSelectDraw(bool Select)
{
	UItransform TempPos = m_Transform[UI_Clear::STAGESELECT];
	ID3D11ShaderResourceView *pTexture;
	if (Select)
	{
		pTexture = m_pChoseTexture[UI_Clear::STAGESELECT];
	}
	else
	{
		pTexture = m_pTexture[UI_Clear::STAGESELECT];
	}

	TempPos.Pos.x += m_ScreenTransform.Pos.x;
	TempPos.Pos.y += m_ScreenTransform.Pos.y;

	m_pSprite->Draw(TempPos.Pos, TempPos.Size, TempPos.Angle,
		DirectX::XMFLOAT2(0.0f, 0.0f), DirectX::XMFLOAT2(1.0f, 1.0f), DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
		pTexture);
}