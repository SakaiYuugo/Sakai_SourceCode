using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BeaconBehaviour : MonoBehaviour
{
	List<GameObject> enemies = new List<GameObject>();
	Timer destroyTimer = new Timer();
	bool enemyCollision;



	private void Start()
	{
		enemyCollision = false;
		destroyTimer.Set(3f);
	}


	private void FixedUpdate()
	{
		if (enemyCollision)
		{
			if (destroyTimer.ScaledUpdate())
			{
				foreach (GameObject enemy in enemies)
				{
					if (enemy == null) continue;
					enemy?.GetComponent<EnemyAssaultmove>()?.ReturnTarget();
				}

				Destroy(this.gameObject);
			}
		}
	}


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			foreach (GameObject enemy in enemies)
			{
				if ((enemy != null) && enemy.name.Contains("Assault"))
				{
					enemyCollision = true;
				}
			}
		}
	}
	


	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			//エネミーをリストに追加
			if (!enemies.Contains(other.gameObject))
			{
				enemies.Add(other.gameObject);
			}

			//ターゲットをビーコンにする
			if (other.gameObject.name.Contains("Assault"))
			{
				other.gameObject.GetComponent<EnemyAssaultmove>().SetTarget(this.gameObject);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			//エネミーをリストから削除
			enemies.Remove(other.gameObject);

			//ターゲットをプレイヤーにする
			if (other.gameObject.name.Contains("Assault"))
			{
				other.GetComponent<EnemyAssaultmove>()?.ReturnTarget();
			}
		}
	}


	private void OnCollisionStay(Collision collision)
	{
		//ビーコンと接触していたら離れる
		if (collision.gameObject.name.Contains("Beacon"))
		{
			Vector3 collisionToThis = this.transform.position - collision.transform.position;
			collisionToThis.y = 0f;

			//XZベクトルが小さかったら適当に変えとく
			if (Mathf.Abs(collisionToThis.x) < 1f && Mathf.Abs(collisionToThis.z) < 1f)
			{
				collisionToThis.x = Random.Range(-1f, 1f);
				collisionToThis.z = Random.Range(-1f, 1f);
			}

			this.GetComponent<Rigidbody>().AddForce(collisionToThis * 100f);
		}
	}
}
