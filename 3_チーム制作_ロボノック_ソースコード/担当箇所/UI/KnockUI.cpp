#include "knockUI.h"
#include "Sprite.h"
#include "DirectXTex/Texture.h"

//********** �萔��` **********
#define MAX_KNOCK_UI  (2)   // UI�̌���
#define ANIM_FRAME    (2)   // �A�j���[�V������1�R�}�̃t���[����
#define ANIM_SPLIT_X (11)	// �摜�̕������i���j
#define ANIM_SPLIT_Y  (1)   // �摜�̕������i�c�j


//--------------------
// �R���X�g���N�^
//--------------------
KnockUI::KnockUI()
{
	m_pSprite = new SpriteManager;

	for (int i = 0; i < MAX_KNOCK_UI; i++)
	{
		// �摜�̈ʒu
		m_KnockUI[i].Pos = { 340.0f + (i * 60.0f), 657.0f };

		// �摜�̃T�C�Y
		m_KnockUI[i].Size = { 65.0f, -70.0f };

		// �摜�̊p�x
		m_KnockUI[i].Angle = { 0.0f, 0.0f, 0.0f };

		// �摜�̊g�k
		m_KnockUI[i].Scale = { 1.0f, 1.0f, 1.0f };

		// �摜�̍����UV���W
		m_KnockUI[i].posTexCoord = { 0.0f, 0.0f };

		// �摜�̕�����
		m_KnockUI[i].sizeTexCoord.x = 1.0f / (float)ANIM_SPLIT_X;
		m_KnockUI[i].sizeTexCoord.y = 1.0f;

		// �摜�̎g�p���
		m_KnockUI[i].use = true;

		// �t���[����
		m_KnockUI[i].frame = 0;

		// �摜�̉��R�}�ڂ�
		m_KnockUI[i].currentAnimNo = 0;

		// �e�N�X�`���ǂݍ���
		LoadTextureFromFile("Assets/2D/number_blue.png", &m_KnockUI[i].pKnockUI);
	}

	// �ł��t���񐔏�����
	m_KnockCnt = 0;


}


//--------------------
// �f�X�g���N�^
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
// �`��
//--------------------
void KnockUI::Draw()
{
	for (int i = 0; i < MAX_KNOCK_UI; i++)
	{
		// Sprite��UV���𑗂�
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
// �ł��t���񐔑���
// �����Fint num  �ł��t������
//----------------------------------
void KnockUI::AddKnock(int num)
{
	m_KnockCnt += num;

	// ����␳
	if (99 < m_KnockCnt)
	{
		m_KnockCnt = 99;
	}

	// �e�N�X�`�����W�X�V
	KnockUI::UpdateTexCoord();
}


//----------------------------------
// �摜��UV���W�X�V
//----------------------------------
void KnockUI::UpdateTexCoord()
{
	int temp = m_KnockCnt;

	for (int i = MAX_KNOCK_UI - 1; 0 <= i; i--)
	{
		// tmp�̒l�̉��ꌅ�擾
		m_KnockUI[i].currentAnimNo = temp % 10;

		// �e�N�X�`�����W�X�V
		m_KnockUI[i].posTexCoord.x = m_KnockUI[i].sizeTexCoord.x *
			(m_KnockUI[i].currentAnimNo % ANIM_SPLIT_X);

		m_KnockUI[i].posTexCoord.y = m_KnockUI[i].sizeTexCoord.y *
			(m_KnockUI[i].currentAnimNo / ANIM_SPLIT_X);

		// �����Ƃ�
		temp /= 10;
	}
}