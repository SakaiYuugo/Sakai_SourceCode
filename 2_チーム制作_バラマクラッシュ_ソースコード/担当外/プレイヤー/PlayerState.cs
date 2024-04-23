using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//StartCoroutine(InvisibleTime());

public class PlayerState : MonoBehaviour
{
    //デバッグ時のみtrueで死なない
    [SerializeField,Tooltip("無敵になります")] bool DebugP = false;

    public struct PLAYER_STATE
    {
        public bool m_INVISIBLE;
        public bool m_DASH;
        public bool m_DEATH;
        public bool m_INVISIBLE_TIME;   //アイテムによる無敵
        public bool m_SPIDER_THREAD; //蜘蛛の糸を食らっているか
        public bool m_SWAMP;    //沼に使っているか
        public bool m_CLEAR;    //クリア後にtrueになる
        
        //初期化関数
        public void Constructor()
        {
            m_INVISIBLE = false;
            m_DASH = false;
            m_DEATH = false;
            m_INVISIBLE_TIME = false;
            
        }

        public void DashEnd() { m_DASH = false; }
              
    }
    public PLAYER_STATE state ; //今の状態を管理
    
    BoxCollider bc;
    DamageEffectController DEC;
    IconMove IconObj;
    PlayerEffect _PlayerEffect;

    [SerializeField] int MAX_HP;
    [SerializeField,ReadOnly] int nHP;
    [SerializeField] int nInvFrame = 10;
    [SerializeField,ReadOnly] int nFrame;   //無敵時間用フレーム
    
    //---- Get,Set関数 ----
    public int GSMAX_HP { get { return MAX_HP; }}
    public int GSnHP { get { return nHP; } }
    public int GSnInvFrame { get { return nInvFrame; } set { nInvFrame = value; } }

    private void Awake()
    {
        System_ObjectManager.playerObject = this.gameObject;
    }

    void Start()
    {
        state.Constructor();    //初期化
        bc = GetComponent<BoxCollider>();   //BoxColliderを取得
        DEC = GameObject.Find("PostEffect").GetComponent<DamageEffectController>();
        nHP = MAX_HP;
        nFrame = 0;
        IconObj = System_ObjectManager.BuffDebuffIconUI.GetComponent<IconMove>();
        _PlayerEffect = GetComponent<PlayerEffect>();
    }

    void FixedUpdate()
    {
        
        if (nHP > MAX_HP) nHP = MAX_HP;    //HPが最大値を超えたら最大値にする
        
        if(nFrame == nInvFrame)
        {
            gameObject.layer = LayerMask.NameToLayer("Player"); //無敵時間終了でレイヤーを戻す
            GetComponent<RendererOnOff>().enabled = false;
            foreach (Renderer renderer in GameObject.Find("human").GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }
                state.m_INVISIBLE = false;    //無敵モード解除
        }

        nFrame++;

        if(nHP <= 0)
        {   //ゲームオーバー


            Debug.Log("死亡");
			this.transform.Find("Defeat").gameObject.SetActive(true);
        }

    }

    //ダメージを受けた時に呼ばれる
    public void Damage()
    {
        if (DebugP) return; //デバッグで無敵ならリターン
        if (TutorialManager.TutorialNow && nHP == 1) return;    //チュートリアルかつHPが1ならリターン
        if (state.m_INVISIBLE) return;   //既にダメージを受けているならリターン
        if (state.m_INVISIBLE_TIME) return; //無敵状態ならリターン
        if (state.m_CLEAR) return;  //クリア後のイベント中ならリターン
        DEC.Damage();
        System_ObjectManager.mainCamera.GetComponent<CameraVibration>().Vibration(0.5f,3.0f);    //カメラを揺らす
        GetComponent<PlayerSE>().SoundDamage();
        gameObject.layer = LayerMask.NameToLayer("HitPlayer");
        state.m_INVISIBLE = true;
        GetComponent<RendererOnOff>().enabled = true;
        nFrame = 0;
        nHP -= 1;
    }

    //回復関数
    public void Heal(int vol)
    {
        DEC.Heal();
        nHP += vol;
    }

    //加速アイテム関数
    public void Dash()
    {
        // アイコン表示
        IconObj.SetIcon(IconMove.IconType.SpeedUp, 10.0f);
        state.m_DASH = true;
    }

    public void StageClear()
    {
        gameObject.layer = LayerMask.NameToLayer("HitPlayer");
    }

    public void InvisibleTimeCoroutine(float time = 5.0f)
    {
        IconObj.SetIcon(IconMove.IconType.Invincible, time);
        StartCoroutine(InvisibleTime(time));
    }

    //無敵アイテム
    IEnumerator InvisibleTime(float time)
    {
        state.m_INVISIBLE_TIME = true;
        gameObject.layer = LayerMask.NameToLayer("HitPlayer");
        _PlayerEffect.StartBarrier();
        yield return new WaitForSeconds(time);   //引数無敵時間
        
        state.m_INVISIBLE_TIME = false;
        _PlayerEffect.EndBarrier();
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

}
