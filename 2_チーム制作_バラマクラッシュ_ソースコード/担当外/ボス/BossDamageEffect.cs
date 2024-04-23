using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossDamageEffect : MonoBehaviour
{
	[SerializeField] Material bossDamage;
	[SerializeField] float time;
	[SerializeField][Range(0f, 1f)] float maxAlpha;

	bool isDamage;
	float alpha;



	private void Start()
	{
		isDamage = false;
		alpha = 0f;
	}

	private void FixedUpdate()
	{
		if (isDamage)
		{
			if (alpha >= 0f)
			{
				alpha -= (Time.deltaTime / time) * maxAlpha;
			}
			else
			{
				alpha = 0f;
				isDamage = false;
			}

			bossDamage.SetFloat("_Alpha", alpha);
		}
	}
	

	public void Damage()
	{
		isDamage = true;
		alpha = maxAlpha;
	}

	private void OnDestroy()
	{
		bossDamage.SetFloat("_Alpha", 0f);
	}
}
