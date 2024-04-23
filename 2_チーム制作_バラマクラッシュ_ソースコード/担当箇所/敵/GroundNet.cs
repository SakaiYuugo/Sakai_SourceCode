using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundNet : MonoBehaviour
{
	[SerializeField, Header("SE")] AudioSource seClip;

	[SerializeField, Header("��������(�b)")] float TimeValue = 5.0f;
	[SerializeField, Header("�����l")]   �@�@float deceleration = 10.0f ;
	private float deltaTime;
	private PlayerMove Player;

	void Start()
    {
		Player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

	private void FixedUpdate()
	{
		// �I�u�W�F�N�g�����݂��Ȃ��ꍇ
		if (this.gameObject == null) { return; }

		deltaTime += Time.deltaTime;
		// ��莞�Ԍo�߂ō폜
		if (TimeValue < deltaTime) { Destroy(this.gameObject); }
	}

	/// <summary>
	/// �v���C���[�ƏՓ˂����ꍇ�A���x�����f�o�t
	/// </summary>
	private void OnCollisionEnter(Collision collision)
	{
		seClip.Play();
		Player.SlowDown(deceleration, TimeValue);
		Destroy(this.gameObject);
	}


}
