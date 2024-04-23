using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuff_HpUp : ZakoBuff_Base
{
    [SerializeField]
    int UpHP = 5;
    int BeforeHP;

    GameObject InstanceEffect;
    /*
    public override void Start()
    {
        EnemyZakoState nowState = GetComponent<EnemyZakoState>();
        nowState.SetUpHp(UpHP);

        InstanceEffect = Instantiate(Resources.Load<GameObject>("Prefab/Effect/HpUpEffect"));
        InstanceEffect.transform.parent = this.gameObject.transform;
        InstanceEffect.transform.localPosition = Vector3.zero;
        base.Start();
    }
    */
    private void OnEnable()
    {
        EnemyZakoState nowState = GetComponent<EnemyZakoState>();
        nowState.SetUpHp(UpHP);

        InstanceEffect = Instantiate(Resources.Load<GameObject>("Prefab/Effect/powerHp"));
        InstanceEffect.transform.parent = this.gameObject.transform;
        InstanceEffect.transform.localPosition = Vector3.zero;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void ReleaseBuff()
    {
        base.ReleaseBuff();
        GetComponent<EnemyZakoState>().SetUpHp(1);
        Destroy(InstanceEffect);
    }

}
