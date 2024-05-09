#pragma once
#include "EnemyBalls.h"

class EnemyAttack : public GameObject
{
public:
	/***************************************************************************
	[�T�v]
	�v���C���[�ɍU��
	
	[����]
	float              const& power�@�@������
	DirectX::XMFLOAT3  const& ThisPos�@���g(�G)�̌��ݍ��W
	
	[�߂�l]
	DirectX::XMFLOAT3�@�����l
	***************************************************************************/
	DirectX::XMFLOAT3 EnemyAttackPlayer(float const& power, DirectX::XMFLOAT3 const& ThisPos);

	/***************************************************************************
	[�T�v]
	�v���C���[�ȊO�̃I�u�W�F�N�g�ɍU��
	
	[����]
	float              const& power�@�@�@������
	int                const& ObjNumber�@�I�u�W�F�N�g�̃��X�g�ԍ�
	DirectX::XMFLOAT3  const& ThisPos�@�@���g(�G)�̌��ݍ��W
	
	[�߂�l]
	DirectX::XMFLOAT3�@�����l
	***************************************************************************/
	DirectX::XMFLOAT3 EnemyAttackObject(float const& power, int const& ObjNumber, DirectX::XMFLOAT3 const& ThisPos);
};