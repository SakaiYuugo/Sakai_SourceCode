using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemySpawn : ZakoEnemySpawn
{
    //�΂�T���n�̒l
    //***** ���x�֌W ***** 
    [SerializeField, Header("�ŏ����x")]
    private float MinHeight = 1.0f;
    [SerializeField, Header("�ō����x")]
    private float MaxHeight = 10.0f;
    [SerializeField, Header("���x�̕�����")]
    private float HeightLane = 5.0f;
    private float height;

    //***** �����֌W *****
    [SerializeField, Header("�ŏ�����")]
    private int MinDistance = 1;
    [SerializeField, Header("�ő勗��")]
    private int MaxDistance = 10;
    [SerializeField, Header("�����̕�����")]
    private int DistanceLane = 5;
    private int Distance;

    //***** �΂�T���͈� *****
    [SerializeField, Header("�΂�T���͈̔�")]
    private float range;
    [SerializeField] int LotScatterTime = 120;   //����I�ɑ�ʂɂ΂�T���p(�b���Z)
    [SerializeField] int OverScatterTime = 600;  //10����̃t���O(�Q�[���I�[�o�[)
    Vector3Int NormalSpawn = new Vector3Int(5, 7, 10);
    Vector3Int LotSpawn = new Vector3Int(10, 15, 20);
    [SerializeField] Vector3Int OverSpawn = new Vector3Int(10, 20, 30);
    [SerializeField] Vector3Int RareRandSpawn = new Vector3Int(5, 10, 15);
    private int NumScatter;                      //�ʏ�΂�T����
    private int NumLotScatter;                   //����2����̑�ʂ΂�T���p��
    private int RareEnemyRand;                   //���A�G���o��m��
    private GameObject Enemy;                    //�΂�T���G
    private int SelectEnemy;                     //�����_���ŎZ�o���ꂽ�G
    private Rigidbody rd;
    private BossHP BossHp;                       //�{�X�̗̑̓N���X
    private float BossHpRate;                    //�{�X�̗̑͂̊���
    private bool HalfSpawn;                      //�{�X�̗̑͂�50����25���ȉ��̎��̃t���O
    private bool HalfHalfSpawn;
    [SerializeField] string HalfHPNews;          //�̗͂�50���̎��̃j���[�X
    [SerializeField] string HalfHalfNews;        //�̗͂�25���̎��̃j���[�X
    [SerializeField] string TwoMinutesNews;      //2���o�ߎ��̃j���[�X
    private float oldCount = 0.0f;
    private float oldLotCount = 0.0f;
    private float oldOverCount = 0.0f;

    //�{�X�̃t���R���K�V�֘A
    DungBeetleState state;
    bool DungBeetleStage = false;

    protected override void Awake()
    {
        base.Awake();        
    }    

    protected override void OnEnable()
    {
        base.OnEnable();

        // ���ς��v�Z
        height = (MaxHeight - MinHeight) / HeightLane;         // ���x
        Distance = (MaxDistance - MinDistance) / DistanceLane;   // ����

        //LotScatterTime *= 60;
        //OverScatterTime *= 60;
        
        HalfSpawn = false;
        HalfHalfSpawn = false;
        

        //�x�[�X�N���X�œ�Փx�̎擾
        //�ʏ�΂�T�����A2���ゲ�Ƃ΂�T�����A10����̕ύX�͉��ŁA��Փx�Ń��A�G�m��
        switch(Level)
        {
            case 0:
                NumScatter = NormalSpawn.x;
                NumLotScatter = LotSpawn.x;
                RareEnemyRand = RareRandSpawn.x;
                break;

            case 1:
                NumScatter = NormalSpawn.y;
                NumLotScatter = LotSpawn.y;
                RareEnemyRand = RareRandSpawn.y;
                break;

            case 2:
                NumScatter = NormalSpawn.z;
                NumLotScatter = LotSpawn.z;
                RareEnemyRand = RareRandSpawn.z;
                break;

            default:
                break;
        }

        if (this.transform.name == "Boss_DungBeetle")
        {
            state = GetComponent<DungBeetleState>();
            DungBeetleStage = true;
        }
        else state = null;

    }

    private void Start()
    {
        BossHp = System_ObjectManager.bossHp;
    }

    protected override void FixedUpdate()
    {
        if (StartSpown && StartCount > 10)
        {
            NormalScatter(NumScatter);
            StartSpown = false;
        }

        BossHpRate = (float)BossHp.GetNowHP() / (float)BossHp.GetMaxHP();   //�{�X�̗̑͊����v�Z
        
        //�J�E���g
        StartCount++;

        oldCount = Count;
        oldLotCount = LotCount;
        oldOverCount = OverCount;
        Counter(true);

        //�{�X�̃t���R���K�V���X�^����Ԃ̏ꍇ
        if (DungBeetleStage && state.state == DungBeetleState.DungBeetleSTATE.m_STUN)
        {
            Counter(false);
            if ((Count > interval) || (LotCount > LotScatterTime) || (OverCount > OverScatterTime))
                Counter(true);
        }
                
        if(Count > interval)
        {//�ʏ�                 
            NormalScatter(NumScatter);
            Count = 0.0f;
        }
        if(LotCount > LotScatterTime)
        {//�Q������
            News(TwoMinutesNews);
            NormalScatter(NumLotScatter);
            LotCount = 0.0f;
        }
        if(OverCount > OverScatterTime)
        {//10���o��
            OverScatter();
            OverCount = 0.0f;
        }

        UnderHalf(BossHpRate);          //�{�X�̗̑͂�5����؂����u��
        UnderHalfHalf(BossHpRate);      //�{�X�̗̑͂�2.5����؂����u��
        
    }

    //�΂�T���p�̊֐�(1����)
    void Scatter()
    {
        if (Enemy != null)
        {            
            rd = Enemy.GetComponent<Rigidbody>();                  

            // �����̌v�Z
            float temp = (MinDistance + (Distance * Random.Range(1, DistanceLane + 1)));

            Vector3 vector1 = new Vector3(0.0f, 0.0f, temp * 100);   //��΂��������ȁH

            // �΂�T�����͈�
            float radius = Random.Range(-range, range);
            radius *= 3.14f / 180.0f;

            Vector3 vector2 = new Vector3(
                        vector1.x * Mathf.Cos(radius) - vector1.z * Mathf.Sin(radius),
                        vector1.y,
                        vector1.x * Mathf.Sin(radius) + vector1.z * Mathf.Cos(radius));

            // ���x�x�N�g��
            rd.AddForce((vector2 * 3) , ForceMode.Impulse);

            // �u���I�ɗ͂�^����(���x�̌v�Z)
            rd.AddForce(new Vector3(0.0f,
                                    (MinHeight + (height * Random.Range(1, HeightLane + 1) * 3)),
                                    0.0f), ForceMode.Impulse);
        }
    }

    //�ʏ�΂�T��
    void NormalScatter(int Num)
    {
        Vector3 distance = new Vector3(0, 10, 0);
        
        SpawnPos = transform.position + distance;  //�o���ʒu�m��

        for(int i = 0; i < Num; i++)
        {
            
            //�����Ŋ֐��Ă�Ŕ���
            SelectEnemy = Random.Range(0, 4);   //�����o�������邩�m��(Atk,Def,Sup,Rare,int�̍ő�l�͔͈͂Ɋ܂܂�Ȃ��炵���̂Ńo�b�t�@)

            //���A�G�̊m�������p
            int rand = Random.Range(0, 100);

            //�����𖞂����Ă����琶��(���ȃv���O�����������Ă��܂���...���Ԃ������璼���[)                        
            if (EnemyPrefab[SelectEnemy].name.Contains("EnemyAssault") && AttackEnemyCount < MaxAttackEnemy)
            {
                if (SelectEnemy < 3) { Enemy = EnemyCreater.Create(0, SpawnPos, Quaternion.identity); }
                else if (rand < RareEnemyRand) { Enemy = RareEnemyCreater.Create(SpawnPos, Quaternion.identity); }
                else Survey = true;
            }                
            else if (EnemyPrefab[SelectEnemy].name.Contains("EnemyDef") && DefEnemyCount < MaxDefEnemy)
            {
                if (SelectEnemy < 3) { Enemy = EnemyCreater.Create(1, SpawnPos, Quaternion.identity); }
                else if (rand < RareEnemyRand) { Enemy = RareEnemyCreater.Create(SpawnPos, Quaternion.identity); }
                else Survey = true;
            }
            else if (EnemyPrefab[SelectEnemy].name.Contains("EnemySupport") && SupportEnemyCount < MaxSupportEnemy)
            {
                if (SelectEnemy < 3) { Enemy = EnemyCreater.Create(SupEnemySpown(), SpawnPos, Quaternion.identity); }
                else if (rand < RareEnemyRand) { Enemy = RareEnemyCreater.Create(SpawnPos, Quaternion.identity); }
                else Survey = true;
            }                
            else Survey = true;
            
            //�����_���őI�΂ꂽ�v�f������𒴂��Ă����ꍇ0���猟�����Ă�������ɒB���Ă��Ȃ����m���������炻��𐶐�
            if (Survey != false)
            {
                for (int j = 0; j < 3; j++)
                {
                    if ((EnemyPrefab[j].name.Contains("EnemyAssault") && AttackEnemyCount >= MaxAttackEnemy)
                        || (EnemyPrefab[j].name.Contains("EnemyDef") && DefEnemyCount >= MaxDefEnemy)
                        || (EnemyPrefab[j].name.Contains("EnemySupport") && SupportEnemyCount >= MaxSupportEnemy))
                    {
                        Enemy = null;
                        continue;
                    }
                    else
                    {
                        if(j != 2) Enemy = EnemyCreater.Create(j, SpawnPos, Quaternion.identity);
                        else Enemy = EnemyCreater.Create(SupEnemySpown(), SpawnPos, Quaternion.identity);

                        break;
                    }
                }
                Survey = false;
            }
            
            

            Scatter();
        }
    }
 
    //����10����ɕύX
    void OverScatter()
    {
        //�ʏ�΂�T�������Փx�ŕύX
        switch(Level)
        {
            case 0:
                NumScatter = OverSpawn.x;
                MaxAttackEnemy *= 2;
                MaxDefEnemy *= 2;
                MaxSupportEnemy *= 2;
                break;

            case 1:
                NumScatter = OverSpawn.y;
                MaxAttackEnemy *= 2;
                MaxDefEnemy *= 2;
                MaxSupportEnemy *= 2;
                break;

            case 2:
                NumScatter = OverSpawn.z;
                MaxAttackEnemy *= 2;
                MaxDefEnemy *= 2;
                MaxSupportEnemy *= 2;

                break;

            default:
                break;
        }
    }

    void UnderHalf(float ResidualHp)
    {
       if((ResidualHp < 0.5f) && (HalfSpawn == false))
       {
            News(HalfHPNews);
            NormalScatter(NumLotScatter);
            HalfSpawn = true;
       }
    }

    void  UnderHalfHalf(float ResidualHp)
    {
        if((ResidualHp < 0.25f) && (HalfHalfSpawn == false))
        {
            News(HalfHalfNews);
            NormalScatter(NumLotScatter);
            HalfHalfSpawn = true;
        }        
    }

    void Counter(bool count)
    {        
        if(count)
        {            
            Count += Time.deltaTime;
            LotCount += Time.deltaTime;
            OverCount += Time.deltaTime;
        }           
        else
        {
            Count = oldCount;
            oldLotCount = LotCount;
            oldOverCount = OverCount;
        }
    }
 
}
