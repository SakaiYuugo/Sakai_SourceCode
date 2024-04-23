using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class BombSphere : BombBase
{
	float scaleMultiply;


	private void Awake()
	{
		//レベルに応じて爆発の大きさを設定
		scaleMultiply = 3 + ((sphereLevel - 1) * 0.5f);
	}

	public override void BeforeDestroy(GameObject explosion)
	{
		base.BeforeDestroy(explosion);

		//爆発の大きさを変化
		explosion.transform.localScale *= scaleMultiply;

		//レベルに応じてダメージを設定
		explosion.GetComponent<ExplosionCollisionBehaviour>().damage = sphereLevel + 1;
	}
}
