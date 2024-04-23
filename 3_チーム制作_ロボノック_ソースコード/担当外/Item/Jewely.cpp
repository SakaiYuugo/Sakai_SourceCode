//#include <stdlib.h>
#include <cstdlib>
#include <ctime>
#include "Jewely.h"
#include "ModelManager.h"
#include "BlockBase.h"
#include "PileBankerBase.h"
#include "Player.h"
#include "Effect_Manager.h"
#include "Collision.h"
#include "CameraMiss.h"

using namespace std;


#define NOTFOUND_NULLBLOCK (999)
#define NOTFOUND_WATERBLOCK (999)
#define ANGLE_Z (1.0f)
#define ANGLE_RIGHT_END (-45.0f)
#define ANGLE_LEFT_END (45.0f)


CJewely::CJewely(Stage_Base * MyStage, int PosX, int PosY)
	:CItemBase(MyStage, PosX, PosY)
	, Down_Speed(50.0f / 40.0f), Move_Speed(50.0f / 40.0f), moving_right(false), moving_left(false), bWater_InFlg(false), moving_down(false)
	, m_angleZ(-45.0f), moving_angle_R(false), moving_angle_L(false)
{
	m_pModelManager->c_AddModel("Jewely", "Assets/NewItem/item_houseki1.fbx");
	m_State = CJewely::JEWELYSTATE::JEWELY_IN;

	m_pGet_JewelySE = new SE("Assets/SE/Get_Jewely.wav");
	m_pBreak_JewelySE = new SE("Assets/SE/Item_Break.wav");

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
	m_efkManager = Effect_Manager::GetManager();
	m_JewelyHandle = m_efkManager->Play(Effect_Manager::GetEffect("Kirakira"), 0, 0, 0);
	std::srand(time(NULL));
}

CJewely::~CJewely()
{
	m_efkManager->StopEffect(m_JewelyHandle);
	delete m_pBreak_JewelySE;
	delete m_pGet_JewelySE;
}

void CJewely::Update()
{	

		if(m_AniFrame == 0)
			m_nRand = rand() % 60 + 60;
		
		DirectX::XMFLOAT3 Pos = { m_DrawPos.X,m_DrawPos.Y,m_DrawPos.Z - 20};
		DirectX::XMFLOAT3 Size = { 10.0f,10.0f,10.0f };
		if (m_AniFrame == m_nRand)
		{
			m_efkManager->SetLocation(m_JewelyHandle, Pos.x, Pos.y, Pos.z);
			Effect_Manager::Play_Effect("Kirakira", Pos, Size);
			m_AniFrame = 0;
		}
		// ブロックの中にない場合(水とガス以外)
		if (m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y) == nullptr ||
			m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER ||
			m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
			m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_MAGMA)
		{
			//下のブロックの情報を取ってくる
			Block_Base* pBlock = m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y + 1);

			//真下が空洞だった場合
			if (pBlock == nullptr || pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER ||
				pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
				pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_MAGMA)
			{
				if (!(moving_left) && !(moving_right))
				{
					// 下に宝石がないなら
					if (m_pMyStage->GetJewely(m_MapPos.X, m_MapPos.Y + 1) == nullptr)
					{
						//下に降りる
						m_DrawPos.Y -= Down_Speed;
						moving_down = true;
						//下のブロックのポジションに着いたら
						if (m_DrawPos.Y <= m_pMyStage->Get_DrawPos(m_MapPos.X, m_MapPos.Y + 1).Y)
						{

							// 描画ポジション更新
							m_DrawPos.Y = m_pMyStage->Get_DrawPos(m_MapPos.X, m_MapPos.Y + 1).Y;

							m_pMyStage->MoveJewely(m_MapPos.X, m_MapPos.Y + 1);
							moving_down = false;
						}
					}
					else
					{
						CItemBase* pJewely = m_pMyStage->GetJewely(m_MapPos.X, m_MapPos.Y + 1);
						ITEM_COINCIDE coincide = pJewely->GetCoinCide();
						moving_down = true;
						//下に降りる
						m_DrawPos.Y -= Down_Speed;
						// 下の宝石と重なったら
						if (pJewely->GetPos().Y > m_DrawPos.Y)
						{
							switch (coincide)
							{
							case ONE:
								pJewely->SetCoinCide(CItemBase::ITEM_COINCIDE::DOUBLE);
								break;
							case DOUBLE:
								pJewely->SetCoinCide(CItemBase::ITEM_COINCIDE::TRIPLE);
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
				//ステージ外に出た場合
				m_pMyStage->Refuge_Item();
				m_State = CJewely::JEWELY_OUT;
				m_PileLineNum = m_pMyStage->Get_MapPos(m_DrawPos.X, m_DrawPos.Y).X;
			}
			else // 真下が固体ブロック
			{

				MoveAngle();

				int leftDownDistance = CheckLeft(0);		//左下の空洞までの距離
				int rightDownDistance = CheckRight(0);	    //右下の空洞までの距離

				CheckCollisionWater();

				//同じ場所に水があるなら
				if (bWater_InFlg)
				{
					//下に空洞がある場合
					if ((leftDownDistance != NOTFOUND_NULLBLOCK || rightDownDistance != NOTFOUND_NULLBLOCK) && !(moving_down))
					{
						MoveJewely(leftDownDistance, rightDownDistance);
					}
				}
				if (!(bWater_InFlg) && moving_left && !(moving_down))
				{
					MoveJewely(leftDownDistance, rightDownDistance);
				}
				if (!(bWater_InFlg) && moving_right && !(moving_down))
				{
					MoveJewely(leftDownDistance, rightDownDistance);
				}
				// マグマの中に入ったらゲームオーバー
				CheckCollisionMagma();
			}
		}
		m_AniFrame++;
}

void CJewely::Draw()
{
	m_DrawPos.Z = -5.0f;
	switch (m_CoinCide)
	{
	case ONE:
		m_pModelManager->c_ModelDraw("Jewely", m_DrawPos.X, m_DrawPos.Y, m_DrawPos.Z
			, m_size.x, m_size.y, m_size.z, 0.0f, 180.0f, m_angleZ);
		break;
	case DOUBLE:
		m_pModelManager->c_ModelDraw("Jewely", m_DrawPos.X - 5.0f, m_DrawPos.Y, m_DrawPos.Z
			, m_size.x, m_size.y, m_size.z, 0.0f, 180.0f, m_angleZ);

		m_pModelManager->c_ModelDraw("Jewely", m_DrawPos.X + 5.0f, m_DrawPos.Y, m_DrawPos.Z
			, m_size.x, m_size.y, m_size.z, 0.0f, 180.0f, m_angleZ);
		break;
	case TRIPLE:
		m_pModelManager->c_ModelDraw("Jewely", m_DrawPos.X - 5.0f, m_DrawPos.Y, m_DrawPos.Z
			, m_size.x, m_size.y, m_size.z, 0.0f, 180.0f, m_angleZ);

		m_pModelManager->c_ModelDraw("Jewely", m_DrawPos.X + 5.0f, m_DrawPos.Y, m_DrawPos.Z
			, m_size.x, m_size.y, m_size.z, 0.0f, 180.0f, m_angleZ);

		m_pModelManager->c_ModelDraw("Jewely", m_DrawPos.X, m_DrawPos.Y, m_DrawPos.Z
			, m_size.x, m_size.y, m_size.z, 0.0f, 180.0f, m_angleZ);
		break;
	default:
		break;
	}
}



void CJewely::RefugeUpdate()
{
	if (m_BreakAniFrame == 0)
	{
		m_DrawPos.Y -= Down_Speed / 2.0f;	// 下へ

	//宝石がプレイヤーの位置よりも下に行ったとき
		DirectX::XMFLOAT3 Position = { m_DrawPos.X,m_DrawPos.Y, m_DrawPos.Z };
		DirectX::XMFLOAT3 PlayerPos = { m_pMyStage->GetPlayer()->GetPos().X ,m_pMyStage->GetPlayer()->GetPos().Y,m_pMyStage->GetPlayer()->GetPos().Z };
		if (Collision::CheckSquare(Position, 30.0f, PlayerPos, 30.0f))
		{
			DirectX::XMFLOAT3 Size = { 50.0f,50.0f,0.0f };
			Effect_Manager::Play_Effect("Item", PlayerPos, Size);
			m_pGet_JewelySE->Play();

			switch (m_CoinCide)
			{
			case ONE:
				m_pMyStage->Mina_Jewely();
				break;
			case DOUBLE:
				m_pMyStage->Mina_Jewely();
				m_pMyStage->Mina_Jewely();
				break;
			case TRIPLE:
				m_pMyStage->Mina_Jewely();
				m_pMyStage->Mina_Jewely();
				m_pMyStage->Mina_Jewely();
				break;
			default:
				break;
			}
			m_IsDestroy = true;
		}
	}
	if (m_DrawPos.Y < m_pMyStage->GetPlayer()->GetPos().Y)
	{
		// 一度だけ入る
		if (m_BreakAniFrame == 0)
		{
			Effect_Manager::Play_Effect("JewelyBreak", { m_DrawPos.X,m_DrawPos.Y + 5 ,m_DrawPos.Z }, { 25.0f,25.0f,25.0f });
			m_pBreak_JewelySE->Play();
			m_IsDestroy = true;
		}
		m_pCameraManager->ChangeCameraNew(C_CameraManager::SCENE_CAMERA_TYPE::_CAMERA_GAME_MISS
			, m_pCameraManager->Get_NowCamera()->GetPos(),
			m_pCameraManager->Get_NowCamera()->GetLook());

		CameraMiss* pMissCamera = dynamic_cast<CameraMiss*>(m_pCameraManager->Get_TypeCamera(C_CameraManager::SCENE_CAMERA_TYPE::_CAMERA_GAME_MISS));
		if (pMissCamera != nullptr)
			pMissCamera->Set({ m_DrawPos.X, m_DrawPos.Y, m_DrawPos.Z });

		m_pMyStage->Set_GameOver(Stage_Base::GAMEOVER_TYPE::GAMEOVER_JEWELY_DOROP);
		m_BreakAniFrame++;
		
		
	}
}

int CJewely::CheckLeft(int distance)
{

	//マップの下端に来たら探索をやめる
	if (m_MapPos.Y + 1 < 0) { return NOTFOUND_NULLBLOCK; }

	if (m_pMyStage->GetBlockInfo(m_MapPos.X - distance, m_MapPos.Y + 1) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X - distance, m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X - distance, m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_WATER)
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

int CJewely::CheckRight(int distance)
{
	//マップの下端に来たら探索をやめる
	if (m_MapPos.Y + 1 < 0) { return NOTFOUND_NULLBLOCK; }

	if (m_pMyStage->GetBlockInfo(m_MapPos.X + distance, m_MapPos.Y + 1) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + distance, m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X + distance, m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_WATER)
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

int CJewely::CheckLeftWater(int distance)
{
	//マップの左端に来たら探索をやめる
	if (m_MapPos.X - (distance + 1) < 0) { return NOTFOUND_WATERBLOCK; }

	//左下が空洞なら探索をやめる
	if (m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y + 1) == nullptr ||
		m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_GAS ||
		m_pMyStage->GetBlockInfo(m_MapPos.X - (distance + 1), m_MapPos.Y + 1)->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
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

int CJewely::CheckRightWater(int distance)
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

void CJewely::CheckCollisionWater()
{
	bWater_InFlg = false;
	Block_Base* pBlock = m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y);
	//水と重なっているなら
	if (pBlock != nullptr)
	{
		if (pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_WATER)
		{
			if ((m_DrawPos.X - 5.0f < pBlock->GetPos().X) &&
				(m_DrawPos.X + 5.0f > pBlock->GetPos().X) &&
				(m_DrawPos.Y - 5.0f < pBlock->GetPos().Y) &&
				m_DrawPos.Y + 5.0f > pBlock->GetPos().Y)
			{
				bWater_InFlg = true;
				bWaterFlg = true;
			}	
		}
	}
}

void CJewely::CheckCollisionMagma()
{
	Block_Base* pBlock = m_pMyStage->GetBlockInfo(m_MapPos.X, m_MapPos.Y);
	if (pBlock != nullptr)
	{
		if (pBlock->GetType() == Block_Base::BLOCK_TYPE::BLOCK_MAGMA)
		{
			if ((m_DrawPos.X - 10.0f < pBlock->GetPos().X + Block_Base::BlockSize / 2) &&
				(m_DrawPos.X + 10.0f > pBlock->GetPos().X - Block_Base::BlockSize / 2) &&
				(m_DrawPos.Y - 10.0f < pBlock->GetPos().Y + Block_Base::BlockSize / 2) &&
				m_DrawPos.Y + 10.0f > pBlock->GetPos().Y - Block_Base::BlockSize / 2)
			{
				// 一度だけ入る
				if (m_BreakAniFrame == 0)
				{
					Effect_Manager::Play_Effect("JewelyBreak", { m_DrawPos.X,m_DrawPos.Y + 5 ,m_DrawPos.Z }, { 25.0f,25.0f,25.0f });
					m_pBreak_JewelySE->Play();
					m_IsDestroy = true;
				}
				m_pCameraManager->ChangeCameraNew(C_CameraManager::SCENE_CAMERA_TYPE::_CAMERA_GAME_MISS
					, m_pCameraManager->Get_NowCamera()->GetPos(),
					m_pCameraManager->Get_NowCamera()->GetLook());

				CameraMiss* pMissCamera = dynamic_cast<CameraMiss*>(m_pCameraManager->Get_TypeCamera(C_CameraManager::SCENE_CAMERA_TYPE::_CAMERA_GAME_MISS));
				if (pMissCamera != nullptr)
					pMissCamera->Set({m_DrawPos.X, m_DrawPos.Y, m_DrawPos.Z});

				m_pMyStage->Set_GameOver(Stage_Base::GAMEOVER_TYPE::GAMEOVER_JEWELY_MELT);
				m_BreakAniFrame++;
			}
		}
	}
}



void CJewely::MoveJewely(int leftDis, int rightDis)
{
	//左距離の方が近かった場合
	if (leftDis < rightDis)
	{
		// 左に宝石がないなら
		if (m_pMyStage->GetJewely(m_MapPos.X - 1, m_MapPos.Y) == nullptr)
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
		else
		{
			CItemBase* pJewely = m_pMyStage->GetJewely(m_MapPos.X - 1, m_MapPos.Y); // 左のアイテム情報
			ITEM_COINCIDE coincide = pJewely->GetCoinCide(); // アイテムの重なり数
			moving_left = true;
			//左に移動
			m_DrawPos.X -= Move_Speed;
			// 左の宝石と重なったら
			if (pJewely->GetPos().X > m_DrawPos.X)
			{
				switch (coincide)
				{
				case ONE:
					pJewely->SetCoinCide(CItemBase::ITEM_COINCIDE::DOUBLE);
					break;
				case DOUBLE:
					pJewely->SetCoinCide(CItemBase::ITEM_COINCIDE::TRIPLE);
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
		//右に宝石がないなら
		if (m_pMyStage->GetJewely(m_MapPos.X + 1, m_MapPos.Y) == nullptr)
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
		else
		{
			CItemBase* pJewely = m_pMyStage->GetJewely(m_MapPos.X + 1, m_MapPos.Y); // 右のアイテム情報
			ITEM_COINCIDE coincide = pJewely->GetCoinCide(); // アイテムの重なり数
			moving_right = true;
			//右に移動
			m_DrawPos.X -= Move_Speed;
			
			// 右の宝石と重なったら
			if (pJewely->GetPos().X < m_DrawPos.X)
			{
				
				switch (coincide)
				{
				case ONE:
					pJewely->SetCoinCide(CItemBase::ITEM_COINCIDE::DOUBLE);
					break;
				case DOUBLE:
					pJewely->SetCoinCide(CItemBase::ITEM_COINCIDE::TRIPLE);
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

void CJewely::MoveAngle()
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
	if (m_angleZ < 0.0f)
	{
		SetAngle(CItemBase::ITEM_ANGLE::RIGHT);
	}
	// 傾きが左なら
	if (m_angleZ > 0.0f)
	{
		SetAngle(CItemBase::ITEM_ANGLE::LEFT);
	}
}
