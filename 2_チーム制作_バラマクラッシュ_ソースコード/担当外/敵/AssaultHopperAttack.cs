using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultHopperAttack : EnemyAttack
{
    enum HOPPERATTACKSTATE
    {
        INIT = 0,
        JUMP,
        JUMPUP,
        JUMPDOWN,
        UNINIT,
        MAX
    }

    [SerializeField]
    float JumpTime = 3.0f;
    [SerializeField]
    float JumpHeight = 10.0f;

    HOPPERATTACKSTATE nowState;
    Vector3 TargetPos;
    Vector3 PosS, Pos1, Pos2, PosE;
    float JumpCount;
    ZakoHopperState TempHopperState;
    Rigidbody rb;

    [SerializeField] AudioClip sound;
    private AudioSource Audiosource;

    protected override void Start()
    {
        base.Start();
        TempHopperState = (ZakoHopperState)State;
        nowState = HOPPERATTACKSTATE.INIT;
        rb = GetComponent<Rigidbody>();

        Audiosource = GetComponent<AudioSource>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //アタックじゃなければ
        if (NowState != EnemyZakoState.ZakoState.Attack)
        {
            nowState = HOPPERATTACKSTATE.INIT;
            return;
        }

        //ベジエ曲線を使ったジャンプ
        switch (nowState)
        {
            case HOPPERATTACKSTATE.INIT:
                {

                    //アニメーションを止めてやる


                    TargetPos = GameObject.Find("Player").transform.position;
                    Vector3 TargetVector = TargetPos - transform.position;
                    float nowDistance = TargetVector.magnitude;
                    Vector3 NormalTargetVector = TargetVector.normalized;

                    PosS = transform.position;
                    Pos1 = transform.position + (NormalTargetVector * (1 / 3)) + Vector3.up * JumpHeight;
                    Pos2 = transform.position + (NormalTargetVector * (2 / 3)) + Vector3.up * JumpHeight;
                    PosE = TargetPos;

                    JumpCount = 0.0f;

                    rb.isKinematic = true;      //重力をなくしてやる

                    nowState = HOPPERATTACKSTATE.JUMP;
                }
                break;
            case HOPPERATTACKSTATE.JUMP:
                {
                    Audiosource.PlayOneShot(sound);
                    transform.position = GetBeziersPos(PosS, Pos1, Pos2, PosE, JumpCount / JumpTime);

                    JumpCount += Time.deltaTime;
                    if(JumpCount > JumpTime)
                    {
                        nowState = HOPPERATTACKSTATE.UNINIT;
                    }
                }
                break;
            case HOPPERATTACKSTATE.UNINIT:
                {
                    //アニメーションを再生してやる

                    rb.isKinematic = false;     //重力ありにしてやる
                    TempHopperState.SetJumpNow(false);
                    State.SetEnemyState(EnemyZakoState.ZakoState.Move);
                    nowState = HOPPERATTACKSTATE.INIT;
                }
                break;
        }
    }

    //ベジエ曲線を持ってくる
    Vector3 GetBeziersPos(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        var oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }
}
