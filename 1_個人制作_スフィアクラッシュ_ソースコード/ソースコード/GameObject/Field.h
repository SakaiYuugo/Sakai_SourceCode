// Field�I�u�W�F�N�g [Field.h]
#pragma once
#include "GameObject.h"

class Field : public GameObject
{
public:
	/***************************************************************************
	[�T�v]
	�v���C���[������
	
	[�߂�l]
	void
	***************************************************************************/
	void Init() override;

public:
	static const float m_Radius;
	static const DirectX::XMFLOAT3 CenterPoint;
};