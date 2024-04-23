using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowAttack : MonoBehaviour
{
    [System.Serializable]
    struct EffectInfo
    {
        [Tooltip("�U���G�t�F�N�g"), SerializeField] public GameObject AtkEffect;
        [Tooltip("�����ꏊ"), SerializeField] public GameObject AtkEffectPos;
    }

    [SerializeField] EffectInfo Effectinfo;
    [Tooltip("�\�����I���t���[��"), SerializeField] int nFinishFrame = 180;

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
        // ��x�������s
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
    //            //��x�������s
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
