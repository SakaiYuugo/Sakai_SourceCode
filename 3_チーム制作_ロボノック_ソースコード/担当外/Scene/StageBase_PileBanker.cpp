#include "StageBase.h"
#include <stdio.h>
#include "BlockBase.h"
#include "ItemBase.h"
#include <Windows.h>
#include "PileBankerBase.h"

//パイルバンカー 
#include "Main_Bunker.h"
#include "Side_Banker.h"

void Stage_Base::NvigateUpdate()
{
	if (m_pNavigate)
	{
		m_pNavigate->Update();
	}

	if (m_pSideBanker)
	{
		m_pSideBanker->Update();
	}
}

void Stage_Base::NvigateDraw()
{
	//パイルバンカー描画
	if (m_pNavigate)
	{
		m_pNavigate->Draw();
	}

	if (m_pSideBanker)
	{
		m_pSideBanker->Draw();
	}
}

//縦杭を作る
void Stage_Base::CreateMainBanker(int LineNum, FloatPos InstancePos, bool Heet)
{
	if (!m_pNavigate)
	{
		Main_Bunker* pMainBanker = new Main_Bunker(this, LineNum, InstancePos, Heet);
		pMainBanker->SetCameraManager(m_pCameraManager);
		pMainBanker->SetModelManager(m_pModelManager);
		pMainBanker->Can_TypeInSide(m_CanTypeIn_Side);
		m_pNavigate = pMainBanker;
	}
}

//横杭を作る
void Stage_Base::CreateSideBanker()
{
	//縦杭は作られているけど横杭は作られていない場合
	if (!m_pSideBanker && m_pNavigate)
	{
		m_pSideBanker = new Side_Banker(this, m_pNavigate);
		m_pSideBanker->SetCameraManager(m_pCameraManager);
		m_pSideBanker->SetModelManager(m_pModelManager);
	}
}

//縦杭を消す
void Stage_Base::DestroyMainBanker()
{
	if (m_pNavigate)
	{
		delete m_pNavigate;
		m_pNavigate = nullptr;
	}
}

//横杭を消す
void Stage_Base::DestroySideBanker()
{
	if (m_pSideBanker)
	{
		delete m_pSideBanker;
		m_pSideBanker = nullptr;
	}
}

//釘打ちの情報を返す
PileBanker* Stage_Base::GetPileBanker()
{
	return m_pNavigate;
}

//横杭の情報を返す
PileBanker* Stage_Base::GetSideBanker()
{
	return m_pSideBanker;
}

//サイドバンカーを打ち込めないようにする
void Stage_Base::SetCantTypeIn_Side()
{
	m_CanTypeIn_Side = false;
}