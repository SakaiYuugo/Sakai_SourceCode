using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//StartCoroutine(InvisibleTime());

public class PlayerState : MonoBehaviour
{
    //�f�o�b�O���̂�true�Ŏ��ȂȂ�
    [SerializeField,Tooltip("���G�ɂȂ�܂�")] bool DebugP = false;

    public struct PLAYER_STATE
    {
        public bool m_INVISIBLE;
        public bool m_DASH;
        public bool m_DEATH;
        public bool m_INVISIBLE_TIME;   //�A�C�e���ɂ�閳�G
        public bool m_SPIDER_THREAD; //�w偂̎���H����Ă��邩
        public bool m_SWAMP;    //���Ɏg���Ă��邩
        public bool m_CLEAR;    //�N���A���true�ɂȂ�
        
        //�������֐�
        public void Constructor()
        {
            m_INVISIBLE = false;
            m_DASH = false;
            m_DEATH = false;
            m_INVISIBLE_TIME = false;
            
        }

        public void DashEnd() { m_DASH = false; }
              
    }
    public PLAYER_STATE state ; //���̏�Ԃ��Ǘ�
    
    BoxCollider bc;
    DamageEffectController DEC;
    IconMove IconObj;
    PlayerEffect _PlayerEffect;

    [SerializeField] int MAX_HP;
    [SerializeField,ReadOnly] int nHP;
    [SerializeField] int nInvFrame = 10;
    [SerializeField,ReadOnly] int nFrame;   //���G���ԗp�t���[��
    
    //---- Get,Set�֐� ----
    public int GSMAX_HP { get { return MAX_HP; }}
    public int GSnHP { get { return nHP; } }
    public int GSnInvFrame { get { return nInvFrame; } set { nInvFrame = value; } }

    private void Awake()
    {
        System_ObjectManager.playerObject = this.gameObject;
    }

    void Start()
    {
        state.Constructor();    //������
        bc = GetComponent<BoxCollider>();   //BoxCollider���擾
        DEC = GameObject.Find("PostEffect").GetComponent<DamageEffectController>();
        nHP = MAX_HP;
        nFrame = 0;
        IconObj = System_ObjectManager.BuffDebuffIconUI.GetComponent<IconMove>();
        _PlayerEffect = GetComponent<PlayerEffect>();
    }

    void FixedUpdate()
    {
        
        if (nHP > MAX_HP) nHP = MAX_HP;    //HP���ő�l�𒴂�����ő�l�ɂ���
        
        if(nFrame == nInvFrame)
        {
            gameObject.layer = LayerMask.NameToLayer("Player"); //���G���ԏI���Ń��C���[��߂�
            GetComponent<RendererOnOff>().enabled = false;
            foreach (Renderer renderer in GameObject.Find("human").GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }
                state.m_INVISIBLE = false;    //���G���[�h����
        }

        nFrame++;

        if(nHP <= 0)
        {   //�Q�[���I�[�o�[


            Debug.Log("���S");
			this.transform.Find("Defeat").gameObject.SetActive(true);
        }

    }

    //�_���[�W���󂯂����ɌĂ΂��
    public void Damage()
    {
        if (DebugP) return; //�f�o�b�O�Ŗ��G�Ȃ烊�^�[��
        if (TutorialManager.TutorialNow && nHP == 1) return;    //�`���[�g���A������HP��1�Ȃ烊�^�[��
        if (state.m_INVISIBLE) return;   //���Ƀ_���[�W���󂯂Ă���Ȃ烊�^�[��
        if (state.m_INVISIBLE_TIME) return; //���G��ԂȂ烊�^�[��
        if (state.m_CLEAR) return;  //�N���A��̃C�x���g���Ȃ烊�^�[��
        DEC.Damage();
        System_ObjectManager.mainCamera.GetComponent<CameraVibration>().Vibration(0.5f,3.0f);    //�J������h�炷
        GetComponent<PlayerSE>().SoundDamage();
        gameObject.layer = LayerMask.NameToLayer("HitPlayer");
        state.m_INVISIBLE = true;
        GetComponent<RendererOnOff>().enabled = true;
        nFrame = 0;
        nHP -= 1;
    }

    //�񕜊֐�
    public void Heal(int vol)
    {
        DEC.Heal();
        nHP += vol;
    }

    //�����A�C�e���֐�
    public void Dash()
    {
        // �A�C�R���\��
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

    //���G�A�C�e��
    IEnumerator InvisibleTime(float time)
    {
        state.m_INVISIBLE_TIME = true;
        gameObject.layer = LayerMask.NameToLayer("HitPlayer");
        _PlayerEffect.StartBarrier();
        yield return new WaitForSeconds(time);   //�������G����
        
        state.m_INVISIBLE_TIME = false;
        _PlayerEffect.EndBarrier();
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

}
