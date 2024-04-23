using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraPlayerToTarget : MonoBehaviour
{
	GameObject player;
	GameObject boss;
	[SerializeField] float distance = 10f;
	[SerializeField] float height = 5f;



	private void Awake()
	{
		System_ObjectManager.mainCamera = this.gameObject;
	}

	void Start()
    {
		player = System_ObjectManager.playerObject;
		Debug.Assert(player != null, "CameraPlayerToTarget.cs : Player�����݂��܂���");
		boss = System_ObjectManager.bossObject;
		Debug.Assert(boss != null, "CameraPlayerToTarget.cs : Boss�����݂��܂���");

		Vector3 playerPos = player.transform.position;	playerPos.y = 1f;
		Vector3 targetPos = boss.transform.position;	targetPos.y = 1f;

		//�ړ�
		Vector3 targetToPlayer = playerPos - targetPos;
		this.transform.position = playerPos;
		this.transform.position += targetToPlayer.normalized * distance;
		this.transform.position = new Vector3(this.transform.position.x, height, this.transform.position.z);

		//��]
		Vector3 playerToTarget = targetPos - playerPos;
		this.transform.rotation = Quaternion.LookRotation(playerToTarget);
	}
	

    void FixedUpdate()
    {
		if (boss == null) { return; }

		Vector3 playerPos = player.transform.position;
		playerPos.y = 1f;

		Vector3 targetPos = boss.transform.position;
		targetPos.y = 1f;


		//�ړ�
		Vector3 targetToPlayer = playerPos - targetPos;
		this.transform.position = playerPos;
		this.transform.position += targetToPlayer.normalized * distance;
		this.transform.position = new Vector3(this.transform.position.x, height, this.transform.position.z);

		//�ǂ���O�ɗ���
		Vector3 playerToThis = this.transform.position - player.transform.position;
		if (Physics.Raycast(playerPos, playerToThis, out RaycastHit hit, playerToThis.magnitude))
		{
			if (hit.collider.tag == "Ground")
			{
				this.transform.position = hit.point;
			}
		}


		//��]
		Vector3 playerToTarget = targetPos - playerPos;
		this.transform.rotation = Quaternion.LookRotation(playerToTarget);
    }
}
