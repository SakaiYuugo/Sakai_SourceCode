using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderNet : MonoBehaviour
{
	[SerializeField, Header("SE")] AudioSource seClip;

	[SerializeField, Header("��������(�b)")] float TimeValue = 5.0f;
	[SerializeField, Header("�����l")]       float deceleration = 10.0f;
	[SerializeField, Header("�n�ʂɒu����")] private GameObject NetObject;
	GameObject Net;
	private PlayerMove Player;

	void Start()
    {
		Player = GameObject.Find("Player").GetComponent<PlayerMove>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		// �v���C���[�ɓ��������ꍇ
		if (collision.gameObject.tag.Contains("Player"))
		{
			seClip.Play();
			Player.SlowDown(deceleration,TimeValue);
			Destroy(this.gameObject);
		}

		// �n�ʂɓ��������ꍇ
		if (collision.gameObject.tag.Contains("Ground"))
		{
			// �n�ʂɃN���̎��̃I�u�W�F�N�g��z�u
			Net = Instantiate(NetObject, this.transform.position, Quaternion.identity);
			Destroy(this.gameObject);
		}
	}

}
