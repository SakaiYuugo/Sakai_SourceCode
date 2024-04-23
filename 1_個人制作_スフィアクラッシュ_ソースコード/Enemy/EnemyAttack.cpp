// �I�u�W�F�N�g�֌W
#include "PlayerBall.h"
#include "EnemyAttack.h"
#include "EnemyMove.h"
// �V�X�e���֌W
#include "Scene.h"
#include "Manager.h"
// �v�Z�֌W
#include "XMFLOAT_Calculation.h"
#include "Random.h"

/***************************************************************************
[�T�v]
�v���C���[�ɍU��

[����]
float              const& power�@�@������
DirectX::XMFLOAT3  const& ThisPos�@���g(�G)�̌��ݍ��W

[�߂�l]
DirectX::XMFLOAT3�@�����l
***************************************************************************/
DirectX::XMFLOAT3 EnemyAttack::EnemyAttackPlayer(float const& power, DirectX::XMFLOAT3 const& ThisPos)
{
	//----- �v���C���[�̈ʒu���擾
	Scene* scene = Manager::GetScene();
	PlayerBall* player = scene->GetGameObject<PlayerBall>();
	DirectX::XMFLOAT3 playerPos = player->GetPosition();

	//----- �v���C���[�Ɍ������x�N�g�������߂�
	DirectX::XMVECTOR vector = DirectX::XMVectorSubtract(
		DirectX::XMLoadFloat3(&playerPos), DirectX::XMLoadFloat3(&ThisPos));
	DirectX::XMFLOAT3 PlayerVector;
	DirectX::XMStoreFloat3(&PlayerVector, vector);

	//----- ���K��
	PlayerVector = VectorNormalize(PlayerVector);

	//----- �����l��Ԃ�
	return DirectX::XMFLOAT3(PlayerVector.x * power,
							 0.0f,
							 PlayerVector.z * power);
}

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
DirectX::XMFLOAT3 EnemyAttack::EnemyAttackObject(float const& power, int const& ObjNumber, DirectX::XMFLOAT3 const& ThisPos)
{
	Scene* scene = Manager::GetScene();
	std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();
	DirectX::XMFLOAT3 ObjPos = ballList[ObjNumber]->GetPosition();

	DirectX::XMVECTOR vector = DirectX::XMVectorSubtract(
		DirectX::XMLoadFloat3(&ObjPos), DirectX::XMLoadFloat3(&ThisPos)
	);

	DirectX::XMFLOAT3 ObjVector;
	DirectX::XMStoreFloat3(&ObjVector, vector);

	//----- ���K��
	ObjVector = VectorNormalize(ObjVector);

	//----- �����x��Ԃ�
	return DirectX::XMFLOAT3(ObjVector.x * power,
							 0.0f,
							 ObjVector.z * power);
}
