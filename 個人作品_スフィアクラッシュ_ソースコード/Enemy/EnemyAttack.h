#pragma once
#include "EnemyBalls.h"

class EnemyAttack : public GameObject
{
public:
	/***************************************************************************
	[�T�v]
	�v���C���[�ɍU��
	
	[����]
	float             power�@�@������
	DirectX::XMFLOAT3 ThisPos�@���g(�G)�̌��ݍ��W
	
	[�߂�l]
	DirectX::XMFLOAT3�@�����l
	***************************************************************************/
	DirectX::XMFLOAT3 EnemyAttackPlayer(float power, DirectX::XMFLOAT3 ThisPos);

	/***************************************************************************
	[�T�v]
	�v���C���[�ȊO�̃I�u�W�F�N�g�ɍU��
	
	[����]
	float             power�@�@�@������
	int               ObjNumber�@�I�u�W�F�N�g�̃��X�g�ԍ�
	DirectX::XMFLOAT3 ThisPos�@�@���g(�G)�̌��ݍ��W
	
	[�߂�l]
	DirectX::XMFLOAT3�@�����l
	***************************************************************************/
	DirectX::XMFLOAT3 EnemyAttackObject(float power, int ObjNumber, DirectX::XMFLOAT3 ThisPos);
};