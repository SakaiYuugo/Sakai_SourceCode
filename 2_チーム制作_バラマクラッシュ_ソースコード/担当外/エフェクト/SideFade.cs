using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SideFade : MonoBehaviour
{
	[SerializeField] Material material;



	enum STATE
	{
		None,
		LeftIn,
		LeftOut,
		RightIn,
		RightOut
	}

	STATE state;
	float threshould;
	float speed;
	string nextScene;
	bool isEnd;

	private void Awake()
	{
		Time.timeScale = 1f;
	}


	private void Start()
	{
		state = STATE.None;
		isEnd = true;

		LeftFadeIn(0.01f);
	}

	IEnumerator Fade()
	{
		AsyncOperation async = null;

		if (state == STATE.LeftOut || state == STATE.RightOut)
		{
			async = SceneManager.LoadSceneAsync(nextScene);
			async.allowSceneActivation = false;
		}

		while (!isEnd)
		{
			switch (state)
			{
			case STATE.LeftIn:
			{
				threshould += speed;
				material.SetFloat("_Threshould", threshould);

				if (threshould > 1f)
				{
					material.SetFloat("_Threshould", 1f);
					isEnd = true;
					state = STATE.None;
				}
			}
			break;
			case STATE.LeftOut:
			{
				threshould += speed;
				material.SetFloat("_Threshould", threshould);

				if (threshould > 0f)
				{
					material.SetFloat("_Threshould", 0f);
					isEnd = true;
					state = STATE.None;
					InputOrder.inputEnable = true;
					async.allowSceneActivation = true;
				}
			}
			break;
			case STATE.RightIn:
			{
				threshould -= speed;
				material.SetFloat("_Threshould", threshould);

				if (threshould < -1f)
				{
					material.SetFloat("_Threshould", -1f);
					isEnd = true;
					state = STATE.None;
				}
			}
			break;
			case STATE.RightOut:
			{
				threshould -= speed;
				material.SetFloat("_Threshould", threshould);

				if (threshould < 0f)
				{
					material.SetFloat("_Threshould", 0f);
					isEnd = true;
					state = STATE.None;
					InputOrder.inputEnable = true;
					async.allowSceneActivation = true;
				}
			}
			break;
			}

			yield return null;
		}
	}

	private void OnDestroy()
	{
		System_ObjectManager.beforeSceneName = SceneManager.GetActiveScene().name;
		material.SetFloat("_Threshould", 1f);
	}


	public void LeftFadeIn(float speed)
	{
		if (!isEnd) return;

		isEnd = false;
		state = STATE.LeftIn;
		this.speed = speed;
		threshould = 0f;

		StartCoroutine(Fade());
	}

	public void LeftFadeOut(float speed, string nextScene)
	{
		if (!isEnd) return;

		isEnd = false;
		state = STATE.LeftOut;
		this.speed = speed;
		threshould = -1f;
		this.nextScene = nextScene;
		InputOrder.inputEnable = false;

		StartCoroutine(Fade());
	}

	public void RightFadeIn(float speed)
	{
		if (!isEnd) return;

		isEnd = false;
		state = STATE.RightIn;
		this.speed = speed;
		threshould = 0f;

		StartCoroutine(Fade());
	}

	public void RightFadeOut(float speed, string nextScene)
	{
		if (!isEnd) return;

		isEnd = false;
		state = STATE.RightOut;
		this.speed = speed;
		threshould = 1f;
		this.nextScene = nextScene;
		InputOrder.inputEnable = false;

		StartCoroutine(Fade());
	}


}
