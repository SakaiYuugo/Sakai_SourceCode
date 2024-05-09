//----- オブジェクト関係
#include "Field.h"
#include "PlayerBall.h"
//----- エフェクト関係
#include "Effect_Collision.h"
#include "Effect_Charge.h"
//----- カメラ関係
#include "Camera.h"
//----- コンポーネント関係
#include "Audio.h"
#include "ModelRenderer.h"
#include "Shader.h"
//----- システム関係
#include "Collision.h"
#include "Input.h"
#include "Manager.h"
#include "Renderer.h"
#include "Random.h"
#include "XMFLOAT_Calculation.h"


/***************************************************************************
[概要]
プレイヤー初期化

[戻り値]
void
***************************************************************************/
void PlayerBall::Init()
{
	//----- コンポーネントを読込
	AddComponent<Shader>()->Load("shader/vertexLightingVS.cso", "shader/vertexLightingPS.cso");
	AddComponent<ModelRenderer>()->Load("asset/model/Ball/Ball_Player.obj");

	//----- ヒットストップ用にビットを設定
	SetObjectBit(TAG_BIT::Player);

	//----- SE準備
	m_CollisionSE = AddComponent<Audio>();
	m_CollisionSE->Load("asset/audio/SE/SE_Collision.wav");

	m_ChargeSE = AddComponent<Audio>();
	m_ChargeSE->Load("asset/audio/SE/SE_Charge.wav");

	m_AttackSE = AddComponent<Audio>();
	m_AttackSE->Load("asset/audio/SE/SE_Attack.wav");

	//----- 初期の回転値を設定
	SetRotation(DirectX::XMFLOAT3(0.0f, DirectX::XMConvertToRadians(-90.0f), 0.0f));

	//----- エフェクト関係の初期化
	m_EffectPlay = false;
}

/***************************************************************************
[概要]
プレイヤー更新処理

[戻り値]
void
***************************************************************************/
void PlayerBall::Update() 
{ 	
	Scene* scene = Manager::GetScene();
	Camera* camera = scene->GetGameObject<Camera>();

	//----- 前方向ベクトル取得
	m_Forward = GetForward();

	//----- 攻撃中、攻撃準備中またはスタン状態の場合、移動不可にする
	if (!m_Attack && !m_Charge)
	{
		VelocitySetting();
	}

	//----- 攻撃中の場合、チャージ不可にする
	if (Input::GetKeyPress('K') && !m_Attack)
	{
		Charge();
	}

	//----- キーを離したら攻撃
	if (Input::GetKeyRelease('K'))
	{
		//----- チャージ状態を解除
		m_Charge = false;
		//----- SE、エフェクトを止める
		m_EffectPlay = false;
		Effect_Charge::SetEffectLoop(false);
		m_SoundPlay  = false;
		m_ChargeSE->Stop();

		Attack(m_ShootPower, camera->GetForward());
	}

	//----- 移動処理
	Move();

	//----- 当たり判定
	if (m_Collision->ColliderWithPlayer() == true)
	{
		//----- SE鳴らす(衝突SE)
		m_CollisionSE->Play(false, 3.0f * VectorLength(m_Velocity));

		//----- エフェクトを表示(衝突エフェクト)
		Effect_Collision* effect_collision = scene->AddGameObject2D<Effect_Collision>(3);
		effect_collision->SetScale(XMFLOAT3_Assign(m_ShootPower + 3.0f));
		effect_collision->SetPosition(DirectX::XMFLOAT3(m_Position.x + m_Forward.x, 1.0f, m_Position.z + m_Forward.z));

		//----- 衝突したら、回転方向を逆向きにする
		m_RotForce = -m_RotForce;

		//----- ヒットストップカウント用を初期化
		m_FrameCount = 0;
	}

	//----- 速度が閾値以下になった場合、攻撃状態を解除する
	if (std::abs(m_Velocity.x) < m_Threshold && std::abs(m_Velocity.z) < m_Threshold && m_Attack)
	{
		//----- 加速力を初期化
		m_ShootPower = 0.0f;
		m_Attack = false;
	}

	//-----　一定座標以下の場合、削除 
	if (m_Position.y <= m_DeadPosY)
	{
		SetDestroy();
	}

	// 現在の座標を保存
	m_PrevPos = m_Position;
}

/***************************************************************************
[概要]
プレイヤーヒットストップ用更新処理

[戻り値]
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
[概要]
移動速度を設定

[戻り値]
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
[概要]
移動処理

[戻り値]
void
***************************************************************************/
void PlayerBall::Move()
{
	//----- 重力
	m_Velocity.y -= m_Gravity;

	//----- 抵抗
	m_Velocity.y -= m_Velocity.y * 0.01f;

	//----- 移動処理
	m_Position += m_Velocity;

	//----- 回転
	m_Rotation += m_Torque;
	m_Rotation.x -= m_RotForce * VectorLength(m_Velocity);

	//----- 摩擦
	m_Velocity *= m_Friction;
	m_Accel *= m_Friction;
	m_RotForce *= m_Friction;

	//----- 場外判定
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
[概要]
攻撃準備

[戻り値]
void
***************************************************************************/
void PlayerBall::Charge()
{
	Scene* scene = Manager::GetScene();

	//----- SEを鳴らす(チャージSE)
	if (!m_SoundPlay)
	{
		m_SoundPlay = true;
		m_ChargeSE->Play(true, 1.0f);
	}	
	
	//----- エフェクトを表示(チャージエフェクト)
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

	//----- チャージ状態にする
	m_Charge = true;   

	if (m_MaxShootPower >= m_ShootPower)
		m_Time += m_AdditionTime;

	//----- チャージ中、回転させる
	m_RotForce = m_MaxRotForce * m_Time;
	m_Torque.x += DirectX::XMConvertToRadians(m_RotForce);

	//----- 加速力を設定 
	m_ShootPower = m_MaxShootPower * m_Time;
}

/***************************************************************************
[概要]
攻撃処理

[引数]
float              const& power　　加速力
DirectX::XMFLOAT3  const& forward　攻撃方向

[戻り値]
void
***************************************************************************/
void PlayerBall::Attack(float const& power, DirectX::XMFLOAT3 const& forward)
{
	//----- SEを鳴らす(攻撃音)
	m_AttackSE->Play(false, m_ShootPower);

	m_Attack = true;   // 攻撃状態にする
	m_Time = 0.0f;
	m_Velocity = DirectX::XMFLOAT3(forward.x * power, 0, forward.z * power);
}