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
        //バックアップを取っておく
        BackupState = skilState;


        skilState.PerfectClearNum = 0;

        // ハチ
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
        // ムカデ
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
        // フンコロガシ
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

        //イージー三ステージ完璧クリア
        skilState.Easy_PerfectClear = System_SaveManager.savedata.BeeIsPerfectCleared[(int)SystemLevelManager.LEVELS.EASY] &&
            System_SaveManager.savedata.CentipedeIsPerfectCleared[(int)SystemLevelManager.LEVELS.EASY] &&
            System_SaveManager.savedata.DungBeetleIsPerfectCleared[(int)SystemLevelManager.LEVELS.EASY];

        //ノーマル三ステージ完璧クリア
        skilState.Normal_PerfectClear = System_SaveManager.savedata.BeeIsPerfectCleared[(int)SystemLevelManager.LEVELS.NORMAL] &&
            System_SaveManager.savedata.CentipedeIsPerfectCleared[(int)SystemLevelManager.LEVELS.NORMAL] &&
            System_SaveManager.savedata.DungBeetleIsPerfectCleared[(int)SystemLevelManager.LEVELS.NORMAL];

        //ハード三ステージクリア
        skilState.Hard_UsuallyClear = System_SaveManager.savedata.BeeIsNormalCleared[(int)SystemLevelManager.LEVELS.HARD] &&
            System_SaveManager.savedata.CentipedeIsNormalCleared[(int)SystemLevelManager.LEVELS.HARD] &&
            System_SaveManager.savedata.DungBeetleIsNormalCleared[(int)SystemLevelManager.LEVELS.HARD];

        //ハード三ステージ完璧クリア
        skilState.Hard_PerfectClear = System_SaveManager.savedata.BeeIsPerfectCleared[(int)SystemLevelManager.LEVELS.HARD] &&
            System_SaveManager.savedata.CentipedeIsPerfectCleared[(int)SystemLevelManager.LEVELS.HARD] &&
            System_SaveManager.savedata.DungBeetleIsPerfectCleared[(int)SystemLevelManager.LEVELS.HARD];


        //前保持していたステータスを比べて、trueになっていれば
        SkilState releaseSkils = GetReleaseState();

        //開放されたらどんどん保持していってあげる
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

    //開放されたスキルを持ってくる
    public SkilState GetReleaseState()
    {
        SkilState gapState = skilState;

        //新しく解放されたスキルを取得
        gapState.PerfectClearNum        = skilState.PerfectClearNum     -   BackupState.PerfectClearNum;
        gapState.Easy_PerfectClear      = skilState.Easy_PerfectClear   && !BackupState.Easy_PerfectClear;
        gapState.Normal_PerfectClear    = skilState.Normal_PerfectClear && !BackupState.Normal_PerfectClear;
        gapState.Hard_PerfectClear      = skilState.Hard_PerfectClear   && !BackupState.Hard_PerfectClear;
        gapState.Hard_UsuallyClear      = skilState.Hard_UsuallyClear   && !BackupState.Hard_UsuallyClear;

        return gapState;
    }

    //この関数を呼ぶまで開放された情報を保持しておく
    public SkilState GetKeepState()
    {
        return CallKeepState;
    }

    public void ChangeKeepState()
    {
        CallKeepState = new SkilState();
    }
}
