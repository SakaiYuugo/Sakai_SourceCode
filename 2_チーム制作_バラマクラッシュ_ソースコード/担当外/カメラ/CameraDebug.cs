using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraDebug : MonoBehaviour
{
	[SerializeField] float moveSpeed = 0.1f;
	[SerializeField] float rotateSpeed = 1.0f;

	private void Start()
	{
		Time.timeScale = 1f;
	}


	void Update()
	{
		//ïœêîêÈåæ
		Vector3 move = Vector3.zero;
		Quaternion rotation = Quaternion.identity;


		//ÉJÉÅÉâê≥ñ 
		Vector3 cameraForward = Vector3.Scale(this.transform.forward, new Vector3(1, 0, 1));
		cameraForward = cameraForward.normalized;

		//ÉJÉÅÉââEë§
		Vector3 cameraSide = Vector3.Scale(this.transform.right, new Vector3(1, 0, 1));
		cameraSide = cameraSide.normalized;

		//à⁄ìÆ
		if (Input.GetKey(KeyCode.W)) { move += cameraForward; }
		if (Input.GetKey(KeyCode.A)) { move -= cameraSide; }
		if (Input.GetKey(KeyCode.S)) { move -= cameraForward; }
		if (Input.GetKey(KeyCode.D)) { move += cameraSide; }
		if (Input.GetKey(KeyCode.Space))	 { move.y += 1; }
		if (Input.GetKey(KeyCode.LeftShift)) { move.y -= 1; }
		this.transform.position += move * moveSpeed;


		//âÒì]
		if (Input.GetKey(KeyCode.UpArrow))		{ rotation *= Quaternion.AngleAxis(rotateSpeed, new Vector3(-1,  0, 0)); }
		if (Input.GetKey(KeyCode.DownArrow))	{ rotation *= Quaternion.AngleAxis(rotateSpeed, new Vector3( 1,  0, 0)); }
		if (Input.GetKey(KeyCode.LeftArrow))	{ rotation *= Quaternion.AngleAxis(rotateSpeed, new Vector3( 0, -1, 0)); }
		if (Input.GetKey(KeyCode.RightArrow))	{ rotation *= Quaternion.AngleAxis(rotateSpeed, new Vector3( 0,  1, 0)); }
		this.transform.rotation *= rotation;

		//Zé≤âÒì]Çå≈íË
		this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.transform.rotation.eulerAngles.y, 0);

		//éûä‘í‚é~
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			if (Time.timeScale == 1f) { Time.timeScale = 0f; }
			if (Time.timeScale == 0f) { Time.timeScale = 1f; }
		}
    }
}
