using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BombAutoExplosion : BombExplosionBase
{
	[SerializeField] float explosionTime = 30f;
	float CntTime;
	

	void Start()
	{
		CntTime = 0.0f;
		Invoke("Explosion", explosionTime);
	}

    private void FixedUpdate()
    {
        if (explosionTime - CntTime < 2.0f)
        {
			flashMan.SetFlash(5);
        }

		CntTime += Time.deltaTime;
    }
}
