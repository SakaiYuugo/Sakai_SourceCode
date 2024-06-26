#include "Scene_Stage2_1.h"
#include "Timer.h"
#include "Game3D.h"
#include "Geometory.h"
#include "CameraDebug.h"
#include "DebugConsole.h"
#include "PileBankerBase.h"
#include "StageBase.h"
#include "Camera_Manager.h"
#include "CameraManager_Game.h"
#include "Input.h"
#include "Effect_Manager.h"
#include "CameraStartEvent.h"
#include "XboxKeyboard.h"
#include "BillBoard.h"


CStage2_1::CStage2_1()
{
	SetStageScene(SCENE::SCENE_STAGE2_1);

	const int X = 7, Y = 8;
	int BlockInfo[X * Y] =
	{
		4, 4, 4, 4, 4, 4, 4,
		5, 5, 5, 2, 2, 2, 2,
		5, 5, 5, 2, 2, 2, 8,
		2, 2, 2, 2, 3, 3, 2,
		8, 2, 2, 2, 3, 3, 2,
		5, 5, 2, 2, 2, 2, 2,
		2, 2, 2, 2, 2, 2, 2,
		2, 2, 2, 2, 2, 2, 2,
	};

	int JewelyInfo[X * Y] =
	{
		0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 1, 0,
		0, 0, 0, 0, 0, 0, 0,
		0, 1, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0,
	};

	int HeetInfo[X * Y] =
	{
		0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,

	};

	CreateStage(X, Y, BlockInfo, JewelyInfo, HeetInfo, true);
	SetBGM("Assets/BGM/Stage2.wav");
	SetBackGround(BACKOBJECT::AREA_2);
	//操作説明画像
	SetTutorialImage("Assets/2D/tutorial/tutorial2.png");

	//ゲーム状態の設定
	if (!m_SaveData.AgainStage)
	{
		SetState(GAME_STATE::STATE0_TUTORIAL);
	}
	else
	{
		SetState(GAME_STATE::STATE1_STARTEVENT);
	}
}

CStage2_1::~CStage2_1()
{

}
