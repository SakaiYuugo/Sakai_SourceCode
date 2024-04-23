using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundNet : MonoBehaviour
{
	[SerializeField, Header("SE")] AudioSource seClip;

	[SerializeField, Header("持続時間(秒)")] float TimeValue = 5.0f;
	[SerializeField, Header("減速値")]   　　float deceleration = 10.0f ;
	private float deltaTime;
	private PlayerMove Player;

	void Start()
    {
		Player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

	private void FixedUpdate()
	{
		// オブジェクトが存在しない場合
		if (this.gameObject == null) { return; }

		deltaTime += Time.deltaTime;
		// 一定時間経過で削除
		if (TimeValue < deltaTime) { Destroy(this.gameObject); }
	}

	/// <summary>
	/// プレイヤーと衝突した場合、速度減少デバフ
	/// </summary>
	private void OnCollisionEnter(Collision collision)
	{
		seClip.Play();
		Player.SlowDown(deceleration, TimeValue);
		Destroy(this.gameObject);
	}


}
