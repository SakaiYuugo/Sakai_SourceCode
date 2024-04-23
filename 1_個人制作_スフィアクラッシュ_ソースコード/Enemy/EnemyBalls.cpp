//----- �I�u�W�F�N�g�֌W
#include "EnemyBalls.h"
#include "EnemyAttack.h"
#include "EnemyMove.h"
#include "PlayerBall.h"
//----- �R���|�[�l���g�֌W
#include "Audio.h"
#include "ModelRenderer.h"
#include "Scene.h"
#include "Shader.h"
//----- �V�X�e���֌W
#include "Collision.h"
#include "Manager.h"
#include "Quaternion.h"
#include "Random.h"
#include "XMFLOAT_Calculation.h"
//----- �G�t�F�N�g
#include "Effect_Charge.h"


//----- �O���[�o���ϐ�
std::vector<std::string> g_NameList;

/***************************************************************************
[�T�v]
���O�Ƀ��f���̖��O��ۑ�

[�߂�l]
void
***************************************************************************/
void EnemyBalls::Load()
{
	//----- ���f���̖��O�����X�g�Ɋi�[
	for (int i = 0; i < m_EnemyNum; i++)
	{
		std::string name = "asset/model/Ball/Ball_" + std::to_string(i + 1) + ".obj";
		g_NameList.push_back(name);
		ModelRenderer::Preload(g_NameList[i].c_str());
	}
}

/***************************************************************************
[�T�v]
�G�̏�����

[�߂�l]
void
***************************************************************************/
void EnemyBalls::Init()
{
	//----- ���f���ǂݍ���
	AddComponent<Shader>()->Load("shader/vertexLightingVS.cso", "shader/vertexLightingPS.cso");
	AddComponent<ModelRenderer>()->Load(g_NameList.front().c_str());
	//-----  �擪�̗v�f���폜
	g_NameList.erase(g_NameList.begin());

	//----- �q�b�g�X�g�b�v�p�Ƀr�b�g��ݒ�
	SetObjectBit(TAG_BIT::Enemy);

	//----- �N�H�[�^�j�I���K�p
	m_UseQuaternion = true;

	//----- �l��������
	m_ChargeTime  = m_MaxChargeTime;  // �U�������ɂ����鎞�Ԃ�ݒ�
	m_AtkCoolTime = Random::GetFloat(1.0f, 3.0f);             // �U���̃N�[���^�C���������_���Ȓl�ŏ�����
	m_SearchTime  = Random::GetFloat(1.0f, m_MaxSearchTime);  // �ǔ�����I�u�W�F�N�g���t���[���ŕύX���邩�̃J�E���g
	m_Dead = false;
	m_PlayerChase = NULL;

	//----- SE����
	m_CollisionSE = AddComponent<Audio>();
	m_CollisionSE->Load("asset/audio/SE/SE_Collision.wav");

	m_ChargeSE = AddComponent<Audio>();
	m_ChargeSE->Load("asset/audio/SE/SE_Charge.wav");

	m_AttackSE = AddComponent<Audio>();
	m_AttackSE->Load("asset/audio/SE/SE_Attack.wav");
}

/***************************************************************************
[�T�v]
�G�̍X�V

[�߂�l]
void
***************************************************************************/
void EnemyBalls::Update()
{
	Scene* scene = Manager::GetScene();
	PlayerBall* player = scene->GetGameObject<PlayerBall>();

	//----- �v���C���[�����݂��Ȃ��ꍇ�A�������Ȃ�
	if (player == nullptr) return;

	//----- �ړ�����
	Move();

	//-----�@�����W�ȉ��̏ꍇ�A�폜 
	if (m_Position.y <= m_DeadPosY)
	{
		SetDestroy();
	}

	//----- �I�u�W�F�N�g���폜����Ă����ꍇ�A�ړ��ȊO�������Ȃ�
	if (m_Dead) return;

	//----- �l�������Ă���
	m_SearchTime -= m_AdditionTime;  // �ǐՂ���I�u�W�F�N�g��ύX����J�E���g
	m_AtkCoolTime -= m_AdditionTime; // �U���̃N�[���^�C�����v��J�E���g

	//----- ��莞�Ԍo�ߌ�A�U���Ώۂ�ύX����
	if (m_SearchTime <= 0.0f)
	{
		m_OnceProcess = true;
		m_SearchTime = Random::GetFloat(1.0f, m_MaxSearchTime);
		SearchObject();
	}

	//----- �U�����A�U���������͈ړ��s�ɂ���
	if (!m_Attack && !m_Charge)
	{
		VelocitySetting();
	}

	//----- �N�[���^�C�����O�ȉ��̏ꍇ
	if (!m_Attack && m_AtkCoolTime <= 0.0f)
	{
		Charge();
	}

	//----- �`���[�W�����莞�Ԍo�߂ōU��
	if (m_Charge)
	{
		Attack();
	}

	//----- ��O����
	if (m_Collision->ColliderWithField(m_Position, m_Scale) == true)
	{
		if (m_Position.y < m_GroundHeight)
		{
			m_Position.y = m_GroundHeight;
			m_Velocity.y = 0.0f;
		}
	}
	else if (m_Collision->ColliderWithField(m_Position, m_Scale) == false && m_Position.y <= -1.0f)
	{
		m_Dead = true;
	}

	//----- ���x��臒l�ȉ��ɂȂ����ꍇ�A�U����Ԃ���������
	if (std::abs(m_Velocity.x) < m_Threshold && std::abs(m_Velocity.z) < m_Threshold && m_Attack)
	{
		m_Attack = false;
		m_ShootPower = 0.0f;
	}
}

/***************************************************************************
[�T�v]
�q�b�g�X�g�b�v�p�̓G�̃A�b�v�f�[�g

[�߂�l]
void
***************************************************************************/
void EnemyBalls::HitStopUpdate()
{
	m_FrameCount++;

	if (GetHitStopTime() < m_FrameCount)
	{
		GameObject::ResetHitStopBit(TAG_BIT::Enemy);
	}
}

/***************************************************************************
[�T�v]
�ړ�����

[�߂�l]
void
***************************************************************************/
void EnemyBalls::Move()
{
	//----- ���݂̍��W��ۑ�
	m_PrevPos = m_Position;

	//----- �d��
	m_Velocity.y -= m_Gravity;

	//----- ��R
	m_Position.y -= m_Position.y * m_Resistance;

	//----- �ړ�����
	m_Position += m_Velocity;

	//----- ��]����
	float length = VectorLength(m_Position - m_PrevPos);
	DirectX::XMFLOAT3 SideVec = Cross(DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f), m_Position - m_PrevPos);
	SideVec = VectorNormalize(SideVec);
	m_Quaternion = m_Quaternion * Quaternion::AngleAxis(SideVec, length * 50.0f);

	//----- ���C
	m_Velocity *= m_Friction;
	m_Accel *= m_Friction;
}

/***************************************************************************
[�T�v]
�I�u�W�F�N�g�����G

[�߂�l]
void
***************************************************************************/
void EnemyBalls::SearchObject()
{
	Scene* scene = Manager::GetScene();
	std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();
	bool PlayerAttack = false;

	//----- ���I�u�W�F�N�g����v���C���[�Ƃ̋������v�Z
	DirectX::XMFLOAT3 playerPos = scene->GetGameObject<PlayerBall>()->GetPosition();
	float PlayerLength = VectorLength(m_Position - playerPos);

	//----- �v���C���[�ȊO�̃I�u�W�F�N�g�Ƃ̋������v�Z
	int ObjNumber = 0;
	float ObjectLength = 100.0f;

	for (int i = 0; i < ballList.size(); i++)
	{
		DirectX::XMFLOAT3 objectPos = ballList[i]->GetPosition();
		float Length = VectorLength(objectPos - m_Position);

		if (Length <= ObjectLength)
		{
			// �������W�̏ꍇ�A�������Ȃ�
			if (Length == 0.0f)
				continue;

			// ��ԋ߂��I�u�W�F�N�g�̋�������
			ObjectLength = Length;
		}
	}

	//----- ��ԋ������߂��I�u�W�F�N�g��ǐ�
	if (PlayerLength <= ObjectLength)
	{
		m_PlayerChase = true;
	}
	else
	{
		m_PlayerChase = false;
	}
	
}

/***************************************************************************
[�T�v]
�ړ����x��ݒ�

[�߂�l]
void
***************************************************************************/
void EnemyBalls::VelocitySetting()
{
	//----- �����l���X�V
	m_Accel = m_Accel + m_AccelForce;

	if (m_MaxAccel < m_Accel)
	{
		m_Accel = m_MaxAccel;
	}

	//----- �Q�[���J�n�������莞�ԁA���S�Ɍ������Ĉړ�
	if (m_OnceProcess == false)
	{
		OnceProcess();
	}
	//----- �^�[�Q�b�g�Ɍ������Ĉړ�
	else
	{
		if (m_PlayerChase == true)
		{
			m_Velocity += (m_EnemyMove->ChasePlayer(m_Position) * m_MaxSpeed * m_Accel);
		}
		else if (m_PlayerChase == false)
		{
			m_Velocity += (m_EnemyMove->ChaseObject(m_Position) * m_MaxSpeed * m_Accel);
		}
	}
}

/***************************************************************************
[�T�v]
�U������

[�߂�l]
void
***************************************************************************/
void EnemyBalls::Charge()
{
	Scene* scene = Manager::GetScene();

	//----- SE��炷(�`���[�WSE)
	if (!m_SoundPlay)
	{
		m_SoundPlay = true;
		m_ChargeSE->Play(true, 0.5f);
	}

	//----- �G�t�F�N�g��\��(�`���[�W�G�t�F�N�g)
	if (!m_EffectPlay)
	{
		m_EffectPlay = true;
		m_Effect_Charge = scene->AddGameObject<Effect_Charge>(3);
		m_Effect_Charge->SetScale(XMFLOAT3_Assign(3.0f));
		m_Effect_Charge->SetPosition(m_Position);
		Effect_Charge::SetEffectLoop(true);
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
	DirectX::XMFLOAT3 SideVec = Cross(DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f), m_Position);
	m_Quaternion = Quaternion::AngleAxis(SideVec, m_MaxRotForce * m_Time * -m_Quaternion.m_CorrectionValue);

	//----- �����͂�ݒ� 
	m_ShootPower = m_MaxShootPower * m_Time;
}

/***************************************************************************
[�T�v]
�U������

[�߂�l]
void
***************************************************************************/
void EnemyBalls::Attack()
{
	m_ChargeTime -= m_AdditionTime;

	if (m_ChargeTime <= 0.0f)
	{
		//----- �U��������Ԃ��������A�U����Ԃɂ���
		m_Charge = false;
		m_Attack = true;

		//----- SE�A�G�t�F�N�g���~�߂�
		m_SoundPlay = false;
		m_ChargeSE->Stop();
		m_EffectPlay = false;
		Effect_Charge::SetEffectLoop(false);

		//----- �v���C���[�ɍU��
		if (m_PlayerChase)
		{	
			m_Velocity = m_EnemyAttack->EnemyAttackPlayer(m_ShootPower, m_Position);
		}
		//----- �����_���ȓG�ɍU��
		else
		{
			Scene* scene = Manager::GetScene();
			std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();
			m_Velocity = m_EnemyAttack->EnemyAttackObject(m_ShootPower, Random::GetInt(0, (int)ballList.size() - 1), m_Position);
		}

		//----- SE��炷(�U����)
		m_AttackSE->Play(false, 0.3f);

		//----- �l��������
		m_AtkCoolTime = Random::GetFloat(1.0f, m_MaxAtkCoolTime);
		m_ChargeTime  = m_MaxChargeTime;
		m_Time        = 0.0f;
	}
}

/***************************************************************************
[�T�v]
�Q�[���J�n���ɁA��莞�ԍs������

[�߂�l]
void
***************************************************************************/
void EnemyBalls::OnceProcess()
{
	//----- ���S�Ɍ������x�N�g�������߂�
	DirectX::XMVECTOR vector = DirectX::XMVectorSubtract(
		DirectX::XMLoadFloat3(&m_OriginPoint), DirectX::XMLoadFloat3(&m_Position));
	DirectX::XMFLOAT3 Vector;
	DirectX::XMStoreFloat3(&Vector, vector);

	//----- ���K��
	Vector = VectorNormalize(Vector);
	//----- �X�e�[�W�̒��S�Ɍ������Đi��
	m_Velocity += (Vector * m_MaxSpeed * m_Accel);
}

/***************************************************************************
[�T�v]
SE(�Փˉ�)��炷

[�߂�l]
void
***************************************************************************/
void EnemyBalls::CollisionSEPlay()
{
	m_CollisionSE->Play(false, 3.0f * VectorLength(m_Velocity));
}

