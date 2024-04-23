using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultDungBeetleMove : EnemyAssaultmove
{
    [SerializeField] float AttackDist = 15.0f;    //フンコロガシ攻撃状態遷移半径
    [SerializeField] int RandomAttack = 50;      //フンコロガシの攻撃確率
    private int AttackCount = 10;                //ランダム判定をする間隔(攻撃)
    private int Count = 0;                       //実際のカウント

    override protected void Start()
    {
        //オブジェクトの状態をもらう
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();

        AttackCount *= 60;       //時間の補正        

        base.Start();
    }

    override protected void FixedUpdate()
    {
        NowState = EnemyState.GetEnemyState();
        if (NowState == EnemyZakoState.ZakoState.Move)
        {            

            base.FixedUpdate();

            //ハチ攻撃遷移関連            
            //攻撃判定範囲に入っていたら
            if(this.gameObject != null && HomingObj != null)
            {
                float dist = Vector3.Distance(this.gameObject.transform.position, HomingObj.transform.position);

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
