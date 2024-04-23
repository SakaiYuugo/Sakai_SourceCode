#pragma once
#ifndef __KNOCK_UI_H__
#define __KNOCK_UI_H__

#include "DirectX.h"
#include "SpriteManager.h"

class KnockUI
{
public:
	KnockUI();
	~KnockUI();
	void Draw();
	void AddKnock(int num);
	void UpdateTexCoord();

	// 打ち付け回数取得
	int GetKnockCnt() { return m_KnockCnt; }

public:
	typedef struct
	{
		DirectX::XMFLOAT2 Pos;          // 画像の表示位置
		DirectX::XMFLOAT2 Size;	        // 画像の大きさ
		DirectX::XMFLOAT3 Angle;        // 角度
		DirectX::XMFLOAT3 Scale;		// 大きさ
		DirectX::XMFLOAT2 posTexCoord;	// テクスチャ座標（左上）
		DirectX::XMFLOAT2 sizeTexCoord;	// テクスチャサイズ(右下) (0.0 ～ 1.0)
		bool use;				        // 使用中フラグ
		int frame;				        // アニメーション管理用フレーム
		int currentAnimNo;		        // アニメーションのコマ番号(左上から０～)
		ID3D11ShaderResourceView* pKnockUI;
	}KNOCKUI_INFO;

private:
	SpriteManager* m_pSprite;
	KnockUI::KNOCKUI_INFO m_KnockUI[2];
	int m_KnockCnt;   // 打ち付け回数保存用
};

#endif //__KNOCK_UI_H__