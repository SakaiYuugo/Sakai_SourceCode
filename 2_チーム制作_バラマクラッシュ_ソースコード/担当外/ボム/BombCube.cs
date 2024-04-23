using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombCube : BombBase
{
	[SerializeField] GameObject bombCube;
	[SerializeField] float blowForce;

	float division;


	private void Awake()
	{
		//レベルに応じて分裂する数を設定
		division = cubeLevel * 2;
	}


	public override void BeforeDestroy(GameObject explosion)
	{
		base.BeforeDestroy(explosion);

		for (int i = 0; i < division; i++)
		{
			//ランダム角度で生成
			Quaternion randomRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
			GameObject objBombCube = Instantiate(bombCube, this.transform.position, randomRotation);
			
			//最初に飛ばす
			Vector3 randomVector = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
			objBombCube.GetComponent<Rigidbody>().AddForce(randomVector * blowForce);
		}

		//ダメージを設定
		explosion.GetComponent<ExplosionCollisionBehaviour>().damage = cubeLevel + 1;
	}
}
