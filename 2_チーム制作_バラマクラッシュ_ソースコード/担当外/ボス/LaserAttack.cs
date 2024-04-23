using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    [System.Serializable]
    struct EffectInfo
    {
        [Tooltip("攻撃エフェクト"), SerializeField] public GameObject AtkEffect;
        [Tooltip("生成場所"), SerializeField] public GameObject AtkEffectPos;
        [Tooltip("予兆エフェクト"), SerializeField] public GameObject SignEffect;
        [Tooltip("生成場所"), SerializeField] public GameObject SignEffectPos;
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
    [System.NonSerialized] public bool bFinishFlg;
	AoE_Laser AoELaserEfk;

    public bool GSFinishFlg { get { return bFinishFlg; } set { bFinishFlg = value; } }
    public int GetFinishFrame { get { return nFinishFrame; } }
    // Start is called before the first frame update
    void Start()
    {
        state = State.Sign;
        frame = 0;
        bFinishFlg = false;

		// AoELaser
		AoELaserEfk = gameObject.GetComponent<AoE_Laser>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {

        switch(state)
        {
            case State.Sign:
                //一度だけ実行
                if (frame <= 0)
                {
                    bFinishFlg = false;
                    GameObject effect = Instantiate(Effectinfo.SignEffect, Effectinfo.SignEffectPos.transform.position, Quaternion.identity);
                    effect.transform.parent = Effectinfo.SignEffectPos.transform;

					// AoEエフェクト
					AoELaserEfk.AoELaser();
				}
                    

                frame++;

                
                if (frame > nFinishFrame)
                {
                    Debug.Log("レーザー");
                    state = State.Attack;
                    frame = 0;
                }
                
                break;
            case State.Attack:
                // 一度だけ実行
                if (frame <= 0)
                {
                    GameObject effect = Instantiate(Effectinfo.AtkEffect, Effectinfo.AtkEffectPos.transform.position, Quaternion.identity);
                    effect.transform.parent = Effectinfo.AtkEffectPos.transform;
                }

                frame++;

                if (frame > nFinishFrame)
                {
                    state = State.Sign;
                    bFinishFlg = true;
                    frame = 0;
                }
                break;
        }
        
    }

    
}
