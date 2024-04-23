//----- �Q�[���I�u�W�F�N�g
#include "EnemyBalls.h"
#include "PlayerBall.h"
#include "Field.h"
//----- �G�t�F�N�g�֌W
#include "Effect_Collision.h"
//----- �R���|�[�l���g�֌W
#include "Audio.h"
#include "Scene.h"
//----- �V�X�e���֌W
#include "Collision.h"
#include "Manager.h"
#include "XMFLOAT_Calculation.h"
#include "Time.h"
#include "UI_Timer.h"

/***************************************************************************
[�T�v]
�v���C���[�Ƃ̓����蔻�菈��

[�߂�l]
bool �Փ˔���
***************************************************************************/
bool Collision::ColliderWithPlayer()
{
	//----- �V�[�����ɑ��݂���I�u�W�F�N�g���擾
	Scene* scene = Manager::GetScene();
	std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();
	PlayerBall* player = scene->GetGameObject<PlayerBall>();

	//----- �G�����݂��Ȃ��ꍇ�A�������Ȃ�
	if (ballList.size() == 0)
	{
		return false;
	}

	float direction;
	DirectX::XMFLOAT3 normal;

	for (int i = 0; i < ballList.size(); i++)
	{
		//----- ���݂̃t���[���̃I�u�W�F�N�g���m�̋������v�Z
		normal = player->GetPosition() - ballList[i]->GetPosition();
		DirectX::XMStoreFloat(&direction, DirectX::XMVector3Length(DirectX::XMLoadFloat3(&normal)));

		//----- �Փ˂��Ă��邩����
		if (direction <= (player->GetScale().x + ballList[i]->GetScale().x) * 1.2f)
		{
			//----- �q�b�g�X�g�b�v����I�u�W�F�N�g���r�b�g�Ŏw��
			GameObject::SetHitStopBit(TAG_BIT::Player);
			GameObject::SetHitStopBit(TAG_BIT::Enemy);
			//----- �q�b�g�X�g�b�v���鎞��(���x�̃x�N�g�� * �␳�l)
			float temp = VectorLength((player->GetVelocity()));
			if (temp > 0.4f)
			{
				SetHitStopTime(15);
			}

			//----- �v���C���[�̓����蔻��v�Z
			CollisionMath(normal, player->GetVelocity(), ballList[i]->GetVelocity(), true, i, 0);

			return true;
		}
	}

	return false;
}

/***************************************************************************
[�T�v]
�G�l�~�[���m�̓����蔻�菈��

[�߂�l]
void
***************************************************************************/
void Collision::ColliderWithNumberBall()
{
	//----- �V�[�����ɑ��݂���I�u�W�F�N�g���擾
	Scene* scene = Manager::GetScene();
	std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();

	float direction;
	DirectX::XMFLOAT3 normal;

	//----- �G���P�̂������Ȃ��ꍇ�A�������Ȃ�
	if (ballList.size() <= 1)
	{
		return;
	}

	for (int i = 0; i < ballList.size() - 1; i++)
	{
		for (int j = i + 1; j < ballList.size(); j++)
		{
			//----- ���݂̃t���[���̃I�u�W�F�N�g���m�̋������v�Z
			normal = ballList[i]->GetPosition() - ballList[j]->GetPosition();
			DirectX::XMStoreFloat(&direction, DirectX::XMVector3Length(DirectX::XMLoadFloat3(&normal)));

			if (direction <= ballList[i]->GetScale().x + ballList[j]->GetScale().x)
			{
				//----- �Փˌv�Z
				CollisionMath(normal, ballList[i]->GetVelocity(), ballList[j]->GetVelocity(), false, i, j);

				//----- SE�炷
				ballList[i]->CollisionSEPlay();

				//----- �G�t�F�N�g��\��
				Effect_Collision* effect = scene->AddGameObject2D<Effect_Collision>(3);
				effect->SetScale(XMFLOAT3_Assign(ballList[i]->GetShootPower() + 3.0f));
				effect->SetPosition(DirectX::XMFLOAT3(
					ballList[i]->GetPosition().x + ballList[i]->GetForward().x,
					1.0f,
					ballList[i]->GetPosition().z + ballList[i]->GetForward().z));
			}

		}
	}
}

/**************************************************************************
[�T�v]
�Փˌv�Z

[����]
DirectX::XMFLOAT3   &      normal�@          �Փ˂����I�u�W�F�N�g�̋���
DirectX::XMFLOAT3�@ const& Vel_1 �@          ���x
DirectX::XMFLOAT3�@ const& Vel_2�@           ���x
bool                const& PlayerColllision�@�v���C���[�ɓ������Ă�����
int                 const& ObjNumber_1�@     ���������I�u�W�F�N�g�̃��X�g�ԍ�
int                 const& ObjNumber_2�@     ���������I�u�W�F�N�g�̃��X�g�ԍ�

[�߂�l]
void
***************************************************************************/
void Collision::CollisionMath(DirectX::XMFLOAT3& normal, DirectX::XMFLOAT3 const& Vel_1, DirectX::XMFLOAT3 const& Vel_2, bool const& PlayerCollision, int const& ObjNumber_1, int const& ObjNumber_2)
{
	float Bounce = 1.0f;
	float Mass_1 = 1.0f;
	float Mass_2 = 1.0f;

	//----- ���K��
	normal = VectorNormalize(normal);

	//----- ���ς��v�Z
	float Vector_1 = VectorDot(normal, Vel_1);
	float Vector_2 = VectorDot(normal, Vel_2);

	DirectX::XMFLOAT3 vv1 = (normal * Vector_1);
	DirectX::XMFLOAT3 vv2 = (normal * Vector_2);

	DirectX::XMFLOAT3 vh1 = Vel_1 - vv1;
	DirectX::XMFLOAT3 vh2 = Vel_2 - vv2;

	float resultVelocity_1 = ((Mass_1 - Bounce * Mass_2) * Vector_1 + (1 + Bounce) *
		Mass_2 * Vector_2) / (Mass_1 + Mass_2);
	float resultVelocity_2 = ((Mass_2 - Bounce * Mass_1) * Vector_2 + (1 + Bounce) *
		Mass_1 * Vector_1) / (Mass_1 + Mass_2);

	DirectX::XMFLOAT3 Velocity_1 = (normal * resultVelocity_1 + vh1);
	DirectX::XMFLOAT3 Velocity_2 = (normal * resultVelocity_2 + vh2);

	//----- �����͂��Z�b�g
	SetResultVelocity(Velocity_1, Velocity_2, PlayerCollision, ObjNumber_1, ObjNumber_2);
}

/***************************************************************************
[�T�v]
�v�Z��̑��x���Z�b�g

[����]
DirectX::XMFLOAT3�@ const& Vel_1 �@          �Փˌv�Z��̑��x
DirectX::XMFLOAT3�@ const& Vel_2�@           �Փˌv�Z��̑��x
bool                const& PlayerColllision�@�v���C���[�ɓ������Ă�����
int                 const& ObjNumber_1�@     ���������I�u�W�F�N�g�̃��X�g�ԍ�
int                 const& ObjNumber_2�@     ���������I�u�W�F�N�g�̃��X�g�ԍ�

[�߂�l]
void
***************************************************************************/
void Collision::SetResultVelocity(DirectX::XMFLOAT3 const& Vel_1, DirectX::XMFLOAT3 const& Vel_2, bool const& PlayerCollision, int const& ObjNumber_1, int const& ObjNumber_2)
{
	//----- �V�[�����̃I�u�W�F�N�g���擾
	Scene* scene = Manager::GetScene();
	std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();
	PlayerBall* player = scene->GetGameObject<PlayerBall>();

	//----- �����͂��Z�b�g
	if (PlayerCollision)
	{
		player->SetVelocity(Vel_1 * 1.2f);
		ballList[ObjNumber_1]->SetVelocity(Vel_2 * 1.2f);
	}
	else
	{
		ballList[ObjNumber_1]->SetVelocity(Vel_1 * 1.2f);
		ballList[ObjNumber_2]->SetVelocity(Vel_2 * 1.2f);
	}
}

/***************************************************************************
[�T�v]
���ꔻ��

[�߂�l]
bool �ڒn����
***************************************************************************/
bool Collision::ColliderWithField(DirectX::XMFLOAT3 const& position, DirectX::XMFLOAT3 const& scale)
{
	float dx = Field::CenterPoint.x - position.x;
	float dz = Field::CenterPoint.z - position.z;
	float dr = Sqrt(dx * dx + dz * dz);

	if (Field::m_Radius <= dr)
	{
		return false;   // ��O
	}

	return true;   // ���
}