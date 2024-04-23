// オブジェクト関係
#include "PlayerBall.h"
#include "EnemyAttack.h"
#include "EnemyMove.h"
// システム関係
#include "Scene.h"
#include "Manager.h"
// 計算関係
#include "XMFLOAT_Calculation.h"
#include "Random.h"

/***************************************************************************
[概要]
プレイヤーに攻撃

[引数]
float              const& power　　加速力
DirectX::XMFLOAT3  const& ThisPos　自身(敵)の現在座標

[戻り値]
DirectX::XMFLOAT3　加速値
***************************************************************************/
DirectX::XMFLOAT3 EnemyAttack::EnemyAttackPlayer(float const& power, DirectX::XMFLOAT3 const& ThisPos)
{
	//----- プレイヤーの位置を取得
	Scene* scene = Manager::GetScene();
	PlayerBall* player = scene->GetGameObject<PlayerBall>();
	DirectX::XMFLOAT3 playerPos = player->GetPosition();

	//----- プレイヤーに向かうベクトルを求める
	DirectX::XMVECTOR vector = DirectX::XMVectorSubtract(
		DirectX::XMLoadFloat3(&playerPos), DirectX::XMLoadFloat3(&ThisPos));
	DirectX::XMFLOAT3 PlayerVector;
	DirectX::XMStoreFloat3(&PlayerVector, vector);

	//----- 正規化
	PlayerVector = VectorNormalize(PlayerVector);

	//----- 加速値を返す
	return DirectX::XMFLOAT3(PlayerVector.x * power,
							 0.0f,
							 PlayerVector.z * power);
}

/***************************************************************************
[概要]
プレイヤー以外のオブジェクトに攻撃

[引数]
float              const& power　　　加速力
int                const& ObjNumber　オブジェクトのリスト番号
DirectX::XMFLOAT3  const& ThisPos　　自身(敵)の現在座標

[戻り値]
DirectX::XMFLOAT3　加速値
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

	//----- 正規化
	ObjVector = VectorNormalize(ObjVector);

	//----- 加速度を返す
	return DirectX::XMFLOAT3(ObjVector.x * power,
							 0.0f,
							 ObjVector.z * power);
}
