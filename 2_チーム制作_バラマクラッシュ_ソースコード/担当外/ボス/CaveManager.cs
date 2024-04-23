using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveManager : MonoBehaviour
{
    [SerializeField]
    SystemLevelManager.LEVELS EnabledLevel = SystemLevelManager.LEVELS.EASY;
    [SerializeField]
    Vector3Int LevelInstEnemyNum = new Vector3Int(5, 10, 15);                       //通常時の敵の生成量
    [SerializeField]
    Vector3Int LevelInstEnemyNum_HarfHP = new Vector3Int(10,15,20);                 //ボスの体力が半分になった時の敵の生成量
    [SerializeField]
    float InstIntervalTime_Second = 30.0f;                                          //通常の時の生成時間
    [SerializeField]
    float BoostInstIntervalTime_Second = 10.0f;                                     //ブースト状態の生成時間
    [SerializeField]
    float BoostTime_Minute = 10.0f;                                                 //ブースト状態になるまでの時間
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
        //レベルに沿って使うか使わないかを決める
        int nowStageLevel = SystemLevelManager.GetLevel();          //今のレベル
        int EnabledLevelNum = (int)EnabledLevel;                    //有効になる最低レベル

        //今のレベルが有効なレベルより低ければ
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
        
        //半分になっているか
        BossHarfHP = (float)bossHP.GetNowHP() / (float)bossHP.GetMaxHP() <= 0.5f;

        //ボスのHPが半分になった瞬間のみ
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
            //ボスのHPが半分上
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
            //ボスのHPが半分以下
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

    //敵を生成する間隔の時間
    public float GetInstTime()
    {
        return InstIntervalTime_Second;
    }

    //ブースとしている時に敵を生成する間隔の時間
    public float GetBoostInstTime()
    {
        return BoostInstIntervalTime_Second;
    }

    //何分でブーストするか
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

    //ボムが当たった時の処理
    public void HitBomb()
    {
        if(BossHarfHP)
        {
            transform.root.GetComponent<CaveCount>().CaveMina();
            //半分になっている
            //壊れる
            Destroy(this.gameObject);
        }
    }

    public bool BossHalfHp()
    {
        if (BossHarfHP) return true;
        else return false;
    }
}
