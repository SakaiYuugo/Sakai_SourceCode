using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeeAnimation : MonoBehaviour
{
    [Tooltip("ボスのモデル"), SerializeField] GameObject  BossModelObj;

    [Tooltip("羽モデル"), SerializeField] GameObject WingModelObj;

    [Tooltip("歩行アニメーションの速度"), SerializeField] float WalkAnimasionSpeed = 0.1f;

    [Tooltip("羽アニメーションの速度"), SerializeField] float WingAnimSpeed = 0.1f;

    private Animator bossAnim;
    private Animator wingAnim;
    private BossBeeAttack bossAtk;
    private LaserAttack LAtk;
    private float frame;
    // Start is called before the first frame update
    void Start()
    {
        bossAnim = BossModelObj.GetComponent<Animator>();
        wingAnim = WingModelObj.GetComponent<Animator>();
        bossAnim.speed = WalkAnimasionSpeed;
        bossAtk = this.GetComponent<BossBeeAttack>();
        LAtk = this.GetComponent<LaserAttack>();
        frame = 0.0f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
       
    }

    public void StopWalkAnimation()
    {
        bossAnim.speed = 0.3f;
        bossAnim.SetBool("walk", false);
    }

    public void StartWalkAnimation()
    {
        bossAnim.speed = WalkAnimasionSpeed;
        bossAnim.SetBool("walk", true);
    }

    public void StartLaserAnimation()
    {
        bossAnim.SetTrigger("beam");
    }
    public void StopLaserAnimation()
    {
        bossAnim.ResetTrigger("beam");
    }

    public void StartBlowAnim()
    {
        bossAnim.SetBool("wait",true);
        wingAnim.speed = WingAnimSpeed;
        wingAnim.enabled = true;
        wingAnim.PlayInFixedTime("BossHatiHaneRotation", 0, 5.0f);
    }
    public void StopBlowAnim()
    {
        bossAnim.SetBool("wait", false);
        wingAnim.enabled = false;
    }

    public void StartWaveAnim()
    {
        bossAnim.SetTrigger("down");
    }

    public void StopWaveAnim()
    {
        bossAnim.ResetTrigger("down");
    }

    public bool DidStoppedWaveAnim()
    {
        if ( bossAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5)
        {
            return true;
        }
        return false;
    }

    public void StartDefeatAnim()
    {
        StopWalkAnimation();
        StopLaserAnimation();
        StopBlowAnim();
        StopWaveAnim();

        bossAnim.speed = 0.6f;
        bossAnim.SetTrigger("defeat");
    }

    public bool DidStoppedDefeatAnim()
    {
            // ちゃんと切り替わった後に判定するために時間を置く
            if (frame >= 5 && bossAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0)
            {
                return true;
            }
        frame += Time.deltaTime;
        return false;
    }
    
}
