using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageEffectController : MonoBehaviour
{
	[SerializeField] Material damageMaterial;

	PlayerState playerState;
	float force = 0f;

	private void Start()
	{
		playerState = System_ObjectManager.playerObject.GetComponent<PlayerState>();
	}

	private void Update()
	{
		if (playerState.GSnHP == 0)
		{
			damageMaterial.SetFloat("_EffectForce", 0f);
		}
	}

	public void Damage()
	{
		damageMaterial.SetColor("_Color", Color.red);
		StartCoroutine(DamageCoroutine());
	}

	public void Heal()
	{
		damageMaterial.SetColor("_Color", Color.green);
		StartCoroutine(DamageCoroutine());
	}


	private IEnumerator DamageCoroutine()
	{
		force = 1f;

		while (true)
		{
			if (force < 0.3f) { break; }

			if (playerState.GSnHP <= 3)
			{
				if (force < 0.9 - (playerState.GSnHP * 0.1f))
				{
					damageMaterial.SetColor("_Color", Color.red); break;
				}
			}

			force -= 0.01f;
			damageMaterial.SetFloat("_EffectForce", force);

			yield return new WaitForEndOfFrame();
		}

	}


	//デバッグ再生終了時に初期化
	private void OnDestroy()
	{
		damageMaterial.SetColor("_Color", Color.red);
		damageMaterial.SetFloat("_EffectForce", 0.3f);
	}
}
