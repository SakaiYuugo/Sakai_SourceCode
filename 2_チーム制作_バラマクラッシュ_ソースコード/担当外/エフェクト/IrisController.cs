using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IrisController : MonoBehaviour
{
	[SerializeField] Material irisMaterial;
	float per = 0f;
	float speed;
	string nextScene;

	public bool isEnd{ get; private set; }

	enum State
	{
		NONE,
		IN,
		OUT
	};

	State eState;


	private void Awake()
	{
		System_ObjectManager.beforeSceneName = SceneManager.GetActiveScene().name;
		Time.timeScale = 1f;
	}


	private void Start()
	{
		isEnd = false;
		IrisIn(0.1f);
	}


	public void IrisOut(float speed, string nextScene)
	{
		isEnd = false;
		eState = State.OUT;
		this.speed = speed;
		this.nextScene = nextScene;

		StartCoroutine(Iris());
	}


	public void IrisIn(float speed)
	{
		isEnd = false;
		eState = State.IN;
		this.speed = speed;
		StartCoroutine(Iris());
	}



	private IEnumerator Iris()
	{
		switch (eState)
		{
		case State.IN:
		{
			while (true)
			{
				if (eState != State.IN) { break; }
				if (1f == per) { break; }

				per = Mathf.Clamp01(per + speed);
				irisMaterial.SetFloat("_Threshold", per);

				yield return null;
			}
		}
		break;
		case State.OUT:
		{
			while (true)
			{
				if (eState != State.OUT) { break; }
				if (per == 0f) { break; }

				per = Mathf.Clamp01(per - speed);
				irisMaterial.SetFloat("_Threshold", per);

				yield return null;
			}

			SceneManager.LoadScene(nextScene);
		}
		break;
		}

		isEnd = true;
	}



	//デバッグ再生終了時に初期化
	private void OnDestroy()
	{
		System_ObjectManager.beforeSceneName = SceneManager.GetActiveScene().name;
		irisMaterial.SetFloat("_Threshold", 1f);
	}
}
