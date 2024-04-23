using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;



public class SelectExplain : MonoBehaviour
{
	TextMeshProUGUI decide;
	TextMeshProUGUI cancel;
	TextMeshProUGUI select;
	TextMeshProUGUI info;


	private void Start()
	{
		decide = this.transform.Find("Decide").GetComponent<TextMeshProUGUI>();
		cancel = this.transform.Find("Cancel").GetComponent<TextMeshProUGUI>();
		select = this.transform.Find("Select").GetComponent<TextMeshProUGUI>();
		info   = this.transform.Find("Info").  GetComponent<TextMeshProUGUI>();
	}


	private void Update()
	{
		if (Gamepad.current == null)
		{
			decide.text = "DECIDE ENTER";
			cancel.text = "CANCEL ESC";
			select.text = "SELECT ARROW";
			info.text   = "INFO   TAB";
		}
		else
		{
			decide.text = "DECIDE B";
			cancel.text = "CANCEL A";
			select.text = "SELECT LSTHICK";
			info.text   = "INFO   X";
		}
	}
}
