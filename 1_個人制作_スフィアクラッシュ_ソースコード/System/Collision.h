#pragma once
#include "GameObject.h"

class Collision : public GameObject
{
public:
	/***************************************************************************
	[�T�v]
	�v���C���[�Ƃ̓����蔻�菈��

	[�߂�l]
	bool �Փ˔���
	***************************************************************************/
	bool ColliderWithPlayer();

	/***************************************************************************
	[�T�v]
	�G�l�~�[���m�̓����蔻�菈��
	
	[�߂�l]
	void
	***************************************************************************/
	void ColliderWithNumberBall();

	/**************************************************************************
	[�T�v]
	�Փˌv�Z

	DirectX::XMFLOAT3   &      normal�@          �Փ˂����I�u�W�F�N�g�̋���
	DirectX::XMFLOAT3�@ const& Vel_1 �@          ���x
	DirectX::XMFLOAT3�@ const& Vel_2�@           ���x
	bool                const& PlayerColllision�@�v���C���[�ɓ������Ă�����
	int                 const& ObjNumber_1�@     ���������I�u�W�F�N�g�̃��X�g�ԍ�
	int                 const& ObjNumber_2�@     ���������I�u�W�F�N�g�̃��X�g�ԍ�

	[�߂�l]
	void
	***************************************************************************/
	void CollisionMath(DirectX::XMFLOAT3& normal, DirectX::XMFLOAT3 const& Vel_1, DirectX::XMFLOAT3 const& Vel_2, bool const& PlayerCollision, int const& ObjNumber_1, int const& ObjNumber_2);

	/***************************************************************************
	[�T�v]
	�v�Z��̑��x���Z�b�g

	[����]
	DirectX::XMFLOAT3 const&�@Vel_1 �@          �Փˌv�Z��̑��x
	DirectX::XMFLOAT3 const&�@Vel_2�@           �Փˌv�Z��̑��x
	bool              const&  PlayerColllision�@�v���C���[�ɓ������Ă�����
	int               const&  ObjNumber_1�@     ���������I�u�W�F�N�g�̃��X�g�ԍ�
	int               const&  ObjNumber_2�@     ���������I�u�W�F�N�g�̃��X�g�ԍ�

	[�߂�l]
	void
	***************************************************************************/
	void SetResultVelocity(DirectX::XMFLOAT3 const& Vel_1, DirectX::XMFLOAT3 const& Vel_2, bool const& PlayerCollision, int const& ObjNumber_1, int const& ObjNumber_2);

	/***************************************************************************
	[�T�v]
	���ꔻ��
	
	[�߂�l]
	bool �ڒn����
	***************************************************************************/
	bool ColliderWithField(DirectX::XMFLOAT3 const& position, DirectX::XMFLOAT3 const& scale);
};
