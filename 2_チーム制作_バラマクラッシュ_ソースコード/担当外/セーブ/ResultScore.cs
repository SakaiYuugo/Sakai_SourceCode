using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScore : MonoBehaviour
{
    public struct StageResutl
    {
        public float GameTime;
        public int MaxBomb;
        public int PlayerHP;
        public int DestEnemyCount;
    }

    public static ResultScore instance = null;
    
    StageResutl stageResult;
    StageInfo.st_StageScore Before_Score;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        //�V������ɂ��Ă��
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        stageResult.GameTime = 0.0f;
        stageResult.MaxBomb = 0;
        stageResult.PlayerHP = 9;
        stageResult.DestEnemyCount = 0;
    }

    public void ResultRest()
    {
        stageResult.GameTime = 0.0f;
        stageResult.MaxBomb = 0;
        stageResult.PlayerHP = 9;
        stageResult.DestEnemyCount = 0;
    }

    //�{���̃��x�������Ă��
    public void SetMaxLevelBomb(int maxBomb)
    {
        stageResult.MaxBomb = maxBomb;
    }

    public void SetPlayerHP(int nowHP)
    {
        stageResult.PlayerHP = nowHP;
    }

    public void SetMaxEnemyDestCount(int EnemyNum)
    {
        stageResult.DestEnemyCount = EnemyNum;
    }

    public void SetGameTime(float time)
    {
        stageResult.GameTime = time;
    }

    //�Q�[���I�����ɌĂ�ł��A����ȊO�ŌĂԂƃX�e�[�W�ݒ�Ńo�O��
    public void SetSaveScore()
    {
        //�X�R�A���擾(�v�Z)
        StageInfo.st_StageScore nowScore = GetScore();  //������Z�[�u���镨�������Ă���
        Before_Score = GetSaveScore();                  //���Z�[�u����Ă�����e�������Ă���

        //�Z�[�u�����Ă��
        System_SaveManager.InData(StageSelect.E_GetNowSelect(), SystemLevelManager.GetLevel_enum()
            , true, nowScore.ClearTime,
#if true
            nowScore.Judge_TwoBomb,
            nowScore.Judge_HPOver,
            nowScore.Judge_EnemyTenDestroy
#else
            true,
            true,
            true
#endif
            );

        //���S�ɃZ�[�u�����Ă��
        System_SaveManager.Save();

        //�X�L�����X�V���Ă��
        ReleaseSkilManager.instance.SkilUpdate();
    }

    public StageResutl GetResult()
    {
        return stageResult;
    }

    public StageInfo.st_StageScore GetBeforeScore()
    {
        return Before_Score;
    }

    //���̃X�R�A�̌��ʂ�Ԃ�(���̔���)
    public StageInfo.st_StageScore GetScore()
    {
        int[] Hyouka_BombNum = { 2, 3, 4 };
        int[] Hyouka_HPNum = { 3, 5, 7 };
        int[] Hyouka_EnemyDestNum = { 10, 20, 30 };

        StageInfo.st_StageScore r_score;
        r_score.ClearTime = stageResult.GameTime;                                                                       //���Ԃ̐ݒ�
        r_score.isClear = true;                                                                                         //�N���A������

#if true
        r_score.Judge_EnemyTenDestroy = stageResult.DestEnemyCount >= Hyouka_EnemyDestNum[SystemLevelManager.GetLevel()];      //�G�l�~�[��|�����Ƃ��o������
        r_score.Judge_TwoBomb = stageResult.MaxBomb >= Hyouka_BombNum[SystemLevelManager.GetLevel()];                   //�{�����x�����グ�邱�Ƃ��o������
        r_score.Judge_HPOver = stageResult.PlayerHP >= Hyouka_HPNum[SystemLevelManager.GetLevel()];                     //HP���c�����Ƃ��o������
#else
        r_score.Judge_EnemyTenDestroy = true;   //�G�l�~�[��|�����Ƃ��o������
        r_score.Judge_TwoBomb = true;           //�{�����x�����グ�邱�Ƃ��o������
        r_score.Judge_HPOver = true;            //HP���c�����Ƃ��o������
#endif

        r_score.isPerfectClear =                                                                                        //�����ɃN���A������
            r_score.Judge_EnemyTenDestroy &&
            r_score.Judge_TwoBomb &&
            r_score.Judge_HPOver;

        return r_score;
    }

    public StageInfo.st_StageScore GetSaveScore()
    {
        StageInfo.st_StageScore saveScore;

        switch(StageSelect.E_GetNowSelect())
        {
            case StageSelect.E_STAGE.BEE_STAGE:
                {
                    saveScore.isClear = System_SaveManager.savedata.                BeeIsNormalCleared[SystemLevelManager.GetLevel()];
                    saveScore.isPerfectClear = System_SaveManager.savedata.         BeeIsPerfectCleared[SystemLevelManager.GetLevel()];
                    saveScore.ClearTime = System_SaveManager.savedata.              BeeCrearedTime[SystemLevelManager.GetLevel()];
                    saveScore.Judge_TwoBomb = System_SaveManager.savedata.          BeeCrealedCondition1[SystemLevelManager.GetLevel()];
                    saveScore.Judge_HPOver = System_SaveManager.savedata.           BeeCrealedCondition2[SystemLevelManager.GetLevel()];
                    saveScore.Judge_EnemyTenDestroy = System_SaveManager.savedata.  BeeCrealedCondition3[SystemLevelManager.GetLevel()];
                }
                break;
            case StageSelect.E_STAGE.CENTIPEDE_STAGE:
                {
                    saveScore.isClear = System_SaveManager.savedata.                CentipedeIsNormalCleared[SystemLevelManager.GetLevel()];
                    saveScore.isPerfectClear = System_SaveManager.savedata.         CentipedeIsPerfectCleared[SystemLevelManager.GetLevel()];
                    saveScore.ClearTime = System_SaveManager.savedata.              CentipedeCrearedTime[SystemLevelManager.GetLevel()];
                    saveScore.Judge_TwoBomb = System_SaveManager.savedata.          CentipedeCrealedCondition1[SystemLevelManager.GetLevel()];
                    saveScore.Judge_HPOver = System_SaveManager.savedata.           CentipedeCrealedCondition2[SystemLevelManager.GetLevel()];
                    saveScore.Judge_EnemyTenDestroy = System_SaveManager.savedata.  CentipedeCrealedCondition3[SystemLevelManager.GetLevel()];

                }
                break;
            case StageSelect.E_STAGE.DUNGBEETLE_STAGE:
                {
                    saveScore.isClear = System_SaveManager.savedata.                DungBeetleIsNormalCleared[SystemLevelManager.GetLevel()];
                    saveScore.isPerfectClear = System_SaveManager.savedata.         DungBeetleIsPerfectCleared[SystemLevelManager.GetLevel()];
                    saveScore.ClearTime = System_SaveManager.savedata.              DungBeetleCrearedTime[SystemLevelManager.GetLevel()];
                    saveScore.Judge_TwoBomb = System_SaveManager.savedata.          DungBeetleCrealedCondition1[SystemLevelManager.GetLevel()];
                    saveScore.Judge_HPOver = System_SaveManager.savedata.           DungBeetleCrealedCondition2[SystemLevelManager.GetLevel()];
                    saveScore.Judge_EnemyTenDestroy = System_SaveManager.savedata.  DungBeetleCrealedCondition3[SystemLevelManager.GetLevel()];
                }
                break;
            default:
                {
                    saveScore.isClear = false;
                    saveScore.isPerfectClear = false;
                    saveScore.ClearTime = 1000.0f;
                    saveScore.Judge_TwoBomb = false;
                    saveScore.Judge_HPOver = false;
                    saveScore.Judge_EnemyTenDestroy = false;
                }
                break;
        }
        return saveScore;
    }

    public bool GetStageLevelAllClear()
    {
        StageInfo.st_StageScore NowScore = GetScore();
        StageInfo.st_StageScore BeforeScore = GetBeforeScore();

        //���̃X�e�[�W�̓�Փx���͂��߂Ċ����N���A
        return NowScore.isPerfectClear && (!BeforeScore.isPerfectClear);
    }

    public bool GetEasyAllClear()
    {
        return
            System_SaveManager.savedata.BeeIsPerfectCleared[(int)SystemLevelManager.LEVELS.EASY] &&
            System_SaveManager.savedata.CentipedeIsPerfectCleared[(int)SystemLevelManager.LEVELS.EASY] &&
            System_SaveManager.savedata.DungBeetleIsPerfectCleared[(int)SystemLevelManager.LEVELS.EASY];
    }

    public bool GetNormalAllClear()
    {
        return 
            System_SaveManager.savedata.BeeIsPerfectCleared[(int)SystemLevelManager.LEVELS.NORMAL] &&
            System_SaveManager.savedata.CentipedeIsPerfectCleared[(int)SystemLevelManager.LEVELS.NORMAL] &&
            System_SaveManager.savedata.DungBeetleIsPerfectCleared[(int)SystemLevelManager.LEVELS.NORMAL];
    }

    public bool GetAllHardClear()
    {
        //�S�Ẵn�[�h���N���A
        return
        System_SaveManager.savedata.BeeIsNormalCleared[(int)SystemLevelManager.LEVELS.HARD] &&
        System_SaveManager.savedata.CentipedeIsNormalCleared[(int)SystemLevelManager.LEVELS.HARD] &&
        System_SaveManager.savedata.DungBeetleIsNormalCleared[(int)SystemLevelManager.LEVELS.HARD];
    }

    public bool GetAllHardPerfectClear()
    {
        return
        System_SaveManager.savedata.BeeIsPerfectCleared[(int)SystemLevelManager.LEVELS.HARD] &&
        System_SaveManager.savedata.CentipedeIsPerfectCleared[(int)SystemLevelManager.LEVELS.HARD] &&
        System_SaveManager.savedata.DungBeetleIsPerfectCleared[(int)SystemLevelManager.LEVELS.HARD];
    }

}
