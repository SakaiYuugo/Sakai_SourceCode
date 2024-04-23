using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretBehaviour : MonoBehaviour
{
	GameObject prefBullet;
	[Header("Paramaters")]
	[SerializeField] float bulletSpeed = 10000f;
	[SerializeField] float attackDelay = 1f;
	[SerializeField] float lifeTime;

	[Space(10)]
	[Header("References")]
	[Space(3)]
	[SerializeField] GameObject objBody;
	[Space(3)]
	[SerializeField] GameObject[] objArms;
	[Space(3)]
	[SerializeField] List<GameObject> attackPoses;


	//”ÍˆÍ“à‚É‚¢‚é“Gˆê——
	List<Collider> enemies = new List<Collider>();

	Vector3 attackPos;	//Ÿ‚Ì’e¶¬À•W
	float attackTimeCount;	//Ÿ‚ÉUŒ‚‚·‚é‚Ü‚Å‚ÌŠÔ‚ğŒv‚é
	AudioSource myAudio;
	Rigidbody myRigidBody;

	Timer lifeTimer = new Timer();



    private void Start()
    {
		prefBullet = Resources.Load<GameObject>("Prefab/Weapon/Bullet");
		myAudio = this.transform.Find("SE").GetComponent<AudioSource>();
		myRigidBody = this.GetComponent<Rigidbody>();
		
		attackPos = attackPoses[0].transform.position;
		attackTimeCount = 0f;

		lifeTimer.Set(lifeTime);
    }



	private void FixedUpdate()
	{
		//ˆê”Ô‹ß‚¢“G‚ğŒŸõ
		GameObject nearEnemy = null;
		float nearEnemyMagnitude = float.PositiveInfinity;

		foreach (Collider enemy in enemies)
		{
			if (enemy != null)
			{
				Vector3 turretToEnemy = enemy.ClosestPoint(this.gameObject.transform.position) - this.gameObject.transform.position;

				if (turretToEnemy.magnitude < nearEnemyMagnitude)
				{
					nearEnemy = enemy.gameObject;
					nearEnemyMagnitude = (enemy.ClosestPoint(this.transform.position) - this.transform.position).magnitude;
				}
			}
		}


		//UŒ‚€”õŠ®—¹ƒtƒ‰ƒO
		bool isReadyAttack = true;


		//”ÍˆÍ“à‚É“G‚ª‚¢‚½‚çUŒ‚‘Ô¨‚É“ü‚é
		if (nearEnemy != null)
		{
			//“G‚Ì•ûŒü‚ğŒvZ
			Vector3 turretToEnemy = nearEnemy.gameObject.transform.position - objBody.transform.position;
			turretToEnemy.y = 0f;


			//“G‚Ì•ûŒü‚ğŒü‚­‚Ü‚Å‚É‚ ‚é’ö“x‹——£‚ª‚ ‚Á‚½‚ç‚ä‚Á‚­‚èŒü‚­
			if (Quaternion.Angle(objBody.transform.rotation, Quaternion.LookRotation(turretToEnemy)) > 3f)
			{
				objBody.transform.rotation = Quaternion.RotateTowards(objBody.transform.rotation, Quaternion.LookRotation(turretToEnemy), 3f);

				//UŒ‚€”õ’†
				isReadyAttack &= false;
			}
			else
			{
				//“G‚Ì•û‚ğŒü‚­
				objBody.transform.rotation = Quaternion.LookRotation(turretToEnemy);

				//UŒ‚€”õŠ®—¹
				isReadyAttack &= true;
			}


			//eŒû‚ªã‚ğŒü‚­
			foreach (GameObject arm in objArms)
			{
				if (arm.transform.localEulerAngles.x > 270f || arm.transform.localEulerAngles.x <= 0f)
				{
					float rotationZ = 0f;
					if (arm.name.Contains("Left")) { rotationZ = -90f; }
					if (arm.name.Contains("Right")) { rotationZ = 90f; }
					arm.transform.localEulerAngles = new Vector3(arm.transform.localEulerAngles.x - 10f, 0f, rotationZ);

					//UŒ‚€”õ’†
					isReadyAttack &= false;
				}
				else
				{
					//UŒ‚€”õŠ®—¹
					isReadyAttack &= true;
				}
			}
		}
		else
		{
			//eŒû‚ª‰º‚ğŒü‚­
			foreach (GameObject arm in objArms)
			{
				if (arm.transform.localEulerAngles.x >= 270f)
				{
					float rotationZ = 0f;
					if (arm.name.Contains("Left")) { rotationZ = -90f; }
					if (arm.name.Contains("Right")) { rotationZ = 90f; }
					arm.transform.localEulerAngles = new Vector3(arm.transform.localEulerAngles.x + 10f, 0f, rotationZ);
				}
			}
		}


		if (attackTimeCount >= attackDelay)
		{
			//”ÍˆÍ“à‚É“G‚ª‚¢‚ÄAUŒ‚€”õ‚ªŠ®—¹‚µ‚Ä‚¢‚½‚çUŒ‚ŠJn
			if (nearEnemy != null && isReadyAttack)
			{
				//’e‚ğ¶¬
				GameObject bullet = Instantiate(prefBullet, attackPos, Quaternion.identity);

				//’e‚ğƒ^ƒŒƒbƒg‚ÌqƒIƒuƒWƒFƒNƒg‚É‚·‚é
				bullet.transform.parent = this.gameObject.transform;

				//’e‚ÌŒü‚«‚ğ’²®
				bullet.transform.forward = (nearEnemy.transform.position - attackPos).normalized;
				bullet.transform.rotation = Quaternion.Euler(bullet.transform.eulerAngles.x + 90f, bullet.transform.eulerAngles.y, bullet.transform.eulerAngles.z);

				//ˆê”Ô‹ß‚¢“G‚ÉŒü‚©‚Á‚Ä’e‚ğŒ‚‚Â
				bullet.GetComponent<Rigidbody>().AddForce((nearEnemy.transform.position - attackPos).normalized * bulletSpeed);

				//”­Ë‰¹‚ğ–Â‚ç‚·
				myAudio.time = 0;
				myAudio.Play();

				//ŠÔ‚ÌƒJƒEƒ“ƒg‚ğ0‚É–ß‚·
				attackTimeCount = 0f;
			}


			//Ÿ‚É’e‚ğo‚·À•W‚ğæ“¾
			if (attackPos == attackPoses[0].transform.position)
			{
				attackPos = attackPoses[1].transform.position;
			}
			else
			{
				attackPos = attackPoses[0].transform.position;
			}
		}


		//ŠÔ‚ğƒJƒEƒ“ƒg‚·‚é
		attackTimeCount += Time.deltaTime;



		//ˆê’èŠÔŒo‚Á‚½‚çÁ‚¦‚é
		if (lifeTimer.ScaledUpdate())
		{
			Destroy(this.gameObject);
		}
	}



	
	private void OnTriggerEnter(Collider other)
	{
		//“G‚ªUŒ‚”ÍˆÍ“à‚É“ü‚Á‚½‚çƒŠƒXƒg‚É‰Á‚¦‚é
		if (other.gameObject.tag == "Enemy")
		{
			enemies.Add(other);
		}
	}
	
	private void OnTriggerExit(Collider other)
	{
		//“G‚ªUŒ‚”ÍˆÍŠO‚Éo‚½‚çƒŠƒXƒg‚©‚çíœ‚·‚é
		if (other.gameObject.tag == "Enemy")
		{
			enemies.Remove(other);
		}
	}


	private void OnCollisionStay(Collision collision)
	{
		//ƒ^ƒŒƒbƒg‚ÆÚG‚µ‚Ä‚¢‚½‚ç—£‚ê‚é
		if (collision.gameObject.name.Contains("Turret"))
		{
			Vector3 collisionToThis = this.transform.position - collision.transform.position;
			collisionToThis.y = 0f;

			//XZƒxƒNƒgƒ‹‚ª¬‚³‚©‚Á‚½‚ç“K“–‚É•Ï‚¦‚Æ‚­
			if (Mathf.Abs(collisionToThis.x) < 1f && Mathf.Abs(collisionToThis.z) < 1f)
			{
				collisionToThis.x = Random.Range(-1f, 1f);
				collisionToThis.z = Random.Range(-1f, 1f);
			}

			myRigidBody.AddForce(collisionToThis * 100f);
		}
	}
}
