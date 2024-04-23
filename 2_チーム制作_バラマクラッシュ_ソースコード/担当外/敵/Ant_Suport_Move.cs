using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant_Suport_Move : EnemySupportmove
{
    [SerializeField, Header("突撃準備を始めるplayerとの距離")] float Length = 15.0f;

    private float distance;
    private float speed;// スピード保管用変数
    // Start is called before the first frame update
    override protected void Start()
    {
        //オブジェクトの状態をもらう
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();
        base.Start();
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
            distance = Vector3.Distance(gameObject.transform.position, PlayerObj.transform.position);
            // 一定の距離にプレイヤーがいるなら
            if (distance <= Length)
            {
                // 停止させる
                base.SetEnemySpeed(0.0f);
                //攻撃状態へ
                EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
            }


            //ターゲットオブジェクトが消滅していたら新しく近くにいる敵をターゲットにする
            if (HomingTarget == null) HomingTarget = SerchNearEnemy("Enemy");

            if (!(HomingTarget == null))
            {
                //ターゲットとの距離
                float enemydist = Vector3.Distance(transform.position, HomingTarget.transform.position);

                this.transform.LookAt(HomingTarget.transform.position);  //常にターゲットの方向を向く
                Vector3 tmo = this.transform.rotation.eulerAngles;
                tmo.x = 0f;
                tmo.z = 0f;
                this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);

                //ターゲットが一定距離以下なら止まる
                if ((enemydist < EnemyDistance) && (HomingTarget != PlayerObj)) transform.Translate(0.0f, 0.0f, 0.0f);
                else transform.Translate(0.0f, 0.0f, MoveSpeed);    //ターゲットの方にZ軸正面で向かっていく
            }
            else
            {
                //ターゲットをプレイヤーにする
                HomingTarget = PlayerObj;

                this.transform.LookAt(HomingTarget.transform.position);  //常にターゲットの方向を向く
                Vector3 tmo = this.transform.rotation.eulerAngles;
                tmo.x = 0f;
                tmo.z = 0f;
                this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);

                transform.Translate(0.0f, 0.0f, MoveSpeed);
            }
        }
    }
}
