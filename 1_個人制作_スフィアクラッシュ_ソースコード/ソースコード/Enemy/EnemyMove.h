#pragma once
#include "EnemyBalls.h"

class EnemyMove : public GameObject
{
public:

	/***************************************************************************
	[概要]
	プレイヤーを追跡
	
	[引数]
	DirectX::XMFLOAT3 ThisPos　自身(敵)の現在座標

	[戻り値]
	DirectX::XMFLOAT3　プレイヤーに向かうベクトル
	***************************************************************************/
	DirectX::XMFLOAT3 ChasePlayer(DirectX::XMFLOAT3 const& ThisPos);

	/***************************************************************************
	[概要]
	プレイヤー以外のオブジェクトをを追跡

	[引数]
	DirectX::XMFLOAT3 ThisPos　自身(敵)の現在座標

	[戻り値]
	DirectX::XMFLOAT3　追跡対象に向かうベクトル
	***************************************************************************/
	DirectX::XMFLOAT3 ChaseObject(DirectX::XMFLOAT3 const& ThisPos);
};