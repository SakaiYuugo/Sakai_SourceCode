#pragma once
#include "EnemyBalls.h"

class EnemyAttack : public GameObject
{
public:
	/***************************************************************************
	[概要]
	プレイヤーに攻撃
	
	[引数]
	float             power　　加速力
	DirectX::XMFLOAT3 ThisPos　自身(敵)の現在座標
	
	[戻り値]
	DirectX::XMFLOAT3　加速値
	***************************************************************************/
	DirectX::XMFLOAT3 EnemyAttackPlayer(float power, DirectX::XMFLOAT3 ThisPos);

	/***************************************************************************
	[概要]
	プレイヤー以外のオブジェクトに攻撃
	
	[引数]
	float             power　　　加速力
	int               ObjNumber　オブジェクトのリスト番号
	DirectX::XMFLOAT3 ThisPos　　自身(敵)の現在座標
	
	[戻り値]
	DirectX::XMFLOAT3　加速値
	***************************************************************************/
	DirectX::XMFLOAT3 EnemyAttackObject(float power, int ObjNumber, DirectX::XMFLOAT3 ThisPos);
};