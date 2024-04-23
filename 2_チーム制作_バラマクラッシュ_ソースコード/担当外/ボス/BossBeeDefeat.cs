using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeeDefeat : BossDefeatBase
{
    [SerializeField,Header("Enemy��������ꏊ(�I�u�W�F�N�g)")]GameObject SpawnObj;
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
            Debug.Log("�A�j���[�V�����I��");
			System_ObjectManager.mainCamera.GetComponent<CameraBossDefeat>().BossAnimationEnd();
			Destroy(this);
		}
	}
}
