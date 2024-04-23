using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFlash : MonoBehaviour
{
	[SerializeField] 
	GameObject[] FlashObjects;
	float Alpha;

	//点滅関係
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
		//爆発をする前なら
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
		//色を変えるオブジェクト
		foreach(Renderer copy in Rend_Colors)
        {
			if (Flash)
			{
				//フラッシュしているとき
				copy.material.SetFloat("_Alpha", Alpha);
			}
			else
			{
				//フラッシュしていない時
				copy.material.SetFloat("_Alpha", 0.0f);
			}
		}

		//切り替えるタイミングで
		if (FlashCount % FlashInterval == 0)
		{
			//光っているか、切り替える
			Flash = !Flash;
		}
		FlashCount++;
	}
}
