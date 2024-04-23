using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemBase : MonoBehaviour
{
	[SerializeField] protected AudioClip seClip;
	[SerializeField] protected float lifeTime;

	GameObject player;
	protected PlayerState playerState;
	protected StrewState strewState;
	protected ChoiceObject choiceObject;

	GameObject texture;
	AudioSource audioSource;

	Timer destroyTimer = new Timer();

	bool isGet;


	private void Awake()
	{
		//参照取得
		audioSource = this.GetComponent<AudioSource>();
	}

	private void Start()
	{
		//参照取得
		texture = this.transform.Find("Texture").gameObject;

		//エラーチェック
		if (System_ObjectManager.itemParent == null) { Destroy(this.gameObject); return; }
		this.transform.parent = System_ObjectManager.itemParent.transform;

		//自動で消えるまでの時間
		destroyTimer.Set(lifeTime);

		//アイテム取得フラグ
		isGet = false;
	}


	private void FixedUpdate()
	{
		destroyTimer.ScaledUpdate();

		//消えそうになったらチカチカし始める
		if (destroyTimer.remainingTime < 5f)
		{
			if (Mathf.PingPong(destroyTimer.remainingTime, 0.25f) < 0.125f)
			{ texture.SetActive(false); }
			else
			{ texture.SetActive(true); }
		}

		//一定時間経過したらアイテムを消す
		if (destroyTimer.isEnd && !isGet)
		{
			texture.SetActive(true);
			Destroy(this.gameObject);
		}

		//アイテム取得後に音が鳴ってなかったらアイテム削除
		if (isGet && !audioSource.isPlaying)
		{
			Destroy(this.gameObject);
		}
	}


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == "Player")
		{
			playerState = System_ObjectManager.playerObject.GetComponent<PlayerState>();
			strewState = System_ObjectManager.playerObject.GetComponentInChildren<StrewState>();
			choiceObject = System_ObjectManager.playerObject.GetComponentInChildren<ChoiceObject>();

			//アイテムを取った時の処理
			OnGetItem();

			//SEを鳴らす
			audioSource.PlayOneShot(seClip);

			//アイテムを消す
			this.GetComponent<Collider>().enabled = false;
			
			//子オブジェクトを全て消す
			foreach(Transform trans in this.transform)
			{
				//親オブジェクトだったら消さない
				if (trans == this.transform) continue;

				//子オブジェクトだったらアクティブを false にする
				trans.gameObject.SetActive(false);
			}

			//アイテム取得フラグ
			isGet = true;
		}
	}


	/// <summary>
	/// アイテムを取った時の処理
	/// </summary>
	protected virtual void OnGetItem() { }
}
