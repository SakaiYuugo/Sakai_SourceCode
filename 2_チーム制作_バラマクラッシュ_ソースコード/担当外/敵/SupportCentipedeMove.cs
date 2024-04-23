using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportCentipedeMove : EnemySupportmove
{
    [SerializeField] float SEnemyDistance = 15.0f;
    private float random;
    private GameObject HomingHead;

    override protected void Start()
    {        
        EnemyDistance = SEnemyDistance;
        random = Random.Range(-5.0f, 5.0f);

        base.Start();

        //インスタンス化したらすぐに探す
        HomingTarget = SerchNearEnemy("Enemy");
        if (HomingTarget != null) HomingHead = HomingTarget.transform.GetChild(0).gameObject;

        //オブジェクトの状態をもらう
        EnemyState = GetComponentInParent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();
    }

    override protected void FixedUpdate()
    {
        NowState = EnemyState.GetEnemyState();

        if (NowState == EnemyZakoState.ZakoState.Move)
        {
            base.FixedUpdate();

            //ターゲットオブジェクトが消滅していたら新しく近くにいる敵をターゲットにする
            if (HomingTarget == null)
            {
                HomingTarget = SerchNearEnemy("Enemy");
                if(HomingTarget != null) HomingHead = HomingTarget.transform.GetChild(0).gameObject;
            }

            if (!(HomingHead == null))
            {
                
                //ターゲットとの距離
                float enemydist = Vector3.Distance(transform.position, HomingHead.transform.position);
                
                this.transform.LookAt(HomingHead.transform.position);  //常にターゲットの方向を向く
                Vector3 tmo = this.transform.rotation.eulerAngles;
                tmo.x = 0f;
                tmo.z = 0f;
                this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);
                
                //ターゲットが一定距離以下なら止まる
                if ((enemydist < EnemyDistance) && (HomingTarget != PlayerObj)) transform.Translate(0.0f, 0.0f, 0.0f);
                else transform.Translate(0.0f, 0.0f, MoveSpeed);    //ターゲットの方にZ軸正面で向かっていく
                //transform.Translate(0.0f, 0.0f, MoveSpeed);
                
                //this.transform.LookAt(HomingHead.transform);
                //transform.Translate(0.0f, 0.0f, MoveSpeed);
                                
            }
            else
            {
                //ターゲットをプレイヤーにする
                //HomingTarget = PlayerObj;

                this.transform.LookAt(PlayerObj.transform);  //常にターゲットの方向を向く
                Vector3 tmo = this.transform.rotation.eulerAngles;
                tmo.x = 0f;
                tmo.z = 0f;
                this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);

                transform.Translate(0.0f, 0.0f, MoveSpeed);
            }

            transform.Translate(0.08f * Mathf.Sin((Time.time + random) * 6), 0.0f, 0.0f);
        }

        //Debug.Log(HomingTarget);
    }
    
}
