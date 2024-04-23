using UnityEngine;

public class DungBeetleState : BossHP
{
    DungBeetle_Scales _DungBeetle_Scales;    //完全委託した鱗粉攻撃
    Rigidbody rb;
    int AddRgFrame; //倒されてから重力をかけるまでの猶予時間

    DungManager Manager;
    DungBarrier _DungBarrier;
    BossHP _HP;
    GameObject _DungBall;
    [SerializeField,ReadOnly] int nConstAtkWaitFrame;   //攻撃せずに移動する時間
    [SerializeField, ReadOnly] int nAtkWaitFrame = 0;  //の、フレームカウント用
    [SerializeField] float fGuardHP;    //ガード状態の体力

    //スタン関連
    [SerializeField] int nConstStunFrame;   //倒れるアニメ
    [SerializeField] int nConstStunReturnFrame; //起き上がりアニメ
    [SerializeField, ReadOnly] int nStunFrame = 0;
    [ReadOnly] public bool bChangeState = true;
    BoxCollider _BoxCollider;   //スタン時の当たり判定調整
    Vector3 CollderOriginalCenter;  //元の当たり判定
    Vector3 CollderOriginalSize;  //元の当たり判定
    Vector3 CollderStunCenter = new Vector3(-0.23f, 8.8f, -11.1f);  //スタン時のの当たり判定
    Vector3 CollderStunSize = new Vector3(8.3f, 13.7f, 29.6f);  //スタン時のの当たり判定    

    [ReadOnly] public int nGoPoint;   //ポイントの何番に向かうか

    public enum DungBeetleSTATE
    {
        m_ROTATION, //方向回転
        m_MOVE, //通常移動
        m_STUN, //糞が破壊されてスタン状態
        m_STUN_RETURN,  //スタンからの復帰
        m_ROLLSTAR,  //たまを飛ばす攻撃
        m_UP_IMPACT,    //糞による衝撃波
        m_GEKKOUTYOU,   //羽を広げて鱗粉を降らす
        m_DEATH,    //倒された
    }

    [ReadOnly] public DungBeetleSTATE state;   //state管理


    protected override void Awake()
    {
        System_ObjectManager.bossObject = this.gameObject;
        base.Awake();
    }

    override protected void Start()
    {
        base.Start();

        _DungBeetle_Scales = GameObject.Find("Rinpun_DungBeetle").GetComponent<DungBeetle_Scales>();

        Manager = GameObject.Find("Boss_Point").GetComponent<DungManager>();

        nGoPoint = 1;

        state = DungBeetleSTATE.m_ROTATION;

        nConstAtkWaitFrame = Random.Range(600, 700);

        _DungBall = GameObject.Find("DungBall");

        _BoxCollider = GetComponent<BoxCollider>();

        CollderOriginalCenter = _BoxCollider.center;
        CollderOriginalSize = _BoxCollider.size;

        rb = GetComponent<Rigidbody>();
        //nHP = MaxHP;
        //hpBarController.SetMaxHp(MaxHP);
        //hpBarController.SetHp(nHP);
        //SystemLevelManager.GetLevel();
    }


    void FixedUpdate()
    {
        //攻撃時間になったら
        if (nConstAtkWaitFrame <= nAtkWaitFrame)
        {
            nConstAtkWaitFrame = Random.Range(600, 900);    //次の攻撃クールタイムを設定
            int Num = Random.Range(0,3);
            switch(Num)
            {
                case 0:
                    state = DungBeetleSTATE.m_UP_IMPACT;
                    break;
                case 1:
                    state = DungBeetleSTATE.m_ROLLSTAR;
                    break;
                case 2:
                    state = DungBeetleSTATE.m_GEKKOUTYOU;
                    break;
            }
            bChangeState = true;
            nAtkWaitFrame = 0;
        }

        //ボスを倒した！(1度だけ入る)   
        if(state != DungBeetleSTATE.m_DEATH && nHP <= 0)
        {
            Debug.Log("クリア");
            state = DungBeetleSTATE.m_DEATH;
            bChangeState = true;
            Manager._DBDefeat.enabled = true;
        }


        //一度だけ実行
        if (bChangeState)
        {
            switch (state)
            {
                case DungBeetleSTATE.m_ROTATION:
                    //回転中は物理演算するな
                    Manager._DBRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    break;
                case DungBeetleSTATE.m_MOVE:
                    //移動中は上下のみ物理演算していい
                    Manager._DBRigidbody.constraints = RigidbodyConstraints.None;
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.None;
                    Manager._DBRigidbody.constraints = RigidbodyConstraints.FreezeRotation |
                        RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.FreezeRotation | 
                        RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

                    break;

                case DungBeetleSTATE.m_STUN:
                    //スタン時は座標のみ動いていい
                    Manager._DBRigidbody.constraints = RigidbodyConstraints.None;
                    Manager._DBRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

                    _DungBall.SetActive(false);
                    _BoxCollider.center = CollderStunCenter;
                    _BoxCollider.size = CollderStunSize;
                    Manager._DungAnime.StunAnime();

                    nStunFrame = 0;
                    break;


                case DungBeetleSTATE.m_STUN_RETURN:
                    Debug.Log("起き上がれ");
                    Manager._DungAnime.WakeUpAnime();
                    Manager._Db.Reproduction();

                    nStunFrame = 0;
                    break;

                case DungBeetleSTATE.m_ROLLSTAR: //虫はガード状態になる
                    Debug.Log("ロールスター");
                    //ここで虫の予備動作
                    Manager._DbRollstar.StartAtk(Manager._DBMove.GetGoPos());
                    break;

                case DungBeetleSTATE.m_UP_IMPACT:
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.None;
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
                    Debug.Log("衝撃波");
                    //ここで虫側の予備動作
                    Manager._DbUpImpact.GetComponent<DungBallUpImpact>().StartAtk();                  
                    break;

                case DungBeetleSTATE.m_GEKKOUTYOU:
                    Debug.Log("月光蝶");
                    _DungBeetle_Scales.SetScales(this.gameObject, _DungBall);
                    break;

                case DungBeetleSTATE.m_DEATH:
                    Manager._DungAnime.DestroyAnime();
                    Manager._DBRigidbody.constraints = RigidbodyConstraints.FreezeAll;  //リジットボディによる運動を止める
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    GetComponent<FallPrevention>().enabled = false; //落ちるようにする
                    GetComponent<BoxCollider>().enabled = false; //落ちるようにする
                    Debug.Log("死亡");
                    break;
            }
            bChangeState = false;
        }

        //毎フレーム入る
        switch (state)
        {
            case DungBeetleSTATE.m_ROTATION:    //方向変える時は主導権をボールに渡す
                Manager._DBMove.Turnaround();
                Manager._Db.Turnaround();

                break;
            case DungBeetleSTATE.m_MOVE:    //私に主導権
                Manager._DungSound.Sound(DungSound.DUNG_SOUND.SOUND_WALK);  //音再生
                Manager._DBMove.Move();
                Manager._Db.FollowBeetle(Manager._DBMove.fDistance);
                nAtkWaitFrame++;
                break;
            case DungBeetleSTATE.m_STUN:

                nStunFrame++;
                if (nStunFrame >= nConstStunFrame)
                {   //スタン時間終了
                    state = DungBeetleSTATE.m_STUN_RETURN;
                    bChangeState = true;
                }
                break;

            case DungBeetleSTATE.m_STUN_RETURN:
                nStunFrame++;
                if (nStunFrame >= nConstStunReturnFrame)
                {
                    state = DungBeetleSTATE.m_MOVE;
                    bChangeState = true;
                    _BoxCollider.center = CollderOriginalCenter;
                    _BoxCollider.size = CollderOriginalSize;
                    _DungBall.SetActive(true);
                }
                break;

            case DungBeetleSTATE.m_ROLLSTAR:

                break;
            case DungBeetleSTATE.m_UP_IMPACT:

                break;
            case DungBeetleSTATE.m_GEKKOUTYOU:
                state = DungBeetleSTATE.m_MOVE;
                bChangeState = true;
                break;
            case DungBeetleSTATE.m_DEATH://180f
                AddRgFrame++;
                if(AddRgFrame >= 200)
                {
                    transform.position += new Vector3(0,-0.1f,0);
                }
                break;
        }
    }

    override public void Damage(int damage)
    {
        if(GetComponent<DungBarrier>().bBarrier)    //ダメージを軽減する状態なら
        {
            nHP -= 1;   //ガード状態なら1ダメ固定
        }
        else
        {
            nHP -= damage;
        }

        hpBarController.SetHp(nHP);
    }


}
