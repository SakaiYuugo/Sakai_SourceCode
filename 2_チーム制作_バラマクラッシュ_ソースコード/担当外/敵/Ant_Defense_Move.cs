using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant_Defense_Move : EnemyDefmove
{
    private float distance;
    [SerializeField,Header("ボスとの距離")] float bossDistance = 80.0f;
    [SerializeField, Header("突撃準備を始めるplayerとの距離")] float Length = 15.0f;
    [SerializeField, Header("回転速度")] float RotateSpeed = 1.0f;

    private float speed;// スピード保管用変数

    // Start is called before the first frame update
    override protected void Start()
    {
        BossDistance = bossDistance;
        MinRotateSpeed = RotateSpeed;
        base.Start();

        //オブジェクトの状態をもらう
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();
        distance = 1000.0f;
        speed = base.MoveSpeed;
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        NowState = EnemyState.GetEnemyState();
        if (NowState == EnemyZakoState.ZakoState.Move)
        {
            base.SetEnemySpeed(speed);
            base.FixedUpdate();

            // playerとの距離を計算
            distance = Vector3.Distance(gameObject.transform.position, Player.transform.position);
            // 一定の距離にプレイヤーがいるなら
            if (distance <= Length)
            {
                // 停止させる
                base.SetEnemySpeed(0.0f);
                //攻撃状態へ
                EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
            }
            else
            {
                Debug.Log("ボスとの距離計算");
                //ボスと自分の距離を取得
                float dis = Vector3.Distance(this.gameObject.transform.position, BossObj.transform.position);

                if ((dis > BossDistance)/* && Change == false*/)
                {
                    Debug.Log("ボスとに近づいた");
                    base.TargetNears();
                }
                else
                {
                    base.TargetRotate();
                }
            }
        }
    }
}
