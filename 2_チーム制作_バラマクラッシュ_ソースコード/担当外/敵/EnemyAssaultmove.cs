using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAssaultmove : MonoBehaviour
{
    protected bool Homing = true;                         //追従するかのフラグ
    protected GameObject HomingObj;                       //追従するオブジェクト
    protected float MoveSpeed;                            //移動速度
    protected bool ColCheck = false;                      //他の敵とぶつかっているか
    protected int rand;                                   //当たったときにはじく？(いらないかも)
    protected Vector2Int RandTargetPos;                          //ターゲート位置をずらすためのランダム格納用
    protected Vector3 TargetposPlus;                      //ターゲットの位置ずらす用
    protected EnemyZakoState EnemyState;                  //状態      
    protected EnemyZakoState.ZakoState NowState;          //現在の自身の状態
    protected BossHP BossHp;

    virtual protected void Start()
    {
        HomingObj = System_ObjectManager.playerObject;     //プレイヤーの名前のオブジェクトを取得
        //計算の仕方が違うのでこっちで適当に修正        
        MoveSpeed = (ZakoEnemySpawn.MoveSpeed / 50) * (1.0f / 3.0f);

        //狙うターゲットの位置をずらす(パックマン方式)
        RandTargetPos.x = Random.Range(-50, 50);
        RandTargetPos.y = Random.Range(-50, 50);
        TargetposPlus = new Vector3(RandTargetPos.x, 0, RandTargetPos.y);

        if(!TutorialManager.TutorialNow)
        {
            BossHp = System_ObjectManager.bossHp;
        }        
    }

    virtual protected void FixedUpdate()
    {                
        if(HomingObj != null)
        {
            //追従の条件
            if (Homing != false && ColCheck == false)
            {
                if (HomingObj == null)
                {
                    HomingObj = System_ObjectManager.playerObject;
                }

                float distance = Vector3.Distance(this.transform.position, HomingObj.transform.position);

                if (distance <= 20)
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

                    //ターゲットの位置についてしまったら新しくターゲットの位置を選定
                    if (Mathf.Abs(this.transform.position.x - (HomingObj.transform.position.x + TargetposPlus.x)) < 2.0f
                        && Mathf.Abs(this.transform.position.z - (HomingObj.transform.position.z + TargetposPlus.z)) < 2.0f)
                    {
                        RandTargetPos.x = Random.Range(-50, 50);
                        RandTargetPos.y = Random.Range(-50, 50);
                        TargetposPlus = new Vector3(RandTargetPos.x, 0, RandTargetPos.y);
                    }
                }

            }

            else if (ColCheck == true)
            {

                rand = Random.Range(-2, 2);
                transform.Translate(MoveSpeed * rand, 0.0f, -MoveSpeed * 3);

                ColCheck = false;
            }
        }
             
        if(GameEventManager.bGameClear)
        {
            MoveSpeed = 0.0f;
        }
        if(!TutorialManager.TutorialNow && BossHp.GetNowHP() <= 0)
        {
            MoveSpeed = 0.0f;
        }

    }

    //追従しているオブジェクトを取得(わざわざごちゃらせていくぅー)
    public GameObject GetHomingObj()
    {
        return HomingObj;
    }

    public virtual void CollisionEnterProcess(Collision copy)
    {

    }

    public virtual void CollisionReleaseProcess(Collision copy)
    {

    }

    //いらないかも
    public virtual void OnCollisionEnter(Collision collision)
    {
        //他の敵に衝突していたら
        if (collision.gameObject.tag == "Enemy")
        {
            ColCheck = true;
        }

        CollisionEnterProcess(collision);
    }

    //いらないかも
    public virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            ColCheck = false;
        }
        CollisionReleaseProcess(collision);
    }
    
    //Set関数(ターゲット)
    public void SetTarget(GameObject gb)
    {
        HomingObj = gb;
    }

    //速度のSet関数
    public void SetEnemySpeed(float Speed) { MoveSpeed = Speed; }

    
    //ターゲット戻す用の関数(もしかしたら使うかも？)
    public void ReturnTarget()
    {
        HomingObj = System_ObjectManager.playerObject;
    }
    
}
