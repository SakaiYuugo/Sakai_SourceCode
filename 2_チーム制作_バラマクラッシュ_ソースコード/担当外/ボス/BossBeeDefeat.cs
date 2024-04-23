using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeeDefeat : BossDefeatBase
{
    [SerializeField,Header("Enemy生成する場所(オブジェクト)")]GameObject SpawnObj;
    BossBeeAnimation Anim;


    protected override void OnEnable()
    {
		base.OnEnable();

		System_ObjectManager.mainCamera.GetComponent<CameraBossDefeat>().bossDistance = 200.0f;
		System_ObjectManager.mainCamera.GetComponent<CameraBossDefeat>().bossHeight = 300.0f;

        Anim = gameObject.GetComponent<BossBeeAnimation>();
	}


    override protected void FixedUpdate()
    {
        base.FixedUpdate();

		if (Anim.DidStoppedDefeatAnim())
		{
            Debug.Log("アニメーション終了");
			System_ObjectManager.mainCamera.GetComponent<CameraBossDefeat>().BossAnimationEnd();
			Destroy(this);
		}
	}
}
