using UnityEngine;

public class DungBeetleState : BossHP
{
    DungBeetle_Scales _DungBeetle_Scales;    //���S�ϑ������ؕ��U��
    Rigidbody rb;
    int AddRgFrame; //�|����Ă���d�͂�������܂ł̗P�\����

    DungManager Manager;
    DungBarrier _DungBarrier;
    BossHP _HP;
    GameObject _DungBall;
    [SerializeField,ReadOnly] int nConstAtkWaitFrame;   //�U�������Ɉړ����鎞��
    [SerializeField, ReadOnly] int nAtkWaitFrame = 0;  //�́A�t���[���J�E���g�p
    [SerializeField] float fGuardHP;    //�K�[�h��Ԃ̗̑�

    //�X�^���֘A
    [SerializeField] int nConstStunFrame;   //�|���A�j��
    [SerializeField] int nConstStunReturnFrame; //�N���オ��A�j��
    [SerializeField, ReadOnly] int nStunFrame = 0;
    [ReadOnly] public bool bChangeState = true;
    BoxCollider _BoxCollider;   //�X�^�����̓����蔻�蒲��
    Vector3 CollderOriginalCenter;  //���̓����蔻��
    Vector3 CollderOriginalSize;  //���̓����蔻��
    Vector3 CollderStunCenter = new Vector3(-0.23f, 8.8f, -11.1f);  //�X�^�����̂̓����蔻��
    Vector3 CollderStunSize = new Vector3(8.3f, 13.7f, 29.6f);  //�X�^�����̂̓����蔻��    

    [ReadOnly] public int nGoPoint;   //�|�C���g�̉��ԂɌ�������

    public enum DungBeetleSTATE
    {
        m_ROTATION, //������]
        m_MOVE, //�ʏ�ړ�
        m_STUN, //�����j�󂳂�ăX�^�����
        m_STUN_RETURN,  //�X�^������̕��A
        m_ROLLSTAR,  //���܂��΂��U��
        m_UP_IMPACT,    //���ɂ��Ռ��g
        m_GEKKOUTYOU,   //�H���L���ėؕ����~�炷
        m_DEATH,    //�|���ꂽ
    }

    [ReadOnly] public DungBeetleSTATE state;   //state�Ǘ�


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
        //�U�����ԂɂȂ�����
        if (nConstAtkWaitFrame <= nAtkWaitFrame)
        {
            nConstAtkWaitFrame = Random.Range(600, 900);    //���̍U���N�[���^�C����ݒ�
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

        //�{�X��|�����I(1�x��������)   
        if(state != DungBeetleSTATE.m_DEATH && nHP <= 0)
        {
            Debug.Log("�N���A");
            state = DungBeetleSTATE.m_DEATH;
            bChangeState = true;
            Manager._DBDefeat.enabled = true;
        }


        //��x�������s
        if (bChangeState)
        {
            switch (state)
            {
                case DungBeetleSTATE.m_ROTATION:
                    //��]���͕������Z�����
                    Manager._DBRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    break;
                case DungBeetleSTATE.m_MOVE:
                    //�ړ����͏㉺�̂ݕ������Z���Ă���
                    Manager._DBRigidbody.constraints = RigidbodyConstraints.None;
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.None;
                    Manager._DBRigidbody.constraints = RigidbodyConstraints.FreezeRotation |
                        RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.FreezeRotation | 
                        RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

                    break;

                case DungBeetleSTATE.m_STUN:
                    //�X�^�����͍��W�̂ݓ����Ă���
                    Manager._DBRigidbody.constraints = RigidbodyConstraints.None;
                    Manager._DBRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

                    _DungBall.SetActive(false);
                    _BoxCollider.center = CollderStunCenter;
                    _BoxCollider.size = CollderStunSize;
                    Manager._DungAnime.StunAnime();

                    nStunFrame = 0;
                    break;


                case DungBeetleSTATE.m_STUN_RETURN:
                    Debug.Log("�N���オ��");
                    Manager._DungAnime.WakeUpAnime();
                    Manager._Db.Reproduction();

                    nStunFrame = 0;
                    break;

                case DungBeetleSTATE.m_ROLLSTAR: //���̓K�[�h��ԂɂȂ�
                    Debug.Log("���[���X�^�[");
                    //�����Œ��̗\������
                    Manager._DbRollstar.StartAtk(Manager._DBMove.GetGoPos());
                    break;

                case DungBeetleSTATE.m_UP_IMPACT:
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.None;
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
                    Debug.Log("�Ռ��g");
                    //�����Œ����̗\������
                    Manager._DbUpImpact.GetComponent<DungBallUpImpact>().StartAtk();                  
                    break;

                case DungBeetleSTATE.m_GEKKOUTYOU:
                    Debug.Log("������");
                    _DungBeetle_Scales.SetScales(this.gameObject, _DungBall);
                    break;

                case DungBeetleSTATE.m_DEATH:
                    Manager._DungAnime.DestroyAnime();
                    Manager._DBRigidbody.constraints = RigidbodyConstraints.FreezeAll;  //���W�b�g�{�f�B�ɂ��^�����~�߂�
                    Manager._DbRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    GetComponent<FallPrevention>().enabled = false; //������悤�ɂ���
                    GetComponent<BoxCollider>().enabled = false; //������悤�ɂ���
                    Debug.Log("���S");
                    break;
            }
            bChangeState = false;
        }

        //���t���[������
        switch (state)
        {
            case DungBeetleSTATE.m_ROTATION:    //�����ς��鎞�͎哱�����{�[���ɓn��
                Manager._DBMove.Turnaround();
                Manager._Db.Turnaround();

                break;
            case DungBeetleSTATE.m_MOVE:    //���Ɏ哱��
                Manager._DungSound.Sound(DungSound.DUNG_SOUND.SOUND_WALK);  //���Đ�
                Manager._DBMove.Move();
                Manager._Db.FollowBeetle(Manager._DBMove.fDistance);
                nAtkWaitFrame++;
                break;
            case DungBeetleSTATE.m_STUN:

                nStunFrame++;
                if (nStunFrame >= nConstStunFrame)
                {   //�X�^�����ԏI��
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
        if(GetComponent<DungBarrier>().bBarrier)    //�_���[�W���y�������ԂȂ�
        {
            nHP -= 1;   //�K�[�h��ԂȂ�1�_���Œ�
        }
        else
        {
            nHP -= damage;
        }

        hpBarController.SetHp(nHP);
    }


}
