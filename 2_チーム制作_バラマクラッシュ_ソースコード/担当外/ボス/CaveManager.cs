using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveManager : MonoBehaviour
{
    [SerializeField]
    SystemLevelManager.LEVELS EnabledLevel = SystemLevelManager.LEVELS.EASY;
    [SerializeField]
    Vector3Int LevelInstEnemyNum = new Vector3Int(5, 10, 15);                       //�ʏ펞�̓G�̐�����
    [SerializeField]
    Vector3Int LevelInstEnemyNum_HarfHP = new Vector3Int(10,15,20);                 //�{�X�̗̑͂������ɂȂ������̓G�̐�����
    [SerializeField]
    float InstIntervalTime_Second = 30.0f;                                          //�ʏ�̎��̐�������
    [SerializeField]
    float BoostInstIntervalTime_Second = 10.0f;                                     //�u�[�X�g��Ԃ̐�������
    [SerializeField]
    float BoostTime_Minute = 10.0f;                                                 //�u�[�X�g��ԂɂȂ�܂ł̎���
    [SerializeField]
    string BossName = "Boss";
    [SerializeField]
    GameObject[] EventImage;
    [SerializeField]
    GameObject CaveSmokes;

    GameObject Boss;
    BossHP bossHP;
    bool BossHarfHP;

    // Start is called before the first frame update
    void Start()
    {
        //���x���ɉ����Ďg�����g��Ȃ��������߂�
        int nowStageLevel = SystemLevelManager.GetLevel();          //���̃��x��
        int EnabledLevelNum = (int)EnabledLevel;                    //�L���ɂȂ�Œ჌�x��

        //���̃��x�����L���ȃ��x�����Ⴏ���
        if (nowStageLevel < EnabledLevelNum)
        {
            Destroy(this.gameObject);
            return;
        }

        BossHarfHP = false;
        Boss = System_ObjectManager.bossObject;
        bossHP = System_ObjectManager.bossHp;

        transform.root.GetComponent<CaveCount>().CaveAdd();

        CaveSmokes.SetActive(false);
    }
    private void FixedUpdate()
    {                
        bool NowBossHarf = BossHarfHP;
        
        //�����ɂȂ��Ă��邩
        BossHarfHP = (float)bossHP.GetNowHP() / (float)bossHP.GetMaxHP() <= 0.5f;

        //�{�X��HP�������ɂȂ����u�Ԃ̂�
        if(!NowBossHarf && BossHarfHP)
        {
            CaveSmokes.SetActive(true);

            GetComponent<Cave_LevelInstEnemy>().SetEnemyNum(
                GetInstEnemyNum());            
        }
    }

    public int GetInstEnemyNum()
    {
        int EnemyInstNum = 0;

        if (!BossHarfHP)
        {
            //�{�X��HP��������
            switch (SystemLevelManager.GetLevel_enum())
            {
                case SystemLevelManager.LEVELS.EASY:
                    EnemyInstNum = LevelInstEnemyNum.x;
                    break;
                case SystemLevelManager.LEVELS.NORMAL:
                    EnemyInstNum = LevelInstEnemyNum.y;
                    break;
                case SystemLevelManager.LEVELS.HARD:
                    EnemyInstNum = LevelInstEnemyNum.z;
                    break;
            }
        }
        else
        {
            //�{�X��HP�������ȉ�
            switch (SystemLevelManager.GetLevel_enum())
            {
                case SystemLevelManager.LEVELS.EASY:
                    EnemyInstNum = LevelInstEnemyNum_HarfHP.x;
                    break;
                case SystemLevelManager.LEVELS.NORMAL:
                    EnemyInstNum = LevelInstEnemyNum_HarfHP.y;
                    break;
                case SystemLevelManager.LEVELS.HARD:
                    EnemyInstNum = LevelInstEnemyNum_HarfHP.z;
                    break;
            }
        }

        return EnemyInstNum;
    }

    //�G�𐶐�����Ԋu�̎���
    public float GetInstTime()
    {
        return InstIntervalTime_Second;
    }

    //�u�[�X�Ƃ��Ă��鎞�ɓG�𐶐�����Ԋu�̎���
    public float GetBoostInstTime()
    {
        return BoostInstIntervalTime_Second;
    }

    //�����Ńu�[�X�g���邩
    public float GetBoostTime()
    {
        return BoostTime_Minute * 60.0f;
    }

    public float GetBossHP()
    {
        return bossHP.GetNowHP();
    }

    public float GetBossHPPercent()
    {
        return bossHP.GetMaxHP();
    }

    //�{���������������̏���
    public void HitBomb()
    {
        if(BossHarfHP)
        {
            transform.root.GetComponent<CaveCount>().CaveMina();
            //�����ɂȂ��Ă���
            //����
            Destroy(this.gameObject);
        }
    }

    public bool BossHalfHp()
    {
        if (BossHarfHP) return true;
        else return false;
    }
}
