using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer_Test: MonoBehaviour
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
	int lastInputV;

	float addRotationH;
	bool lastInputH;

	bool isFrontReset;
	bool isBackReset;
	float nowCameraSpeed;



	private void Awake()
	{
		System_ObjectManager.mainCamera = this.gameObject;
		System_ObjectManager.mainCameraAudioSource = this.GetComponent<AudioSource>();
	}

	private void Start()
	{
		player = System_ObjectManager.playerObject;
		addRotationV = 0f;
		addRotationH = 0f;

		nowCameraSpeed = 0f;
	}


	private void FixedUpdate()
	{
		Vector2 rightStick = InputOrder.GetRightStick();

		bool isStickMove = !(rightStick.x is 0f & rightStick.y is 0f);
		bool isAllowKeyDownVertical = InputOrder.LeftArrow_Key() | InputOrder.RightArrow_Key();
		bool isAllowKeyDownHorizontal = InputOrder.UpArrow_Key() | InputOrder.DownArrow_Key();

		//�J���������Z�b�g���邩�`�F�b�N
		if (InputOrder.GetCameraResetButton())
		{
			isFrontReset = true;
		}
		if (InputOrder.S_Key())
		{
			isBackReset = true;
		}

		if (isStickMove || isAllowKeyDownVertical || isAllowKeyDownHorizontal)
		{
			//�J���������X�ɑ���
			nowCameraSpeed += rotationSpeedAcceralation;
			nowCameraSpeed = Mathf.Clamp(nowCameraSpeed, 0f, rotationSpeed);

			//�J�����ړ����v�Z
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

			//�J�������Z�b�g�𒆎~
			isFrontReset = false;
			isBackReset = false;
		}
		else
		{
			if (lastInputH)
			{
				//���J�����̊��������
				nowCameraSpeed = 0f;
			}

			//�J���������X�ɒx��
			nowCameraSpeed -= rotationSpeedAcceralation;
			nowCameraSpeed = Mathf.Clamp(nowCameraSpeed, 0f, rotationSpeed);

			if (lastInputV == -1) { addRotationV -= nowCameraSpeed; }
			if (lastInputV == +1) { addRotationV += nowCameraSpeed; }
		}


		//-----------------------------------------
		// �ǉ���]�̌v�Z
		//-----------------------------------------
		//����]��-180�`180�ɕ␳
		if (addRotationV >= 180f)
		{
			addRotationV -= 360f;
		}
		if (addRotationV < -180f)
		{
			addRotationV += 360f;
		}


		//�c��]������ɖ߂�
		if (addRotationH < -maxRotationHorizontal)
		{
			addRotationH = -maxRotationHorizontal;
		}
		else
		if (addRotationH > 0f)
		{
			addRotationH = 0f;
		}

		//�J���������Z�b�g�����Ԃ������珙�X�Ƀ��Z�b�g����
		if (isFrontReset)
		{
			float turnBackSpeed = resetSpeed;

			//�c�������߂�ꍇ�̓X�s�[�h����
			if (!addRotationV.Equals(0f) && !addRotationH.Equals(0f))
			{
				turnBackSpeed /= 2;
			}

			//��
			if (addRotationV < -resetSpeed) { addRotationV += turnBackSpeed; }
			else
			if (addRotationV > +resetSpeed) { addRotationV -= turnBackSpeed; }
			else
			{ addRotationV = 0f; }

			//�c
			if (addRotationH < -resetSpeed) { addRotationH += turnBackSpeed; }
			else
			if (addRotationH > +resetSpeed) { addRotationH -= turnBackSpeed; }
			else
			{ addRotationH = 0f; }

			//��ʒu�܂ōs������I��
			if (addRotationV == 0f && addRotationH == 0f) { isFrontReset = false; }
		}

		//�J���������Z�b�g�����Ԃ������珙�X�Ƀ��Z�b�g����
		if (isBackReset)
		{
			float turnBackSpeed = resetSpeed;

			//�c�������߂�ꍇ�̓X�s�[�h����
			if (!addRotationV.Equals(180f) && !addRotationH.Equals(0f))
			{
				turnBackSpeed /= 2;
			}

			//��
			if (addRotationV >= 0f && addRotationV < +180 - resetSpeed) { addRotationV += turnBackSpeed; }
			else
			if (addRotationV < 0f && addRotationV > -180 + resetSpeed) { addRotationV -= turnBackSpeed; }
			else
			{ addRotationV = 180f; }

			//�c
			if (addRotationH < -resetSpeed) { addRotationH += turnBackSpeed; }
			else
			if (addRotationH > +resetSpeed) { addRotationH -= turnBackSpeed; }
			else
			{ addRotationH = 0f; }

			//��ʒu�܂ōs������I��
			if (addRotationV == 180f && addRotationH == 0f) { isBackReset = false; }
		}



		//---------------------------------
		// �ړ� �� ��]
		//---------------------------------
		//�^�[�Q�b�g�ƍ��W�����킹��
		this.transform.position = player.transform.position;

		//�v���C���[�̉�]�ɍ��킹��
		this.transform.rotation = Quaternion.Euler(player.transform.eulerAngles.x, player.transform.eulerAngles.y, 0f);
		this.transform.RotateAround(this.transform.position, Vector3.up, 180f);

		//���_����̊p�x�ɕύX���� (��͂�����Əォ�猩��)
		this.transform.rotation = Quaternion.Euler(defaultAngle, this.transform.eulerAngles.y, 0f);

		//���̒ǉ���]
		this.transform.rotation = Quaternion.Euler(0f, addRotationV, 0f) * this.transform.rotation;

		//���� (�����_) ��ύX����
		this.transform.Translate(0f, height, 0f);

		//�^�[�Q�b�g�����苗�������
		this.transform.position -= this.transform.forward * distance;

		//�c�̒ǉ���]
		this.transform.RotateAround(player.transform.position, this.transform.right, addRotationH);



		//---------------------------------
		// �n�ʂƂ̓����蔻��
		//---------------------------------
		Vector3 playerToCamera = this.transform.position - player.transform.position;

		foreach (RaycastHit groundHit in Physics.RaycastAll(player.transform.position, playerToCamera, playerToCamera.magnitude))
		{
			if (groundHit.transform.tag is "Ground")
			{
				this.transform.position = groundHit.point + this.transform.forward * 0.1f;
				break;
			}

		}

		//---------------------------------
		// ��Q���Ƃ̓����蔻��
		//---------------------------------
		Physics.Raycast(this.transform.position, -playerToCamera, out RaycastHit objectHit);

		if (objectHit.transform.name != "Player" &&
			objectHit.collider != null &&
			Mathf.Abs(addRotationV) > 90f)
		{
			this.transform.position = objectHit.point + (-this.transform.forward * distance);
		}
	}
}