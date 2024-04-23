using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossBarrier : MonoBehaviour
{
	[SerializeField] Material material;
	[SerializeField] float distance;

	public bool barrierEnable;
	float alpha;




	private void Start()
	{
		alpha = 0f;
		material.SetFloat("_Distance", distance);
	}


	private void FixedUpdate()
	{
		if (barrierEnable)
		{
			alpha = Mathf.Clamp(alpha + 0.01f, 0f, 0.5f);
		}
		else
		{
			alpha = Mathf.Clamp(alpha - 0.01f, 0f, 0.5f);
		}

		material.SetFloat("_Alpha", alpha);
	}


	private void OnDestroy()
	{
		material.SetFloat("_Distance", 0f);
		material.SetFloat("_Alpha", 0f);
	}
}
