using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletBehaviour : MonoBehaviour
{
	[SerializeField] int damage = 1;
	Transform parent;
	SphereCollider parentSphere;


	private void Start()
	{
		parent = this.transform.parent;
		parentSphere = parent.gameObject.GetComponent<SphereCollider>();
	}


	private void FixedUpdate()
	{
		Vector3 turretToBullet = this.transform.position - parent.position;

		if (turretToBullet.magnitude > parentSphere.radius * parent.lossyScale.magnitude)
		{
			Destroy(this.gameObject);
		}
	}



	private void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag)
		{
		case "Enemy":
		{
			other.gameObject.GetComponent<EnemyZakoState>().SetDamege(damage);
			Destroy(this.gameObject);
		}
		break;
		}
		
	}
}
