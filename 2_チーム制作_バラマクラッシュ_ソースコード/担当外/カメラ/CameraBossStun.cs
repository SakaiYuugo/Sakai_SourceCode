using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class CameraBossStun : CameraBossBase
{
	[SerializeField] GameObject prefabBossStunUI;
	[SerializeField] float maxDistance;
	[SerializeField] float minDistance;

	//�^�錾
	enum STATE
	{
		Before,
		Aproach,
		After
	}

	//�Q�ƃf�[�^
	static GameObject gameUI;
	GameObject bossStunUI;
	TextMeshProUGUI bossStunText;

	//�����f�[�^
	STATE state;
	Vector3 bossToPlayer;
	float alpha;
	Timer lerpTimer = new Timer();
	Timer cameraTimer = new Timer();

	
	
	protected override void Start()
	{
		base.Start();

		if (gameUI == null)
		{
			gameUI = GameObject.Find("GameUI");
		}

		//�X�e�[�g������
		state = STATE.Before;

		//�v���C���[���W���{�X�Ɠ��������ɂ���
		Vector3 playerPos = player.transform.position;
		playerPos.y = boss.transform.position.y;

		//�{�X����v���C���[�ւ̃x�N�g��
		bossToPlayer = playerPos - boss.transform.position;
		bossToPlayer = bossToPlayer.normalized;

		//�x�N�g�������45�x��]
		bossToPlayer = Quaternion.Euler(45, 0, 0) * bossToPlayer;

		//�{�X�̕�������
		this.transform.position = boss.transform.position + bossToPlayer * maxDistance;
		this.transform.rotation = Quaternion.LookRotation(-bossToPlayer);

		//�J�������ړ�����̂ɂ����鎞��
		lerpTimer.Set(0.5f);

		//�J�������ړ����n�߂�܂ł̎���
		cameraTimer.Set(1.5f);

		//�X���[���[�V�����ɂ���
		Time.timeScale = 0.2f;

		//�{�X�X�^��UI�𐶐�
		bossStunUI = Instantiate(prefabBossStunUI, gameUI.transform);
		bossStunText = bossStunUI.transform.Find("BossStunText").GetComponent<TextMeshProUGUI>();
	}


	private void Update()
	{
		switch (state)
		{
		case STATE.Before:
		{
			if (cameraTimer.UnscaledUpdate())
			{
				//�X�e�[�g�ύX
				state = STATE.Aproach;
			}
		}
		break;
		case STATE.Aproach:
		{
			if (lerpTimer.UnscaledUpdate())
			{
				//�J�������؂�ւ��܂ł̎���
				cameraTimer.Set(1f);

				//�X�e�[�g�ύX
				state = STATE.After;
			}

			bossStunText.color -= new Color(0f, 0f, 0f, 0.05f);
			this.transform.position = boss.transform.position + Vector3.Lerp(bossToPlayer * maxDistance, bossToPlayer * minDistance, lerpTimer.elapsedTime01);
		}
		break;
		case STATE.After:
		{
			bossStunText.color -= new Color(0f, 0f, 0f, 0.05f);

			if (cameraTimer.UnscaledUpdate() && bossStunText.color.a <= 0f)
			{
				//�{�X�X�^��UI���폜
				Destroy(bossStunUI);

				//���Ԃ̃X�s�[�h�����ɖ߂�
				Time.timeScale = 1f;

				//�J�������v���C���[�J�����ɐݒ�
				cameraManager.ChangeCamera("Player_CasualCamera");
			}
		}
		break;
		}
	}
}
