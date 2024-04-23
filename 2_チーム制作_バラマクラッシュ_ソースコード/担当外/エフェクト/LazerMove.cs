using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerMove : EffectMove
{
    private GameObject Target;

    private Vector3 TargetPos;

    // Start is called before the first frame update
    override protected void Start()
    {
        Target = GameObject.Find("Player");
        TargetPos = Target.transform.position;
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        // 対象物とエフェクト生成座標からベクトルを算出
        Vector3 vector3 = TargetPos - gameObject.transform.position;
        // Quaternion(回転値)を取得
        Quaternion quaternion = Quaternion.LookRotation(vector3);
        gameObject.transform.rotation = quaternion;
    }
}
