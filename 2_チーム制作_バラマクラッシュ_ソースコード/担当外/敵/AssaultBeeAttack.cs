using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultBeeAttack : EnemyAttack
{
    //攻撃アニメーションがあるならなくなるかも
     enum ABAttack
     {
        Preparation,  //準備
        PreAttack,    //攻撃準備
        Attack,       //攻撃
        EndAttack,    //攻撃終了
        Return        //元の位置に戻る
     }



    [SerializeField] int Atk = 1;
    [SerializeField] float AttackTilt = 40;    //どれくらい傾くか
    [SerializeField] GameObject ShootPoint;
    [SerializeField] GameObject Needle;
    [SerializeField] int ShootSpeed = 2000;
    [SerializeField] AudioClip sound;
    private AudioSource Audiosource;

    float PerCount;           //0～1の間の数字が入る(0～100％)
    float PerAddNum;          //上の変数に足す数が入る
    Quaternion EndTilt;       //最初(傾く前)の傾きが入る
    Quaternion StartTilt;     //最後(傾いた後)の傾きが入る
    Quaternion AtkEndTilt;    //針を出すときの傾き

    private ABAttack NowAttack; //ステータス

    override protected void Start()
    {
        Attack = Atk;
        NowAttack = ABAttack.Preparation;
        PerCount = 0.0f;
        PerAddNum = 0.0f;

        Audiosource = GetComponent<AudioSource>();
        base.Start();
    }

    
    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        if (NowState == EnemyZakoState.ZakoState.Attack)
        {
            switch(NowAttack)
            {
                //準備
                case ABAttack.Preparation:
                    StartTilt = transform.rotation;
                    EndTilt = Quaternion.AngleAxis(AttackTilt, transform.right) * transform.rotation;
                    AtkEndTilt = Quaternion.AngleAxis(-50.0f, transform.right) * transform.rotation;
                    NowAttack = ABAttack.PreAttack;
                    PerCount = 0.0f;
                    PerAddNum = 0.0f;
                    break;

                //後ろに回転
                case ABAttack.PreAttack:

                    //回転の補間
                    transform.rotation = Quaternion.Lerp(StartTilt, EndTilt, PerCount);

                    PerAddNum += 0.005f;

                    PerCount += PerAddNum;


                    if (PerCount >= 1.0f)
                    {
                        PerCount = PerAddNum = 0.0f;

                        //元に戻すモーションにチェンジ
                        NowAttack = ABAttack.Attack;
                    }

                    break;

                //攻撃
                case ABAttack.Attack:

                    Audiosource.PlayOneShot(sound);

                    //回転の補間
                    transform.rotation = Quaternion.Lerp(EndTilt, AtkEndTilt, PerCount);

                    PerAddNum += 0.005f;

                    PerCount += PerAddNum;

                    if (PerCount >= 1.0f)
                    {
                        PerCount = PerAddNum = 0.0f;

                        NowAttack = ABAttack.EndAttack;                        
                    }

                    break;

                //元の位置の回転に戻す
                case ABAttack.EndAttack:
                    //攻撃の処理を書いてください

                    //針を生成？                    
                    GameObject bullet = Instantiate(Needle, ShootPoint.transform.position, Quaternion.identity);
                    Rigidbody bulletrb = bullet.GetComponent<Rigidbody>();
                    bulletrb.AddForce(transform.forward * ShootSpeed);                       
                    
                    //元に戻すモーションにチェンジ
                   NowAttack = ABAttack.Return;                    

                    break;

                case ABAttack.Return:
                    //回転の補間
                    transform.rotation = Quaternion.Lerp(AtkEndTilt, StartTilt, PerCount);

                    PerAddNum += 0.005f;

                    PerCount += PerAddNum;


                    if (PerCount >= 1.0f)
                    {
                        PerCount = PerAddNum = 0.0f;

                        //元に戻すモーションにチェンジ
                        NowAttack = ABAttack.Preparation;
                        //オブジェクトの状態を移動に変更
                        State.SetEnemyState(EnemyZakoState.ZakoState.Move);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
