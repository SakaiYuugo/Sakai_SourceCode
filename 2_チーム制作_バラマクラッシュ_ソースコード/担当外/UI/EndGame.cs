using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : BaseSelectState
{
    public override void DecisionProcess()
    {
        Debug.Log("�Q�[���I��");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
            Application.Quit();//�Q�[���v���C�I��
#endif
    }
}
