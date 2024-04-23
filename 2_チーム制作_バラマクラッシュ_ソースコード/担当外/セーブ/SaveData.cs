using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // 蜂ステージのデータ
    public bool[] BeeIsNormalCleared = new bool[(int)SystemLevelManager.LEVELS.MAX];          // クリア
    public bool[] BeeIsPerfectCleared = new bool[(int)SystemLevelManager.LEVELS.MAX];   // 完全クリア
    public float[] BeeCrearedTime = new float[(int)SystemLevelManager.LEVELS.MAX];      // クリア時間
    public bool[] BeeCrealedCondition1 = new bool[(int)SystemLevelManager.LEVELS.MAX];  // 条件1(ボム)
    public bool[] BeeCrealedCondition2 = new bool[(int)SystemLevelManager.LEVELS.MAX];  // 条件2(プレイヤーHP)
    public bool[] BeeCrealedCondition3 = new bool[(int)SystemLevelManager.LEVELS.MAX];  // 条件3(エネミー破壊)

    // ムカデステージのデータ
    public bool[] CentipedeIsNormalCleared = new bool[(int)SystemLevelManager.LEVELS.MAX];
    public bool[] CentipedeIsPerfectCleared = new bool[(int)SystemLevelManager.LEVELS.MAX];
    public float[] CentipedeCrearedTime = new float[(int)SystemLevelManager.LEVELS.MAX];
    public bool[] CentipedeCrealedCondition1 = new bool[(int)SystemLevelManager.LEVELS.MAX];
    public bool[] CentipedeCrealedCondition2 = new bool[(int)SystemLevelManager.LEVELS.MAX];
    public bool[] CentipedeCrealedCondition3 = new bool[(int)SystemLevelManager.LEVELS.MAX];

    // フンコロガシステージのデータ
    public bool[] DungBeetleIsNormalCleared = new bool[(int)SystemLevelManager.LEVELS.MAX];
    public bool[] DungBeetleIsPerfectCleared = new bool[(int)SystemLevelManager.LEVELS.MAX];
    public float[] DungBeetleCrearedTime = new float[(int)SystemLevelManager.LEVELS.MAX];
    public bool[] DungBeetleCrealedCondition1 = new bool[(int)SystemLevelManager.LEVELS.MAX];
    public bool[] DungBeetleCrealedCondition2 = new bool[(int)SystemLevelManager.LEVELS.MAX];
    public bool[] DungBeetleCrealedCondition3 = new bool[(int)SystemLevelManager.LEVELS.MAX];

    public bool GetAllClear()
    {
        for(int i = 0;i < (int)SystemLevelManager.LEVELS.MAX;i++)
        {
            if(!BeeIsNormalCleared[i])
            {
                return false;
            }            
        }

        for (int i = 0; i < (int)SystemLevelManager.LEVELS.MAX; i++)
        {
            if (!CentipedeIsNormalCleared[i])
            {
                return false;
            }
        }

        for (int i = 0; i < (int)SystemLevelManager.LEVELS.MAX; i++)
        {
            if (!DungBeetleIsNormalCleared[i])
            {
                return false;
            }
        }

        return true;
    }
}
