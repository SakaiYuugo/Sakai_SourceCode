using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombCylinder : BombBase
{
	[SerializeField] GameObject poisonGas;
	float gasLifetime;


	private void Awake()
	{
		//レベルに応じてガスの生存時間を設定
		gasLifetime = cylinderLevel * 3;
	}

	public override void BeforeDestroy(GameObject explosion)
	{
		base.BeforeDestroy(explosion);

		//ガスを生成
		GameObject objPoisonGas = Instantiate(poisonGas, this.transform.position, Quaternion.identity);
		objPoisonGas.transform.localScale = explosion.transform.localScale * 2f;    //２倍ぐらいで同じ大きさになる

		//ガスの生存時間を設定
		objPoisonGas.GetComponent<PoisonGas>().lifeTime = gasLifetime;

		//ダメージを設定
		explosion.GetComponent<ExplosionCollisionBehaviour>().damage = cylinderLevel + 1;
	}
}
