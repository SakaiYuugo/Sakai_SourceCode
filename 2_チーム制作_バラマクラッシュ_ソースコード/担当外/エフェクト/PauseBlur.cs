using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseBlur : MonoBehaviour
{
	[SerializeField] Material pauseMaterial;
	float blurForce = 0f;
	float speed;

	enum State
	{
		NONE,
		IN,
		OUT
	};

	State eState;

	
	public void BlurOut(float speed)
	{
		eState = State.OUT;
		this.speed = speed;
		StartCoroutine(Blur());
	}


	public void BlurIn(float speed)
	{
		eState = State.IN;
		this.speed = speed;
		StartCoroutine(Blur());
	}



	private IEnumerator Blur()
	{
		switch (eState)
		{
		case State.IN:
		{
			while (true)
			{
				if (eState != State.IN) { break; }
				if (blurForce < 0f) { blurForce = 0f; break; }

				pauseMaterial.SetFloat("_Blur", blurForce);
				blurForce -= speed * Time.fixedDeltaTime;

				yield return new WaitForEndOfFrame();
			}
		}
		break;
		case State.OUT:
		{
			while (true)
			{
				if (eState != State.OUT) { break; }
				if (1f < blurForce) { blurForce = 1f; break; }

				pauseMaterial.SetFloat("_Blur", blurForce);
				blurForce += speed * Time.fixedDeltaTime;

				yield return new WaitForEndOfFrame();
			}
		}
		break;
		}
	}
	


	//デバッグ再生終了時に初期化
	private void OnDestroy()
	{
		pauseMaterial.SetFloat("_Blur", 0f);
	}
}
