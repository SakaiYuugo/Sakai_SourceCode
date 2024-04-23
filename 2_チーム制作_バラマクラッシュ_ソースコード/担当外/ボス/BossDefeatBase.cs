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
		//ゲームクリアフラグ
		GameEventManager.bGameClear = true;

		//ボス撃破時カメラに切り替え
		cameraManager = System_ObjectManager.mainCameraManager;
		cameraManager.ChangeCamera("BossDefeatCamera");

		//プレイヤーにクリアを通知
        GameObject.Find("Player").GetComponent<PlayerState>().state.m_CLEAR = true;

		if (boss == null)
		{ boss = System_ObjectManager.bossObject; }

		if (player == null)
		{ player = System_ObjectManager.playerObject; }


		//色々と止めたり消したりする処理
		{ 
			//敵スポーン停止
            if(System_ObjectManager.bossObject.name == "Boss_Bee")
            {
                GameObject.Find("EnemySpawnPosition").GetComponent<BossEnemySpawn>().enabled = false;
            }
            else
            {
                this.GetComponent<BossEnemySpawn>().enabled = false;
            }
			
			//プレイヤー移動停止
			player.GetComponent<PlayerMove>().enabled = false;
			player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			player.transform.rotation = Quaternion.Euler(0f, player.transform.eulerAngles.y, 0f);
			
			//敵・アイテムを削除
			Destroy(GameObject.Find("Enemies"));
			Destroy(GameObject.Find("Items"));

			//ボムをすべて削除
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bomb"))
			{
				Destroy(obj);
			}
		}

		//爆発の間隔を設定
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
