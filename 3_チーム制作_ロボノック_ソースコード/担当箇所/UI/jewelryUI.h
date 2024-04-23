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

	// �Z�b�^�[
	void SetPlayer(C_Player* pPlayer) { m_pPlayer = pPlayer; }
	void SetCameraManager(C_CameraManager* pCameraManager) { m_pCameraManager = pCameraManager; }

public:
	typedef struct
	{
		DirectX::XMFLOAT2 Size;                  // �摜�̑傫��
		DirectX::XMFLOAT2 Pos;				     // �摜�̕\���ʒu
		DirectX::XMFLOAT3 Angle;	             // �p�x
		bool GetJewelry;                         // ��΂��擾����������
		ID3D11ShaderResourceView* pJewelryNot;   // ���l��UI�̏��
		ID3D11ShaderResourceView* pJewelryGet;   // �l��UI�̏��

		// �x�W�G�Ȑ��p
		DirectX::XMFLOAT2 StartPos;              // �x�W�G�p�J�n���W
		DirectX::XMFLOAT2 EndPos;                // �x�W�G�p�I�����W
		DirectX::XMFLOAT2 ControlPos1;           // �x�W�G�p����_
		DirectX::XMFLOAT2 ControlPos2;           // �x�W�G�p����_
		int MaxCnt;								 // �A�j���[�V�������J�E���g
		int CurrentCnt;                          // ���݂̃J�E���g
		bool Use;                                // �A�j���[�V�����̗L��
		bool AnimeFlg;
	}JEWELRYUI_INFO;

private:
	JewelryUI::JEWELRYUI_INFO m_Jewelry[6];
	SpriteManager* m_pSprite;
	C_Player* m_pPlayer;
	C_CameraManager* m_pCameraManager;
	DirectX::XMFLOAT2 m_2DJewelyPos;
	int m_UINum;   // ��΂�����邩�̕ۑ��p�ϐ�
	int m_Count;
};

#endif //  __JEWELRY_UI_H__
