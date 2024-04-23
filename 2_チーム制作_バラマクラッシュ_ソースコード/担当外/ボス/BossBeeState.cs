using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BossBeeAttack))]
[RequireComponent(typeof(BossMove))]
public class BossBeeState : BossHP
{
    [SerializeField,Header("攻撃の種類")] int AttackTypeNum = 3;

    [SerializeField, Header("EASYでのHP")]  int EasyHp = 150;
    [SerializeField, Header("NORMALでのHP")] int NormalHp = 200;
    [SerializeField, Header("HARDでのHP")] int HardHp = 250;

    [SerializeField, Header("攻撃開始までの秒数(low)")] int StartAtkLow = 10;
    [SerializeField, Header("攻撃開始までの秒数(hight)")] int StartAtkHight = 15;

    [SerializeField, Header("AudioSource")] public AudioClip[] audios;

    private BossBeeAttack BossAtk;
    private BossMove Bossmove;
    private EffectSpawn effectSpawn;
    private BossBeeAnimation BossAnim;
    private BossBeeDefeat BeeDefeat;
    private CaveCount caveCount;
    private BossBarrier[] Barriers;
    //private BossBarrier Barrier;
    private AudioSource audioSource;

    public enum State
    {
        Move,
        Attack,
        Death
    }

    private State NowState;     // 現在の状態
    private bool IsCheck;       // 一度だけ実行したい時使う
    private float frame;
    private int StartAtkFrame;  // 攻撃開始までの時間
    private int Atknum;
    private float originPosY;
    private int OldAtkNum;
    private int DefeatFrame;
    private int HalfHp;         // 半分のHp
    private bool IsBarrier;     // バリアを貼っているかどうか

    override protected void Awake()
    {
		System_ObjectManager.bossObject = this.gameObject;
        base.Awake();

        switch (SystemLevelManager.GetLevel_enum())
        {
            case SystemLevelManager.LEVELS.EASY:
                MaxHP = EasyHp;
                break;
            case SystemLevelManager.LEVELS.NORMAL:
                MaxHP = NormalHp;
                break;
            case SystemLevelManager.LEVELS.HARD:
                MaxHP = HardHp;
                break;
        }
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        BossAtk = this.GetComponent<BossBeeAttack>();
        Bossmove = this.GetComponent<BossMove>();
        effectSpawn = this.GetComponent<EffectSpawn>();
        NowState = State.Move;
        IsCheck = false;
        Atknum = 0;
        OldAtkNum = Atknum;
        originPosY = gameObject.transform.position.y;
        BossAnim = gameObject.GetComponent<BossBeeAnimation>();
        BeeDefeat = gameObject.GetComponent<BossBeeDefeat>();
        caveCount = GameObject.Find("Cave").GetComponent<CaveCount>();
        Barriers = gameObject.GetComponents<BossBarrier>();
        //Barrier = gameObject.GetComponent<BossBarrier>();
        audioSource = gameObject.GetComponent<AudioSource>();
        DefeatFrame = 0;
        HalfHp = MaxHP / 2;
        IsBarrier = false;
    }


    private void FixedUpdate()
    {
        // 上に浮き上がる場合があるので対策
        Vector3 NowPosition = gameObject.transform.position;
        NowPosition.y = originPosY;
        gameObject.transform.position = NowPosition;

        Random.InitState(System.DateTime.Now.Millisecond);

        hpBarController.SetHp(nHP);

        // HPが半分より下になり、洞窟がある場合はバリアを貼る
        if (nHP <= HalfHp && caveCount.GetCaveNum() >= 1 && !IsBarrier)
        {
            foreach(var Barrier in Barriers)
            {
                Barrier.barrierEnable = true;
            }
            
            IsBarrier = true;
        }
        else if(caveCount.GetCaveNum() <= 0)
        {
            foreach (var Barrier in Barriers)
            {
                Barrier.barrierEnable = false;
            }
            IsBarrier = false ;
        }
        

        if (nHP <= 0 && NowState != State.Death)          // HPが0になったら
        {
            Debug.Log("BossのHpが0になった");
            frame = 0;
            NowState = State.Death;
        }

        switch (NowState)
        {
            case State.Move:
                if (frame <= 0)
                {
                    Bossmove.StartBoss();
                    StartAtkFrame = Random.Range(StartAtkLow, StartAtkHight);
                }
                frame += Time.deltaTime;
                if (frame > StartAtkFrame)    // 攻撃する時間になったら攻撃状態へ
                {
                    Atknum = Random.Range(1, AttackTypeNum + 1);
                    //ちゃんとランダムにならねえ
                    if(Atknum == OldAtkNum)
                    {
                        Atknum++;
                        if (Atknum > AttackTypeNum)
                            Atknum = 1;
                    }
                    Bossmove.StopBoss();
                    frame = 0;
                    OldAtkNum = Atknum;
                    NowState = State.Attack;
                }
                break;
            case State.Attack:
                // モデルが待機状態になったら
                if (Bossmove.FinishFlg)
                {
                    BossAtk.Attack(Atknum);
                }
                break;
            case State.Death:

                if (!IsCheck)
                {
                    BeeDefeat.enabled = true;
                    foreach (var Barrier in Barriers)
                    {
                        Barrier.barrierEnable = false;
                    }
                    Bossmove.StopBoss();  // 動きをストップ
                    BossAnim.StartDefeatAnim(); // 倒れるアニメーション再生
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);// 回転しないようにする
                    IsCheck = true;
                }
                Defeat();
                break;
        }

    }

    override public void Damage(int damage)
    {
        audioSource.PlayOneShot(audios[2]);
        if (IsBarrier)
        {
            nHP -= 1;
        }
        else
        {
            nHP -= damage;
        }
    }

    public void SetStateBoss(State state)
    {
        NowState = state;
    }

    public State GetState()
    {
        return NowState;
    }

    public void Defeat()
    {

        if (BossAnim.DidStoppedDefeatAnim())
        {
            if(DefeatFrame <= 0)
            {
				//System_ObjectManager.mainCamera.GetComponent<CameraBossDefeat>().BossAnimationEnd();
                Debug.Log("死ぬアニメーション終了");
                audioSource.PlayOneShot(audios[0]);
            }
            DefeatFrame++;
        }

    }

    public AudioClip GetAudio(int num)
    {
        return audios[num];
    }
}
