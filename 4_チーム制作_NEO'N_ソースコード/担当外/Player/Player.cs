using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using StateMachine = DesignPatterns.State.StateMachine;

public class Player : MonoBehaviour
{
    [HideInInspector] public bool nearCheck = false;
    [HideInInspector] public bool isNitro = false;
    [HideInInspector] public bool useNitro = false;
    [HideInInspector] public bool SuperDashReady;
    [HideInInspector] public bool isGoal; 
    [HideInInspector] public bool playerDestroy = false;
    [HideInInspector] public bool SuperDashMode;
                      private bool soundFlg = false;
                      
    [HideInInspector] public int[] n = new int[3];
    [HideInInspector] public int lanenum;

    [HideInInspector] public float velMax;
    [HideInInspector] public float lerpTime = 0f;
    [HideInInspector] public float lerpTime2 = 0f;
    [HideInInspector] public float lerpValue = 0f;
    [HideInInspector] public float t;
    [HideInInspector] public float impulse; 
    [HideInInspector] public float cameraPosZ;
    [HideInInspector] public float nitro = 300f;
                      public float nitroMax = 300f;
                      public float accelerate = 5f;
                      public float gravity = 500f;
                      public float moveValue = 7;
                      public float slowMotionTime;
                      public float sideMoveSecond;
                      public float jumpPower;
                      
    public Image[] nitroGaugeBack;
    public Image[] nitroGaugeFront;
    
    [HideInInspector] public ParticleSystem speedEffect;
    [HideInInspector] public ParticleSystem sideSpeedEffect;
    [HideInInspector] public ParticleSystem deathEffect;
                      public ParticleSystem JumpEffect;
    public ParticleSystem obstacleDestroyEffect;
    
    public GameObject CutinUI;
     public Rigidbody rb;
                      public AudioClip[] SE;
    [HideInInspector] public GameSoundPlayer sound;
    [HideInInspector] public AudioSource audiosource;

                      internal static Player player;
                      private StateMachine playerStateMachine;
                      public StateMachine PlayerStateMachine => playerStateMachine;
    private void Awake()
    {
        player = this;
        playerStateMachine = new StateMachine(this);

        
    }
   
    
    void Start()
    {
        playerStateMachine.Initialize(playerStateMachine.Idle);
    }


    private void Update()
    {
       if(!playerDestroy) playerStateMachine.Update();
    }

    void FixedUpdate()
    {
        if(!playerDestroy) playerStateMachine.FixedUpdate();
    }


    //ゲージ消費の処理
    public void NitroBoost(float value)
    {
        if (nitro - value > 0f) nitro -= value;
        else
        {
            nitro = 0;
            value = nitroMax * (nitroGaugeBack.Length - 1) / nitroGaugeBack.Length;
        }

        useNitro = true;
        lerpValue = value;
        lerpTime = 0f;
        lerpTime2 = 0f;
    }
    
    public void NitroRecover(int value)
    {
        useNitro = true;
        if (nitro >= nitroMax)
        {
            if (!soundFlg)
            {
                audiosource.clip = SE[0];
                audiosource.Play();
                soundFlg = true;
            }
            
            SuperDashReady = true;
        }
        else
        {
            NitroBoost(-value); //ゲージ消費
            SuperDashReady = false;
        }
    }
    
    

    public IEnumerator Jump()
    {
        JumpEffect.Play(); //エフェクト生成
        transform.DOBlendableMoveBy(transform.up * jumpPower, sideMoveSecond).SetLoops(1,LoopType.Yoyo);
        
        AudioSource.PlayClipAtPoint(sound.playerJump.file, transform.position, sound.playerJump.volume); // ジャンプ音再生

        if (nearCheck) StartCoroutine("SlowMotion");
        
        yield return new WaitForSeconds(0.5f);
        PlayerStateMachine.TransitionTo(PlayerStateMachine.Idle);
    }

    public IEnumerator Superdash()
    {
        SuperDashMode = true; // 前ダッシュ開始
        SuperDashReady = false;
        CutinUI.GetComponent<CutinUI>().CutIn();　// カットイン表示
        
       AudioSource.PlayClipAtPoint(sound.cutin.file,transform.position, sound.cutin.volume);
        yield return new WaitForSecondsRealtime(2f);
        
        speedEffect.Play(); //エフェクト生成
        
        AudioSource.PlayClipAtPoint(sound.acceleration.file, transform.position, sound.acceleration.volume);

        NitroBoost(300); //ゲージ消費
        rb.AddForce(transform.forward * impulse * 0.1f, ForceMode.Impulse); //前にAddForce
        cameraPosZ = -40;
        
        yield return new WaitForSeconds(3.0f); //3秒待機
        
        speedEffect.Stop(); //エフェクト停止
        SuperDashMode = false; //前ダッシュ終了
        cameraPosZ = -9.5f;
        PlayerStateMachine.TransitionTo(PlayerStateMachine.Idle);
    }
    
    
    public IEnumerator Wdash()
    {
            isNitro = true; // 前ダッシュ開始
            speedEffect.Play(); //エフェクト生成
            player.GetComponent<Rigidbody>().AddForce(player.transform.forward * player.impulse* 0.1f, ForceMode.Impulse); //前にAddForce
            
            cameraPosZ = -20;

            yield return new WaitForSeconds(1.0f);
        
            speedEffect.Stop(); //エフェクト停止
            
            isNitro = false; //前ダッシュ終了
            cameraPosZ = -9.5f;
            PlayerStateMachine.TransitionTo(PlayerStateMachine.Idle);
    }
    

    public IEnumerator Adash()
    {
        if (lanenum != -1)
        {
            lanenum--;
            if (nearCheck)
            {
                sideSpeedEffect.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
                sideSpeedEffect.transform.localPosition = new Vector3(-1.47f, sideSpeedEffect.transform.localPosition.y,
                    sideSpeedEffect.transform.localPosition.z);
                sideSpeedEffect.Play();
                
                transform.DOMove(transform.position + (-transform.right * moveValue), sideMoveSecond /2).OnComplete(() => 
                    { PlayerStateMachine.TransitionTo(PlayerStateMachine.Idle); });

                
                StartCoroutine("SlowMotion");
                yield return null;
            }
            else
            {
                transform.DOBlendableMoveBy(-transform.right * moveValue, sideMoveSecond).OnComplete(() => { PlayerStateMachine.TransitionTo(PlayerStateMachine.Idle); });
                AudioSource.PlayClipAtPoint(sound.sideStep.file, transform.position, sound.sideStep.volume); // サイドステップ音再生
            }
        }

    }

    public IEnumerator Ddash()
    {
        if (lanenum != 1)
        {
            lanenum++;
            if (nearCheck)
            {
                sideSpeedEffect.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, -90.0f, 0.0f));
                sideSpeedEffect.transform.localPosition = new Vector3(1.47f, sideSpeedEffect.transform.localPosition.y,
                    sideSpeedEffect.transform.localPosition.z);
                sideSpeedEffect.Play();
                
                transform.DOMove(transform.position + (transform.right * moveValue), sideMoveSecond/2).OnComplete(() => 
                    { PlayerStateMachine.TransitionTo(PlayerStateMachine.Idle); });

                StartCoroutine("SlowMotion");
                yield return null;
            }
            else
            {
                transform.DOBlendableMoveBy(transform.right * moveValue, sideMoveSecond)
                    .OnComplete(() => { PlayerStateMachine.TransitionTo(PlayerStateMachine.Idle); });
                AudioSource.PlayClipAtPoint(sound.sideStep.file, transform.position, sound.sideStep.volume); // サイドステップ音再生
            }
        }
    }

    IEnumerator SlowMotion()
    {
        GetComponent<SlowMotion>().StartSlowMotion();
             
        sound = Camera.main.GetComponent<GameSoundPlayer>();
        AudioSource.PlayClipAtPoint(sound.playerSlow.file, transform.position, sound.playerSlow.volume); // スロー音再生
             
        yield return new WaitForSecondsRealtime(slowMotionTime);
        GetComponent<SlowMotion>().StopSlowMotion();
                 
        velMax *= 1.04f; //もし障害物をギリギリで避けたら最大速度1.5倍
        NitroRecover(30);
    }

    public void Death()
    {
        if (SuperDashMode == false)
        {
            // 衝突SE
            audiosource.clip = SE[1];
            audiosource.Play();

            playerDestroy = true;
            Destroy(Instantiate(deathEffect, transform), 0.6f);
            FindFirstObjectByType<Observer>().clearflg = false;
            FindFirstObjectByType<Observer>().SetTime();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            isGoal = true;
            Camera.main.transform.parent = other.transform;
        }
    }
}
