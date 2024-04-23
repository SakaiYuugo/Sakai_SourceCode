using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class SelectTutorial : MonoBehaviour
{
	[SerializeField] Sprite LB;
	[SerializeField] Sprite Q;

	GameObject key;


	private void Start()
	{
		key = this.transform.GetChild(0).gameObject;
	}

	private void Update()
	{
		if (Gamepad.current == null)
		{ key.GetComponent<Image>().sprite = Q; }
		else
		{ key.GetComponent<Image>().sprite = LB; }
	}
}
