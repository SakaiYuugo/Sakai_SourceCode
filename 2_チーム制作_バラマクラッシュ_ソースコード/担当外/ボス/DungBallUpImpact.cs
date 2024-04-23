using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine;

public class DungBallUpImpact : MonoBehaviour
{
    DungManager Manager;
    ShockWaveAttack _ShockWaveAttack;

    int nConstWaitFrame = 100;   //��Œ��̃A�j���[�V�������Ԃ�����
    [SerializeField] int nConstSequelFrame = 50;   //�㌄����
    [SerializeField,ReadOnly] int nFrame;
    bool bChangeState;
    float fYetPosY;

    DungDebris DD;
    [SerializeField] GameObject Debris;

    public enum UpImpactSTATE
    {
        m_None,
        m_WAIT,
        m_UP,
        m_IMPACT,
    }
    [ReadOnly] public UpImpactSTATE state;
    [SerializeField] Vector3 force; //�����΂�����

    void Start()
    {
        Manager = GameObject.Find("Boss_Point").GetComponent<DungManager>();

        state = UpImpactSTATE.m_None;
        GameObject Beetle = GameObject.Find("DungBeetle");
        _ShockWaveAttack = GetComponent<ShockWaveAttack>();
        bChangeState = true;
    }

    void FixedUpdate()
    {
        //��x��������
        if(bChangeState)
        {
            switch (state)
            {
                case UpImpactSTATE.m_WAIT:
                    fYetPosY = transform.position.y;
                    Manager._DungAnime.BeamUpAnime();
                    nFrame = 0;
                    break;

                case UpImpactSTATE.m_UP:
                    transform.LookAt(Vector3.down);
                    Vector3 temp = new Vector3(20, 2, 20);
                    Manager._DbRigidbody.AddForce(force,ForceMode.Impulse);
                    Manager._DbRigidbody.velocity = new Vector3(0, Manager._DbRigidbody.velocity.y, 0);
                    Manager._DungSound.Sound(DungSound.DUNG_SOUND.SOUND_UPBALL);    //���Đ�
                    nFrame = 0;
                    break;

                case UpImpactSTATE.m_IMPACT:
                    //�Ռ��g�Đ�
                    _ShockWaveAttack.Attack();
                    Manager._DungSound.Sound(DungSound.DUNG_SOUND.SOUND_IMPACT);    //���Đ�
                    Instantiate(Debris, this.transform.position, Quaternion.identity).GetComponent<DungDebris>().StartAtk();
                    nFrame = 0;
                    break;
            }
            bChangeState = false;
        }


        switch (state)
        {
            case UpImpactSTATE.m_WAIT:
                nFrame++;
                if (nConstWaitFrame <= nFrame)
                {
                    state = UpImpactSTATE.m_UP;
                    bChangeState = true;
                }
                break;

            case UpImpactSTATE.m_UP:
                nFrame++;

                //����ς�ی��͎c���Ă���
                if (335 <= nFrame)
                {
                    state = UpImpactSTATE.m_IMPACT;
                    bChangeState = true;
                }

                //�╨�ł͂Ȃ��Ȃ���
                Debug.DrawRay(transform.position, Vector3.down * 30, Color.red);

                Ray ray = new Ray(transform.position, Vector3.down);
                foreach(RaycastHit hit in Physics.RaycastAll(ray,10.0f))
                {
                    if (hit.collider.gameObject.tag == "Ground")
                    {
                        state = UpImpactSTATE.m_IMPACT;
                        bChangeState = true;
                    }
                }
                break;

            case UpImpactSTATE.m_IMPACT:
                nFrame++;

                if (nConstSequelFrame <= nFrame)
                {
                    state = UpImpactSTATE.m_None;
                    bChangeState = true;
                    
                    Manager._DBState.state = DungBeetleState.DungBeetleSTATE.m_MOVE;
                    Manager._DungAnime.WalkAnim();
                    Manager._DBState.bChangeState = true;
                }
                break;
        }
    }

    public void StartAtk()
    {
        state = UpImpactSTATE.m_WAIT;
        bChangeState = true;
        nFrame = 0;
    }


}
