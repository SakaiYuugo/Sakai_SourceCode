//----- ゲームオブジェクト
#include "EnemyBalls.h"
#include "PlayerBall.h"
#include "Field.h"
//----- エフェクト関係
#include "Effect_Collision.h"
//----- コンポーネント関係
#include "Audio.h"
#include "Scene.h"
//----- システム関係
#include "Collision.h"
#include "Manager.h"
#include "XMFLOAT_Calculation.h"
#include "Time.h"
#include "UI_Timer.h"

/***************************************************************************
[概要]
プレイヤーとの当たり判定処理

[戻り値]
bool 衝突判定
***************************************************************************/
bool Collision::ColliderWithPlayer()
{
	//----- シーン内に存在するオブジェクトを取得
	Scene* scene = Manager::GetScene();
	std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();
	PlayerBall* player = scene->GetGameObject<PlayerBall>();

	//----- 敵が存在しない場合、何もしない
	if (ballList.size() == 0)
	{
		return false;
	}

	float direction;
	DirectX::XMFLOAT3 normal;

	for (int i = 0; i < ballList.size(); i++)
	{
		//----- 現在のフレームのオブジェクト同士の距離を計算
		normal = player->GetPosition() - ballList[i]->GetPosition();
		DirectX::XMStoreFloat(&direction, DirectX::XMVector3Length(DirectX::XMLoadFloat3(&normal)));

		//----- 衝突しているか判定
		if (direction <= (player->GetScale().x + ballList[i]->GetScale().x) * 1.2f)
		{
			//----- ヒットストップするオブジェクトをビットで指定
			GameObject::SetHitStopBit(TAG_BIT::Player);
			GameObject::SetHitStopBit(TAG_BIT::Enemy);
			//----- ヒットストップする時間(速度のベクトル * 補正値)
			float temp = VectorLength((player->GetVelocity()));
			if (temp > 0.4f)
			{
				SetHitStopTime(15);
			}

			//----- プレイヤーの当たり判定計算
			CollisionMath(normal, player->GetVelocity(), ballList[i]->GetVelocity(), true, i, 0);

			return true;
		}
	}

	return false;
}

/***************************************************************************
[概要]
エネミー同士の当たり判定処理

[戻り値]
void
***************************************************************************/
void Collision::ColliderWithNumberBall()
{
	//----- シーン内に存在するオブジェクトを取得
	Scene* scene = Manager::GetScene();
	std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();

	float direction;
	DirectX::XMFLOAT3 normal;

	//----- 敵が１体しかいない場合、何もしない
	if (ballList.size() <= 1)
	{
		return;
	}

	for (int i = 0; i < ballList.size() - 1; i++)
	{
		for (int j = i + 1; j < ballList.size(); j++)
		{
			//----- 現在のフレームのオブジェクト同士の距離を計算
			normal = ballList[i]->GetPosition() - ballList[j]->GetPosition();
			DirectX::XMStoreFloat(&direction, DirectX::XMVector3Length(DirectX::XMLoadFloat3(&normal)));

			if (direction <= ballList[i]->GetScale().x + ballList[j]->GetScale().x)
			{
				//----- 衝突計算
				CollisionMath(normal, ballList[i]->GetVelocity(), ballList[j]->GetVelocity(), false, i, j);

				//----- SE鳴らす
				ballList[i]->CollisionSEPlay();

				//----- エフェクトを表示
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
[概要]
衝突計算

[引数]
DirectX::XMFLOAT3   &      normal　          衝突したオブジェクトの距離
DirectX::XMFLOAT3　 const& Vel_1 　          速度
DirectX::XMFLOAT3　 const& Vel_2　           速度
bool                const& PlayerColllision　プレイヤーに当たっていたか
int                 const& ObjNumber_1　     当たったオブジェクトのリスト番号
int                 const& ObjNumber_2　     当たったオブジェクトのリスト番号

[戻り値]
void
***************************************************************************/
void Collision::CollisionMath(DirectX::XMFLOAT3& normal, DirectX::XMFLOAT3 const& Vel_1, DirectX::XMFLOAT3 const& Vel_2, bool const& PlayerCollision, int const& ObjNumber_1, int const& ObjNumber_2)
{
	float Bounce = 1.0f;
	float Mass_1 = 1.0f;
	float Mass_2 = 1.0f;

	//----- 正規化
	normal = VectorNormalize(normal);

	//----- 内積を計算
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

	//----- 加速力をセット
	SetResultVelocity(Velocity_1, Velocity_2, PlayerCollision, ObjNumber_1, ObjNumber_2);
}

/***************************************************************************
[概要]
計算後の速度をセット

[引数]
DirectX::XMFLOAT3　 const& Vel_1 　          衝突計算後の速度
DirectX::XMFLOAT3　 const& Vel_2　           衝突計算後の速度
bool                const& PlayerColllision　プレイヤーに当たっていたか
int                 const& ObjNumber_1　     当たったオブジェクトのリスト番号
int                 const& ObjNumber_2　     当たったオブジェクトのリスト番号

[戻り値]
void
***************************************************************************/
void Collision::SetResultVelocity(DirectX::XMFLOAT3 const& Vel_1, DirectX::XMFLOAT3 const& Vel_2, bool const& PlayerCollision, int const& ObjNumber_1, int const& ObjNumber_2)
{
	//----- シーン内のオブジェクトを取得
	Scene* scene = Manager::GetScene();
	std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();
	PlayerBall* player = scene->GetGameObject<PlayerBall>();

	//----- 加速力をセット
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
[概要]
足場判定

[戻り値]
bool 接地判定
***************************************************************************/
bool Collision::ColliderWithField(DirectX::XMFLOAT3 const& position, DirectX::XMFLOAT3 const& scale)
{
	float dx = Field::CenterPoint.x - position.x;
	float dz = Field::CenterPoint.z - position.z;
	float dr = Sqrt(dx * dx + dz * dz);

	if (Field::m_Radius <= dr)
	{
		return false;   // 場外
	}

	return true;   // 場内
}