using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeTailCollision : MonoBehaviour
{
    public GameObject Boss;

    Vector3 bossPos;
    private ShockWaveAttack wave;
    private BossBeeAttack AtkState;
    private Vector3 OriginPos;
    private BossBeeAnimation bossAnim;
    public enum TailState
    {
        Start,
        Up,
        Hit,
        None // âΩÇ‡ÇµÇ»Ç¢
    }
    [System.NonSerialized]
    public TailState tailState;


    public TailState GSTailState { get { return tailState; } set { tailState = value; } }
    // Start is called before the first frame update
    void Start()
    {
        bossPos = Boss.transform.position;
        tailState = TailState.None;
        wave = Boss.GetComponent<ShockWaveAttack>();
        AtkState = Boss.GetComponent<BossBeeAttack>();
        bossAnim = Boss.GetComponent<BossBeeAnimation>();
    }

    private void FixedUpdate()
    {
        bossPos = Boss.gameObject.transform.position;
        if(AtkState.GetAtkType == BossBeeAttack.AttakType.Wave)
        {
            if (this.transform.position.y > bossPos.y + 30)
            {
                // êKîˆÇêUÇËè„Ç∞ÇΩ
                tailState = TailState.Up;
            }
        }
        
        switch (tailState)
        {
            case TailState.Start:
                OriginPos = gameObject.transform.position;
                tailState = TailState.None;
                break;
            case TailState.Up:
                if(bossAnim.DidStoppedWaveAnim())
                {
                    tailState = TailState.Hit;
                }
                
                break;
            case TailState.Hit:
                wave.Attack();
                if(wave.bFinishFlg)
                {
                    tailState = TailState.None;
                }
                break;
            case TailState.None:
                break;
        }

    }

    
}
