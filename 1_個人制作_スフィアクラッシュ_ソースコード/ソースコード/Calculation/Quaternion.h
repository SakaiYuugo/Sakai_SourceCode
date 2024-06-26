#pragma once
#include "Renderer.h"

class Quaternion
{
public:
	/***************************************************************************
	[概要]
	デフォルトコンストラクタ
	***************************************************************************/
	Quaternion();

	/***************************************************************************
	[概要]
	コンストラクタ

	[引数]
	float x, y, z　回転軸
	flaot w 　　　  回転値
	***************************************************************************/
	Quaternion(float x, float y, float z, float w);

	/***************************************************************************
	[概要]
	デストラクタ
	***************************************************************************/
	~Quaternion();

	/***************************************************************************
	[概要]
	演算子のオーバーロード クォータニオン同士の積の計算
	***************************************************************************/
	Quaternion operator* (Quaternion const& q);
	Quaternion operator*= (Quaternion const& q);

	/***************************************************************************
	[概要]
	単位ベクトルを軸に回転

	[引数]
	const DirectX::XMFLOAT3& vector　回転軸のベクトル

	[戻り値]
	const DirectX::XMFLOAT3& 回転の計算後の値
	***************************************************************************/
	const DirectX::XMFLOAT3& RotateVector(DirectX::XMFLOAT3 const& vector);

	/***************************************************************************
	[概要]
	正規化

	[戻り値]
	const Quaternion 正規化後の値
	***************************************************************************/
	const Quaternion Normalize();

	/***************************************************************************
	[概要]
	クォータニオンの値を設定

	[引数]
	float x, y, z　回転軸
	flaot w 　　　  回転値

	[戻り値]
	void
	***************************************************************************/
	void SetQuaternion(float x, float y, float z, float w);

public:

	/***************************************************************************
	[概要]
	度数をラジアン値に変換し、回転軸と回転角度からクォータニオンを生成

	[引数]
	DirectX::XMFLOAT3 Axis 　回転軸
	float             angle  回転値（度数

	[戻り値]
	Quaternion　クォータニオン
	***************************************************************************/
	static Quaternion AngleAxis(DirectX::XMFLOAT3 Axis, float angle);

	/***************************************************************************
	[概要]
	回転軸と回転角度からクォータニオンを生成
	
	[引数]
	DirectX::XMFLOAT3 Axis 　回転軸
	float             angle  回転値（ラジアン
	
	[戻り値]
	Quaternion　クォータニオン
	***************************************************************************/
	static Quaternion RadianAxis(DirectX::XMFLOAT3 Axis, float radian);

public:
	DirectX::XMVECTOR v;
	float m_CorrectionValue = 80.0f;   // 回転の補正値

};