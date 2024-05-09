#include "Quaternion.h"
#include "XMFLOAT_Calculation.h"

/***************************************************************************
[�T�v]
�f�t�H���g�R���X�g���N�^
***************************************************************************/
Quaternion::Quaternion()
	:Quaternion(0.0f, 0.0f, 0.0f, 1.0f)
{
}

/***************************************************************************
[�T�v]
�R���X�g���N�^

[����]
float x, y, z�@��]��
flaot w �@�@�@  ��]�l
***************************************************************************/
Quaternion::Quaternion(float x, float y, float z, float w)
{
	DirectX::XMFLOAT4 f = DirectX::XMFLOAT4{ x, y, z, w };
	v = DirectX::XMLoadFloat4(&f);
}

/***************************************************************************
[�T�v]
�f�X�g���N�^
***************************************************************************/
Quaternion::~Quaternion()
{
}

/***************************************************************************
[�T�v]
���Z�q�̃I�[�o�[���[�h �N�H�[�^�j�I�����m�̐ς̌v�Z
***************************************************************************/
Quaternion Quaternion::operator*(Quaternion const& q)
{
	Quaternion r_q; // �߂�l
	r_q.v = DirectX::XMQuaternionMultiply(this->v, q.v); // �{���͌�납��|���邪�A����͑O����|����
	return r_q;
}
Quaternion Quaternion::operator*=(Quaternion const& q)
{
	this->v = DirectX::XMQuaternionMultiply(this->v, q.v);
	return *this;
}

/***************************************************************************
[�T�v]
�P�ʃx�N�g�������ɉ�]

[����]
const DirectX::XMFLOAT3& vector�@��]���̃x�N�g��

[�߂�l]
const DirectX::XMFLOAT3& ��]�̌v�Z��̒l
***************************************************************************/
const DirectX::XMFLOAT3& Quaternion::RotateVector(DirectX::XMFLOAT3 const& vector)
{
	Quaternion Q = (*this);
	Quaternion vq(vector.x, vector.y, vector.z, 1.0f);  // �x�N�g�����N�H�[�^�j�I���ɕϊ�

	DirectX::XMFLOAT4 inv_f;
	DirectX::XMStoreFloat4(&inv_f, Q.v);
	Quaternion inv_q(-inv_f.x, -inv_f.y, -inv_f.z, inv_f.w);  //�@�����N�H�[�^�j�I��

	// �v�Z
	Quaternion mq;
	mq = inv_q * vq;
	mq *= Q;

	// �l��Ԋ�
	DirectX::XMFLOAT4 f4;
	DirectX::XMStoreFloat4(&f4, mq.v);

	return DirectX::XMFLOAT3(f4.x, f4.y, f4.z);
}

/***************************************************************************
[�T�v]
���K��

[�߂�l]
const Quaternion ���K����̒l
***************************************************************************/
const Quaternion Quaternion::Normalize()
{
	Quaternion normalize;
	normalize.v = DirectX::XMQuaternionNormalize(this->v);
	return normalize;
}

/***************************************************************************
[�T�v]
�N�H�[�^�j�I���̒l��ݒ�

[����]
float x, y, z�@��]��
flaot w �@�@�@  ��]�l

[�߂�l]
void
***************************************************************************/
void Quaternion::SetQuaternion(float x, float y, float z, float w)
{
	DirectX::XMFLOAT4 f = DirectX::XMFLOAT4(x, y, z, w);
	v = DirectX::XMLoadFloat4(&f);
}

/***************************************************************************
[�T�v]
�x�������W�A���l�ɕϊ����A��]���Ɖ�]�p�x����N�H�[�^�j�I���𐶐�

[����]
DirectX::XMFLOAT3 Axis �@��]��
float             angle  ��]�l�i�x��

[�߂�l]
Quaternion�@�N�H�[�^�j�I��
***************************************************************************/
Quaternion Quaternion::AngleAxis(DirectX::XMFLOAT3 Axis, float angle)
{
	float radian = DirectX::XMConvertToRadians(angle);
	Quaternion q = RadianAxis(Axis, radian);
	return q;
}

/***************************************************************************
[�T�v]
��]���Ɖ�]�p�x����N�H�[�^�j�I���𐶐�

[����]
DirectX::XMFLOAT3 Axis �@��]��
float             angle  ��]�l�i���W�A��

[�߂�l]
Quaternion�@�N�H�[�^�j�I��
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