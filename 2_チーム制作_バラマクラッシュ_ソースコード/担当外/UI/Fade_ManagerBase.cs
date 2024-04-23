using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade_ManagerBase : MonoBehaviour
{
    enum FADESTATE
    {
        NONE = 0,
        FADEIN,
        LOAD,
        FADEOUT,
        MAX
    }
    FADESTATE NowState;
    public static bool InstFade = false;
    string NextSceneName;

    private void Awake()
    {
        if (InstFade)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        InstFade = true;
    }

    public void Start()
    {
        switch (NowState)
        {
            case FADESTATE.NONE:

                break;
            case FADESTATE.FADEIN:

                break;
            case FADESTATE.LOAD:

                break;
            case FADESTATE.FADEOUT:

                break;
            case FADESTATE.MAX:

                break;
        }
    }

    public virtual void FadeIn_Init()
    {

    }

    public virtual void FadeOut_Init()
    {

    }

    public virtual void Update()
    {
        switch (NowState)
        {
            case FADESTATE.NONE:

                break;
            case FADESTATE.FADEIN:

                break;
            case FADESTATE.LOAD:

                break;
            case FADESTATE.FADEOUT:

                break;
            case FADESTATE.MAX:

                break;
        }
    }


    public void ChangeScneeofFade(string SceneName)
    {
        NextSceneName = SceneName;
    }
}
