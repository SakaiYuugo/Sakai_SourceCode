using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowAttack : MonoBehaviour
{
    [System.Serializable]
    struct EffectInfo
    {
        [Tooltip("攻撃エフェクト"), SerializeField] public GameObject AtkEffect;
        [Tooltip("生成場所"), SerializeField] public GameObject AtkEffectPos;
    }

    [SerializeField] EffectInfo Effectinfo;
    [Tooltip("予兆が終わるフレーム"), SerializeField] int nFinishFrame = 180;

    private enum State
    {
        Sign,
        Attack
    }
    private State state;
    private int frame;
    public bool bFinishFlg;
    public bool GSFinishFlg { get { return bFinishFlg; } set { bFinishFlg = value; } }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Sign;
        frame = 0;
        bFinishFlg = false;
    }

    
    public void Attack()
    {
        // 一度だけ実行
        if (frame <= 0)
        {
            GameObject effect = Instantiate(Effectinfo.AtkEffect, Effectinfo.AtkEffectPos.transform.position, Quaternion.identity);
            effect.transform.parent = Effectinfo.AtkEffectPos.transform;
        }

        frame++;

        if (frame > nFinishFrame)
        {
            //state = State.Sign;
            bFinishFlg = true;
            //frame = 0;
        }

    //    switch (state)
    //    {
    //        case State.Sign:
    //            //一度だけ実行
    //            if (frame <= 0)
    //            {
    //                bFinishFlg = false;
    //            }
                

    //            frame++;


    //            if (frame > nFinishFrame)
    //            {
    //                state = State.Attack;
    //                frame = 0;
    //            }

    //            break;
    //        case State.Attack:
                
    //            break;
    //    }

    }

}
