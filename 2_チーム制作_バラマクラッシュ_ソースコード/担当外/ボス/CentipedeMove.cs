using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CentipedeMove : MonoBehaviour
{
	[Header("Paramater")]
	[SerializeField] float moveSpeed;
	[SerializeField] float rotateSpeed;

	[Header("SE")]
	[SerializeField] AudioClip seMove;
	[SerializeField] float seInterval;

	CentipedeState state;

	Vector3 targetPos;
	Timer timer = new Timer();

	Timer seTimer = new Timer();



	private void Start()
	{
		state = this.transform.parent.GetComponent<CentipedeState>();
	}


	private void OnEnable()
	{
		//ランダム初期化
		Random.InitState(System.DateTime.Now.Millisecond * System.DateTime.Now.Second);

		targetPos = Vector3.zero;
		timer.Set(20f);

		//SE
		seTimer.Set(0f);
	}


	private void FixedUpdate()
	{
		//一定時間ごとにSEを鳴らす
		if (seTimer.ScaledUpdate())
		{
			System_ObjectManager.mainCameraAudioSource?.PlayOneShot(seMove, 0.8f);
			seTimer.Set(seInterval);
		}

		//目標座標の変更
		if (targetPos == Vector3.zero)
		{
			while (true)
			{
				//次の目的地を決定
				targetPos = new Vector3(Random.Range(-150f, 150f), 0f, Random.Range(-150f, 150f));

				//次の目的地は自分の座標から30M以上離れた場所にする
				if ((targetPos - this.transform.position).magnitude > 30f) break;
			}
;
			targetPos.y = this.transform.position.y;
		}

		//ボスから目標までの向き
		Vector3 thisToTarget = (targetPos - this.transform.position).normalized;		
		
		//前に向かって移動
		this.transform.position += this.transform.forward * moveSpeed;

		//目標方向に少しずつ回転
		this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(thisToTarget), rotateSpeed);

		//目標地点近くまで到達したら次の座標を目指す
		if ((targetPos - this.transform.position).magnitude < 50f)
		{
			targetPos = Vector3.zero;
		}

		//時間を計測
		if (timer.ScaledUpdate())
		{
			this.GetComponent<AudioSource>().Stop();
			state.StateChange();
		}
	}
}
