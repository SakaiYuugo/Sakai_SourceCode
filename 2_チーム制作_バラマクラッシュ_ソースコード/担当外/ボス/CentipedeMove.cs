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
		//�����_��������
		Random.InitState(System.DateTime.Now.Millisecond * System.DateTime.Now.Second);

		targetPos = Vector3.zero;
		timer.Set(20f);

		//SE
		seTimer.Set(0f);
	}


	private void FixedUpdate()
	{
		//��莞�Ԃ��Ƃ�SE��炷
		if (seTimer.ScaledUpdate())
		{
			System_ObjectManager.mainCameraAudioSource?.PlayOneShot(seMove, 0.8f);
			seTimer.Set(seInterval);
		}

		//�ڕW���W�̕ύX
		if (targetPos == Vector3.zero)
		{
			while (true)
			{
				//���̖ړI�n������
				targetPos = new Vector3(Random.Range(-150f, 150f), 0f, Random.Range(-150f, 150f));

				//���̖ړI�n�͎����̍��W����30M�ȏ㗣�ꂽ�ꏊ�ɂ���
				if ((targetPos - this.transform.position).magnitude > 30f) break;
			}
;
			targetPos.y = this.transform.position.y;
		}

		//�{�X����ڕW�܂ł̌���
		Vector3 thisToTarget = (targetPos - this.transform.position).normalized;		
		
		//�O�Ɍ������Ĉړ�
		this.transform.position += this.transform.forward * moveSpeed;

		//�ڕW�����ɏ�������]
		this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(thisToTarget), rotateSpeed);

		//�ڕW�n�_�߂��܂œ��B�����玟�̍��W��ڎw��
		if ((targetPos - this.transform.position).magnitude < 50f)
		{
			targetPos = Vector3.zero;
		}

		//���Ԃ��v��
		if (timer.ScaledUpdate())
		{
			this.GetComponent<AudioSource>().Stop();
			state.StateChange();
		}
	}
}
