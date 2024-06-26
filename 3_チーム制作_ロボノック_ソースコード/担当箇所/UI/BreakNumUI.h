#pragma once
#ifndef __BREAK_NUM_UI_H__
#define __BREAK_NUM_UI_H__

#include "DirectX.h"
#include "SpriteManager.h"

class BreakNumUI
{
public:
	BreakNumUI();
	~BreakNumUI();
	void Draw();
	void AddNum(int num);
	void UpdateTexCoord();

	// ブロック破壊数取得
	int GetBreckCnt() { return m_BreakCnt; }
public:
	typedef struct
	{
		DirectX::XMFLOAT2 Pos;             // 画像の表示位置
		DirectX::XMFLOAT2 Size;	           // 画像の大きさ
		DirectX::XMFLOAT3 Angle;           // 角度
		DirectX::XMFLOAT3 Scale;		   // 大きさ
		DirectX::XMFLOAT2 posTexCoord;	   // テクスチャ座標（左上）
		DirectX::XMFLOAT2 sizeTexCoord;	   // テクスチャサイズ(右下) (0.0 〜 1.0)
		bool use;				           // 使用中フラグ
		int frame;				           // アニメーション管理用フレーム
		int currentAnimNo;		           // アニメーションのコマ番号(左上から０〜)
		ID3D11ShaderResourceView* pBreakUI;
	}BREAKUI_INFO;

private:
	SpriteManager* m_pSprite;
	BreakNumUI::BREAKUI_INFO m_BreakUI[2];
	int m_BreakCnt;
};

#endif // __BREAK_NUM_UI_H__
