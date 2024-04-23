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
        //“G‚Ì¶¬‚ğ‚·‚é
        if(InstCount <= 0.0f)
        {
            if(EnemyObjects.Length > 0)
            {
                //“G‚ğ”‚¾‚¯¶¬
                for(int i = 0;i < EnemyInstNum;i++)
                {
                    int EnemyNum = Random.Range(0, EnemyObjects.Length);
                    Instantiate(EnemyObjects[EnemyNum], transform.position, transform.rotation);
                    if(false)//“G‚ÌãŒÀ‚É‚È‚Á‚Ä‚¢‚ê‚Î
                    {
                        break;
                    }
                }
            }

            //ƒJƒEƒ“ƒg‚ÌXV
            if (BoostCount > 0.0f)
            {
                //“G‚ğ¶¬‚·‚éŠÔŠu‚Í’·‚­
                InstCount = manager.GetInstTime();
            }
            else//ƒu[ƒXƒg‚ªŠ|‚©‚Á‚Ä‚¢‚é
            {
                //“G‚Ì¶¬‚·‚éŠ´Šo‚Í’Z‚­
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
