using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class CameraBossStun : CameraBossBase
{
	[SerializeField] GameObject prefabBossStunUI;
	[SerializeField] float maxDistance;
	[SerializeField] float minDistance;

	//型宣言
	enum STATE
	{
		Before,
		Aproach,
		After
	}

	//参照データ
	static GameObject gameUI;
	GameObject bossStunUI;
	TextMeshProUGUI bossStunText;

	//内部データ
	STATE state;
	Vector3 bossToPlayer;
	float alpha;
	Timer lerpTimer = new Timer();
	Timer cameraTimer = new Timer();

	
	
	protected override void Start()
	{
		base.Start();

		if (gameUI == null)
		{
			gameUI = GameObject.Find("GameUI");
		}

		//ステート初期化
		state = STATE.Before;

		//プレイヤー座標をボスと同じ高さにする
		Vector3 playerPos = player.transform.position;
		playerPos.y = boss.transform.position.y;

		//ボスからプレイヤーへのベクトル
		bossToPlayer = playerPos - boss.transform.position;
		bossToPlayer = bossToPlayer.normalized;

		//ベクトルを上に45度回転
		bossToPlayer = Quaternion.Euler(45, 0, 0) * bossToPlayer;

		//ボスの方を向く
		this.transform.position = boss.transform.position + bossToPlayer * maxDistance;
		this.transform.rotation = Quaternion.LookRotation(-bossToPlayer);

		//カメラが移動するのにかかる時間
		lerpTimer.Set(0.5f);

		//カメラが移動し始めるまでの時間
		cameraTimer.Set(1.5f);

		//スローモーションにする
		Time.timeScale = 0.2f;

		//ボススタンUIを生成
		bossStunUI = Instantiate(prefabBossStunUI, gameUI.transform);
		bossStunText = bossStunUI.transform.Find("BossStunText").GetComponent<TextMeshProUGUI>();
	}


	private void Update()
	{
		switch (state)
		{
		case STATE.Before:
		{
			if (cameraTimer.UnscaledUpdate())
			{
				//ステート変更
				state = STATE.Aproach;
			}
		}
		break;
		case STATE.Aproach:
		{
			if (lerpTimer.UnscaledUpdate())
			{
				//カメラが切り替わるまでの時間
				cameraTimer.Set(1f);

				//ステート変更
				state = STATE.After;
			}

			bossStunText.color -= new Color(0f, 0f, 0f, 0.05f);
			this.transform.position = boss.transform.position + Vector3.Lerp(bossToPlayer * maxDistance, bossToPlayer * minDistance, lerpTimer.elapsedTime01);
		}
		break;
		case STATE.After:
		{
			bossStunText.color -= new Color(0f, 0f, 0f, 0.05f);

			if (cameraTimer.UnscaledUpdate() && bossStunText.color.a <= 0f)
			{
				//ボススタンUIを削除
				Destroy(bossStunUI);

				//時間のスピードを元に戻す
				Time.timeScale = 1f;

				//カメラをプレイヤーカメラに設定
				cameraManager.ChangeCamera("Player_CasualCamera");
			}
		}
		break;
		}
	}
}
