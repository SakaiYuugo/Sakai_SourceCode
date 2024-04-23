using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultCentipedeMove : EnemyAssaultmove
{
    [SerializeField] float AttackDist = 5.0f;    //ムカデ攻撃状態遷移半径
    [SerializeField] int RandomAttack = 30;      //ムカデの攻撃確率
    private int AttackCount = 10;                //ランダム判定をする間隔
    private int Count = 0;                       //実際のカウント
    private float random;                        //くねくね動く時のランダム幅

    override protected void Start()
    {
        EnemyState = GetComponentInParent<ZakoCentipedeState>();
        NowState = EnemyState.GetEnemyState();
        
        base.Start();

        AttackCount = AttackCount * 60;             //時間の補正
        random = Random.Range(-5.0f, 5.0f);
 
    }

    
    override protected void FixedUpdate()
    {
        
        NowState = EnemyState.GetEnemyState();
        if (NowState == EnemyZakoState.ZakoState.Move)
        {                                                                 
            float distance = Vector3.Distance(this.transform.position, HomingObj.transform.position);
            if (distance <= 30)
            {//プレイヤーとの距離が30より小さくなったらプレイヤーターゲット
                this.transform.LookAt(HomingObj.transform.position);
                transform.Translate(0.0f, 0.0f, MoveSpeed);
            }
            else
            {
                //常にプレイヤー(ちょっとずらす)の方向を向く
                this.transform.LookAt(HomingObj.transform.position + TargetposPlus);                
                Vector3 tmo = this.transform.rotation.eulerAngles;
                tmo.x = 0f;
                tmo.z = 0f;
                this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);
                transform.Translate(0.0f, 0.0f, MoveSpeed);
                    
                //ターゲットの位置についてしまったら新しくターゲットの位置を選定(x軸とz軸の距離が2.0f以下になっていたら)
                if (Mathf.Abs(this.transform.position.x - (HomingObj.transform.position.x + TargetposPlus.x)) < 2.0f
                     && Mathf.Abs(this.transform.position.z - (HomingObj.transform.position.z + TargetposPlus.z)) < 2.0f)
                {
                    RandTargetPos.x = Random.Range(-50, 50);
                    RandTargetPos.y = Random.Range(-50, 50);
                    TargetposPlus = new Vector3(RandTargetPos.x, 0, RandTargetPos.y);
                }                    
            }                                     

            //くねくね動く
            transform.Translate(0.08f * Mathf.Sin((Time.time + random) * 6), 0.0f, 0.0f);

            //プレイヤーとの距離でダメージ判定(恐らくレイヤー関係なく判定取れるはず)
            float dist = Vector3.Distance(this.gameObject.transform.position, HomingObj.transform.position);
            if (HomingObj.transform.name == "Player")
            {              
                if (dist <= 1.0) EnemyState.SetDamege(1);
            }
            
            //ムカデ攻撃遷移関連            
            //攻撃判定範囲に入っていたら
            if (dist < AttackDist)
            {
                bool Atk = false;    //攻撃するかの判定用
                if(Count == 0)
                {//攻撃判定時間
                    //ランダム計算
                    int rand = Random.Range(0, 100);
                    if (rand <= RandomAttack) Atk = true;
                }

                //攻撃遷移
                if(Atk == true)
                {
                    Count++;
                    EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
                }
                else
                {
                    Count++;
                    if (Count > AttackCount) Count = 0;
                }
            }            
        }

        if(GameEventManager.bGameClear)
        {
            MoveSpeed = 0.0f;
        }
        if (!TutorialManager.TutorialNow && BossHp.GetNowHP() <= 0)
        {
            MoveSpeed = 0.0f;
        }
    }
    
}
