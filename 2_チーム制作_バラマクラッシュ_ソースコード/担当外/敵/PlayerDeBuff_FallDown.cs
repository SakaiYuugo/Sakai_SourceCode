using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeBuff_FallDown : ZakoBuff_Base
{
    float DownFall;
    PlayerMove PlayerMoveTemp;
    GameObject InstanceEffect;

    IconMove iconMove;
    /*
    //プレイヤーについていること前提
    public override void Start()
    {
        DownFall = 10.0f;
        base.Start();
        PlayerMoveTemp = GetComponent<PlayerMove>();

        InstanceEffect = Instantiate(Resources.Load<GameObject>("Prefab/Effect/FallDownEffect"));
        InstanceEffect.transform.parent = this.gameObject.transform;
        InstanceEffect.transform.localPosition = Vector3.zero;

        DebuffSpeed();
    }
    */
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    //デバフの内容
    void DebuffSpeed()
    {
        PlayerMoveTemp.SlowDown(DownFall,BuffTime);
    }

    public override void ReleaseBuff()
    {
        Destroy(InstanceEffect);
        base.ReleaseBuff();
    }

    private void OnEnable()
    {
        DownFall = 10.0f;
        PlayerMoveTemp = GetComponent<PlayerMove>();

        InstanceEffect = Instantiate(Resources.Load<GameObject>("Prefab/Effect/FallDownEffect2"));
        InstanceEffect.transform.parent = this.gameObject.transform;
        InstanceEffect.transform.localPosition = Vector3.zero;

        DebuffSpeed();

        iconMove = GameObject.Find("BuffDebufIcon").GetComponent<IconMove>();
        iconMove.SetIcon(IconMove.IconType.Harden,BuffTime);
    }

}
