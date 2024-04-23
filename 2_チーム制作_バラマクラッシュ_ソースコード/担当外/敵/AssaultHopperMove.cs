using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//突撃バッタの動き
public class AssaultHopperMove : EnemyAssaultmove
{
    [SerializeField] float AttackDist = 5.0f;    //バッタ攻撃状態遷移半径
    [SerializeField] int RandomAttack = 50;      //バッタの攻撃確率
    [SerializeField] float JumpPowerValue = 500.0f;
    private float CoolCount = 0;
    Rigidbody rb;
    ZakoHopperState TempHopperState;

    protected override void Start()
    {
        //オブジェクトの状態をもらう
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();
        CoolCount = 10.0f;
        rb = GetComponent<Rigidbody>();
        TempHopperState = EnemyState as ZakoHopperState;

        base.Start();
    }

    protected override void FixedUpdate()
    {
        NowState = EnemyState.GetEnemyState();
        if (NowState == EnemyZakoState.ZakoState.Move)
        {
            base.FixedUpdate();

            if (CoolCount > 0.0f)
            {
                //カウントダウン
                CoolCount -= Time.deltaTime;
                if (CoolCount < 0.0f)
                {
                    CoolCount = 0.0f;
                }

                return;
            }

            //攻撃判定範囲に入っていたら
            Vector3 TempA = this.gameObject.transform.position, TempB = HomingObj.transform.position;
            TempA.y = TempB.y = 0.0f;
            float dist = Vector3.Distance(TempA, TempB);

            if (dist < AttackDist)
            {
                //ランダムで攻撃をする
                if (CoolCount == 0)
                {
                    //攻撃確率計算
                    int rand = Random.Range(0, 100);        //ランダムな数字を持ってくる
                    if (rand <= RandomAttack)//攻撃をする
                    {
                        EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
                    }
                    CoolCount = 10.0f;
                }
            }

            
        }
    }

    public override void CollisionEnterProcess(Collision copy)
    {
        base.CollisionEnterProcess(copy);
        if(copy.gameObject.name == "Ground")
        {
            //地面についた
            TempHopperState.SetJumpNow(false);
        }
    }
}
