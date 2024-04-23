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

	// �ł��t���񐔎擾
	int GetKnockCnt() { return m_KnockCnt; }

public:
	typedef struct
	{
		DirectX::XMFLOAT2 Pos;          // �摜�̕\���ʒu
		DirectX::XMFLOAT2 Size;	        // �摜�̑傫��
		DirectX::XMFLOAT3 Angle;        // �p�x
		DirectX::XMFLOAT3 Scale;		// �傫��
		DirectX::XMFLOAT2 posTexCoord;	// �e�N�X�`�����W�i����j
		DirectX::XMFLOAT2 sizeTexCoord;	// �e�N�X�`���T�C�Y(�E��) (0.0 �` 1.0)
		bool use;				        // �g�p���t���O
		int frame;				        // �A�j���[�V�����Ǘ��p�t���[��
		int currentAnimNo;		        // �A�j���[�V�����̃R�}�ԍ�(���ォ��O�`)
		ID3D11ShaderResourceView* pKnockUI;
	}KNOCKUI_INFO;

private:
	SpriteManager* m_pSprite;
	KnockUI::KNOCKUI_INFO m_KnockUI[2];
	int m_KnockCnt;   // �ł��t���񐔕ۑ��p
};

#endif //__KNOCK_UI_H__