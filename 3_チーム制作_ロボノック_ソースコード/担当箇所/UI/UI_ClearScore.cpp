#include "UI_ClearScore.h"
#include "Sprite.h"
#include "DirectXTex/Texture.h"

//--------------------
// �R���X�g���N�^
//--------------------
UI_ClearScore::UI_ClearScore()
{
	const unsigned int SCREEN_CENTER_SIDE = 1280 * 0.5f;
	const unsigned int SCREEN_CENTER_LENGTH = 720 * 0.5f;

	m_pSprite = new SpriteManager;

	m_TextureNum = { 11,1 };
	m_TextureSize = { 1.0f / m_TextureNum.x ,1.0f / m_TextureNum.y };
	m_Transform.Pos	 = {0.0f,0.0f};
	m_Transform.Size = {40.0f,-40.0f};

	LoadTextureFromFile("Assets/2D/number_blue.png", &m_pScoreTexture);
}

//--------------------
// �f�X�g���N�^
//--------------------
UI_ClearScore::~UI_ClearScore()
{
	m_pScoreTexture->Release();
	delete m_pSprite;
}

//--------------------
// �`��
//--------------------
void UI_ClearScore::Draw(int Score)
{
	int tempScore = Score;	//�X�R�A���R�s�[���Ă���
	float Distance = 35.0f;	//���l�Ƃ̊Ԋu

	UItransform TempTransform = m_Transform;

	TempTransform.Pos.x = TempTransform.Pos.x + (Distance * 3 * 0.5f - Distance * 0.5f);

	//���͎O��
	for (int i = 0; i < 3; i++)
	{
		//�ŏ���
		int MinScore = tempScore % 10;

		DirectX::XMFLOAT2 UVPos;

		//�T�C�Y�ƍŏ�������ꏊ���v�Z
		UVPos.x = MinScore * m_TextureSize.x;
		UVPos.y = 0.0f;
		
		//�`��
		m_pSprite->Draw(TempTransform.Pos, TempTransform.Size, DirectX::XMFLOAT3(0.0f,0.0f,0.0f),
			UVPos, m_TextureSize, DirectX::XMFLOAT4(1.0f, 1.0f, 1.0f, 1.0f),
			m_pScoreTexture);

		tempScore = tempScore / 10;
		TempTransform.Pos.x -= Distance;
	}
}