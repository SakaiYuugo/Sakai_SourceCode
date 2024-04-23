using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieCount : MonoBehaviour
{
    public static uint EnemyAllDeadCount = 0;
    public static uint EnemyDeadCount;

    private void Awake()
    {
        EnemyDeadCount = 0;
    }

    public static void DeadCount()
    {
        EnemyDeadCount++;

        if(!TutorialManager.TutorialNow)
            EnemyAllDeadCount++;
    }
}
