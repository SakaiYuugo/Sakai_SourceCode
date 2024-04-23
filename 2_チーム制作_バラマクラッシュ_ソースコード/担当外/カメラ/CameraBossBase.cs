using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraBossBase : MonoBehaviour
{
	protected GameObject player;
	protected GameObject boss;
	protected MainCameraManager cameraManager;


	private void Awake()
	{
		System_ObjectManager.mainCamera = this.gameObject;
		System_ObjectManager.mainCameraAudioSource = this.GetComponent<AudioSource>();
	}

	virtual protected void Start()
	{
		player = System_ObjectManager.playerObject;
		boss = System_ObjectManager.bossObject;
		cameraManager = System_ObjectManager.mainCameraManager;
	}
}
