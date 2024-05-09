#pragma once
#include "EnemyBalls.h"

class EnemyAttack : public GameObject
{
public:
	/***************************************************************************
	[概要]
	プレイヤーに攻撃
	
	[引数]
	float              const& power　　加速力
	DirectX::XMFLOAT3  const& ThisPos　自身(敵)の現在座標
	
	[戻り値]
	DirectX::XMFLOAT3　加速値
	***************************************************************************/
	DirectX::XMFLOAT3 EnemyAttackPlayer(float const& power, DirectX::XMFLOAT3 const& ThisPos);

	/***************************************************************************
	[概要]
	プレイヤー以外のオブジェクトに攻撃
	
	[引数]
	float              const& power　　　加速力
	int                const& ObjNumber　オブジェクトのリスト番号
	DirectX::XMFLOAT3  const& ThisPos　　自身(敵)の現在座標
	
	[戻り値]
	DirectX::XMFLOAT3　加速値
	***************************************************************************/
	DirectX::XMFLOAT3 EnemyAttackObject(float const& power, int const& ObjNumber, DirectX::XMFLOAT3 const& ThisPos);
};