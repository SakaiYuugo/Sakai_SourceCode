using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectState : MonoBehaviour
{
    [Tooltip("èIóπÉtÉåÅ[ÉÄ"), SerializeField] protected int FinishFrame = 180;

    public enum Effectstate
    {
        Play,
        End
    }
    protected Effectstate NowState;
    protected int Frame;

    public Effectstate GSNowState{ get { return NowState; }set { NowState = value; } }
    public int GSFinishFrame { get { return FinishFrame; } set { FinishFrame = value; } }

    // Start is called before the first frame update
    virtual protected void Start()
    {
        Frame = 0;
        NowState = Effectstate.Play;
    }

    
    virtual protected void FixedUpdate()
    {
        switch (NowState)
        {
            case Effectstate.Play:
                Frame++;
                if (Frame % FinishFrame == 0)
                {
                    NowState = Effectstate.End;
                    Frame = 0;
                }
                break;
            case Effectstate.End:
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }


}
