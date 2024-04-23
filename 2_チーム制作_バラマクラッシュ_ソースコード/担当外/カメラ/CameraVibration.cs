using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraVibration : MonoBehaviour
{
	bool isVibration;
	float time;
	float force;
	Vector3 preRandomRotate;
	


	public void Vibration(float time, float force)
	{
		this.time = time;
		this.force = force;

		isVibration = true;
		preRandomRotate = new Vector3(0f, 0f, 0f);
	}
	

	private void FixedUpdate()
	{		
		if (isVibration)
		{
			//現在フレームの回転から前フレームのランダム回転を引いた値を取得
			Vector3 eulerRotate = this.transform.eulerAngles - preRandomRotate;

			//ランダムな回転を作成
			Vector3 randomRotate = new Vector3();
			randomRotate.x += Random.Range(-1f, 1f) * force;
			randomRotate.y += Random.Range(-1f, 1f) * force;
			randomRotate.z += Random.Range(-1f, 1f) * force;

			//前フレームのランダム回転を保存
			preRandomRotate = randomRotate;

			//ランダム回転を適用する
			this.transform.eulerAngles = eulerRotate + randomRotate;


			//振動時間が終わった場合
			if (time < 0)
			{
				//カメラの回転を元に戻す
				this.transform.eulerAngles -= preRandomRotate;
				isVibration = false; 
			}

			//時間を測定
			time -= Time.deltaTime;
		}
	}
}
