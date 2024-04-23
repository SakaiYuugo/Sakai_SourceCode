using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossNameBehaviour : MonoBehaviour
{
	[SerializeField] float time;

	Image image;
	Timer timer = new Timer();

	GameObject mainCamera;


	private void Start()
	{
		image = this.GetComponent<Image>();
		timer.Set(time);

		mainCamera = GameObject.Find("Main Camera");
	}

	private void Update()
	{
		timer.UnscaledUpdate();
		image.color = new Color(1f, 1f, 1f, Mathf.Clamp01(timer.remainingTime));

		if (timer.isEnd			||
			mainCamera == null	)
		{
			Destroy(this.gameObject);
		}
	}
}
