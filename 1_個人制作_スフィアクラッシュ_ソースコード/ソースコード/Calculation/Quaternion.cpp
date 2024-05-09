#include "Quaternion.h"
#include "XMFLOAT_Calculation.h"

/***************************************************************************
[概要]
デフォルトコンストラクタ
***************************************************************************/
Quaternion::Quaternion()
	:Quaternion(0.0f, 0.0f, 0.0f, 1.0f)
{
}

/***************************************************************************
[概要]
コンストラクタ

[引数]
float x, y, z　回転軸
flaot w 　　　  回転値
***************************************************************************/
Quaternion::Quaternion(float x, float y, float z, float w)
{
	DirectX::XMFLOAT4 f = DirectX::XMFLOAT4{ x, y, z, w };
	v = DirectX::XMLoadFloat4(&f);
}

/***************************************************************************
[概要]
デストラクタ
***************************************************************************/
Quaternion::~Quaternion()
{
}

/***************************************************************************
[概要]
演算子のオーバーロード クォータニオン同士の積の計算
***************************************************************************/
Quaternion Quaternion::operator*(Quaternion const& q)
{
	Quaternion r_q; // 戻り値
	r_q.v = DirectX::XMQuaternionMultiply(this->v, q.v); // 本来は後ろから掛けるが、今回は前から掛ける
	return r_q;
}
Quaternion Quaternion::operator*=(Quaternion const& q)
{
	this->v = DirectX::XMQuaternionMultiply(this->v, q.v);
	return *this;
}

/***************************************************************************
[概要]
単位ベクトルを軸に回転

[引数]
const DirectX::XMFLOAT3& vector　回転軸のベクトル

[戻り値]
const DirectX::XMFLOAT3& 回転の計算後の値
***************************************************************************/
const DirectX::XMFLOAT3& Quaternion::RotateVector(DirectX::XMFLOAT3 const& vector)
{
	Quaternion Q = (*this);
	Quaternion vq(vector.x, vector.y, vector.z, 1.0f);  // ベクトルをクォータニオンに変換

	DirectX::XMFLOAT4 inv_f;
	DirectX::XMStoreFloat4(&inv_f, Q.v);
	Quaternion inv_q(-inv_f.x, -inv_f.y, -inv_f.z, inv_f.w);  //　共役クォータニオン

	// 計算
	Quaternion mq;
	mq = inv_q * vq;
	mq *= Q;

	// 値を返還
	DirectX::XMFLOAT4 f4;
	DirectX::XMStoreFloat4(&f4, mq.v);

	return DirectX::XMFLOAT3(f4.x, f4.y, f4.z);
}

/***************************************************************************
[概要]
正規化

[戻り値]
const Quaternion 正規化後の値
***************************************************************************/
const Quaternion Quaternion::Normalize()
{
	Quaternion normalize;
	normalize.v = DirectX::XMQuaternionNormalize(this->v);
	return normalize;
}

/***************************************************************************
[概要]
クォータニオンの値を設定

[引数]
float x, y, z　回転軸
flaot w 　　　  回転値

[戻り値]
void
***************************************************************************/
void Quaternion::SetQuaternion(float x, float y, float z, float w)
{
	DirectX::XMFLOAT4 f = DirectX::XMFLOAT4(x, y, z, w);
	v = DirectX::XMLoadFloat4(&f);
}

/***************************************************************************
[概要]
度数をラジアン値に変換し、回転軸と回転角度からクォータニオンを生成

[引数]
DirectX::XMFLOAT3 Axis 　回転軸
float             angle  回転値（度数

[戻り値]
Quaternion　クォータニオン
***************************************************************************/
Quaternion Quaternion::AngleAxis(DirectX::XMFLOAT3 Axis, float angle)
{
	float radian = DirectX::XMConvertToRadians(angle);
	Quaternion q = RadianAxis(Axis, radian);
	return q;
}

/***************************************************************************
[概要]
回転軸と回転角度からクォータニオンを生成

[引数]
DirectX::XMFLOAT3 Axis 　回転軸
float             angle  回転値（ラジアン

[戻り値]
Quaternion　クォータニオン
***************************************************************************/
Quaternion Quaternion::RadianAxis(DirectX::XMFLOAT3 Axis, float radian)
{
	Quaternion q;
	DirectX::XMFLOAT3 normalize = VectorNormalize(Axis);
	DirectX::XMFLOAT4 f4 = DirectX::XMFLOAT4(normalize.x, normalize.y, normalize.z, 0.0f);
	DirectX::XMVECTOR axis_v = DirectX::XMLoadFloat4(&f4);

	q.v = DirectX::XMQuaternionRotationNormal(axis_v, radian);

	return q;
}