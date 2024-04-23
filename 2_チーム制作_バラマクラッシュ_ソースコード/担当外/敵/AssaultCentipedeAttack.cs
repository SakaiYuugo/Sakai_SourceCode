using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultCentipedeAttack : EnemyAttack
{
    public enum ACAttack
    {
        Bend,        //身体の中心(四つ目の節)をy軸方向に持ち上げる
        Fall,        //持ち上がった身体を地面に倒す
        Attack       //身体が倒し終わって衝撃波発生
    }

    [SerializeField] GameObject ShockWave;    //発生させる衝撃波
    [SerializeField] int Atk = 1;   //攻撃力
    private ACAttack ACAState;      //攻撃の状態
    private int AttackCount;        //攻撃のカウント
    private Vector3 Startpos;       //攻撃開始時の位置
    private bool AttackState;       //攻撃の状態になっているか

    [SerializeField] AudioClip Sound;
    private AudioSource Audiosource;

    override protected void Start()
    {
        Attack = Atk;
        State = GetComponentInParent<EnemyZakoState>();
        NowState = State.GetEnemyState();
        ACAState = ACAttack.Bend;
        AttackCount = 0;
        //Startpos = this.transform.position;
        AttackState = false;

        Audiosource = GetComponent<AudioSource>();
    }

    
    override protected void FixedUpdate()
    {
        NowState = State.GetEnemyState();
        if(NowState == EnemyZakoState.ZakoState.Attack)
        {
            if(AttackState != true)
            {
                Startpos = this.transform.position;
                AttackState = true;
            }            

            switch (ACAState)
            {
                case ACAttack.Bend:
                    //身体を持ち上げる
                    //transform.Translate(0.0f, 0.3f, 0.0f);
                    Vector3 bend = this.transform.position;
                    bend.y += 0.3f;
                    this.transform.position = bend;

                    //攻撃開始位置から
                    if (this.transform.position.y > Startpos.y + 4.0f)
                        SetACAState(ACAttack.Fall);

                    break;

                case ACAttack.Fall:
                    //身体を打ち付ける
                    transform.Translate(0.0f, -0.25f, 0.05f);

                    if(this.transform.position.y < Startpos.y)
                    {
                        Vector3 pos = this.transform.position;
                        pos.y = Startpos.y;
                        transform.position = pos;
                        SetACAState(ACAttack.Attack);
                    }
                    break;

                case ACAttack.Attack:

                    Audiosource.PlayOneShot(Sound);

                    //衝撃波発生
                    if(AttackCount == 0)
                    {
                        Instantiate(ShockWave, this.transform.position, Quaternion.identity);
                    }

                    AttackCount++;

                    if (AttackCount >= 60)
                    {
                        AttackCount = 0;
                        AttackState = false;
                        SetACAState(ACAttack.Bend);

                        //移動状態に遷移
                        State.SetEnemyState(EnemyZakoState.ZakoState.Move);
                    }
                    break;

                default:
                    break;
            }
            //Debug.Log(ACAState + "現在の状態");
        }
    }

    public ACAttack GetACAState()
    {
        return ACAState;
    }

    //念のため
    public void SetACAState(ACAttack state)
    {
        ACAState = state;
    }
}
