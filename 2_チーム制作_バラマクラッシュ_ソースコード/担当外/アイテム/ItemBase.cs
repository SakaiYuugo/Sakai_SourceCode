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
		//�Q�Ǝ擾
		audioSource = this.GetComponent<AudioSource>();
	}

	private void Start()
	{
		//�Q�Ǝ擾
		texture = this.transform.Find("Texture").gameObject;

		//�G���[�`�F�b�N
		if (System_ObjectManager.itemParent == null) { Destroy(this.gameObject); return; }
		this.transform.parent = System_ObjectManager.itemParent.transform;

		//�����ŏ�����܂ł̎���
		destroyTimer.Set(lifeTime);

		//�A�C�e���擾�t���O
		isGet = false;
	}


	private void FixedUpdate()
	{
		destroyTimer.ScaledUpdate();

		//���������ɂȂ�����`�J�`�J���n�߂�
		if (destroyTimer.remainingTime < 5f)
		{
			if (Mathf.PingPong(destroyTimer.remainingTime, 0.25f) < 0.125f)
			{ texture.SetActive(false); }
			else
			{ texture.SetActive(true); }
		}

		//��莞�Ԍo�߂�����A�C�e��������
		if (destroyTimer.isEnd && !isGet)
		{
			texture.SetActive(true);
			Destroy(this.gameObject);
		}

		//�A�C�e���擾��ɉ������ĂȂ�������A�C�e���폜
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

			//�A�C�e������������̏���
			OnGetItem();

			//SE��炷
			audioSource.PlayOneShot(seClip);

			//�A�C�e��������
			this.GetComponent<Collider>().enabled = false;
			
			//�q�I�u�W�F�N�g��S�ď���
			foreach(Transform trans in this.transform)
			{
				//�e�I�u�W�F�N�g������������Ȃ�
				if (trans == this.transform) continue;

				//�q�I�u�W�F�N�g��������A�N�e�B�u�� false �ɂ���
				trans.gameObject.SetActive(false);
			}

			//�A�C�e���擾�t���O
			isGet = true;
		}
	}


	/// <summary>
	/// �A�C�e������������̏���
	/// </summary>
	protected virtual void OnGetItem() { }
}
