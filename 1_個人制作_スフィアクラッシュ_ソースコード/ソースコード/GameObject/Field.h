// Fieldオブジェクト [Field.h]
#pragma once
#include "GameObject.h"

class Field : public GameObject
{
public:
	/***************************************************************************
	[概要]
	プレイヤー初期化
	
	[戻り値]
	void
	***************************************************************************/
	void Init() override;

public:
	static const float m_Radius;
	static const DirectX::XMFLOAT3 CenterPoint;
};