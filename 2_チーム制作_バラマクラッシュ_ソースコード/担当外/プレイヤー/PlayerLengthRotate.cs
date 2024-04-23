using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLengthRotate : MonoBehaviour
{
    PlayerRay FrontRay;
    PlayerRay BackRay;

    [SerializeField, Tooltip("���t���[���X����p�x")] float fAngle;   //���t���[���X����p�x
    [SerializeField, Tooltip("�p�x��ς��Ȃ�臒l")] float fOverVol; //�o�C�N���X����P�\�ƂȂ�l
    [SerializeField,Tooltip("��΂����C�̒���")] float fRange;  //��΂����C�̒����i�������C�̓͂��Ȃ����󒆂ɂ���j
    [SerializeField,ReadOnly] float fFront;
    [SerializeField, ReadOnly] float fBack;
    int nFrame = 0; //�A���œ��������ɉ�����t���[�����J�E���g
    bool bFront;
    bool bBack;
    [SerializeField] float PlayerAngle;    //�������œƗ����ăv���C���[�̑O��̊p�x���Ǘ�

    void Start()
    {
        FrontRay = GameObject.Find("RayPointFront").GetComponent<PlayerRay>();
        BackRay = GameObject.Find("RayPointBack").GetComponent<PlayerRay>();
    }

    void FixedUpdate()
    {
        //�v���C���[�̑O�ƌ��ɂ��Ă郌�C�̒������擾
        fFront = FrontRay.GetDistance(-Vector3.up, fRange, "Ground", "Ruin");
        fBack = BackRay.GetDistance(-Vector3.up, fRange, "Ground", "Ruin");

        //��ŕς���

        if (fBack < -100.0f || fFront < -100.0f) return;    //���C���n�ʂɓ͂��Ȃ���(�₩���яo������Ƃ�)

        //�p�x��ς���
        float sub = fBack - fFront; 
        if (sub > fOverVol) //�����������l���傫���Ȃ�
        {
            bBack = true;
            nFrame++;
            transform.RotateAround(transform.position, transform.right, fAngle * ((nFrame + 9) / 10.0f));
            PlayerAngle += fAngle * ((nFrame + 9) / 10.0f);
        }
        else
        {
            bBack = false;
        }


        sub = fFront - fBack;
        if (sub > fOverVol) //�����������l���傫���Ȃ�
        {
            bFront = true;
            nFrame++;
            transform.RotateAround(transform.position, -transform.right, fAngle * ((nFrame + 9) / 10.0f));
            PlayerAngle -= fAngle * ((nFrame + 9) / 10.0f);
        }
        else
        {
            bFront = false;
        }

        if(!bFront && bBack)    //�p�x���ς��Ȃ�(���ʂɂ���)�Ȃ�
        {
            nFrame = 0;
        }

    }
}
