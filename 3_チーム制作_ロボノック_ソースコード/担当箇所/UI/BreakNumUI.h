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

	// �u���b�N�j�󐔎擾
	int GetBreckCnt() { return m_BreakCnt; }
public:
	typedef struct
	{
		DirectX::XMFLOAT2 Pos;             // �摜�̕\���ʒu
		DirectX::XMFLOAT2 Size;	           // �摜�̑傫��
		DirectX::XMFLOAT3 Angle;           // �p�x
		DirectX::XMFLOAT3 Scale;		   // �傫��
		DirectX::XMFLOAT2 posTexCoord;	   // �e�N�X�`�����W�i����j
		DirectX::XMFLOAT2 sizeTexCoord;	   // �e�N�X�`���T�C�Y(�E��) (0.0 �` 1.0)
		bool use;				           // �g�p���t���O
		int frame;				           // �A�j���[�V�����Ǘ��p�t���[��
		int currentAnimNo;		           // �A�j���[�V�����̃R�}�ԍ�(���ォ��O�`)
		ID3D11ShaderResourceView* pBreakUI;
	}BREAKUI_INFO;

private:
	SpriteManager* m_pSprite;
	BreakNumUI::BREAKUI_INFO m_BreakUI[2];
	int m_BreakCnt;
};

#endif // __BREAK_NUM_UI_H__
