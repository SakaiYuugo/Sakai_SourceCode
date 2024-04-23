using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemySpawn : ZakoEnemySpawn
{
    //ばら撒く系の値
    //***** 高度関係 ***** 
    [SerializeField, Header("最小高度")]
    private float MinHeight = 1.0f;
    [SerializeField, Header("最高高度")]
    private float MaxHeight = 10.0f;
    [SerializeField, Header("高度の分割数")]
    private float HeightLane = 5.0f;
    private float height;

    //***** 距離関係 *****
    [SerializeField, Header("最小距離")]
    private int MinDistance = 1;
    [SerializeField, Header("最大距離")]
    private int MaxDistance = 10;
    [SerializeField, Header("距離の分割数")]
    private int DistanceLane = 5;
    private int Distance;

    //***** ばら撒き範囲 *****
    [SerializeField, Header("ばら撒きの範囲")]
    private float range;
    [SerializeField] int LotScatterTime = 120;   //定期的に大量にばら撒く用(秒換算)
    [SerializeField] int OverScatterTime = 600;  //10分後のフラグ(ゲームオーバー)
    Vector3Int NormalSpawn = new Vector3Int(5, 7, 10);
    Vector3Int LotSpawn = new Vector3Int(10, 15, 20);
    [SerializeField] Vector3Int OverSpawn = new Vector3Int(10, 20, 30);
    [SerializeField] Vector3Int RareRandSpawn = new Vector3Int(5, 10, 15);
    private int NumScatter;                      //通常ばら撒き数
    private int NumLotScatter;                   //現状2分後の大量ばら撒き用数
    private int RareEnemyRand;                   //レア敵が出る確率
    private GameObject Enemy;                    //ばら撒く敵
    private int SelectEnemy;                     //ランダムで算出された敵
    private Rigidbody rd;
    private BossHP BossHp;                       //ボスの体力クラス
    private float BossHpRate;                    //ボスの体力の割合
    private bool HalfSpawn;                      //ボスの体力が50％と25％以下の時のフラグ
    private bool HalfHalfSpawn;
    [SerializeField] string HalfHPNews;          //体力が50％の時のニュース
    [SerializeField] string HalfHalfNews;        //体力が25％の時のニュース
    [SerializeField] string TwoMinutesNews;      //2分経過時のニュース
    private float oldCount = 0.0f;
    private float oldLotCount = 0.0f;
    private float oldOverCount = 0.0f;

    //ボスのフンコロガシ関連
    DungBeetleState state;
    bool DungBeetleStage = false;

    protected override void Awake()
    {
        base.Awake();        
    }    

    protected override void OnEnable()
    {
        base.OnEnable();

        // 平均を計算
        height = (MaxHeight - MinHeight) / HeightLane;         // 高度
        Distance = (MaxDistance - MinDistance) / DistanceLane;   // 距離

        //LotScatterTime *= 60;
        //OverScatterTime *= 60;
        
        HalfSpawn = false;
        HalfHalfSpawn = false;
        

        //ベースクラスで難易度の取得
        //通常ばら撒き数、2分後ごとばら撒き数、10分後の変更は下で、難易度でレア敵確率
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

        BossHpRate = (float)BossHp.GetNowHP() / (float)BossHp.GetMaxHP();   //ボスの体力割合計算
        
        //カウント
        StartCount++;

        oldCount = Count;
        oldLotCount = LotCount;
        oldOverCount = OverCount;
        Counter(true);

        //ボスのフンコロガシがスタン状態の場合
        if (DungBeetleStage && state.state == DungBeetleState.DungBeetleSTATE.m_STUN)
        {
            Counter(false);
            if ((Count > interval) || (LotCount > LotScatterTime) || (OverCount > OverScatterTime))
                Counter(true);
        }
                
        if(Count > interval)
        {//通常                 
            NormalScatter(NumScatter);
            Count = 0.0f;
        }
        if(LotCount > LotScatterTime)
        {//２分ごと
            News(TwoMinutesNews);
            NormalScatter(NumLotScatter);
            LotCount = 0.0f;
        }
        if(OverCount > OverScatterTime)
        {//10分経過
            OverScatter();
            OverCount = 0.0f;
        }

        UnderHalf(BossHpRate);          //ボスの体力が5割を切った瞬間
        UnderHalfHalf(BossHpRate);      //ボスの体力が2.5割を切った瞬間
        
    }

    //ばら撒き用の関数(1つずつ)
    void Scatter()
    {
        if (Enemy != null)
        {            
            rd = Enemy.GetComponent<Rigidbody>();                  

            // 距離の計算
            float temp = (MinDistance + (Distance * Random.Range(1, DistanceLane + 1)));

            Vector3 vector1 = new Vector3(0.0f, 0.0f, temp * 100);   //飛ばす距離かな？

            // ばら撒かれる範囲
            float radius = Random.Range(-range, range);
            radius *= 3.14f / 180.0f;

            Vector3 vector2 = new Vector3(
                        vector1.x * Mathf.Cos(radius) - vector1.z * Mathf.Sin(radius),
                        vector1.y,
                        vector1.x * Mathf.Sin(radius) + vector1.z * Mathf.Cos(radius));

            // 速度ベクトル
            rd.AddForce((vector2 * 3) , ForceMode.Impulse);

            // 瞬発的に力を与える(高度の計算)
            rd.AddForce(new Vector3(0.0f,
                                    (MinHeight + (height * Random.Range(1, HeightLane + 1) * 3)),
                                    0.0f), ForceMode.Impulse);
        }
    }

    //通常ばら撒き
    void NormalScatter(int Num)
    {
        Vector3 distance = new Vector3(0, 10, 0);
        
        SpawnPos = transform.position + distance;  //出現位置確定

        for(int i = 0; i < Num; i++)
        {
            
            //ここで関数呼んで判定
            SelectEnemy = Random.Range(0, 4);   //何を出現させるか確定(Atk,Def,Sup,Rare,intの最大値は範囲に含まれないらしいのでバッファ)

            //レア敵の確率調整用
            int rand = Random.Range(0, 100);

            //条件を満たしていたら生成(嫌なプログラムを書いてしまった...時間あったら直そー)                        
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
            
            //ランダムで選ばれた要素が上限を超えていた場合0から検査していき上限に達していないモノがあったらそれを生成
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
 
    //現状10分後に変更
    void OverScatter()
    {
        //通常ばら撒き数を難易度で変更
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
