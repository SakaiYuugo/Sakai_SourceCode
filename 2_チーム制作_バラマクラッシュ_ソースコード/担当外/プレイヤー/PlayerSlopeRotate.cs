using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlopeRotate : MonoBehaviour
{


    [SerializeField, ReadOnly] float m_Angle = 0.0f;
    [SerializeField] float Speed;        //角度の変化量
    [SerializeField] float BackSpeed;    //元に戻る速度
    [SerializeField] float MaxAngle;

    Animator BendAnime;
    float fAnimeAngle;   //アニメーションを使った角度（-1～1）

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
                //キーが押されていないとき、元に戻ろうとする
                if(m_Angle > 0.0f)
                {
                    m_Angle -= BackSpeed;
                }
                if (m_Angle < 0.0f)
                {
                    m_Angle += BackSpeed;
                }
                if(m_Angle < 2.0f && m_Angle > -2.0f)   //+=2度になったら0度とみなす
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
                if (m_Angle < 2.0f && m_Angle > -2.0f)   //+=2度になったら0度とみなす
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
                if (m_Angle < 2.0f && m_Angle > -2.0f)   //+=2度になったら0度とみなす
                {
                    m_Angle = 0.0f;
                }
                fAnimeAngle -= 0.05f;

                break;
            case DIRECTION.RIGHT_DRIHT:
                m_Angle += Speed;
                if (m_Angle >= MaxAngle) m_Angle = MaxAngle;    //最大角超えたら最大角にする

                break;
            case DIRECTION.LEFT_DRIFT:
                m_Angle -= Speed;
                if (m_Angle <= -MaxAngle) m_Angle = -MaxAngle;  //最大角超えたら最大角にする

                break;
            default:
                break;
        }
        
        transform.localRotation = Quaternion.AngleAxis(m_Angle,Vector3.forward);    //角度を変える
    }
}
