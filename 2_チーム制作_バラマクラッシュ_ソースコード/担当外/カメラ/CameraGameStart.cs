using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraGameStart : MonoBehaviour
{
	[SerializeField] GameObject bossName;
	[SerializeField] GameObject missionStart;
	[SerializeField] float moveTime;

	GameObject player;
	GameObject boss;
	Timer timer = new Timer();

	Vector3 playerLookPosition;
	Quaternion playerLookRotation;

	Vector3 bossLookPosition;
	Quaternion bossLookRotation;

	GameObject objBossName;


	enum STATE
	{
		LookPlayer,
		PlayerToBoss,
		LookBoss,
		BossToPlayer,
	}

	STATE nowState;



	private void Awake()
	{
		System_ObjectManager.mainCamera = this.gameObject;
		System_ObjectManager.mainCameraAudioSource = this.GetComponent<AudioSource>();
	}








	private void Start()
	{
		Time.timeScale = 1f;

		player = System_ObjectManager.playerObject;
		boss = System_ObjectManager.bossObject;

		Transform playerLook = this.transform.Find("PlayerLook");
		playerLookPosition = playerLook.transform.position;
		playerLookRotation = playerLook.transform.rotation;

		Transform bossLook = this.transform.Find("BossLook");
		bossLookPosition = bossLook.transform.position;
		bossLookRotation = bossLook.transform.rotation;

		this.transform.position = playerLookPosition;
		this.transform.rotation = playerLookRotation;

		nowState = STATE.LookPlayer;
	}


	private void Update()
	{
		Time.timeScale = 0f;

		if (InputOrder.Enter_Key())
		{
			Time.timeScale = 1f;
			Physics.autoSimulation = true;
			Instantiate(missionStart, GameObject.Find("GameUI").transform);
			System_ObjectManager.mainCameraManager.ChangeCamera("Player_CasualCamera", false);
		}

		switch (nowState)
		{
		case STATE.LookPlayer:
		{
			System_ObjectManager.playerObject.GetComponent<PlayerFanfare>().Fanfare();
		}
		break;
		case STATE.PlayerToBoss:
		{
			timer.UnscaledUpdate();
			this.transform.position = Vector3.Lerp(playerLookPosition, bossLookPosition, timer.elapsedTime01);
			this.transform.rotation = Quaternion.Slerp(playerLookRotation, bossLookRotation, timer.elapsedTime01);

			if (timer.isEnd)
			{
				objBossName = Instantiate(bossName, GameObject.Find("GameUI").transform);
				nowState = STATE.LookBoss;
			}
		}
		break;
		case STATE.LookBoss:
		{
			if (objBossName == null)
			{
				nowState = STATE.BossToPlayer; 
				timer.Set(moveTime);
			}
		}
		break;
		case STATE.BossToPlayer:
		{
			timer.UnscaledUpdate();
			this.transform.position = Vector3.Lerp(bossLookPosition, playerLookPosition, timer.elapsedTime01);
			this.transform.rotation = Quaternion.Slerp(bossLookRotation, playerLookRotation, timer.elapsedTime01);

			if (timer.isEnd)
			{
				Time.timeScale = 1f;
				Instantiate(missionStart, GameObject.Find("GameUI").transform);
				System_ObjectManager.mainCameraManager.ChangeCamera("Player_CasualCamera", true);
			}
		}
		break;
		}
	}


	


	public void EndPlayerMove() { nowState = STATE.PlayerToBoss; timer.Set(moveTime); }
}
