using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // �I�X�e�[�W�̃f�[�^
    public bool[] BeeIsNormalCleared = new bool[(int)SystemLevelManager.LEVELS.MAX];          // �N���A
    public bool[] BeeIsPerfectCleared = new bool[(int)SystemLevelManager.LEVELS.MAX];   // ���S�N���A
    public float[] BeeCrearedTime = new float[(int)SystemLevelManager.LEVELS.MAX];      // �N���A����
    public bool[] BeeCrealedCondition1 = new bool[(int)SystemLevelManager.LEVELS.MAX];  // ����1(�{��)
    public bool[] BeeCrealedCondition2 = new bool[(int)SystemLevelManager.LEVELS.MAX];  // ����2(�v���C���[HP)
    public bool[] BeeCrealedCondition3 = new bool[(int)SystemLevelManager.LEVELS.MAX];  // ����3(�G�l�~�[�j��)

    // ���J�f�X�e�[�W�̃f�[�^
    public bool[] CentipedeIsNormalCleared = new bool[(int)SystemLevelManager.LEVELS.MAX];
    public bool[] CentipedeIsPerfectCleared = new bool[(int)SystemLevelManager.LEVELS.MAX];
    public float[] CentipedeCrearedTime = new float[(int)SystemLevelManager.LEVELS.MAX];
    public bool[] CentipedeCrealedCondition1 = new bool[(int)SystemLevelManager.LEVELS.MAX];
    public bool[] CentipedeCrealedCondition2 = new bool[(int)SystemLevelManager.LEVELS.MAX];
    public bool[] CentipedeCrealedCondition3 = new bool[(int)SystemLevelManager.LEVELS.MAX];

    // �t���R���K�V�X�e�[�W�̃f�[�^
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
