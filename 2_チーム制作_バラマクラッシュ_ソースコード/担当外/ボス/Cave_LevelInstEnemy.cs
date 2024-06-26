using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave_LevelInstEnemy : MonoBehaviour
{
    [SerializeField]
    GameObject[] EnemyObjects;

    CaveManager manager;
    int EnemyInstNum;
    float InstCount;
    float BoostCount;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<CaveManager>();
        EnemyInstNum = manager.GetInstEnemyNum();
        InstCount = Random.Range(0.0f, manager.GetInstTime());
        BoostCount = manager.GetBoostTime();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //敵の生成をする
        if(InstCount <= 0.0f)
        {
            if(EnemyObjects.Length > 0)
            {
                //敵を数だけ生成
                for(int i = 0;i < EnemyInstNum;i++)
                {
                    int EnemyNum = Random.Range(0, EnemyObjects.Length);
                    Instantiate(EnemyObjects[EnemyNum], transform.position, transform.rotation);
                    if(false)//敵の上限になっていれば
                    {
                        break;
                    }
                }
            }

            //カウントの更新
            if (BoostCount > 0.0f)
            {
                //敵を生成する間隔は長く
                InstCount = manager.GetInstTime();
            }
            else//ブーストが掛かっている
            {
                //敵の生成する感覚は短く
                InstCount = manager.GetBoostInstTime();
            }
        }

        if(BoostCount > 0.0f)
        {
            BoostCount -= Time.deltaTime;
        }
        InstCount -= Time.deltaTime;
    }

    public void SetEnemyNum(int num)
    {
        EnemyInstNum = num;
    }
}
