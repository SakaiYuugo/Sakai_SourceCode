// オブジェクト関係
#include "PlayerBall.h"
#include "EnemyMove.h"
// システム関係
#include "Scene.h"
#include "Manager.h"
// 計算関係
#include "XMFLOAT_Calculation.h"
#include "Random.h"


/***************************************************************************
[概要]
プレイヤーを追跡

[引数]
DirectX::XMFLOAT3 const& ThisPos　自身(敵)の現在座標

[戻り値]
DirectX::XMFLOAT3　プレイヤーに向かうベクトル
***************************************************************************/
DirectX::XMFLOAT3 EnemyMove::ChasePlayer(DirectX::XMFLOAT3 const& ThisPos)
{
	//----- プレイヤーの位置を取得
	Scene* scene = Manager::GetScene();
	DirectX::XMFLOAT3 playerPos = scene->GetGameObject<PlayerBall>()->GetPosition();

	//----- プレイヤーに向かうベクトルを求める
	DirectX::XMVECTOR vector = DirectX::XMVectorSubtract(DirectX::XMLoadFloat3(&playerPos), DirectX::XMLoadFloat3(&ThisPos));
	DirectX::XMFLOAT3 PlayerVector;
	DirectX::XMStoreFloat3(&PlayerVector, vector);

	//----- 正規化
	PlayerVector = VectorNormalize(PlayerVector);

	return PlayerVector;
}

/***************************************************************************
[概要]
プレイヤー以外のオブジェクトをを追跡

[引数]
DirectX::XMFLOAT3 ThisPos　自身(敵)の現在座標

[戻り値]
DirectX::XMFLOAT3　追跡対象に向かうベクトル
***************************************************************************/
DirectX::XMFLOAT3 EnemyMove::ChaseObject(DirectX::XMFLOAT3 const& ThisPos)
{
	//----- シーンに存在するプレイヤー以外のオブジェクトを取得
	Scene* scene = Manager::GetScene();
	std::vector<EnemyBalls*> ballList = scene->GetGameObjects<EnemyBalls>();

	//----- 値を初期化
	float NearLength = 100.0f;
	DirectX::XMFLOAT3 ObjectVector = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);

	//----- 周囲に存在するオブジェクトで、一番距離が近い物を計算
	for (int i = 0; i < ballList.size(); i++)
	{
		DirectX::XMFLOAT3 objectPos = ballList[i]->GetPosition();

		DirectX::XMFLOAT3 direction = objectPos - ThisPos;
		float length;
		DirectX::XMStoreFloat(&length, DirectX::XMVector3Length(DirectX::XMLoadFloat3(&direction)));

		if (length <= NearLength)
		{
			//----- 同じ座標の場合、何もしない
			if (length == 0.0f)
				continue;

			//----- 一番近いオブジェクトの距離を代入
			NearLength = length;

			//----- 一番近いオブジェクトに向かうベクトルを求める
			DirectX::XMVECTOR vector = DirectX::XMVectorSubtract(DirectX::XMLoadFloat3(&objectPos), DirectX::XMLoadFloat3(&ThisPos));
			DirectX::XMStoreFloat3(&ObjectVector, vector);	
		}
	}

	//----- 正規化
	ObjectVector = VectorNormalize(ObjectVector);

	return ObjectVector;
}