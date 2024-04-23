using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderNet : MonoBehaviour
{
	[SerializeField, Header("SE")] AudioSource seClip;

	[SerializeField, Header("持続時間(秒)")] float TimeValue = 5.0f;
	[SerializeField, Header("減速値")]       float deceleration = 10.0f;
	[SerializeField, Header("地面に置く糸")] private GameObject NetObject;
	GameObject Net;
	private PlayerMove Player;

	void Start()
    {
		Player = GameObject.Find("Player").GetComponent<PlayerMove>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		// プレイヤーに当たった場合
		if (collision.gameObject.tag.Contains("Player"))
		{
			seClip.Play();
			Player.SlowDown(deceleration,TimeValue);
			Destroy(this.gameObject);
		}

		// 地面に当たった場合
		if (collision.gameObject.tag.Contains("Ground"))
		{
			// 地面にクモの糸のオブジェクトを配置
			Net = Instantiate(NetObject, this.transform.position, Quaternion.identity);
			Destroy(this.gameObject);
		}
	}

}
