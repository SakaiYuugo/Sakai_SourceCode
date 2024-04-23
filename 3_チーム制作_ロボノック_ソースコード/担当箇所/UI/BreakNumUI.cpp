#include "BreakNumUI.h"
#include "Sprite.h"
#include "DirectXTex/Texture.h"

//********** �萔��` **********
#define MAX_BREAK_UI        (2)   // UI�̌���
#define ANIM_BREAK_FRAME    (2)   // �A�j���[�V������1�R�}�̃t���[����
#define ANIM_BREAK_SPLIT_X (11)	  // �摜�̕������i���j
#define ANIM_BREAK_SPLIT_Y  (1)   // �摜�̕������i�c�j


//--------------------
// �R���X�g���N�^
//--------------------
BreakNumUI::BreakNumUI()
{
	m_pSprite = new SpriteManager;

	for (int i = 0; i < MAX_BREAK_UI; i++)
	{
		// �摜�̈ʒu
		m_BreakUI[i].Pos = { 130.0f + (i * 60.0f), 657.0f };

		// �摜�̃T�C�Y
		m_BreakUI[i].Size = { 65.0f, -70.0f };

		// �摜�̊p�x
		m_BreakUI[i].Angle = { 0.0f, 0.0f, 0.0f };

		// �摜�̊g�k
		m_BreakUI[i].Scale = { 1.0f, 1.0f, 1.0f };

		// �摜�̍����UV���W
		m_BreakUI[i].posTexCoord = { 0.0f, 0.0f };

		// �摜�̕�����
		m_BreakUI[i].sizeTexCoord.x = 1.0f / (float)ANIM_BREAK_SPLIT_X;
		m_BreakUI[i].sizeTexCoord.y = 1.0f;

		// �摜�̎g�p���
		m_BreakUI[i].use = true;

		// �t���[����
		m_BreakUI[i].frame = 0;

		// �摜�̉��R�}�ڂ�
		m_BreakUI[i].currentAnimNo = 0;

		// �e�N�X�`���ǂݍ���
		LoadTextureFromFile("Assets/2D/number_blue.png", &m_BreakUI[i].pBreakUI);
	}

	// �ł��t���񐔏�����
	m_BreakCnt = 0;
}


//--------------------
// �f�X�g���N�^
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
// �`��
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
// �j�󐔑���
// �����Fint num  �j�󂵂��u���b�N�̐�
//--------------------------------------
void BreakNumUI::AddNum(int num)
{

		m_BreakCnt += 1;

		// ����␳
		if (99 < m_BreakCnt)
		{
			m_BreakCnt = 99;
		}

		// �e�N�X�`�����W�X�V
		BreakNumUI::UpdateTexCoord();

}



//----------------------------------
// �摜��UV���W�X�V
//----------------------------------
void BreakNumUI::UpdateTexCoord()
{
	int temp = m_BreakCnt;

	for (int i = MAX_BREAK_UI - 1; 0 <= i; i--)
	{
		// tmp�̒l�̉��ꌅ�擾
		m_BreakUI[i].currentAnimNo = temp % 10;

		// �e�N�X�`�����W�X�V
		m_BreakUI[i].posTexCoord.x = m_BreakUI[i].sizeTexCoord.x *
			(m_BreakUI[i].currentAnimNo % ANIM_BREAK_SPLIT_X);

		m_BreakUI[i].posTexCoord.y = m_BreakUI[i].sizeTexCoord.y *
			(m_BreakUI[i].currentAnimNo / ANIM_BREAK_SPLIT_X);

		// �����Ƃ�
		temp /= 10;
	}
}