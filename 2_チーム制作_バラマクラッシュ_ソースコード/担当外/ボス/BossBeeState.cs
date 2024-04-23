using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BossBeeAttack))]
[RequireComponent(typeof(BossMove))]
public class BossBeeState : BossHP
{
    [SerializeField,Header("�U���̎��")] int AttackTypeNum = 3;

    [SerializeField, Header("EASY�ł�HP")]  int EasyHp = 150;
    [SerializeField, Header("NORMAL�ł�HP")] int NormalHp = 200;
    [SerializeField, Header("HARD�ł�HP")] int HardHp = 250;

    [SerializeField, Header("�U���J�n�܂ł̕b��(low)")] int StartAtkLow = 10;
    [SerializeField, Header("�U���J�n�܂ł̕b��(hight)")] int StartAtkHight = 15;

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

    private State NowState;     // ���݂̏��
    private bool IsCheck;       // ��x�������s���������g��
    private float frame;
    private int StartAtkFrame;  // �U���J�n�܂ł̎���
    private int Atknum;
    private float originPosY;
    private int OldAtkNum;
    private int DefeatFrame;
    private int HalfHp;         // ������Hp
    private bool IsBarrier;     // �o���A��\���Ă��邩�ǂ���

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
        // ��ɕ����オ��ꍇ������̂ő΍�
        Vector3 NowPosition = gameObject.transform.position;
        NowPosition.y = originPosY;
        gameObject.transform.position = NowPosition;

        Random.InitState(System.DateTime.Now.Millisecond);

        hpBarController.SetHp(nHP);

        // HP��������艺�ɂȂ�A���A������ꍇ�̓o���A��\��
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
        

        if (nHP <= 0 && NowState != State.Death)          // HP��0�ɂȂ�����
        {
            Debug.Log("Boss��Hp��0�ɂȂ���");
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
                if (frame > StartAtkFrame)    // �U�����鎞�ԂɂȂ�����U����Ԃ�
                {
                    Atknum = Random.Range(1, AttackTypeNum + 1);
                    //�����ƃ����_���ɂȂ�˂�
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
                // ���f�����ҋ@��ԂɂȂ�����
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
                    Bossmove.StopBoss();  // �������X�g�b�v
                    BossAnim.StartDefeatAnim(); // �|���A�j���[�V�����Đ�
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);// ��]���Ȃ��悤�ɂ���
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
                Debug.Log("���ʃA�j���[�V�����I��");
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
