#include <stdlib.h>
#include <time.h>
#include "Heat.h"
#include "ModelManager.h"
#include "PileBankerBase.h"
#include "BlockBase.h"
#include "Player.h"
#include "Effect_Manager.h"
#include "Collision.h"

using namespace std;

#define NOTFOUND_NULLBLOCK (999)
#define NOTFOUND_WATERBLOCK (999)
#define ANGLE_Z (1.0f)
#define ANGLE_RIGHT_END (-45.0f)
#define ANGLE_LEFT_END (45.0f)

CHeat::CHeat(Stage_Base * MyStage, int PosX, int PosY)
	:CItemBase(MyStage, PosX, PosY)
	, Down_Speed(50.0f / 40.0f), Move_Speed(50.0f / 40.0f), moving_right(false), moving_left(false), bWater_InFlg(false), moving_down(false)
	, m_angleZ(-45.0f)
{
	m_pModelManager->c_AddModel("Heat", "Assets/NewItem/item_heatitem1.fbx");
	m_State = CHeat::HEATSTATE::HEAT_IN;
	m_pGet_HeetSE = new SE("Assets/SE/Get_Heet.wav");
	m_pBreak_HeetSE = new SE("Assets/SE/Item_Break.wav");
	if (m_AngleNum == 1)
	{
		m_angleZ = 45.0f;
		m_AngleNum = 0;
	}
	else
	{
		m_angleZ = -45.0f;
		m_AngleNum = 1;
	}
}

CHeat::~CHeat()
{
	delete m_pBreak_HeetSE;
	delete m_pGet_HeetSE;
}

void CHeat::Update()
{
	// �u���b�N�̒��ɂȂ��ꍇ(���ƃK�X�ȊO)
	if (m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER ||
		m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_MAGMA)
	{

		//���̏�������Ă���
		Block_Base* pBlock = m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y + 1);

		//�^�����󓴂������ꍇ
		if (pBlock == nullptr || pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER ||
			pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
			pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_MAGMA)
		{
			if (!(moving_left) && !(moving_right))
			{
				// ���Ƀq�[�g�A�C�e�����Ȃ��Ȃ�
				if (m_pMyStage->GetHeet(m_MapPos.X, m_MapPos.Y + 1) == nullptr)
				{
					//���ɍ~���
					m_DrawPos.Y -= Down_Speed;
					moving_down = true;
					//���̃u���b�N�̃|�W�V�����ɒ�������
					if (m_DrawPos.Y <= m_pMyStage->Get_DrawPos(m_MapPos.X, m_MapPos.Y + 1).Y)
					{
						// �`��|�W�V�����X�V
						m_DrawPos.Y = m_pMyStage->Get_DrawPos(m_MapPos.X, m_MapPos.Y + 1).Y;

						m_pMyStage->MoveHeet(m_MapPos.X, m_MapPos.Y + 1);
						moving_down = false;
					}
				}	 
				else
				{
					CItemBase* pHeat = m_pMyStage->GetHeet(m_MapPos.X, m_MapPos.Y + 1);
					ITEM_COINCIDE coincide = pHeat->GetCoinCide();
					moving_down = true;
					//���ɍ~���
					m_DrawPos.Y -= Down_Speed;
	
					if (pHeat->GetPos().Y > m_DrawPos.Y)
					{
						switch (coincide)
						{
						case ONE:
							pHeat->SetCoinCide(CItemBase::ITEM_COINCIDE::DOUBLE);
							break;
						case DOUBLE:
							pHeat->SetCoinCide(CItemBase::ITEM_COINCIDE::TRIPLE);
							break;
						default:
							break;
						}
						m_IsDestroy = true;
					}
				}
			}
		}
		else if (pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_INVINCIBLE)//�����O�������ꍇ
		{
			m_pMyStage->Refuge_Item();
			m_State = CHeat::HEAT_OUT;
			m_PileLineNum = m_pMyStage->Get_MapPos(m_DrawPos.X, m_DrawPos.Y).X;
		}
		else // �^�����ő̃u���b�N
		{
			MoveAngle();

			int leftDownDistance = CheckLeft(0);	//�����̋󓴂܂ł̋���
			int rightDownDistance = CheckRight(0);	//�E���̋󓴂܂ł̋���

			CheckCollisionWater();

			//�ړ��ł���ꏊ�ɐ��u���b�N�����������ꍇ
			if (bWater_InFlg)
			{
				//���ɋ󓴂�����ꍇ
				if ((leftDownDistance != NOTFOUND_NULLBLOCK || rightDownDistance != NOTFOUND_NULLBLOCK) && !(moving_down))
				{
					MoveHeat(leftDownDistance, rightDownDistance);
				}
			}
			if (!(bWater_InFlg) && moving_left && !(moving_down))
			{
				MoveHeat(leftDownDistance, rightDownDistance);
			}

			if (!(bWater_InFlg) && moving_right && !(moving_down))
			{
				MoveHeat(leftDownDistance, rightDownDistance);
			}
		}

		CheckCollisionMagma();
	}

}

void CHeat::Draw()
{
	m_DrawPos.Z = -20.0f;
	switch (m_CoinCide)
	{
	case ONE:
		m_pModelManager->c_ModelDraw("Heat", m_DrawPos.X, m_DrawPos.Y, m_DrawPos.Z
			, 0.4f, 0.4f, 0.5f, 0.0f, 0.0f, m_angleZ);
		break;
	case DOUBLE:
		m_pModelManager->c_ModelDraw("Heat", m_DrawPos.X - 5.0f, m_DrawPos.Y, m_DrawPos.Z
			, 0.3f, 0.3f, 0.4f, 0.0f, 0.0f, m_angleZ);
		m_pModelManager->c_ModelDraw("Heat", m_DrawPos.X + 5.0f, m_DrawPos.Y, m_DrawPos.Z
			, 0.3f, 0.3f, 0.4f, 0.0f, 0.0f, m_angleZ);
		break;
	case TRIPLE:
		m_pModelManager->c_ModelDraw("Heat", m_DrawPos.X - 5.0f, m_DrawPos.Y, m_DrawPos.Z
			, 0.3f, 0.3f, 0.4f, 0.0f, 0.0f, m_angleZ);
		m_pModelManager->c_ModelDraw("Heat", m_DrawPos.X + 5.0f, m_DrawPos.Y, m_DrawPos.Z
			, 0.3f, 0.3f, 0.4f, 0.0f, 0.0f, m_angleZ);
		m_pModelManager->c_ModelDraw("Heat", m_DrawPos.X, m_DrawPos.Y, m_DrawPos.Z
			, 0.3f, 0.3f, 0.4f, 0.0f, 0.0f, m_angleZ);
		break;
	default:
		break;
	}
	
}


void CHeat::RefugeUpdate()
{
	if (m_BreakAniFrame == 0)
	{
		m_DrawPos.Y -= Down_Speed / 2.0f;	// ����
	}
	
	C_Player* pPlayer = m_pMyStage->GetPlayer();

	//��΂��v���C���[�̈ʒu�������ɍs�����Ƃ�
	DirectX::XMFLOAT3 Position = { m_DrawPos.X,m_DrawPos.Y, m_DrawPos.Z };
	DirectX::XMFLOAT3 PlayerPos = { m_pMyStage->GetPlayer()->GetPos().X ,m_pMyStage->GetPlayer()->GetPos().Y,m_pMyStage->GetPlayer()->GetPos().Z };
	if (Collision::CheckSquare(Position, 30.0f, PlayerPos, 30.0f))
	{
		DirectX::XMFLOAT3 Size = { 50.0f,50.0f,0.0f };
		Effect_Manager::Play_Effect("Heat", PlayerPos, Size);
		m_pGet_HeetSE->Play();
		switch (m_CoinCide)
		{
		case ONE:
			pPlayer->AddHeetItem();
			break;
		case DOUBLE:
			pPlayer->AddHeetItem();
			pPlayer->AddHeetItem();
			break;
		case TRIPLE:
			pPlayer->AddHeetItem();
			pPlayer->AddHeetItem();
			pPlayer->AddHeetItem();
			break;
		default:
			break;
		}
		m_IsDestroy = true;
	}		
	else if (m_DrawPos.Y < m_pMyStage->GetPlayer()->GetPos().Y)
	{
		if (m_BreakAniFrame == 0)
		{
			Effect_Manager::Play_Effect("HeatBreak", { m_DrawPos.X,m_DrawPos.Y + 10 ,m_DrawPos.Z }, { 25.0f,25.0f,25.0f });
			m_pBreak_HeetSE->Play();
			m_IsDestroy = true; // ���Ȃ��������
		}	
	}
}

int CHeat::CheckLeft(int distance)
{
	//�}�b�v�̉��[�ɗ�����T������߂�
	if (m_MapPos.Y + 1 < 0) { return NOTFOUND_NULLBLOCK; }

	if (m_pMyStage->GetBlockInfo(m_MapPos.X - distance, m_MapPos.Y + 1) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X - distance, m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS)
	{
		return distance;
	}

	//�}�b�v�̍��[�ɗ�����T������߂�
	if (m_MapPos.X - (distance + 1) < 0) { return NOTFOUND_NULLBLOCK; }

	//���Ƀu���b�N��������ΒT���𑱂���
	if (m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_WATER)
	{
		return CheckLeft(distance + 1);
	}
	else
		moving_left = false;

	//�T�����I���
	return NOTFOUND_NULLBLOCK;
}

int CHeat::CheckRight(int distance)
{
	//�}�b�v�̉��[�ɗ�����T������߂�
	if (m_MapPos.Y + 1 < 0) { return NOTFOUND_NULLBLOCK; }

	if (m_pMyStage->GetBlockInfo(m_MapPos.X + distance, m_MapPos.Y + 1) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + distance, m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS)
	{
		return distance;
	}

	//�}�b�v�̉E�[�ɗ�����T������߂�
	if (m_MapPos.X + (distance + 1) > m_pMyStage->GetStageWidthNum().X - 1) { return NOTFOUND_NULLBLOCK; }

	//�E�Ƀu���b�N��������ΒT���𑱂���
	if (m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_WATER)
	{
		return CheckRight(distance + 1);
	}
	else
		moving_right = false;

	//�T�����I���
	return NOTFOUND_NULLBLOCK;
}

int CHeat::CheckLeftWater(int distance)
{
	//�}�b�v�̍��[�ɗ�����T������߂�
	if (m_MapPos.X - (distance + 1) < 0) { return NOTFOUND_WATERBLOCK; }

	//�E�����󓴂Ȃ�T������߂�
	if (m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y + 1) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
	{
		return NOTFOUND_WATERBLOCK;
	}

	//���Ƀu���b�N�������Ă��̉����󓴂���Ȃ��Ȃ�T���𑱂���
	if (m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y) == nullptr)
	{
		return CheckLeftWater(distance + 1);
	}
	else if (m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS)
	{
		return CheckRightWater(distance + 1);
	}

	//���̃u���b�N�����Ȃ猻�ݒn�܂ł̋�����Ԃ�
	if (m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER) { return distance + 1; }

	//�T�����I���
	return NOTFOUND_WATERBLOCK;
}

int CHeat::CheckRightWater(int distance)
{
	//�}�b�v�̉E�[�ɗ�����T������߂�
	if (m_MapPos.X + (distance + 1) >= m_pMyStage->GetStageWidthNum().X - 1) { return NOTFOUND_WATERBLOCK; }

	//�E�����󓴂Ȃ�T������߂�
	if (m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y + 1) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
	{
		return NOTFOUND_WATERBLOCK;
	}

	//�E�Ƀu���b�N�������Ă��̉����󓴂���Ȃ��Ȃ�T���𑱂���
	if (m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y) == nullptr)
	{
		return CheckRightWater(distance + 1);
	}
	else if (m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS)
	{
		return CheckRightWater(distance + 1);
	}

	//�E�̃u���b�N�����Ȃ猻�ݒn�܂ł̋�����Ԃ�
	if (m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER) { return distance + 1; }

	//�T�����I���
	return NOTFOUND_WATERBLOCK;
}

void CHeat::MoveHeat(int leftDis, int rightDis)
{
	//�������̕����߂������ꍇ
	if (leftDis < rightDis)
	{
		//���Ƀq�[�g�A�C�e�����Ȃ��Ȃ�
		if (m_pMyStage->GetHeet(m_MapPos.X - 1, m_MapPos.Y) == nullptr)
		{
			//���Ɉړ�
			m_DrawPos.X -= Move_Speed;
			moving_left = true;
			//���̃u���b�N�̃|�W�V�����ɒ�������
			if (m_DrawPos.X <= m_pMyStage->Get_DrawPos(m_MapPos.X - 1, m_MapPos.Y).X)
			{
				// �`��|�W�V�����X�V
				m_DrawPos.X = m_pMyStage->Get_DrawPos(m_MapPos.X - 1, m_MapPos.Y).X;
				// �}�b�v��̃|�W�V�����X�V
				m_pMyStage->MoveHeet(m_MapPos.X - 1, m_MapPos.Y);
				moving_left = false;
			}
		}
		else
		{
			CItemBase* pHeat = m_pMyStage->GetHeet(m_MapPos.X - 1, m_MapPos.Y);
			ITEM_COINCIDE coincide = pHeat->GetCoinCide();
			moving_left = true;
			//���Ɉړ�
			m_DrawPos.X -= Move_Speed;
			// ���̕�΂Əd�Ȃ�����
			if (pHeat->GetPos().X > m_DrawPos.X)
			{
				switch (coincide)
				{
				case ONE:
					pHeat->SetCoinCide(CItemBase::ITEM_COINCIDE::DOUBLE);
					break;
				case DOUBLE:
					pHeat->SetCoinCide(CItemBase::ITEM_COINCIDE::TRIPLE);
					break;
				default:
					break;
				}
				m_IsDestroy = true;
			}
		}
	}

	//�E�̋����̕����߂������܂��͓��������Ȃ�
	if (leftDis > rightDis)
	{
		//�E�Ƀq�[�g�A�C�e�����Ȃ��Ȃ�
		if (m_pMyStage->GetHeet(m_MapPos.X + 1, m_MapPos.Y) == nullptr)
		{
			//�E�Ɉړ�
			m_DrawPos.X += Move_Speed;
			moving_right = true;
			//�E�̃u���b�N�̃|�W�V�����ɒ�������
			if (m_DrawPos.X >= m_pMyStage->Get_DrawPos(m_MapPos.X + 1, m_MapPos.Y).X)
			{
				// �`��|�W�V�����X�V
				m_DrawPos.X = m_pMyStage->Get_DrawPos(m_MapPos.X + 1, m_MapPos.Y).X;
				// �}�b�v��̃|�W�V�����X�V
				m_pMyStage->MoveHeet(m_MapPos.X + 1, m_MapPos.Y);
				moving_right = false;
			}
		}
		else
		{
			CItemBase* pHeat = m_pMyStage->GetHeet(m_MapPos.X + 1, m_MapPos.Y);
			ITEM_COINCIDE coincide = pHeat->GetCoinCide();
			moving_right = true;
			//�E�Ɉړ�
			m_DrawPos.X -= Move_Speed;
			// �E�̕�΂Əd�Ȃ�����
			if (pHeat->GetPos().X < m_DrawPos.X)
			{
				switch (coincide)
				{
				case ONE:
					pHeat->SetCoinCide(CItemBase::ITEM_COINCIDE::DOUBLE);
					break;
				case DOUBLE:
					pHeat->SetCoinCide(CItemBase::ITEM_COINCIDE::TRIPLE);
					break;
				default:
					break;
				}
				m_IsDestroy = true;
			}
		}
	}

	//����������������
	if (leftDis == rightDis)
	{
		if (m_pMyStage->GetBlockInfo(m_MapPos.X + 1, m_MapPos.Y) != nullptr)
		{
			if (m_pMyStage->GetBlockInfo(m_MapPos.X + 1, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
			{
				//�E�Ɉړ�
				m_DrawPos.X += Move_Speed;
				moving_right = true;

				//�E�̃u���b�N�̃|�W�V�����ɒ�������
				if (m_DrawPos.X >= m_pMyStage->Get_DrawPos(m_MapPos.X + 1, m_MapPos.Y).X)
				{
					// �`��|�W�V�����X�V
					m_DrawPos.X = m_pMyStage->Get_DrawPos(m_MapPos.X + 1, m_MapPos.Y).X;
					// �}�b�v��̃|�W�V�����X�V
					m_pMyStage->MoveJewely(m_MapPos.X + 1, m_MapPos.Y);

					// �}�b�v��̃|�W�V�����܂Œ�������t���O������
					moving_right = false;
				}
			}
		}
		if (m_pMyStage->GetBlockInfo(m_MapPos.X - 1, m_MapPos.Y) != nullptr)
		{
			if (m_pMyStage->GetBlockInfo(m_MapPos.X - 1, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
			{
				//���Ɉړ�
				m_DrawPos.X -= Move_Speed;
				moving_left = true;

				//���̃u���b�N�̃|�W�V�����ɒ�������
				if (m_DrawPos.X <= m_pMyStage->Get_DrawPos(m_MapPos.X - 1, m_MapPos.Y).X)
				{
					// �`��|�W�V�����X�V
					m_DrawPos.X = m_pMyStage->Get_DrawPos(m_MapPos.X - 1, m_MapPos.Y).X;
					// �}�b�v��̃|�W�V�����X�V
					m_pMyStage->MoveJewely(m_MapPos.X - 1, m_MapPos.Y);

					moving_left = false;
				}
			}
		}
	}
}

void CHeat::CheckCollisionWater()
{
	Block_Base* pBlock = m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y);
	bWater_InFlg = false;
	//���Əd�Ȃ��Ă���Ȃ�
	if (m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y) != nullptr)
	{
		if (pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
		{
			if ((m_DrawPos.X - 5.0f <= pBlock->GetPos().X) &&
				(m_DrawPos.X + 5.0f >= pBlock->GetPos().X) &&
				(m_DrawPos.Y - 5.0f < pBlock->GetPos().Y) &&
				(m_DrawPos.Y + 5.0f > pBlock->GetPos().Y))
			{
				bWater_InFlg = true;
				bWaterFlg = true;
			}
		}
	}

	// ���Ɉ�x�G�ꂽ��ԂŐ���������荶�ɂ���Ȃ�
	if (m_pMyStage->GetBlockInfo(m_MapPos.X + 1, m_MapPos.Y) != nullptr && bWaterFlg)
	{
		if (m_pMyStage->GetBlockInfo(m_MapPos.X + 1, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
			bWater_InFlg = true;
	}
	// ���Ɉ�x�G�ꂽ��ԂŐ����������E�ɂ���Ȃ�
	if (m_pMyStage->GetBlockInfo(m_MapPos.X - 1, m_MapPos.Y) != nullptr && bWaterFlg)
	{
		if (m_pMyStage->GetBlockInfo(m_MapPos.X - 1, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
			bWater_InFlg = true;
	}
}

void CHeat::CheckCollisionMagma()
{
	Block_Base* pBlock = m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y);
	if (pBlock != nullptr)
	{
		if (pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_MAGMA)
		{
			if ((m_DrawPos.X - 5.0f < pBlock->GetPos().X + Block_Base::BlockSize / 2) &&
				(m_DrawPos.X + 5.0f > pBlock->GetPos().X - Block_Base::BlockSize / 2) &&
				(m_DrawPos.Y - 5.0f < pBlock->GetPos().Y + Block_Base::BlockSize / 2) &&
				m_DrawPos.Y + 5.0f > pBlock->GetPos().Y - Block_Base::BlockSize / 2)
			{
				Effect_Manager::Play_Effect("HeatBreak", { m_DrawPos.X,m_DrawPos.Y + 10 ,m_DrawPos.Z }, { 25.0f,25.0f,25.0f });
				m_pBreak_HeetSE->Play();
				m_IsDestroy = true;
			}
		}
	}
}

void CHeat::MoveAngle()
{
	// �p�x���E���E�l�ȏ�ɂȂ�����
	if (m_angleZ <= ANGLE_RIGHT_END)
	{
		moving_angle_R = false;
		moving_angle_L = true;

	}
	// �p�x�������E�l�ȉ��ɂȂ�����
	if (m_angleZ >= ANGLE_LEFT_END)
	{
		moving_angle_L = false;
		moving_angle_R = true;
	}

	// �E�ɌX���Ă�Ȃ�
	if (moving_angle_R)
	{
		m_angleZ -= ANGLE_Z;
	}
	else
	{
		m_angleZ += ANGLE_Z;
	}

	//�X�����E�Ȃ�
	if (m_angleZ < 0)
	{
		SetAngle(CItemBase::ITEM_ANGLE::RIGHT);
	}
	// �X�������Ȃ�
	if (m_angleZ > 0)
	{
		SetAngle(CItemBase::ITEM_ANGLE::LEFT);
	}
}
