using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class SelectEnding : MonoBehaviour
{
	[SerializeField] Sprite ending;
	[SerializeField] Sprite RB;
	[SerializeField] Sprite E;

	GameObject key;


	private void Start()
	{
		if (System_SaveManager.isEndingEnable)
		{
			this.GetComponent<Image>().sprite = ending;

			key = this.transform.GetChild(0).gameObject;
			key.SetActive(true);
		}
		else
		{
			Destroy(this);
		}
	}

	private void Update()
	{
		if (Gamepad.current == null)
		{ key.GetComponent<Image>().sprite = E; }
		else
		{ key.GetComponent<Image>().sprite = RB; }
	}
}
