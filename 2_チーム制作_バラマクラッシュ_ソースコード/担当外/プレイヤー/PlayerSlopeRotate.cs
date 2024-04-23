using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlopeRotate : MonoBehaviour
{


    [SerializeField, ReadOnly] float m_Angle = 0.0f;
    [SerializeField] float Speed;        //�p�x�̕ω���
    [SerializeField] float BackSpeed;    //���ɖ߂鑬�x
    [SerializeField] float MaxAngle;

    Animator BendAnime;
    float fAnimeAngle;   //�A�j���[�V�������g�����p�x�i-1�`1�j

    public enum DIRECTION
    {
        CENTER,
        RIGHT,
        LEFT,
        RIGHT_DRIHT,
        LEFT_DRIFT,
    };

    void Start()
    {
        BendAnime = GameObject.Find("bike_anim1").GetComponent<Animator>();
    }

    public void Rotate(DIRECTION mode)
    {
        BendAnime.SetFloat("hangoff", fAnimeAngle);
        fAnimeAngle = Mathf.Clamp(fAnimeAngle, -1.0f, 1.0f);

        switch (mode)
        {
            case DIRECTION.CENTER:
                //�L�[��������Ă��Ȃ��Ƃ��A���ɖ߂낤�Ƃ���
                if(m_Angle > 0.0f)
                {
                    m_Angle -= BackSpeed;
                }
                if (m_Angle < 0.0f)
                {
                    m_Angle += BackSpeed;
                }
                if(m_Angle < 2.0f && m_Angle > -2.0f)   //+=2�x�ɂȂ�����0�x�Ƃ݂Ȃ�
                {
                    m_Angle = 0.0f;
                }

                if (fAnimeAngle < 0.0f) fAnimeAngle += 0.05f;
                if (fAnimeAngle > 0.0f) fAnimeAngle -= 0.05f;

                break;
            case DIRECTION.RIGHT:
                fAnimeAngle += 0.05f;
                if (m_Angle > 0.0f)
                {
                    m_Angle -= BackSpeed;
                }
                if (m_Angle < 0.0f)
                {
                    m_Angle += BackSpeed;
                }
                if (m_Angle < 2.0f && m_Angle > -2.0f)   //+=2�x�ɂȂ�����0�x�Ƃ݂Ȃ�
                {
                    m_Angle = 0.0f;
                }
                break;
            case DIRECTION.LEFT:
                if (m_Angle > 0.0f)
                {
                    m_Angle -= BackSpeed;
                }
                if (m_Angle < 0.0f)
                {
                    m_Angle += BackSpeed;
                }
                if (m_Angle < 2.0f && m_Angle > -2.0f)   //+=2�x�ɂȂ�����0�x�Ƃ݂Ȃ�
                {
                    m_Angle = 0.0f;
                }
                fAnimeAngle -= 0.05f;

                break;
            case DIRECTION.RIGHT_DRIHT:
                m_Angle += Speed;
                if (m_Angle >= MaxAngle) m_Angle = MaxAngle;    //�ő�p��������ő�p�ɂ���

                break;
            case DIRECTION.LEFT_DRIFT:
                m_Angle -= Speed;
                if (m_Angle <= -MaxAngle) m_Angle = -MaxAngle;  //�ő�p��������ő�p�ɂ���

                break;
            default:
                break;
        }
        
        transform.localRotation = Quaternion.AngleAxis(m_Angle,Vector3.forward);    //�p�x��ς���
    }
}
