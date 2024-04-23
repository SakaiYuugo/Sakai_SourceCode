using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class BombSphere : BombBase
{
	float scaleMultiply;


	private void Awake()
	{
		//���x���ɉ����Ĕ����̑傫����ݒ�
		scaleMultiply = 3 + ((sphereLevel - 1) * 0.5f);
	}

	public override void BeforeDestroy(GameObject explosion)
	{
		base.BeforeDestroy(explosion);

		//�����̑傫����ω�
		explosion.transform.localScale *= scaleMultiply;

		//���x���ɉ����ă_���[�W��ݒ�
		explosion.GetComponent<ExplosionCollisionBehaviour>().damage = sphereLevel + 1;
	}
}
