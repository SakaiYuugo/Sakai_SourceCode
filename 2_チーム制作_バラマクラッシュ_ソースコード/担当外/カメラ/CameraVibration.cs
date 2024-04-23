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
			//���݃t���[���̉�]����O�t���[���̃����_����]���������l���擾
			Vector3 eulerRotate = this.transform.eulerAngles - preRandomRotate;

			//�����_���ȉ�]���쐬
			Vector3 randomRotate = new Vector3();
			randomRotate.x += Random.Range(-1f, 1f) * force;
			randomRotate.y += Random.Range(-1f, 1f) * force;
			randomRotate.z += Random.Range(-1f, 1f) * force;

			//�O�t���[���̃����_����]��ۑ�
			preRandomRotate = randomRotate;

			//�����_����]��K�p����
			this.transform.eulerAngles = eulerRotate + randomRotate;


			//�U�����Ԃ��I������ꍇ
			if (time < 0)
			{
				//�J�����̉�]�����ɖ߂�
				this.transform.eulerAngles -= preRandomRotate;
				isVibration = false; 
			}

			//���Ԃ𑪒�
			time -= Time.deltaTime;
		}
	}
}
