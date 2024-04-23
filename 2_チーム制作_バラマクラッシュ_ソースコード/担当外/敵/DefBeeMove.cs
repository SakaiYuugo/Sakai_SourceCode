using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefBeeMove : EnemyDefmove
{
    [SerializeField] float DBossDistance = 80.0f;   //ボスとの距離
    [SerializeField] float DAttackDistance = 20.0f; //ボスの護衛から攻撃するときの距離
    [SerializeField] float DMinRotateSpeed = 1.0f;    //回転速度
    [SerializeField] GameObject DBossObj;
    [SerializeField] float AttackDist = 5.0f;
    [SerializeField] int RandomAttack = 50;      //ハチの攻撃確率
    private int AttackCount = 10;                //ランダム判定をする間隔
    private int Count = 0;                       //実際のカウント

    // Start is called before the first frame update
    override protected void Start()
    {
        BossDistance = DBossDistance;
        AttackDistance = DAttackDistance;
        MinRotateSpeed = DMinRotateSpeed;
        BossObj = DBossObj;

        //オブジェクトの状態をもらう
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();

        base.Start();
    }

    // Update is called once per frame
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
