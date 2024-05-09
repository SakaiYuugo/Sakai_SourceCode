//----- �I�u�W�F�N�g�֌W
#include "Field.h"
#include "PlayerBall.h"
//----- �G�t�F�N�g�֌W
#include "Effect_Collision.h"
#include "Effect_Charge.h"
//----- �J�����֌W
#include "Camera.h"
//----- �R���|�[�l���g�֌W
#include "Audio.h"
#include "ModelRenderer.h"
#include "Shader.h"
//----- �V�X�e���֌W
#include "Collision.h"
#include "Input.h"
#include "Manager.h"
#include "Renderer.h"
#include "Random.h"
#include "XMFLOAT_Calculation.h"


/***************************************************************************
[�T�v]
�v���C���[������

[�߂�l]
void
***************************************************************************/
void PlayerBall::Init()
{
	//----- �R���|�[�l���g��Ǎ�
	AddComponent<Shader>()->Load("shader/vertexLightingVS.cso", "shader/vertexLightingPS.cso");
	AddComponent<ModelRenderer>()->Load("asset/model/Ball/Ball_Player.obj");

	//----- �q�b�g�X�g�b�v�p�Ƀr�b�g��ݒ�
	SetObjectBit(TAG_BIT::Player);

	//----- SE����
	m_CollisionSE = AddComponent<Audio>();
	m_CollisionSE->Load("asset/audio/SE/SE_Collision.wav");

	m_ChargeSE = AddComponent<Audio>();
	m_ChargeSE->Load("asset/audio/SE/SE_Charge.wav");

	m_AttackSE = AddComponent<Audio>();
	m_AttackSE->Load("asset/audio/SE/SE_Attack.wav");

	//----- �����̉�]�l��ݒ�
	SetRotation(DirectX::XMFLOAT3(0.0f, DirectX::XMConvertToRadians(-90.0f), 0.0f));

	//----- �G�t�F�N�g�֌W�̏�����
	m_EffectPlay = false;
}

/***************************************************************************
[�T�v]
�v���C���[�X�V����

[�߂�l]
void
***************************************************************************/
void PlayerBall::Update() 
{ 	
	Scene* scene = Manager::GetScene();
	Camera* camera = scene->GetGameObject<Camera>();

	//----- �O�����x�N�g���擾
	m_Forward = GetForward();

	//----- �U�����A�U���������܂��̓X�^����Ԃ̏ꍇ�A�ړ��s�ɂ���
	if (!m_Attack && !m_Charge)
	{
		VelocitySetting();
	}

	//----- �U�����̏ꍇ�A�`���[�W�s�ɂ���
	if (Input::GetKeyPress('K') && !m_Attack)
	{
		Charge();
	}

	//----- �L�[�𗣂�����U��
	if (Input::GetKeyRelease('K'))
	{
		//----- �`���[�W��Ԃ�����
		m_Charge = false;
		//----- SE�A�G�t�F�N�g���~�߂�
		m_EffectPlay = false;
		Effect_Charge::SetEffectLoop(false);
		m_SoundPlay  = false;
		m_ChargeSE->Stop();

		Attack(m_ShootPower, camera->GetForward());
	}

	//----- �ړ�����
	Move();

	//----- �����蔻��
	if (m_Collision->ColliderWithPlayer() == true)
	{
		//----- SE�炷(�Փ�SE)
		m_CollisionSE->Play(false, 3.0f * VectorLength(m_Velocity));

		//----- �G�t�F�N�g��\��(�Փ˃G�t�F�N�g)
		Effect_Collision* effect_collision = scene->AddGameObject2D<Effect_Collision>(3);
		effect_collision->SetScale(XMFLOAT3_Assign(m_ShootPower + 3.0f));
		effect_collision->SetPosition(DirectX::XMFLOAT3(m_Position.x + m_Forward.x, 1.0f, m_Position.z + m_Forward.z));

		//----- �Փ˂�����A��]�������t�����ɂ���
		m_RotForce = -m_RotForce;

		//----- �q�b�g�X�g�b�v�J�E���g�p��������
		m_FrameCount = 0;
	}

	//----- ���x��臒l�ȉ��ɂȂ����ꍇ�A�U����Ԃ���������
	if (std::abs(m_Velocity.x) < m_Threshold && std::abs(m_Velocity.z) < m_Threshold && m_Attack)
	{
		//----- �����͂�������
		m_ShootPower = 0.0f;
		m_Attack = false;
	}

	//-----�@�����W�ȉ��̏ꍇ�A�폜 
	if (m_Position.y <= m_DeadPosY)
	{
		SetDestroy();
	}

	// ���݂̍��W��ۑ�
	m_PrevPos = m_Position;
}

/***************************************************************************
[�T�v]
�v���C���[�q�b�g�X�g�b�v�p�X�V����

[�߂�l]
void
***************************************************************************/
void PlayerBall::HitStopUpdate()
{
	m_FrameCount++;

	if (GetHitStopTime() < m_FrameCount)
	{
		GameObject::ResetHitStopBit(TAG_BIT::Player);
	}
}

/***************************************************************************
[�T�v]
�ړ����x��ݒ�

[�߂�l]
void
***************************************************************************/
void PlayerBall::VelocitySetting()
{
	if (Input::GetKeyPress('W'))
	{
		m_Accel += m_Accel + 0.001f;

		if (m_MaxAccel <= m_Accel)
		{
			m_Accel = m_MaxAccel;
		}

		m_Velocity += (m_Forward * m_MaxSpeed * m_Accel);
		m_RotForce = m_MaxRotForce * VectorLength(m_Velocity);
		m_Torque.x += DirectX::XMConvertToRadians(m_RotForce);
	}
	if (Input::GetKeyPress('S'))
	{
		m_Accel += m_Accel + 0.001f;
	
		if (m_MaxAccel <= m_Accel)
		{
			m_Accel = m_MaxAccel;
		}
	
		m_Velocity -= (m_Forward * m_MaxSpeed * m_Accel);
		m_RotForce = m_MaxRotForce * VectorLength(m_Velocity);
		m_Torque.x -= DirectX::XMConvertToRadians(m_RotForce);;
	}
}

/***************************************************************************
[�T�v]
�ړ�����

[�߂�l]
void
***************************************************************************/
void PlayerBall::Move()
{
	//----- �d��
	m_Velocity.y -= m_Gravity;

	//----- ��R
	m_Velocity.y -= m_Velocity.y * 0.01f;

	//----- �ړ�����
	m_Position += m_Velocity;

	//----- ��]
	m_Rotation += m_Torque;
	m_Rotation.x -= m_RotForce * VectorLength(m_Velocity);

	//----- ���C
	m_Velocity *= m_Friction;
	m_Accel *= m_Friction;
	m_RotForce *= m_Friction;

	//----- ��O����
	if (m_Collision->ColliderWithField(m_Position, m_Scale) == true)
	{
		if (m_Position.y < m_GroundHeight)
		{
			m_Position.y = m_GroundHeight;
			m_Velocity.y = 0.0f;
		}
	}
}

/***************************************************************************
[�T�v]
�U������

[�߂�l]
void
***************************************************************************/
void PlayerBall::Charge()
{
	Scene* scene = Manager::GetScene();

	//----- SE��炷(�`���[�WSE)
	if (!m_SoundPlay)
	{
		m_SoundPlay = true;
		m_ChargeSE->Play(true, 1.0f);
	}	
	
	//----- �G�t�F�N�g��\��(�`���[�W�G�t�F�N�g)
	if (!m_EffectPlay)
	{
		m_EffectPlay = true;
		m_Effect_Charge = scene->AddGameObject<Effect_Charge>(3);
		m_Effect_Charge->SetScale(XMFLOAT3_Assign(3.0f));
		m_Effect_Charge->SetPosition(m_Position);
		m_Effect_Charge->SetEffectLoop(true);
	}
	else
	{
		m_Effect_Charge->SetPosition(GetPosition());
	}

	//----- �`���[�W��Ԃɂ���
	m_Charge = true;   

	if (m_MaxShootPower >= m_ShootPower)
		m_Time += m_AdditionTime;

	//----- �`���[�W���A��]������
	m_RotForce = m_MaxRotForce * m_Time;
	m_Torque.x += DirectX::XMConvertToRadians(m_RotForce);

	//----- �����͂�ݒ� 
	m_ShootPower = m_MaxShootPower * m_Time;
}

/***************************************************************************
[�T�v]
�U������

[����]
float              const& power�@�@������
DirectX::XMFLOAT3  const& forward�@�U������

[�߂�l]
void
***************************************************************************/
void PlayerBall::Attack(float const& power, DirectX::XMFLOAT3 const& forward)
{
	//----- SE��炷(�U����)
	m_AttackSE->Play(false, m_ShootPower);

	m_Attack = true;   // �U����Ԃɂ���
	m_Time = 0.0f;
	m_Velocity = DirectX::XMFLOAT3(forward.x * power, 0, forward.z * power);
}