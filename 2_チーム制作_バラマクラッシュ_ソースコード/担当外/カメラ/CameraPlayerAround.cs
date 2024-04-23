using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerAround : MonoBehaviour
{
	[SerializeField] float height;
	[SerializeField] float distance;
	[SerializeField] float defaultAngle;
	[SerializeField] float rotationSpeed;
	[SerializeField] float rotationSpeedAcceralation;
	[SerializeField] float maxRotationHorizontal;
	[SerializeField] float resetSpeed;

	GameObject player;

	float addRotationV;
	float addRotationH;
	int lastInputV;
	bool lastInputH;
	
	float nowCameraSpeed;



	private void Awake()
	{
		System_ObjectManager.mainCamera = this.gameObject;
		System_ObjectManager.mainCameraAudioSource = this.GetComponent<AudioSource>();
	}

	private void Start()
	{
		System_ObjectManager.mainCamera = this.gameObject;
		System_ObjectManager.mainCameraAudioSource = this.GetComponent<AudioSource>();

		player = System_ObjectManager.playerObject;
		addRotationV = 0f;
		addRotationH = 0f;

		nowCameraSpeed = 0f;
	}


	private void FixedUpdate()
	{
		Vector2 rightStick = InputOrder.GetRightStick();

		bool isStickMove = !(rightStick.x == 0f && rightStick.y == 0f);
		bool isAllowKeyDownVertical = InputOrder.LeftArrow_Key() || InputOrder.RightArrow_Key();
		bool isAllowKeyDownHorizontal = InputOrder.UpArrow_Key() || InputOrder.DownArrow_Key();


		if (isStickMove || isAllowKeyDownVertical || isAllowKeyDownHorizontal)
		{
			//カメラを徐々に速く
			nowCameraSpeed += rotationSpeedAcceralation;
			nowCameraSpeed = Mathf.Clamp(nowCameraSpeed, 0f, rotationSpeed);

			//カメラ移動を計算
			if (isStickMove)
			{
				addRotationV += rightStick.x * nowCameraSpeed;
				lastInputV = rightStick.x > 0 ? +1 : -1;

				addRotationH += -rightStick.y * nowCameraSpeed;
			}
			if (InputOrder.LeftArrow_Key()) { addRotationV -= nowCameraSpeed; lastInputV = -1; }
			if (InputOrder.RightArrow_Key()) { addRotationV += nowCameraSpeed; lastInputV = +1; }

			lastInputH = false;
			if (InputOrder.UpArrow_Key()) { addRotationH -= nowCameraSpeed; lastInputH = true; }
			if (InputOrder.DownArrow_Key()) { addRotationH += nowCameraSpeed; lastInputH = true; }
		}
		else
		{
			if (lastInputH)
			{
				//横カメラの滑りを消す
				nowCameraSpeed = 0f;
			}

			//カメラを徐々に遅く
			nowCameraSpeed -= rotationSpeedAcceralation;
			nowCameraSpeed = Mathf.Clamp(nowCameraSpeed, 0f, rotationSpeed);

			if (lastInputV == -1) { addRotationV -= nowCameraSpeed; }
			if (lastInputV == +1) { addRotationV += nowCameraSpeed; }
		}


		//-----------------------------------------
		// 追加回転の計算
		//-----------------------------------------
		//横回転を-180～180に補正
		if (addRotationV >= 180f)
		{
			addRotationV -= 360f;
		}
		if (addRotationV < -180f)
		{
			addRotationV += 360f;
		}


		//縦回転を上限に戻す
		if (addRotationH < -maxRotationHorizontal)
		{
			addRotationH = -maxRotationHorizontal;
		}
		else
		if (addRotationH > 0f)
		{
			addRotationH = 0f;
		}


		//---------------------------------
		// 移動 ＆ 回転
		//---------------------------------
		//ターゲットと座標を合わせる
		this.transform.position = player.transform.position;

		//回転リセット
		this.transform.rotation = Quaternion.identity;

		//視点を基準の角度に変更する (基準はちょっと上から見る)
		this.transform.rotation = Quaternion.Euler(defaultAngle, this.transform.eulerAngles.y, 0f);

		//横の追加回転
		this.transform.rotation = Quaternion.Euler(0f, addRotationV, 0f) * this.transform.rotation;

		//高さ (注視点) を変更する
		this.transform.Translate(0f, height, 0f);

		//ターゲットから一定距離離れる
		this.transform.position -= this.transform.forward * distance;

		//縦の追加回転
		this.transform.RotateAround(player.transform.position, this.transform.right, addRotationH);



		//---------------------------------
		// 地面との当たり判定
		//---------------------------------
		Vector3 playerToCamera = this.transform.position - player.transform.position;

		foreach (RaycastHit groundHit in Physics.RaycastAll(player.transform.position, playerToCamera, playerToCamera.magnitude))
		{
			if (groundHit.transform.tag == "Ground")
			{
				this.transform.position = groundHit.point + this.transform.forward * 0.1f;
				break;
			}
		}
	}
}