using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombCylinder : BombBase
{
	[SerializeField] GameObject poisonGas;
	float gasLifetime;


	private void Awake()
	{
		//���x���ɉ����ăK�X�̐������Ԃ�ݒ�
		gasLifetime = cylinderLevel * 3;
	}

	public override void BeforeDestroy(GameObject explosion)
	{
		base.BeforeDestroy(explosion);

		//�K�X�𐶐�
		GameObject objPoisonGas = Instantiate(poisonGas, this.transform.position, Quaternion.identity);
		objPoisonGas.transform.localScale = explosion.transform.localScale * 2f;    //�Q�{���炢�œ����傫���ɂȂ�

		//�K�X�̐������Ԃ�ݒ�
		objPoisonGas.GetComponent<PoisonGas>().lifeTime = gasLifetime;

		//�_���[�W��ݒ�
		explosion.GetComponent<ExplosionCollisionBehaviour>().damage = cylinderLevel + 1;
	}
}
