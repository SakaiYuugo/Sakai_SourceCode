#include "jewelryUI.h"
#include "Sprite.h"
#include "DirectXTex/Texture.h"

#define MAX_JEWELRY     (3)
#define MAX_CUREECTCNT (55)

//---------------------------------
// �R���X�g���N�^
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
// �f�X�g���N�^
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
// ��΂��쐬
// �����Fint num �쐬������UI�̐�
//----------------------------------------
void JewelryUI::MakeJewelryUI(int Num)
{
	// �ۑ��p�ϐ��ɍ쐬����UI�̐�����
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

		LoadTextureFromFile("Assets/2D/�_�C��_���l��.png", &m_Jewelry[i].pJewelryNot);
		LoadTextureFromFile("Assets/2D/�_�C��_�l��.png", &m_Jewelry[i].pJewelryGet);
	}

}


//----------------------------------------
// ��΂�UI��؂�ւ���
//----------------------------------------
void JewelryUI::GetJewelryUI()
{
	m_Jewelry[m_Count + m_UINum].GetJewelry = true;
	tmp++;
}


//----------------------------------------
// ��Ύ擾���̃A�j���[�V�����X�V
//----------------------------------------
void JewelryUI::GetMoveJewelry()
{
	for (int i = m_UINum; i < m_UINum * 2; i++)
	{
		// ��΂��擾���Ă����ꍇ
		if (m_Jewelry[i].GetJewelry && !m_Jewelry[i].AnimeFlg)
		{
			// ��x������鏈��
			if (m_Jewelry[i].Use == false)
			{
				//�����̍��W����X�N���[�����W�ɕϊ�����
				m_2DJewelyPos = m_pCameraManager->ChangeScreenPos(m_pPlayer->GetPos());
				// �v���C���[�̌��ݍ��W���擾
				m_Jewelry[i].Pos = m_2DJewelyPos;

				// �J�n�n�_�̐ݒ�
				m_Jewelry[i].StartPos = m_2DJewelyPos;

				// �I���n�_
				m_Jewelry[i].EndPos = m_Jewelry[i - m_UINum].Pos;

				// ����_�P
				m_Jewelry[i].ControlPos1 = {
					(m_Jewelry[i].EndPos.x - m_Jewelry[i].StartPos.x) * 0.4f,
					(m_Jewelry[i].EndPos.y - m_Jewelry[i].StartPos.y) * 0.15f
				};

				// ����_�Q
				m_Jewelry[i].ControlPos2 = {
				(m_Jewelry[i].EndPos.x - m_Jewelry[i].StartPos.x) * 0.3f,
				(m_Jewelry[i].EndPos.y - m_Jewelry[i].StartPos.y) * 0.03f
				};

				// �t���O�؂�ւ�
				m_Jewelry[i].Use = true;
			}
		

			// �J�E���^�̍X�V
			m_Jewelry[i].CurrentCnt++;

			float Bzier0 = pow(1 - (float)m_Jewelry[i].CurrentCnt / m_Jewelry[i].MaxCnt, 3);
			float Bzier1 = 
				 pow(3 * (float)m_Jewelry[i].CurrentCnt / m_Jewelry[i].MaxCnt * 
				(pow(1 - (float)m_Jewelry[i].CurrentCnt / m_Jewelry[i].MaxCnt, 2)), 3);

			float Bzier2 = 
				 pow(3 * (float)m_Jewelry[i].CurrentCnt / m_Jewelry[i].MaxCnt, 2) *
				(1 - (float)m_Jewelry[i].CurrentCnt / m_Jewelry[i].MaxCnt);
			float Bzier3 = pow((float)m_Jewelry[i].CurrentCnt / m_Jewelry[i].MaxCnt, 3);

			// ���W�̍X�V
			m_Jewelry[i].Pos.x = (m_Jewelry[i].StartPos.x    * Bzier0) +
				                   (m_Jewelry[i].ControlPos1.x * Bzier1) +
								   (m_Jewelry[i].ControlPos2.x * Bzier2) +
				                   (m_Jewelry[i].EndPos.x      * Bzier3);

			m_Jewelry[i].Pos.y = (m_Jewelry[i].StartPos.y    * Bzier0) +
				                   (m_Jewelry[i].ControlPos1.y * Bzier1) +
							       (m_Jewelry[i].ControlPos2.y * Bzier2) +
				                   (m_Jewelry[i].EndPos.y      * Bzier3);

			// �I������
			if (m_Jewelry[i].EndPos.x - 25.0f <= m_Jewelry[i].Pos.x &&
				m_Jewelry[i].EndPos.x + 25.0f >= m_Jewelry[i].Pos.x)
			{
				if (m_Jewelry[i].EndPos.y - 25.0f <= m_Jewelry[i].Pos.y &&
					m_Jewelry[i].EndPos.y + 25.0f >= m_Jewelry[i].Pos.y)
				{
					// �ړ����Ɏg�p����UI����ʊO��
					m_Jewelry[i].Size = { 0.0f, 0.0f };
					m_Jewelry[i].Pos = { 1500.0f, 800.0f };
					m_Jewelry[i].AnimeFlg = true;

					// �l�����̕��UI�̕\��
					m_Jewelry[i - m_UINum].GetJewelry = true;

				}
			}
		}
	}

}


//--------------------
// �`��
//--------------------
void JewelryUI::Draw()
{
	for (int i = 0; i < m_UINum * 2; i++)
	{
		// ��΂����l���̏ꍇ
		if (m_Jewelry[i].GetJewelry == false)
		{
			m_pSprite->Draw(m_Jewelry[i].Pos, m_Jewelry[i].Size, m_Jewelry[i].Angle,
							{ 0.0f,0.0f }, { 1.0f,1.0f }, { 1.0f,1.0f,1.0f,1.0f }, 
							m_Jewelry[i].pJewelryNot);
		}

		// ��΂��l�����Ă���ꍇ
		if (m_Jewelry[i].GetJewelry == true)
		{
			m_pSprite->Draw(m_Jewelry[i].Pos, m_Jewelry[i].Size, m_Jewelry[i].Angle,
							{ 0.0f,0.0f }, { 1.0f,1.0f }, { 1.0f,1.0f,1.0f,1.0f }, 
							m_Jewelry[i].pJewelryGet);
		}
	
	}
}