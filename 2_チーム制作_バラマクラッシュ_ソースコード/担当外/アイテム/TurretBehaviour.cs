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


	//範囲内にいる敵一覧
	List<Collider> enemies = new List<Collider>();

	Vector3 attackPos;	//次の弾生成座標
	float attackTimeCount;	//次に攻撃するまでの時間を計る
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
		//一番近い敵を検索
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


		//攻撃準備完了フラグ
		bool isReadyAttack = true;


		//範囲内に敵がいたら攻撃態勢に入る
		if (nearEnemy != null)
		{
			//敵の方向を計算
			Vector3 turretToEnemy = nearEnemy.gameObject.transform.position - objBody.transform.position;
			turretToEnemy.y = 0f;


			//敵の方向を向くまでにある程度距離があったらゆっくり向く
			if (Quaternion.Angle(objBody.transform.rotation, Quaternion.LookRotation(turretToEnemy)) > 3f)
			{
				objBody.transform.rotation = Quaternion.RotateTowards(objBody.transform.rotation, Quaternion.LookRotation(turretToEnemy), 3f);

				//攻撃準備中
				isReadyAttack &= false;
			}
			else
			{
				//敵の方を向く
				objBody.transform.rotation = Quaternion.LookRotation(turretToEnemy);

				//攻撃準備完了
				isReadyAttack &= true;
			}


			//銃口が上を向く
			foreach (GameObject arm in objArms)
			{
				if (arm.transform.localEulerAngles.x > 270f || arm.transform.localEulerAngles.x <= 0f)
				{
					float rotationZ = 0f;
					if (arm.name.Contains("Left")) { rotationZ = -90f; }
					if (arm.name.Contains("Right")) { rotationZ = 90f; }
					arm.transform.localEulerAngles = new Vector3(arm.transform.localEulerAngles.x - 10f, 0f, rotationZ);

					//攻撃準備中
					isReadyAttack &= false;
				}
				else
				{
					//攻撃準備完了
					isReadyAttack &= true;
				}
			}
		}
		else
		{
			//銃口が下を向く
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
			//範囲内に敵がいて、攻撃準備が完了していたら攻撃開始
			if (nearEnemy != null && isReadyAttack)
			{
				//弾を生成
				GameObject bullet = Instantiate(prefBullet, attackPos, Quaternion.identity);

				//弾をタレットの子オブジェクトにする
				bullet.transform.parent = this.gameObject.transform;

				//弾の向きを調整
				bullet.transform.forward = (nearEnemy.transform.position - attackPos).normalized;
				bullet.transform.rotation = Quaternion.Euler(bullet.transform.eulerAngles.x + 90f, bullet.transform.eulerAngles.y, bullet.transform.eulerAngles.z);

				//一番近い敵に向かって弾を撃つ
				bullet.GetComponent<Rigidbody>().AddForce((nearEnemy.transform.position - attackPos).normalized * bulletSpeed);

				//発射音を鳴らす
				myAudio.time = 0;
				myAudio.Play();

				//時間のカウントを0に戻す
				attackTimeCount = 0f;
			}


			//次に弾を出す座標を取得
			if (attackPos == attackPoses[0].transform.position)
			{
				attackPos = attackPoses[1].transform.position;
			}
			else
			{
				attackPos = attackPoses[0].transform.position;
			}
		}


		//時間をカウントする
		attackTimeCount += Time.deltaTime;



		//一定時間経ったら消える
		if (lifeTimer.ScaledUpdate())
		{
			Destroy(this.gameObject);
		}
	}



	
	private void OnTriggerEnter(Collider other)
	{
		//敵が攻撃範囲内に入ったらリストに加える
		if (other.gameObject.tag == "Enemy")
		{
			enemies.Add(other);
		}
	}
	
	private void OnTriggerExit(Collider other)
	{
		//敵が攻撃範囲外に出たらリストから削除する
		if (other.gameObject.tag == "Enemy")
		{
			enemies.Remove(other);
		}
	}


	private void OnCollisionStay(Collision collision)
	{
		//タレットと接触していたら離れる
		if (collision.gameObject.name.Contains("Turret"))
		{
			Vector3 collisionToThis = this.transform.position - collision.transform.position;
			collisionToThis.y = 0f;

			//XZベクトルが小さかったら適当に変えとく
			if (Mathf.Abs(collisionToThis.x) < 1f && Mathf.Abs(collisionToThis.z) < 1f)
			{
				collisionToThis.x = Random.Range(-1f, 1f);
				collisionToThis.z = Random.Range(-1f, 1f);
			}

			myRigidBody.AddForce(collisionToThis * 100f);
		}
	}
}
