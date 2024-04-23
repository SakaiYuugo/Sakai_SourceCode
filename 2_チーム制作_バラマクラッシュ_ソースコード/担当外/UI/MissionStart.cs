using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MissionStart : MonoBehaviour
{
	TextMeshProUGUI text;
	Timer timer = new Timer();

	


	private void Start()
	{
		text = this.GetComponent<TextMeshProUGUI>();

		timer.Set(2f);
	}


	private void Update()
	{
		timer.UnscaledUpdate();

		text.color = Color.white * Mathf.PingPong(timer.elapsedTime * 5f, 1f);

		if (timer.remainingTime < 0.5f)
		{
			if (text.color.a < 0.001f)
			{
				Destroy(this.gameObject);
			}
		}
	}
}
