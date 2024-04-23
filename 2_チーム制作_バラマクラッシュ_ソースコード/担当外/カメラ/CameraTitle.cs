using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraTitle : MonoBehaviour
{
	[SerializeField] Vector3 titlePos;
	[SerializeField] Vector3 selectPos;
	[SerializeField] Vector3 gameStartPos;
	[SerializeField] float cameraSpeed;

	[SerializeField] GameObject leftDoor;
	[SerializeField] GameObject rightDoor;

	string nextScene;


	public enum CameraState
	{
		Title,
		Select,
		GameStart,
		GameBack
	};

	CameraState cameraState;
	bool stateChange;

	GameObject mainCamera;
	GameObject titleUI;
	GameObject selectUI;

	Timer timer = new Timer();


	private void Start()
	{
		Time.timeScale = 1f;

		cameraState = CameraState.Title;
		stateChange = false;

		mainCamera = GameObject.Find("Main Camera");
		titleUI = GameObject.Find("TitleUI");
		selectUI = GameObject.Find("Menu_StageSelect");

		if (System_ObjectManager.beforeSceneName == null)
		{
			selectUI.SetActive(false);
			mainCamera.transform.position = titlePos;
			leftDoor.transform.localPosition = Vector3.zero;
			rightDoor.transform.localPosition = Vector3.zero;
		}
		else
		{
			mainCamera.transform.position = gameStartPos;
			CameraChange(CameraState.GameBack);
		}
	}


	private void FixedUpdate()
	{
		if (stateChange)
		{
			switch (cameraState)
			{
			case CameraState.Title:
			{
				mainCamera.transform.Translate(0f, 0f, -cameraSpeed);

				if (Vector3.Distance(mainCamera.transform.position, titlePos) < cameraSpeed)
				{
					mainCamera.transform.position = titlePos;
					titleUI.SetActive(true);
					stateChange = false;
				}
			}
			break;
			case CameraState.Select:
			{
				mainCamera.transform.Translate(0f, 0f, cameraSpeed);

				if (Vector3.Distance(mainCamera.transform.position, selectPos) < cameraSpeed)
				{
					mainCamera.transform.position = selectPos;
					selectUI.SetActive(true);
					stateChange = false;
				}
			}
			break;
			case CameraState.GameStart:
			{
				mainCamera.transform.Translate(0f, 0f, cameraSpeed);

				if (Vector3.Distance(mainCamera.transform.position, gameStartPos) < 40f)
				{
					if (leftDoor.transform.localPosition.z < 0.003f)
					{
						leftDoor.transform.Translate(0f, 0f, 0.2f);
						rightDoor.transform.Translate(0f, 0f, -0.2f);
					}
					else
					{
						leftDoor.transform.localPosition = new Vector3(0f, 0f, 0.003f);
						rightDoor.transform.localPosition = new Vector3(0f, 0f, -0.003f);
					}
				}

				if (Vector3.Distance(mainCamera.transform.position, gameStartPos) < cameraSpeed)
				{
					mainCamera.transform.position = gameStartPos;
					stateChange = false;
					System_SceneChange.instance.SetNextScene(nextScene);
				}
			}
			break;
			case CameraState.GameBack:
			{
				mainCamera.transform.Translate(0f, 0f, -cameraSpeed);

				if (leftDoor.transform.localPosition.z > 0f)
				{
					leftDoor.transform.Translate(0f, 0f, -0.2f);
					rightDoor.transform.Translate(0f, 0f, 0.2f);
				}
				else
				{
					leftDoor.transform.localPosition = Vector3.zero;
					rightDoor.transform.localPosition = Vector3.zero;
				}

				if (Vector3.Distance(mainCamera.transform.position, selectPos) < cameraSpeed)
				{
					mainCamera.transform.position = selectPos;
					selectUI.SetActive(true);
					stateChange = false;
				}
			}
			break;
			}
		}
		
	}


	public void CameraChange(CameraState cameraState, string nextScene = "")
	{
		this.nextScene = nextScene;

		this.cameraState = cameraState;
		stateChange = true;

		titleUI.SetActive(false);
		selectUI.SetActive(false);

		timer.Set(cameraSpeed);
	}
}
