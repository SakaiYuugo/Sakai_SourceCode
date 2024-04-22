//----- オブジェクト関係
#include "EnemyBalls.h"
#include "EnemyAttack.h"
#include "EnemyMove.h"
#include "PlayerBall.h"
//----- コンポーネント関係
#include "Audio.h"
#include "ModelRenderer.h"
#include "Scene.h"
#include "Shader.h"
//----- システム関係
#include "Collision.h"
#include "Manager.h"
#include "Quaternion.h"
#include "Random.h"
#include "XMFLOAT_Calculation.h"
//----- エフェクト
#include "Effect_Charge.h"


//----- グルーバル変数
std::vector<std::string> g_NameList;

/***************************************************************************
[概要]
事前にモデルの名前を保存

[戻り値]
void
***************************************************************************/
void EnemyBalls::Load()
{
	//----- モデルの名前をリストに格納
	for (int i = 0; i < m_EnemyNum; i++)
	{
		std::string name = "asset/model/Ball/Ball_" + std::to_string(i + 1) + ".obj";
		g_NameList.push_back(name);
		ModelRenderer::Preload(g_NameList[i].c_str());
	}
}

/***************************************************************************
[概要]
敵の初期化

[戻り値]
void
***************************************************************************/
void EnemyBalls::Init()
{
	//----- モデル読み込み
	AddComponent<Shader>()->Load("shader/vertexLightingVS.cso", "shader/vertexLightingPS.cso");
	AddComponent<ModelRenderer>()->Load(g_NameList.front().c_str());
	//-----  先頭の要素を削除
	g_NameList.erase(g_NameList.begin());

	//----- ヒットストップ用にビットを設定
	SetObjectBit(TAG_BIT::Enemy);

	//----- クォータニオン適用
	m_UseQuaternion = true;

	//----- 値を初期化
	m_ChargeTime  = m_MaxChargeTime;  // 攻撃準備にかかる時間を設定
	m_AtkCoolTime = Random::GetFloat(1.0f, 3.0f);             // 攻撃のクールタイムをランダムな値で初期化
	m_SearchTime  = Random::GetFloat(1.0f, m_MaxSearchTime);  // 追尾するオブジェクト何フレームで変更するかのカウント
	m_Dead = false;
	m_PlayerChase = NULL;

	//----- SE準備
	m_CollisionSE = AddComponent<Audio>();
	m_CollisionSE->Load("asset/audio/SE/SE_Collision.wav");

	m_ChargeSE = AddComponent<Audio>();
	m_ChargeSE->Load("asset/audio/SE/SE_Charge.wav");

	m_AttackSE = AddComponent<Audio>();
	m_AttackSE->Load("asset/audio/SE/SE_Attack.wav");
}

/***************************************************************************
[概要]
敵の更新

[戻り値]
void
***************************************************************************/
void EnemyBalls::Update()
{
	Scene* scene = Manager::GetScene();
	PlayerBall* player = scene->GetGameObject<PlayerBall>();

	//----- プレイヤーが存在しない場合、何もしない
	if (player == nullptr) return;

	//----- 移動処理
	Move();

	//-----　一定座標以下の場合、削除 
	if (m_Position.y <= m_DeadPosY)
	{
		SetDestroy();
	}

	//----- オブジェクトが削除されていた場合、移動以外何もしない
	if (m_Dead) return;

	//----- 値を引いていく
	m_SearchTime -= m_AdditionTime;  // 追跡するオブジェクトを変更するカウント
	m_AtkCoolTime -= m_AdditionTime; // 攻撃のクールタイムを計るカウント

	//----- 一定時間経過後、攻撃対象を変更する
	if (m_SearchTime <= 0.0f)
	{
		m_OnceProcess = true;
		m_SearchTime = Random::GetFloat(1.0f, m_MaxSearchTime);
		SearchObject();
	}

	//----- 攻撃中、攻撃準備中は移動不可にする
	if (!m_Attack && !m_Charge)
	{
		VelocitySetting();
	}

	//----- クールタイムが０以下の場合
	if (!m_Attack && m_AtkCoolTime <= 0.0f)
	{
		Charge();
	}

	//----- チャージから一定時間経過で攻撃
	if (m_Charge)
	{
		Attack();
	}

	//----- 場外判定
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

	//----- 速度が閾値以下になった場合、攻撃状態を解除する
	if (std::abs(m_Velocity.x) < m_Threshold && std::abs(m_Velocity.z) < m_Threshold && m_Attack)
	{
		m_Attack = false;
		m_ShootPower = 0.0f;
	}
}

/***************************************************************************
[概要]
ヒットストップ用の敵のアップデート

[戻り値]
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
[概要]
移動処理

[戻り値]
void
***************************************************************************/
void EnemyBalls::Move()
{
	//----- 現在の座標を保存
	m_PrevPos = m_Position;

	//----- 重力
	m_Velocity.y -= m_Gravity;

	//----- 抵抗
	m_Position.y -= m_Position.y * m_Resistance;

	//----- 移動処理
	m_Position += m_Velocity;

	//----- 回転処理
	float length = VectorLength(m_Position - m_PrevPos);
	DirectX::XMFLOAT3 SideVec = Cross(DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f), m_Position - m_PrevPos);
	SideVec = VectorNormalize(SideVec);
	m_Quaternion = m_Quaternion * Quaternion::AngleAxis(SideVec, length * 50.0f);

	//----- 摩擦
	m_Velocity *= m_Friction;
	m_Accel *= m_Friction;
}

/***************************************************************************
[概要]
オブジェクトを索敵

[戻り値]
void
***************************************************************************/
void EnemyBalls::SearchObject()
{
	Scene* scene = Manager::GetScene();
	std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();
	bool PlayerAttack = false;

	//----- 自オブジェクトからプレイヤーとの距離を計算
	DirectX::XMFLOAT3 playerPos = scene->GetGameObject<PlayerBall>()->GetPosition();
	float PlayerLength = VectorLength(m_Position - playerPos);

	//----- プレイヤー以外のオブジェクトとの距離を計算
	int ObjNumber = 0;
	float ObjectLength = 100.0f;

	for (int i = 0; i < ballList.size(); i++)
	{
		DirectX::XMFLOAT3 objectPos = ballList[i]->GetPosition();
		float Length = VectorLength(objectPos - m_Position);

		if (Length <= ObjectLength)
		{
			// 同じ座標の場合、何もしない
			if (Length == 0.0f)
				continue;

			// 一番近いオブジェクトの距離を代入
			ObjectLength = Length;
		}
	}

	//----- 一番距離が近いオブジェクトを追跡
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
[概要]
移動速度を設定

[戻り値]
void
***************************************************************************/
void EnemyBalls::VelocitySetting()
{
	//----- 加速値を更新
	m_Accel = m_Accel + m_AccelForce;

	if (m_MaxAccel < m_Accel)
	{
		m_Accel = m_MaxAccel;
	}

	//----- ゲーム開始時から一定時間、中心に向かって移動
	if (m_OnceProcess == false)
	{
		OnceProcess();
	}
	//----- ターゲットに向かって移動
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
[概要]
攻撃準備

[戻り値]
void
***************************************************************************/
void EnemyBalls::Charge()
{
	Scene* scene = Manager::GetScene();

	//----- SEを鳴らす(チャージSE)
	if (!m_SoundPlay)
	{
		m_SoundPlay = true;
		m_ChargeSE->Play(true, 0.5f);
	}

	//----- エフェクトを表示(チャージエフェクト)
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

	//----- チャージ状態にする
	m_Charge = true;

	if (m_MaxShootPower >= m_ShootPower)
		m_Time += m_AdditionTime;

	//----- チャージ中、回転させる
	DirectX::XMFLOAT3 SideVec = Cross(DirectX::XMFLOAT3(0.0f, 1.0f, 0.0f), m_Position);
	m_Quaternion = Quaternion::AngleAxis(SideVec, m_MaxRotForce * m_Time * -m_Quaternion.m_CorrectionValue);

	//----- 加速力を設定 
	m_ShootPower = m_MaxShootPower * m_Time;
}

/***************************************************************************
[概要]
攻撃処理

[戻り値]
void
***************************************************************************/
void EnemyBalls::Attack()
{
	m_ChargeTime -= m_AdditionTime;

	if (m_ChargeTime <= 0.0f)
	{
		//----- 攻撃準備状態を解除し、攻撃状態にする
		m_Charge = false;
		m_Attack = true;

		//----- SE、エフェクトを止める
		m_SoundPlay = false;
		m_ChargeSE->Stop();
		m_EffectPlay = false;
		Effect_Charge::SetEffectLoop(false);

		//----- プレイヤーに攻撃
		if (m_PlayerChase)
		{	
			m_Velocity = m_EnemyAttack->EnemyAttackPlayer(m_ShootPower, m_Position);
		}
		//----- ランダムな敵に攻撃
		else
		{
			Scene* scene = Manager::GetScene();
			std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();
			m_Velocity = m_EnemyAttack->EnemyAttackObject(m_ShootPower, Random::GetInt(0, (int)ballList.size() - 1), m_Position);
		}

		//----- SEを鳴らす(攻撃音)
		m_AttackSE->Play(false, 0.3f);

		//----- 値を初期化
		m_AtkCoolTime = Random::GetFloat(1.0f, m_MaxAtkCoolTime);
		m_ChargeTime  = m_MaxChargeTime;
		m_Time        = 0.0f;
	}
}

/***************************************************************************
[概要]
ゲーム開始時に、一定時間行う処理

[戻り値]
void
***************************************************************************/
void EnemyBalls::OnceProcess()
{
	//----- 中心に向かうベクトルを求める
	DirectX::XMVECTOR vector = DirectX::XMVectorSubtract(
		DirectX::XMLoadFloat3(&m_OriginPoint), DirectX::XMLoadFloat3(&m_Position));
	DirectX::XMFLOAT3 Vector;
	DirectX::XMStoreFloat3(&Vector, vector);

	//----- 正規化
	Vector = VectorNormalize(Vector);
	//----- ステージの中心に向かって進む
	m_Velocity += (Vector * m_MaxSpeed * m_Accel);
}

/***************************************************************************
[概要]
SE(衝突音)を鳴らす

[戻り値]
void
***************************************************************************/
void EnemyBalls::CollisionSEPlay()
{
	m_CollisionSE->Play(false, 3.0f * VectorLength(m_Velocity));
}

