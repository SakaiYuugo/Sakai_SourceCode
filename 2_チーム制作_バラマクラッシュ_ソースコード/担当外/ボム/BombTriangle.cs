using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombTriangle : BombBase
{
	[SerializeField] AudioSource seBound;
	Rigidbody rigidBody;
	float force;



	private void Awake()
	{
		rigidBody = this.GetComponent<Rigidbody>();

		//���x���ɉ����Ē��˂鋭����ݒ�
		force = 1 + ((triangleLevel - 1) * 0.5f);
	}

	public override void BeforeDestroy(GameObject explosion)
	{
		base.BeforeDestroy(explosion);

		//���x���ɉ����ă_���[�W��ݒ�
		explosion.GetComponent<ExplosionCollisionBehaviour>().damage = cubeLevel + 1;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag is "Ground")
		{
			rigidBody.AddForce(Vector3.up * force * 500f);
			seBound.Play();
		}
	}
}
