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
			//�G�l�~�[�����X�g�ɒǉ�
			if (!enemies.Contains(other.gameObject))
			{
				enemies.Add(other.gameObject);
			}

			//�^�[�Q�b�g���r�[�R���ɂ���
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
			//�G�l�~�[�����X�g����폜
			enemies.Remove(other.gameObject);

			//�^�[�Q�b�g���v���C���[�ɂ���
			if (other.gameObject.name.Contains("Assault"))
			{
				other.GetComponent<EnemyAssaultmove>()?.ReturnTarget();
			}
		}
	}


	private void OnCollisionStay(Collision collision)
	{
		//�r�[�R���ƐڐG���Ă����痣���
		if (collision.gameObject.name.Contains("Beacon"))
		{
			Vector3 collisionToThis = this.transform.position - collision.transform.position;
			collisionToThis.y = 0f;

			//XZ�x�N�g����������������K���ɕς��Ƃ�
			if (Mathf.Abs(collisionToThis.x) < 1f && Mathf.Abs(collisionToThis.z) < 1f)
			{
				collisionToThis.x = Random.Range(-1f, 1f);
				collisionToThis.z = Random.Range(-1f, 1f);
			}

			this.GetComponent<Rigidbody>().AddForce(collisionToThis * 100f);
		}
	}
}
