using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BossDefeatBase : MonoBehaviour
{
	protected MainCameraManager cameraManager;

	protected GameObject boss;
	protected GameObject player;
	
	protected Timer timer = new Timer();


	protected virtual void OnEnable()
	{
		//�Q�[���N���A�t���O
		GameEventManager.bGameClear = true;

		//�{�X���j���J�����ɐ؂�ւ�
		cameraManager = System_ObjectManager.mainCameraManager;
		cameraManager.ChangeCamera("BossDefeatCamera");

		//�v���C���[�ɃN���A��ʒm
        GameObject.Find("Player").GetComponent<PlayerState>().state.m_CLEAR = true;

		if (boss == null)
		{ boss = System_ObjectManager.bossObject; }

		if (player == null)
		{ player = System_ObjectManager.playerObject; }


		//�F�X�Ǝ~�߂���������肷�鏈��
		{ 
			//�G�X�|�[����~
            if(System_ObjectManager.bossObject.name == "Boss_Bee")
            {
                GameObject.Find("EnemySpawnPosition").GetComponent<BossEnemySpawn>().enabled = false;
            }
            else
            {
                this.GetComponent<BossEnemySpawn>().enabled = false;
            }
			
			//�v���C���[�ړ���~
			player.GetComponent<PlayerMove>().enabled = false;
			player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			player.transform.rotation = Quaternion.Euler(0f, player.transform.eulerAngles.y, 0f);
			
			//�G�E�A�C�e�����폜
			Destroy(GameObject.Find("Enemies"));
			Destroy(GameObject.Find("Items"));

			//�{�������ׂč폜
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bomb"))
			{
				Destroy(obj);
			}
		}

		//�����̊Ԋu��ݒ�
		timer.Set(0.5f);
	}



	protected virtual void FixedUpdate()
	{
		if (timer.ScaledUpdate())
		{
			float distance = 30f;
			Vector3 pos = boss.transform.position + new Vector3(Random.Range(-distance, distance), Random.Range(-distance, distance), Random.Range(-distance, distance));

			Color color = Color.white;
			switch (Random.Range(1,4))
			{
			case 1: { color = Color.red; }		break;
			case 2: { color = Color.green; }	break;
			case 3: { color = Color.cyan; }		break;
			case 4: { color = Color.yellow; }	break;
			}

			GameObject objExplosion = ExplosionCreater.Create(pos, Quaternion.identity);
			objExplosion.GetComponent<ExplosionBehaviour>().SetColor(color);
			objExplosion.transform.localScale *= 10f;
			
			timer.Reset();
		}
	}
}
