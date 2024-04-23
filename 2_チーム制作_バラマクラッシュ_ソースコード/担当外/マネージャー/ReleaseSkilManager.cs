using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseSkilManager : MonoBehaviour
{
    public static ReleaseSkilManager instance;

    public struct SkilState
    {
        public int NormalClearNum;
        public int  PerfectClearNum;
        public bool Easy_PerfectClear;
        public bool Normal_PerfectClear;
        public bool Hard_UsuallyClear;
        public bool Hard_PerfectClear;
    };

    SkilState skilState;
    SkilState BackupState;
    SkilState CallKeepState;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }


        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        SkilUpdate();
        CallKeepState = new SkilState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkilUpdate()
    {
        //�o�b�N�A�b�v������Ă���
        BackupState = skilState;


        skilState.PerfectClearNum = 0;

        // �n�`
        for (int i = 0; i < (int)SystemLevelManager.LEVELS.MAX; i++)
        {
            if (System_SaveManager.savedata.BeeIsNormalCleared[i])
            {
                skilState.NormalClearNum++;
            }

            if (System_SaveManager.savedata.BeeIsPerfectCleared[i])
            {
                skilState.PerfectClearNum++;
            }
        }
        // ���J�f
        for (int i = 0; i < (int)SystemLevelManager.LEVELS.MAX; i++)
        {
            if (System_SaveManager.savedata.CentipedeIsNormalCleared[i])
            {
                skilState.NormalClearNum++;
            }

            if (System_SaveManager.savedata.CentipedeIsPerfectCleared[i])
            {
                skilState.PerfectClearNum++;
            }
        }
        // �t���R���K�V
        for (int i = 0; i < (int)SystemLevelManager.LEVELS.MAX; i++)
        {
            if (System_SaveManager.savedata.DungBeetleIsNormalCleared[i])
            {
                skilState.NormalClearNum++;
            }

            if (System_SaveManager.savedata.DungBeetleIsPerfectCleared[i])
            {
                skilState.PerfectClearNum++;
            }
        }

        //�C�[�W�[�O�X�e�[�W�����N���A
        skilState.Easy_PerfectClear = System_SaveManager.savedata.BeeIsPerfectCleared[(int)SystemLevelManager.LEVELS.EASY] &&
            System_SaveManager.savedata.CentipedeIsPerfectCleared[(int)SystemLevelManager.LEVELS.EASY] &&
            System_SaveManager.savedata.DungBeetleIsPerfectCleared[(int)SystemLevelManager.LEVELS.EASY];

        //�m�[�}���O�X�e�[�W�����N���A
        skilState.Normal_PerfectClear = System_SaveManager.savedata.BeeIsPerfectCleared[(int)SystemLevelManager.LEVELS.NORMAL] &&
            System_SaveManager.savedata.CentipedeIsPerfectCleared[(int)SystemLevelManager.LEVELS.NORMAL] &&
            System_SaveManager.savedata.DungBeetleIsPerfectCleared[(int)SystemLevelManager.LEVELS.NORMAL];

        //�n�[�h�O�X�e�[�W�N���A
        skilState.Hard_UsuallyClear = System_SaveManager.savedata.BeeIsNormalCleared[(int)SystemLevelManager.LEVELS.HARD] &&
            System_SaveManager.savedata.CentipedeIsNormalCleared[(int)SystemLevelManager.LEVELS.HARD] &&
            System_SaveManager.savedata.DungBeetleIsNormalCleared[(int)SystemLevelManager.LEVELS.HARD];

        //�n�[�h�O�X�e�[�W�����N���A
        skilState.Hard_PerfectClear = System_SaveManager.savedata.BeeIsPerfectCleared[(int)SystemLevelManager.LEVELS.HARD] &&
            System_SaveManager.savedata.CentipedeIsPerfectCleared[(int)SystemLevelManager.LEVELS.HARD] &&
            System_SaveManager.savedata.DungBeetleIsPerfectCleared[(int)SystemLevelManager.LEVELS.HARD];


        //�O�ێ����Ă����X�e�[�^�X���ׂāAtrue�ɂȂ��Ă����
        SkilState releaseSkils = GetReleaseState();

        //�J�����ꂽ��ǂ�ǂ�ێ����Ă����Ă�����
        CallKeepState.PerfectClearNum       = releaseSkils.PerfectClearNum;
        CallKeepState.Easy_PerfectClear     = !CallKeepState.Easy_PerfectClear   && releaseSkils.Easy_PerfectClear;
        CallKeepState.Normal_PerfectClear   = !CallKeepState.Normal_PerfectClear && releaseSkils.Normal_PerfectClear;
        CallKeepState.Hard_PerfectClear     = !CallKeepState.Hard_PerfectClear   && releaseSkils.Hard_PerfectClear;
        CallKeepState.Hard_UsuallyClear     = !CallKeepState.Hard_UsuallyClear   && releaseSkils.Hard_UsuallyClear;
    }

    public SkilState GetSkilState()
    {
        return skilState;
    }

    public SkilState GetBackState()
    {
        return BackupState;
    }

    //�J�����ꂽ�X�L���������Ă���
    public SkilState GetReleaseState()
    {
        SkilState gapState = skilState;

        //�V����������ꂽ�X�L�����擾
        gapState.PerfectClearNum        = skilState.PerfectClearNum     -   BackupState.PerfectClearNum;
        gapState.Easy_PerfectClear      = skilState.Easy_PerfectClear   && !BackupState.Easy_PerfectClear;
        gapState.Normal_PerfectClear    = skilState.Normal_PerfectClear && !BackupState.Normal_PerfectClear;
        gapState.Hard_PerfectClear      = skilState.Hard_PerfectClear   && !BackupState.Hard_PerfectClear;
        gapState.Hard_UsuallyClear      = skilState.Hard_UsuallyClear   && !BackupState.Hard_UsuallyClear;

        return gapState;
    }

    //���̊֐����ĂԂ܂ŊJ�����ꂽ����ێ����Ă���
    public SkilState GetKeepState()
    {
        return CallKeepState;
    }

    public void ChangeKeepState()
    {
        CallKeepState = new SkilState();
    }
}
