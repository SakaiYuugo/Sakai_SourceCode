using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEnemySpawn : ZakoEnemySpawn
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

    [SerializeField] int OverScatterTime = 600;  //10����̃t���O(�Q�[���I�[�o�[)
    Vector3Int NormalSpawn = new Vector3Int(5, 7, 10);
    [SerializeField] Vector3Int RareRandSpawn = new Vector3Int(5, 10, 20);
    Vector3Int BossHalfHPSpawn = new Vector3Int(7, 10, 15);
    private int NumScatter;                      //�ʏ�΂�T����
    private int RareEnemyRand;                   //���A�G���o��m��
    private GameObject Enemy;                    //�΂�T���G
    private int SelectEnemy;                     //�����_���ŎZ�o���ꂽ�G
    private float rand;                            //���������炷�ׂ̃����_��
    private float BossHpPersent;                 //�{�X�̎c��HP�̊���
    private bool HalfHp;                         //�{�X�̗̑͂�5����؂��Ă��邩
    private CaveManager Hp;                      //���A�̃}�l�[�W���[���擾
    [SerializeField] string BossHalfHpNews;      //�{�X�̗̑͂�50���̎��̃j���[�X

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //�����Ń{�X�̎c��Hp�ɃA�N�Z�X�ł���悤�ɂ���
        HalfHp = false;
        Hp = GetComponentInParent<CaveManager>();

        // ���ς��v�Z
        height = (MaxHeight - MinHeight) / HeightLane;         // ���x
        Distance = (MaxDistance - MinDistance) / DistanceLane;   // ����

        //OverScatterTime *= 60;

        rand = Random.Range(0, 3);

        //�x�[�X�N���X�œ�Փx�̎擾
        //�ʏ�΂�T�����A2���ゲ�Ƃ΂�T�����A10����̕ύX�͉��ŁA��Փx�Ń��A�G�m��
        switch (Level)
        {
            case 0:
                NumScatter = NormalSpawn.x;
                RareEnemyRand = RareRandSpawn.x;
                break;

            case 1:
                NumScatter = NormalSpawn.y;
                RareEnemyRand = RareRandSpawn.y;
                break;

            case 2:
                NumScatter = NormalSpawn.z;
                RareEnemyRand = RareRandSpawn.z;
                break;

            default:
                break;
        }
    }

    protected override void FixedUpdate()
    { 
        if (!TutorialManager.TutorialNow)
        {
            if (StartSpown && StartCount > 10)
            {
                NormalScatter(NumScatter);
                StartSpown = false;
            }

            //�J�E���g
            StartCount++;
            Count += Time.deltaTime;
            OverCount += Time.deltaTime;

            if (Count > (interval + rand))
            {//�ʏ�
                NormalScatter(NumScatter);
                Count = 0.0f;
            }
            if (OverCount > OverScatterTime)
            {//10���o��
                OverScatter();
                OverCount = 0.0f;
            }



            //�{�X�̗̑͂��T����؂��Ă������̏����[�o�O���o��̂Ń`���[�g���A���̂���܂������߂�Ȃ���
            if (Hp.BossHalfHp() != false && HalfHp == false)
            {
                UnderHalf();
                HalfHp = true;
            }
        }       
    }

    void Scatter()
    {
        if (Enemy != null)
        {
            // ���e�̃X�e�[�^�X���擾(��΂�����)
            Rigidbody rd = Enemy.GetComponent<Rigidbody>();

            // �����̌v�Z
            float temp = (MinDistance + (Distance * Random.Range(1, DistanceLane + 1)));

            Vector3 vector1 = new Vector3(0.0f, 0.0f, temp);   //��΂��������ȁH

            // �΂�T�����͈�
            float radius = Random.Range(-range, range);
            radius *= 3.14f / 180.0f;

            Vector3 vector2 = new Vector3(
                        vector1.x * Mathf.Cos(radius) - vector1.z * Mathf.Sin(radius),
                        vector1.y,
                        vector1.x * Mathf.Sin(radius) + vector1.z * Mathf.Cos(radius));

            // ���x�x�N�g��
            rd.AddForce((vector2 * 1), ForceMode.Impulse);

            // �u���I�ɗ͂�^����(���x�̌v�Z)
            rd.AddForce(new Vector3(0.0f,
                                    (MinHeight + (height * Random.Range(1, HeightLane + 1) * 3)),
                                    0.0f), ForceMode.Impulse);

        }
    }

    //�ʏ�΂�T��
    void NormalScatter(int Num)
    {
        if(EnemyPrefab.Length == 0)
        {
            return;
        }

        Vector3 distance = new Vector3(0, 0, 0);

        SpawnPos = transform.position + distance;  //�o���ʒu�m��

        for (int i = 0; i < Num; i++)
        {
            //�����Ŋ֐��Ă�Ŕ���
            SelectEnemy = Random.Range(0, 4);   //�����o�������邩�m��(Atk,Def,Sup,Rare)

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
                        if (j != 2) Enemy = EnemyCreater.Create(j, SpawnPos, Quaternion.identity);
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
        interval = 10;
        //�ʏ�΂�T�������Փx�ŕύX
        switch (Level)
        {
            case 0:
                MaxAttackEnemy *= 2;
                MaxDefEnemy *= 2;
                MaxSupportEnemy *= 2;
                break;

            case 1:
                MaxAttackEnemy *= 2;
                MaxDefEnemy *= 2;
                MaxSupportEnemy *= 2;
                break;

            case 2:
                MaxAttackEnemy *= 2;
                MaxDefEnemy *= 2;
                MaxSupportEnemy *= 2;

                break;

            default:
                break;
        }
    }

    //�{�X�̗̑͂�5����؂��Ă������̏���
    void UnderHalf()
    {
        News(BossHalfHpNews);

        switch (Level)
        {
            case 0:
                NumScatter = BossHalfHPSpawn.x;
                break;

            case 1:
                NumScatter = BossHalfHPSpawn.y;
                break;

            case 2:
                NumScatter = BossHalfHPSpawn.z;
                break;

            default:
                break;
        }
    }
    
}