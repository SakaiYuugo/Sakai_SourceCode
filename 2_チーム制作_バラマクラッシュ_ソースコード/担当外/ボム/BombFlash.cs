using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFlash : MonoBehaviour
{
	[SerializeField] 
	GameObject[] FlashObjects;
	float Alpha;

	//�_�Ŋ֌W
	List<Renderer> Rend_Colors;
	bool Explosion;
	bool Flash;
	int FlashCount;
	int FlashInterval;

	void Awake()
    {
		Explosion = false;
		Flash = false;
		FlashCount = 0;
		FlashInterval = 0;
		Alpha = 0.6f;
	}

	
	void Start()
    {
		Rend_Colors = new List<Renderer>();

		foreach(GameObject copy in FlashObjects)
        {
			
			Renderer Temp = copy.GetComponent<Renderer>();
			Temp.material.SetColor("_AddColor", Color.red);
			Temp.material.SetFloat("_Toumeido", 0.0f);
			Rend_Colors.Add(Temp);
		}
	}
	
	
    void Update()
    {
		//����������O�Ȃ�
        if(Explosion)
        {
			ExplosionFlash();
        }
    }

	public void SetFlash(int flashInterval)
    {
		if (!Explosion)
		{
			FlashInterval = flashInterval;
			Explosion = true;
			Flash = true;
		}
    }

	void ExplosionFlash()
	{
		//�F��ς���I�u�W�F�N�g
		foreach(Renderer copy in Rend_Colors)
        {
			if (Flash)
			{
				//�t���b�V�����Ă���Ƃ�
				copy.material.SetFloat("_Alpha", Alpha);
			}
			else
			{
				//�t���b�V�����Ă��Ȃ���
				copy.material.SetFloat("_Alpha", 0.0f);
			}
		}

		//�؂�ւ���^�C�~���O��
		if (FlashCount % FlashInterval == 0)
		{
			//�����Ă��邩�A�؂�ւ���
			Flash = !Flash;
		}
		FlashCount++;
	}
}
