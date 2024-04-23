using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemLevelManager : MonoBehaviour
{
    public enum LEVELS
    {
        EASYÅ@= 0,
        NORMAL,
        HARD,
        MAX
    }
    public static bool InitEnd = false;
    public static int LevelNum = 0;

    [SerializeField]
    LEVELS Level = LEVELS.EASY;

    //0-easy
    //1-normal
    //2-hard

    // Start is called before the first frame update
    void Start()
    {
        if (!InitEnd)
        { 
            LevelNum = (int)Level;
            InitEnd = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Level = (LEVELS)LevelNum;
    }

    public static void SetLevel(int level)
    {
        level = (level % 3 + 3) % 3;
        LevelNum = level;
        InitEnd = true;
    }

    public static void SetLevel_enum(LEVELS level)
    {
        int TempLevel = (int)level;
        SetLevel(TempLevel);
    }

    public static void SetLevel(LEVELS level)
    {
        int TempLevel = (int)level;
        SetLevel(TempLevel);
    }

    public static int GetLevel()
    {
        return LevelNum;
    }

    public static LEVELS GetLevel_enum()
    {
        return (LEVELS)LevelNum;
    }
}
