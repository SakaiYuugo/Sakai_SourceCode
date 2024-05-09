#pragma once
#include "EnemyBalls.h"

class EnemyMove : public GameObject
{
public:

	/***************************************************************************
	[�T�v]
	�v���C���[��ǐ�
	
	[����]
	DirectX::XMFLOAT3 ThisPos�@���g(�G)�̌��ݍ��W

	[�߂�l]
	DirectX::XMFLOAT3�@�v���C���[�Ɍ������x�N�g��
	***************************************************************************/
	DirectX::XMFLOAT3 ChasePlayer(DirectX::XMFLOAT3 const& ThisPos);

	/***************************************************************************
	[�T�v]
	�v���C���[�ȊO�̃I�u�W�F�N�g����ǐ�

	[����]
	DirectX::XMFLOAT3 ThisPos�@���g(�G)�̌��ݍ��W

	[�߂�l]
	DirectX::XMFLOAT3�@�ǐՑΏۂɌ������x�N�g��
	***************************************************************************/
	DirectX::XMFLOAT3 ChaseObject(DirectX::XMFLOAT3 const& ThisPos);
};