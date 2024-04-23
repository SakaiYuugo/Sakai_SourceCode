using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : BaseSelectState
{
    public override void DecisionProcess()
    {
        Debug.Log("ゲーム終了");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
            Application.Quit();//ゲームプレイ終了
#endif
    }
}
