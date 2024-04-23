using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombCubeMini : BombBase
{
	[SerializeField] int damage;

	public override void BeforeDestroy(GameObject explosion)
	{
		base.BeforeDestroy(explosion);

		explosion.transform.localScale *= 0.5f;

		explosion.GetComponent<ExplosionCollisionBehaviour>().damage = damage;
	}
}
