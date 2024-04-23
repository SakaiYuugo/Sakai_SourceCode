using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefDungBeetleMove : EnemyDefmove
{
    [SerializeField] float DBossDistance = 80.0f;     //ボスとの距離
    [SerializeField] float DAttackDistance = 20.0f;   //ボスの護衛から攻撃するときの距離
    [SerializeField] float DMinRotateSpeed = 1.0f;    //回転速度

    [SerializeField] float AttackDist = 15.0f;    //フンコロガシ攻撃状態遷移半径
    [SerializeField] int RandomAttack = 50;      //フンコロガシの攻撃確率
    private int AttackCount = 10;                //ランダム判定をする間隔(攻撃)
    private int Count = 0;                       //実際のカウント

    override protected void Start()
    {
        BossDistance = DBossDistance;
        AttackDistance = DAttackDistance;
        MinRotateSpeed = DMinRotateSpeed;

        //オブジェクトの状態をもらう
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();

        base.Start();
    }

    override protected void FixedUpdate()
    {
        NowState = EnemyState.GetEnemyState();

        if (NowState == EnemyZakoState.ZakoState.Move)
        {
            base.FixedUpdate();

            //ボスと自分の距離を取得
            float dis = Vector3.Distance(this.gameObject.transform.position, BossObj.transform.position);

            if ((dis > BossDistance)/* && Change == false*/)
            {
                TargetNears();
            }
            else
            {
                TargetRotate();

                //ハチ攻撃遷移関連            
                //攻撃判定範囲に入っていたら
                float dist = Vector3.Distance(this.gameObject.transform.position, PlayerObj.transform.position);
                if (dist < AttackDist)
                {
                    bool Atk = false;
                    if (Count == 0)
                    {
                        //ランダム計算
                        int rand = Random.Range(0, 100);
                        if (rand <= RandomAttack) Atk = true;
                    }

                    //攻撃遷移
                    if (Atk == true)
                    {
                        transform.LookAt(PlayerObj.transform.position);
                        EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
                        Count++;
                    }
                    else
                    {
                        Count++;
                        if (Count > AttackCount) Count = 0;
                    }
                }
            }
        }
    }
}
