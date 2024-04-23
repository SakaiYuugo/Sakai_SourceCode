#pragma once
#ifndef  __JEWELRY_UI_H__
#define  __JEWELRY_UI_H__

#include "DirectX.h"
#include "SpriteManager.h"
#include "Player.h"
#include <string>
#include <list>
#include "Camera_Manager.h"

class JewelryUI
{
public:
	JewelryUI();
	~JewelryUI();
	void MakeJewelryUI(int Num);
	void GetJewelryUI();
	void GetMoveJewelry();
	void Draw();

	// セッター
	void SetPlayer(C_Player* pPlayer) { m_pPlayer = pPlayer; }
	void SetCameraManager(C_CameraManager* pCameraManager) { m_pCameraManager = pCameraManager; }

public:
	typedef struct
	{
		DirectX::XMFLOAT2 Size;                  // 画像の大きさ
		DirectX::XMFLOAT2 Pos;				     // 画像の表示位置
		DirectX::XMFLOAT3 Angle;	             // 角度
		bool GetJewelry;                         // 宝石を取得したか判定
		ID3D11ShaderResourceView* pJewelryNot;   // 未獲得UIの情報
		ID3D11ShaderResourceView* pJewelryGet;   // 獲得UIの情報

		// ベジエ曲線用
		DirectX::XMFLOAT2 StartPos;              // ベジエ用開始座標
		DirectX::XMFLOAT2 EndPos;                // ベジエ用終了座標
		DirectX::XMFLOAT2 ControlPos1;           // ベジエ用制御点
		DirectX::XMFLOAT2 ControlPos2;           // ベジエ用制御点
		int MaxCnt;								 // アニメーション総カウント
		int CurrentCnt;                          // 現在のカウント
		bool Use;                                // アニメーションの有無
		bool AnimeFlg;
	}JEWELRYUI_INFO;

private:
	JewelryUI::JEWELRYUI_INFO m_Jewelry[6];
	SpriteManager* m_pSprite;
	C_Player* m_pPlayer;
	C_CameraManager* m_pCameraManager;
	DirectX::XMFLOAT2 m_2DJewelyPos;
	int m_UINum;   // 宝石を何個作るかの保存用変数
	int m_Count;
};

#endif //  __JEWELRY_UI_H__
