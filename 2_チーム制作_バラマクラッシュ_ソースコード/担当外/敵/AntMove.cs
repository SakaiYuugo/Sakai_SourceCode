using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMove : EnemyAssaultmove
{
    private float distance;
    [SerializeField,Header("突撃準備を始めるplayerとの距離")] float Length = 15.0f;

    private float speed;// スピード保管用変数
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //オブジェクトの状態をもらう
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();
        distance = 1000.0f;
        speed = base.MoveSpeed;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        NowState = EnemyState.GetEnemyState();
        if (NowState == EnemyZakoState.ZakoState.Move)
        {
            base.SetEnemySpeed(speed);
            base.FixedUpdate();
            // playerとの距離を計算
            distance = Vector3.Distance(gameObject.transform.position, base.GetHomingObj().transform.position);
            // 一定の距離にプレイヤーがいるなら
            if(distance <= Length)
            {
                // 停止させる
                base.SetEnemySpeed(0.0f);
                //攻撃状態へ
                EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
            }
        }
    }
}
