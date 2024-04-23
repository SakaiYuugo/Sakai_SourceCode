using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrewMove : MonoBehaviour
{
	//----- ���x -----
	[SerializeField, Header("�{���ŏ����x")] private float MinHeight = 1.0f;
	[SerializeField, Header("�{���ō����x")] private float MaxHeight = 10.0f;
	[SerializeField, Header("���x�̕�����")] private float HeightLane = 5.0f;
	private float height;

	//----- ���� -----
	[SerializeField, Header("�{���ŏ�����")] private int MinDistance = 1;
	[SerializeField, Header("�{���ő勗��")] private int MaxDistance = 10;
	[SerializeField, Header("�����̕�����")] private int DistanceLane = 5;
	private int Distance;

	//----- ��] -----
	[SerializeField, Header("�{���ŏ���]���x")] private float MinRotSpeed = 10.0f;
	[SerializeField, Header("�{���ő��]���x")] private float MaxRotSpeed = 90.0f;
	[SerializeField, Header("��]�̕�����")] private int RotLane = 5;
	private float Rot;
	Quaternion rotarion;

	//----- �΂�T�� -----
	[SerializeField, Header("�΂�T���͈̔�")] private float Range = 90.0f;
	[SerializeField, Header("�p���[�̕␳�l")] private float PowerValue = 0.1f;

	private GameObject Bomb;
	private GameObject Player;
	private float NowSpeed;

	
	void Start()
    {
		// �v���C���[�����݂��Ă��邩�ǂ���
		Player = GameObject.Find("Player");

		// ���ϒl���v�Z
		height   = (MaxHeight - MinHeight) / HeightLane;         // ���x
		Distance = (MaxDistance - MinDistance) / DistanceLane;   // ����
		Rot      = (MaxRotSpeed - MinRotSpeed) / RotLane;        // ��]
	}

	//---------------------------------------
	// �΂�T��
	//---------------------------------------
	public void Strew(GameObject Object)
	{
		// �X�e�[�^�X���擾
		NowSpeed     = Player.GetComponent<PlayerMove>().GSNowSpeed;// ���݂̃X�s�[�h
		Rigidbody rd = Object.GetComponent<Rigidbody>();            // ���e
		int PushCnt  = GetComponent<StrewState>().GetPushTime();    // �L�[�����b������Ă�����

		// ��]�̌v�Z
		float tempRot = (MinRotSpeed + (Rot * Random.Range(-90f, 90f)));
		Vector3 torque = new Vector3(tempRot, tempRot, tempRot);
		rd.AddTorque(torque, ForceMode.Impulse);

		// �����̌v�Z
		float temp = (MinDistance + (Distance * Random.Range(1, DistanceLane + 1)));
		if (NowSpeed < 1.0f) { NowSpeed += 1.0f; }  // �X�s�[�h��1�����̏ꍇ�␳
		NowSpeed *= PowerValue;    // �l��␳�l�Œ���

		Vector3 vector1 = transform.forward * temp;

		// �΂�T�����͈�
		float radius = Random.Range(-Range , Range);
		radius *= Mathf.PI / 180.0f;
		
		Vector3 vector2 = new Vector3(
					vector1.x * Mathf.Cos(radius) - vector1.z * Mathf.Sin(radius),
					vector1.y,
					vector1.x * Mathf.Sin(radius) + vector1.z * Mathf.Cos(radius));

		// ���x�x�N�g��
		rd.AddForce(NowSpeed * PushCnt * vector2, ForceMode.Impulse);

		// �u���I�ɗ͂�^����(���x�̌v�Z)
		float heightValue = MinHeight + (height * Random.Range(1, HeightLane + 1));
		rd.AddForce(new Vector3(0.0f, heightValue * PushCnt * NowSpeed, 0.0f), ForceMode.Impulse);
	}
}
