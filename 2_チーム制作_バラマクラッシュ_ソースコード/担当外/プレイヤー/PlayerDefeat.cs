using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerDefeat : MonoBehaviour
{
	[SerializeField] VisualEffect effect;
	[SerializeField] GameObject prefabExplosion;
	[SerializeField] float explosionDelayTime;

	Timer timer = new Timer();

	
	enum STATE
	{
		ExplosionBefore,
		ExplosionAfter
	}
	STATE state;



	private void OnEnable()
	{
		effect.SetBool("Enable", true);

		state = STATE.ExplosionBefore;

		timer.Set(explosionDelayTime);

		Time.timeScale = 0f;
		System_ObjectManager.playerObject.GetComponent<PlayerMove>().enabled = false;
	}

	private void Update()
	{
		switch (state)
		{
		case STATE.ExplosionBefore:
		{
			timer.UnscaledUpdate();

			if (timer.isEnd)
			{
				effect.SetBool("Enable", false);
				Instantiate(prefabExplosion, this.transform.position, Quaternion.identity).GetComponentInChildren<VisualEffect>().SetVector4("Color", Color.white);
				timer.Set(0.1f);
				state = STATE.ExplosionAfter;
			}
		}
		break;
		case STATE.ExplosionAfter:
		{
			if (timer.UnscaledUpdate())
			{
				//プレイヤー削除
				this.transform.parent.gameObject.SetActive(false);

				//シーンの読み込み
				System_SceneChange.instance.SetNextScene("GameOverScene");
			}
		}
		break;
		}
	}
}
