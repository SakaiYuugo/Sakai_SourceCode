using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrewMove : MonoBehaviour
{
	//----- 高度 -----
	[SerializeField, Header("ボム最小高度")] private float MinHeight = 1.0f;
	[SerializeField, Header("ボム最高高度")] private float MaxHeight = 10.0f;
	[SerializeField, Header("高度の分割数")] private float HeightLane = 5.0f;
	private float height;

	//----- 距離 -----
	[SerializeField, Header("ボム最小距離")] private int MinDistance = 1;
	[SerializeField, Header("ボム最大距離")] private int MaxDistance = 10;
	[SerializeField, Header("距離の分割数")] private int DistanceLane = 5;
	private int Distance;

	//----- 回転 -----
	[SerializeField, Header("ボム最小回転速度")] private float MinRotSpeed = 10.0f;
	[SerializeField, Header("ボム最大回転速度")] private float MaxRotSpeed = 90.0f;
	[SerializeField, Header("回転の分割数")] private int RotLane = 5;
	private float Rot;
	Quaternion rotarion;

	//----- ばら撒き -----
	[SerializeField, Header("ばら撒きの範囲")] private float Range = 90.0f;
	[SerializeField, Header("パワーの補正値")] private float PowerValue = 0.1f;

	private GameObject Bomb;
	private GameObject Player;
	private float NowSpeed;

	
	void Start()
    {
		// プレイヤーが存在いているかどうか
		Player = GameObject.Find("Player");

		// 平均値を計算
		height   = (MaxHeight - MinHeight) / HeightLane;         // 高度
		Distance = (MaxDistance - MinDistance) / DistanceLane;   // 距離
		Rot      = (MaxRotSpeed - MinRotSpeed) / RotLane;        // 回転
	}

	//---------------------------------------
	// ばら撒き
	//---------------------------------------
	public void Strew(GameObject Object)
	{
		// ステータスを取得
		NowSpeed     = Player.GetComponent<PlayerMove>().GSNowSpeed;// 現在のスピード
		Rigidbody rd = Object.GetComponent<Rigidbody>();            // 爆弾
		int PushCnt  = GetComponent<StrewState>().GetPushTime();    // キーが何秒押されていたか

		// 回転の計算
		float tempRot = (MinRotSpeed + (Rot * Random.Range(-90f, 90f)));
		Vector3 torque = new Vector3(tempRot, tempRot, tempRot);
		rd.AddTorque(torque, ForceMode.Impulse);

		// 距離の計算
		float temp = (MinDistance + (Distance * Random.Range(1, DistanceLane + 1)));
		if (NowSpeed < 1.0f) { NowSpeed += 1.0f; }  // スピードが1未満の場合補正
		NowSpeed *= PowerValue;    // 値を補正値で調整

		Vector3 vector1 = transform.forward * temp;

		// ばら撒かれる範囲
		float radius = Random.Range(-Range , Range);
		radius *= Mathf.PI / 180.0f;
		
		Vector3 vector2 = new Vector3(
					vector1.x * Mathf.Cos(radius) - vector1.z * Mathf.Sin(radius),
					vector1.y,
					vector1.x * Mathf.Sin(radius) + vector1.z * Mathf.Cos(radius));

		// 速度ベクトル
		rd.AddForce(NowSpeed * PushCnt * vector2, ForceMode.Impulse);

		// 瞬発的に力を与える(高度の計算)
		float heightValue = MinHeight + (height * Random.Range(1, HeightLane + 1));
		rd.AddForce(new Vector3(0.0f, heightValue * PushCnt * NowSpeed, 0.0f), ForceMode.Impulse);
	}
}
