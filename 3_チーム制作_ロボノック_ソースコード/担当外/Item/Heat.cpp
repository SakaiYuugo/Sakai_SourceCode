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
	// ブロックの中にない場合(水とガス以外)
	if (m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER ||
		m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_MAGMA)
	{

		//下の情報を取ってくる
		Block_Base* pBlock = m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y + 1);

		//真下が空洞だった場合
		if (pBlock == nullptr || pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER ||
			pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
			pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_MAGMA)
		{
			if (!(moving_left) && !(moving_right))
			{
				// 下にヒートアイテムがないなら
				if (m_pMyStage->GetHeet(m_MapPos.X, m_MapPos.Y + 1) == nullptr)
				{
					//下に降りる
					m_DrawPos.Y -= Down_Speed;
					moving_down = true;
					//下のブロックのポジションに着いたら
					if (m_DrawPos.Y <= m_pMyStage->Get_DrawPos(m_MapPos.X, m_MapPos.Y + 1).Y)
					{
						// 描画ポジション更新
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
					//下に降りる
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
		else if (pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_INVINCIBLE)//下が外だった場合
		{
			m_pMyStage->Refuge_Item();
			m_State = CHeat::HEAT_OUT;
			m_PileLineNum = m_pMyStage->Get_MapPos(m_DrawPos.X, m_DrawPos.Y).X;
		}
		else // 真下が固体ブロック
		{
			MoveAngle();

			int leftDownDistance = CheckLeft(0);	//左下の空洞までの距離
			int rightDownDistance = CheckRight(0);	//右下の空洞までの距離

			CheckCollisionWater();

			//移動できる場所に水ブロックが見つかった場合
			if (bWater_InFlg)
			{
				//下に空洞がある場合
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
		m_DrawPos.Y -= Down_Speed / 2.0f;	// 落下
	}
	
	C_Player* pPlayer = m_pMyStage->GetPlayer();

	//宝石がプレイヤーの位置よりも下に行ったとき
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
			m_IsDestroy = true; // 取れなかったら壊す
		}	
	}
}

int CHeat::CheckLeft(int distance)
{
	//マップの下端に来たら探索をやめる
	if (m_MapPos.Y + 1 < 0) { return NOTFOUND_NULLBLOCK; }

	if (m_pMyStage->GetBlockInfo(m_MapPos.X - distance, m_MapPos.Y + 1) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X - distance, m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS)
	{
		return distance;
	}

	//マップの左端に来たら探索をやめる
	if (m_MapPos.X - (distance + 1) < 0) { return NOTFOUND_NULLBLOCK; }

	//左にブロックが無ければ探索を続ける
	if (m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_WATER)
	{
		return CheckLeft(distance + 1);
	}
	else
		moving_left = false;

	//探索を終わる
	return NOTFOUND_NULLBLOCK;
}

int CHeat::CheckRight(int distance)
{
	//マップの下端に来たら探索をやめる
	if (m_MapPos.Y + 1 < 0) { return NOTFOUND_NULLBLOCK; }

	if (m_pMyStage->GetBlockInfo(m_MapPos.X + distance, m_MapPos.Y + 1) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + distance, m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS)
	{
		return distance;
	}

	//マップの右端に来たら探索をやめる
	if (m_MapPos.X + (distance + 1) > m_pMyStage->GetStageWidthNum().X - 1) { return NOTFOUND_NULLBLOCK; }

	//右にブロックが無ければ探索を続ける
	if (m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_WATER)
	{
		return CheckRight(distance + 1);
	}
	else
		moving_right = false;

	//探索を終わる
	return NOTFOUND_NULLBLOCK;
}

int CHeat::CheckLeftWater(int distance)
{
	//マップの左端に来たら探索をやめる
	if (m_MapPos.X - (distance + 1) < 0) { return NOTFOUND_WATERBLOCK; }

	//右下が空洞なら探索をやめる
	if (m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y + 1) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
	{
		return NOTFOUND_WATERBLOCK;
	}

	//左にブロックが無くてその下が空洞じゃないなら探索を続ける
	if (m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y) == nullptr)
	{
		return CheckLeftWater(distance + 1);
	}
	else if (m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS)
	{
		return CheckRightWater(distance + 1);
	}

	//左のブロックが水なら現在地までの距離を返す
	if (m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER) { return distance + 1; }

	//探索を終わる
	return NOTFOUND_WATERBLOCK;
}

int CHeat::CheckRightWater(int distance)
{
	//マップの右端に来たら探索をやめる
	if (m_MapPos.X + (distance + 1) >= m_pMyStage->GetStageWidthNum().X - 1) { return NOTFOUND_WATERBLOCK; }

	//右下が空洞なら探索をやめる
	if (m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y + 1) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
	{
		return NOTFOUND_WATERBLOCK;
	}

	//右にブロックが無くてその下が空洞じゃないなら探索を続ける
	if (m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y) == nullptr)
	{
		return CheckRightWater(distance + 1);
	}
	else if (m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS)
	{
		return CheckRightWater(distance + 1);
	}

	//右のブロックが水なら現在地までの距離を返す
	if (m_pMyStage->GetBlockInfo(m_MapPos.X + (distance + 1), m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER) { return distance + 1; }

	//探索を終わる
	return NOTFOUND_WATERBLOCK;
}

void CHeat::MoveHeat(int leftDis, int rightDis)
{
	//左距離の方が近かった場合
	if (leftDis < rightDis)
	{
		//左にヒートアイテムがないなら
		if (m_pMyStage->GetHeet(m_MapPos.X - 1, m_MapPos.Y) == nullptr)
		{
			//左に移動
			m_DrawPos.X -= Move_Speed;
			moving_left = true;
			//左のブロックのポジションに着いたら
			if (m_DrawPos.X <= m_pMyStage->Get_DrawPos(m_MapPos.X - 1, m_MapPos.Y).X)
			{
				// 描画ポジション更新
				m_DrawPos.X = m_pMyStage->Get_DrawPos(m_MapPos.X - 1, m_MapPos.Y).X;
				// マップ上のポジション更新
				m_pMyStage->MoveHeet(m_MapPos.X - 1, m_MapPos.Y);
				moving_left = false;
			}
		}
		else
		{
			CItemBase* pHeat = m_pMyStage->GetHeet(m_MapPos.X - 1, m_MapPos.Y);
			ITEM_COINCIDE coincide = pHeat->GetCoinCide();
			moving_left = true;
			//左に移動
			m_DrawPos.X -= Move_Speed;
			// 左の宝石と重なったら
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

	//右の距離の方が近かったまたは同じ距離なら
	if (leftDis > rightDis)
	{
		//右にヒートアイテムがないなら
		if (m_pMyStage->GetHeet(m_MapPos.X + 1, m_MapPos.Y) == nullptr)
		{
			//右に移動
			m_DrawPos.X += Move_Speed;
			moving_right = true;
			//右のブロックのポジションに着いたら
			if (m_DrawPos.X >= m_pMyStage->Get_DrawPos(m_MapPos.X + 1, m_MapPos.Y).X)
			{
				// 描画ポジション更新
				m_DrawPos.X = m_pMyStage->Get_DrawPos(m_MapPos.X + 1, m_MapPos.Y).X;
				// マップ上のポジション更新
				m_pMyStage->MoveHeet(m_MapPos.X + 1, m_MapPos.Y);
				moving_right = false;
			}
		}
		else
		{
			CItemBase* pHeat = m_pMyStage->GetHeet(m_MapPos.X + 1, m_MapPos.Y);
			ITEM_COINCIDE coincide = pHeat->GetCoinCide();
			moving_right = true;
			//右に移動
			m_DrawPos.X -= Move_Speed;
			// 右の宝石と重なったら
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

	//同じ距離だったら
	if (leftDis == rightDis)
	{
		if (m_pMyStage->GetBlockInfo(m_MapPos.X + 1, m_MapPos.Y) != nullptr)
		{
			if (m_pMyStage->GetBlockInfo(m_MapPos.X + 1, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
			{
				//右に移動
				m_DrawPos.X += Move_Speed;
				moving_right = true;

				//右のブロックのポジションに着いたら
				if (m_DrawPos.X >= m_pMyStage->Get_DrawPos(m_MapPos.X + 1, m_MapPos.Y).X)
				{
					// 描画ポジション更新
					m_DrawPos.X = m_pMyStage->Get_DrawPos(m_MapPos.X + 1, m_MapPos.Y).X;
					// マップ上のポジション更新
					m_pMyStage->MoveJewely(m_MapPos.X + 1, m_MapPos.Y);

					// マップ上のポジションまで着いたらフラグを下す
					moving_right = false;
				}
			}
		}
		if (m_pMyStage->GetBlockInfo(m_MapPos.X - 1, m_MapPos.Y) != nullptr)
		{
			if (m_pMyStage->GetBlockInfo(m_MapPos.X - 1, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
			{
				//左に移動
				m_DrawPos.X -= Move_Speed;
				moving_left = true;

				//左のブロックのポジションに着いたら
				if (m_DrawPos.X <= m_pMyStage->Get_DrawPos(m_MapPos.X - 1, m_MapPos.Y).X)
				{
					// 描画ポジション更新
					m_DrawPos.X = m_pMyStage->Get_DrawPos(m_MapPos.X - 1, m_MapPos.Y).X;
					// マップ上のポジション更新
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
	//水と重なっているなら
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

	// 水に一度触れた状態で水が自分より左にあるなら
	if (m_pMyStage->GetBlockInfo(m_MapPos.X + 1, m_MapPos.Y) != nullptr && bWaterFlg)
	{
		if (m_pMyStage->GetBlockInfo(m_MapPos.X + 1, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
			bWater_InFlg = true;
	}
	// 水に一度触れた状態で水が自分より右にあるなら
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
	// 角度が右限界値以上になったら
	if (m_angleZ <= ANGLE_RIGHT_END)
	{
		moving_angle_R = false;
		moving_angle_L = true;

	}
	// 角度が左限界値以下になったら
	if (m_angleZ >= ANGLE_LEFT_END)
	{
		moving_angle_L = false;
		moving_angle_R = true;
	}

	// 右に傾いてるなら
	if (moving_angle_R)
	{
		m_angleZ -= ANGLE_Z;
	}
	else
	{
		m_angleZ += ANGLE_Z;
	}

	//傾きが右なら
	if (m_angleZ < 0)
	{
		SetAngle(CItemBase::ITEM_ANGLE::RIGHT);
	}
	// 傾きが左なら
	if (m_angleZ > 0)
	{
		SetAngle(CItemBase::ITEM_ANGLE::LEFT);
	}
}
