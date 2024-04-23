using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BombCollisionExplosion : BombExplosionBase
{
	[Header("SE")]
	[SerializeField] AudioSource seCollisionBomb;
	[SerializeField] AudioSource seCollisionPlayer;
	[SerializeField] AudioSource seCollisionGround;

	[Header("�������Ă���{���̑傫���ɖ߂�܂ł̎���")]
	[SerializeField] float secondReturnScale = 1f;

	[Header("�����ɓ������Ă��甚������܂ł̎���")]
	[SerializeField] float delayExplosion = 2f;

	[Header("�����ɓ������Đ�����ԗ�")]
	[SerializeField] float blowForce = 1000f;

	[Header("���e�ɓ����������ɔ������鎞�̔��e�̑��x")]
	[SerializeField] float collisionSpeed = 3f;

	Vector3 defaultScale;
	bool collidedGround;
	float time = 0;

    

    void Start()
    {
		defaultScale = this.transform.localScale;
		this.transform.localScale *= 0.01f;

		collidedGround = false;
	}


	private void FixedUpdate()
	{
		if (time < secondReturnScale)
		{
			this.transform.localScale = time * defaultScale;
			time += Time.deltaTime;
		}
		else
		{
			this.transform.localScale = defaultScale;
			explosionable = true;
		}
	}


	private void OnCollisionEnter(Collision collision)
	{
		if (explosionable)
		{
			if (collision.gameObject.tag == "Player") { seCollisionPlayer.Play(); }
			if (collision.gameObject.tag == "Enemy") { Explosion(); }
			if (collision.gameObject.tag == "StageObject") { Explosion(); }
			if (collision.gameObject.tag == "BigTree") { Explosion(); }
			if (collision.gameObject.tag == "Ground") { collidedGround = true; seCollisionGround.Play(); }

			if (collision.gameObject.tag == "Bomb")
			{
				seCollisionBomb.Play();

				if (collidedGround)
				{
					Rigidbody rigBomb = collision.gameObject.GetComponent<Rigidbody>();
					float force = rigBomb.velocity.magnitude;

					if (force >= collisionSpeed &&
						explosionable)
					{
						Explosion();
					}
				}
			}
		}

		if (collision.gameObject.tag == "Boss")
		{
			Explosion();
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (!this.transform.localScale.Equals(defaultScale)) { return; }

		if (other.gameObject.name.Contains("Explosion"))
		{
			Vector3 blowVector = this.transform.position - other.gameObject.transform.position;
			blowVector.y = 0f;
			blowVector.Normalize();
			blowVector.y = 3f;
			this.gameObject.GetComponent<Rigidbody>().AddForce(blowVector * blowForce);

			flashMan.SetFlash(5);
			Invoke("Explosion", delayExplosion);
		}
	}
	
}
