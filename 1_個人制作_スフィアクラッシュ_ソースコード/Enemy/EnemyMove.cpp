// �I�u�W�F�N�g�֌W
#include "PlayerBall.h"
#include "EnemyMove.h"
// �V�X�e���֌W
#include "Scene.h"
#include "Manager.h"
// �v�Z�֌W
#include "XMFLOAT_Calculation.h"
#include "Random.h"


/***************************************************************************
[�T�v]
�v���C���[��ǐ�

[����]
DirectX::XMFLOAT3 const& ThisPos�@���g(�G)�̌��ݍ��W

[�߂�l]
DirectX::XMFLOAT3�@�v���C���[�Ɍ������x�N�g��
***************************************************************************/
DirectX::XMFLOAT3 EnemyMove::ChasePlayer(DirectX::XMFLOAT3 const& ThisPos)
{
	//----- �v���C���[�̈ʒu���擾
	Scene* scene = Manager::GetScene();
	DirectX::XMFLOAT3 playerPos = scene->GetGameObject<PlayerBall>()->GetPosition();

	//----- �v���C���[�Ɍ������x�N�g�������߂�
	DirectX::XMVECTOR vector = DirectX::XMVectorSubtract(DirectX::XMLoadFloat3(&playerPos), DirectX::XMLoadFloat3(&ThisPos));
	DirectX::XMFLOAT3 PlayerVector;
	DirectX::XMStoreFloat3(&PlayerVector, vector);

	//----- ���K��
	PlayerVector = VectorNormalize(PlayerVector);

	return PlayerVector;
}

/***************************************************************************
[�T�v]
�v���C���[�ȊO�̃I�u�W�F�N�g����ǐ�

[����]
DirectX::XMFLOAT3 ThisPos�@���g(�G)�̌��ݍ��W

[�߂�l]
DirectX::XMFLOAT3�@�ǐՑΏۂɌ������x�N�g��
***************************************************************************/
DirectX::XMFLOAT3 EnemyMove::ChaseObject(DirectX::XMFLOAT3 const& ThisPos)
{
	//----- �V�[���ɑ��݂���v���C���[�ȊO�̃I�u�W�F�N�g���擾
	Scene* scene = Manager::GetScene();
	std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();

	//----- �l��������
	float NearLength = 100.0f;
	DirectX::XMFLOAT3 ObjectVector = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);

	//----- ���͂ɑ��݂���I�u�W�F�N�g�ŁA��ԋ������߂������v�Z
	for (int i = 0; i < ballList.size(); i++)
	{
		DirectX::XMFLOAT3 objectPos = ballList[i]->GetPosition();

		DirectX::XMFLOAT3 direction = objectPos - ThisPos;
		float length;
		DirectX::XMStoreFloat(&length, DirectX::XMVector3Length(DirectX::XMLoadFloat3(&direction)));

		if (length <= NearLength)
		{
			//----- �������W�̏ꍇ�A�������Ȃ�
			if (length == 0.0f)
				continue;

			//----- ��ԋ߂��I�u�W�F�N�g�̋�������
			NearLength = length;

			//----- ��ԋ߂��I�u�W�F�N�g�Ɍ������x�N�g�������߂�
			DirectX::XMVECTOR vector = DirectX::XMVectorSubtract(DirectX::XMLoadFloat3(&objectPos), DirectX::XMLoadFloat3(&ThisPos));
			DirectX::XMStoreFloat3(&ObjectVector, vector);	
		}
	}

	//----- ���K��
	ObjectVector = VectorNormalize(ObjectVector);

	return ObjectVector;
}