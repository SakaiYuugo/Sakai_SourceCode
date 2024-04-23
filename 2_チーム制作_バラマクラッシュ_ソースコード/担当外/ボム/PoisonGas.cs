using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;


public class PoisonGas : MonoBehaviour
{
	[SerializeField] int damage;
	[SerializeField] float damageInterval;

	[HideInInspector] public float lifeTime;

	static GameObject boss;
	static BossHP hp;
	static BossDamageEffect damageEffect;
	VisualEffect effect;

	Timer lifeTimer = new Timer();
	Timer damageTimer = new Timer();


	private void Start()
	{
		if (boss == null)
		{
			boss = System_ObjectManager.bossObject;
		}

		effect = this.GetComponent<VisualEffect>();

		lifeTimer.Set(lifeTime);
		damageTimer.Set(damageInterval);
	}


	private void FixedUpdate()
	{
		lifeTimer.ScaledUpdate();
		damageTimer.ScaledUpdate();

		if (lifeTimer.remainingTime < 1.5f)
		{
			effect.Stop();
		}

		if (lifeTimer.isEnd)
		{
			Destroy(this.gameObject);
		}
	}


	private void OnTriggerStay(Collider other)
	{
		//��莞�Ԃ��ƂɃ_���[�W���������
		if (!damageTimer.isEnd) return;
		damageTimer.Reset();


		//�{�X�Ƀ_���[�W��^����
		if (other.tag == "Boss")
		{
			if (boss == null) return;
			
			//�_���[�W��^����
			other.gameObject.GetComponent<BossHP>()?.Damage(damage);
			hp?.Damage(damage);

			//�_���[�W�G�t�F�N�g��t����
			damageEffect?.Damage();
		}

		//�G�Ƀ_���[�W��^����
		if (other.tag == "Enemy")
		{
			other.gameObject.GetComponent<EnemyZakoState>()?.SetDamege(damage);
		}
	}
}
