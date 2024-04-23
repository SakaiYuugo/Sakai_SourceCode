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


	//�͈͓��ɂ���G�ꗗ
	List<Collider> enemies = new List<Collider>();

	Vector3 attackPos;	//���̒e�������W
	float attackTimeCount;	//���ɍU������܂ł̎��Ԃ��v��
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
		//��ԋ߂��G������
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


		//�U�����������t���O
		bool isReadyAttack = true;


		//�͈͓��ɓG��������U���Ԑ��ɓ���
		if (nearEnemy != null)
		{
			//�G�̕������v�Z
			Vector3 turretToEnemy = nearEnemy.gameObject.transform.position - objBody.transform.position;
			turretToEnemy.y = 0f;


			//�G�̕����������܂łɂ�����x�����������������������
			if (Quaternion.Angle(objBody.transform.rotation, Quaternion.LookRotation(turretToEnemy)) > 3f)
			{
				objBody.transform.rotation = Quaternion.RotateTowards(objBody.transform.rotation, Quaternion.LookRotation(turretToEnemy), 3f);

				//�U��������
				isReadyAttack &= false;
			}
			else
			{
				//�G�̕�������
				objBody.transform.rotation = Quaternion.LookRotation(turretToEnemy);

				//�U����������
				isReadyAttack &= true;
			}


			//�e�����������
			foreach (GameObject arm in objArms)
			{
				if (arm.transform.localEulerAngles.x > 270f || arm.transform.localEulerAngles.x <= 0f)
				{
					float rotationZ = 0f;
					if (arm.name.Contains("Left")) { rotationZ = -90f; }
					if (arm.name.Contains("Right")) { rotationZ = 90f; }
					arm.transform.localEulerAngles = new Vector3(arm.transform.localEulerAngles.x - 10f, 0f, rotationZ);

					//�U��������
					isReadyAttack &= false;
				}
				else
				{
					//�U����������
					isReadyAttack &= true;
				}
			}
		}
		else
		{
			//�e������������
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
			//�͈͓��ɓG�����āA�U���������������Ă�����U���J�n
			if (nearEnemy != null && isReadyAttack)
			{
				//�e�𐶐�
				GameObject bullet = Instantiate(prefBullet, attackPos, Quaternion.identity);

				//�e���^���b�g�̎q�I�u�W�F�N�g�ɂ���
				bullet.transform.parent = this.gameObject.transform;

				//�e�̌����𒲐�
				bullet.transform.forward = (nearEnemy.transform.position - attackPos).normalized;
				bullet.transform.rotation = Quaternion.Euler(bullet.transform.eulerAngles.x + 90f, bullet.transform.eulerAngles.y, bullet.transform.eulerAngles.z);

				//��ԋ߂��G�Ɍ������Ēe������
				bullet.GetComponent<Rigidbody>().AddForce((nearEnemy.transform.position - attackPos).normalized * bulletSpeed);

				//���ˉ���炷
				myAudio.time = 0;
				myAudio.Play();

				//���Ԃ̃J�E���g��0�ɖ߂�
				attackTimeCount = 0f;
			}


			//���ɒe���o�����W���擾
			if (attackPos == attackPoses[0].transform.position)
			{
				attackPos = attackPoses[1].transform.position;
			}
			else
			{
				attackPos = attackPoses[0].transform.position;
			}
		}


		//���Ԃ��J�E���g����
		attackTimeCount += Time.deltaTime;



		//��莞�Ԍo�����������
		if (lifeTimer.ScaledUpdate())
		{
			Destroy(this.gameObject);
		}
	}



	
	private void OnTriggerEnter(Collider other)
	{
		//�G���U���͈͓��ɓ������烊�X�g�ɉ�����
		if (other.gameObject.tag == "Enemy")
		{
			enemies.Add(other);
		}
	}
	
	private void OnTriggerExit(Collider other)
	{
		//�G���U���͈͊O�ɏo���烊�X�g����폜����
		if (other.gameObject.tag == "Enemy")
		{
			enemies.Remove(other);
		}
	}


	private void OnCollisionStay(Collision collision)
	{
		//�^���b�g�ƐڐG���Ă����痣���
		if (collision.gameObject.name.Contains("Turret"))
		{
			Vector3 collisionToThis = this.transform.position - collision.transform.position;
			collisionToThis.y = 0f;

			//XZ�x�N�g����������������K���ɕς��Ƃ�
			if (Mathf.Abs(collisionToThis.x) < 1f && Mathf.Abs(collisionToThis.z) < 1f)
			{
				collisionToThis.x = Random.Range(-1f, 1f);
				collisionToThis.z = Random.Range(-1f, 1f);
			}

			myRigidBody.AddForce(collisionToThis * 100f);
		}
	}
}
