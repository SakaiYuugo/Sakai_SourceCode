#pragma once
#include "GameObject.h"

class Collision : public GameObject
{
public:
	/***************************************************************************
	[概要]
	プレイヤーとの当たり判定処理

	[戻り値]
	bool 衝突判定
	***************************************************************************/
	bool ColliderWithPlayer();

	/***************************************************************************
	[概要]
	エネミー同士の当たり判定処理
	
	[戻り値]
	void
	***************************************************************************/
	void ColliderWithNumberBall();

	/**************************************************************************
	[概要]
	衝突計算

	[引数]
	DirectX::XMFLOAT3　normal　          衝突したオブジェクトの距離
	DirectX::XMFLOAT3　Vel_1 　          速度
	DirectX::XMFLOAT3　Vel_2　           速度
	bool               PlayerColllision　プレイヤーに当たっていたか
	int                ObjNumber_1　     当たったオブジェクトのリスト番号
	int                ObjNumber_2　     当たったオブジェクトのリスト番号

	[戻り値]
	void
	***************************************************************************/
	void CollisionMath(DirectX::XMFLOAT3 normal, DirectX::XMFLOAT3 Vel_1, DirectX::XMFLOAT3 Vel_2, bool PlayerCollision, int ObjNumber_1, int ObjNumber_2);

	/***************************************************************************
	[概要]
	計算後の速度をセット

	[引数]
	DirectX::XMFLOAT3　Vel_1 　          衝突計算後の速度
	DirectX::XMFLOAT3　Vel_2　           衝突計算後の速度
	bool               PlayerColllision　プレイヤーに当たっていたか
	int                ObjNumber_1　     当たったオブジェクトのリスト番号
	int                ObjNumber_2　     当たったオブジェクトのリスト番号

	[戻り値]
	void
	***************************************************************************/
	void SetResultVelocity(DirectX::XMFLOAT3 Vel_1, DirectX::XMFLOAT3 Vel_2, bool PlayerCollision, int ObjNumber_1, int ObjNumber_2);

	/***************************************************************************
	[概要]
	足場判定
	
	[戻り値]
	bool 接地判定
	***************************************************************************/
	bool ColliderWithField(DirectX::XMFLOAT3 position, DirectX::XMFLOAT3 scale);
};
