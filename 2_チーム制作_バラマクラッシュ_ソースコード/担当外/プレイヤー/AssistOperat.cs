using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistOperat : MonoBehaviour
{
    PlayerRay _CenterRay;
    PlayerRay _RightRay;
    PlayerRay _LeftRay;
    PlayerRay _CenterRay2;
    PlayerRay _RightRay2;
    PlayerRay _LeftRay2;
    [SerializeField,Tooltip("�ǂ̂��炢���������Q���𔭌����邩")] float fRayRange;    //��΂����C�̒���

    void Start()
    {
        _CenterRay = GameObject.Find("RayCenter").GetComponent<PlayerRay>();
        _RightRay = GameObject.Find("RayRight").GetComponent<PlayerRay>();
        _LeftRay = GameObject.Find("RayLeft").GetComponent<PlayerRay>();
        _CenterRay2 = GameObject.Find("RayCenter2").GetComponent<PlayerRay>();
        _RightRay2 = GameObject.Find("RayRight2").GetComponent<PlayerRay>();
        _LeftRay2 = GameObject.Find("RayLeft2").GetComponent<PlayerRay>();
    }

    public PlayerMove.ASSIST AssistCheck()
    {
        if (!InputOrder.W_Key()) return PlayerMove.ASSIST.m_STAND;    //�O�i���Ă��Ȃ��Ȃ珈���𒆒f����
        if (InputOrder.A_Key() || InputOrder.D_Key()) return PlayerMove.ASSIST.m_STAND; //���E�Ɉړ����Ă���Ȃ珈���𒆒f����
        //�܂�A�O�i���Ă��邪���E�Ɉړ����Ă��Ȃ��āA�I�u�W�F�N�g�ɓ����肻���ɂȂ������ɓ���

        float UseCenterRange; //������̎g�����C��������
        //�܂������̃Z���^�[���C���`�F�b�N
        UseCenterRange = _CenterRay.GetDistance(-transform.forward, fRayRange, "Ruin", "StageObject", "Wall", "Cave");   //��Q���`�F�b�N

        if (UseCenterRange <= -100) //�����̃��C��������Ȃ��Ȃ�㑤�������Ă݂�
        {
            //�����Ȃ�㑤�̃Z���^�[���C���`�F�b�N
            UseCenterRange = _CenterRay2.GetDistance(-transform.forward, fRayRange, "Ruin", "StageObject", "Wall", "Cave");   //��Q���`�F�b�N
        }


        float RightRange;
        float LeftRange;
        //�@���̍��E���烌�C���΂�
        RightRange = _RightRay.GetDistance(-transform.forward, fRayRange, "Ruin", "StageObject", "Wall", "Cave");
        LeftRange = _LeftRay.GetDistance(-transform.forward, fRayRange, "Ruin", "StageObject", "Wall", "Cave");

        //�A���������̍��E���C��������Ȃ��Ȃ�㑤�������Ă݂�
        if (RightRange <= -100)
        {
            RightRange = _RightRay2.GetDistance(-transform.forward, fRayRange, "Ruin", "StageObject", "Wall", "Cave");   //��Q���`�F�b�N
        }
        if (LeftRange <= -100)
        {
            LeftRange = _LeftRay2.GetDistance(-transform.forward, fRayRange, "Ruin", "StageObject", "Wall", "Cave");   //��Q���`�F�b�N
        }

        //�B���C�̌��ʂɉ����ď���
        if (UseCenterRange >= -100) //�^�񒆂̃��C������������
        {
            //���E�̃��C�̒����������A�������͂ǂ�����������Ă��Ȃ��Ȃ珈���𒆒f�i������Ȃ����-999���߂�j
            if (RightRange == LeftRange)
            {
                Debug.Log("�[�X�X�X");
                return PlayerMove.ASSIST.m_STAND;
            }

            //�B-�@�ǂ��炩�̃��C���������Ă��Ȃ��ꍇ
            if (RightRange < 0)   //�E���C���������Ă��Ȃ��Ȃ�
            {
                if (UseCenterRange > LeftRange) //�������C�̕��������Ȃ�E�ɍs��
                {
                    return PlayerMove.ASSIST.m_RIGHT;
                }
                if (UseCenterRange < LeftRange) //�����C�̕��������Ȃ獶�ɍs��
                {
                    return PlayerMove.ASSIST.m_LEFT;
                }
            }
            if (RightRange > 0)   //�����C���������Ă��Ȃ��Ȃ�
            {
                if (UseCenterRange > RightRange)    //�������C�̕��������Ȃ獶�ɍs��
                {
                    return PlayerMove.ASSIST.m_LEFT;
                }
                if (UseCenterRange < LeftRange) //�����C�̕��������Ȃ�E�ɍs��
                {
                    return PlayerMove.ASSIST.m_RIGHT;
                }
            }
            //�C�����̍��E���C���������Ă���ꍇ
            if (RightRange > LeftRange) //�E���C�̕�������(����)�Ȃ�E�ɍs��
            {
                return PlayerMove.ASSIST.m_RIGHT;
            }
            if (RightRange < LeftRange) //�����C�̕�������(����)�Ȃ獶�ɍs��
            {
                return PlayerMove.ASSIST.m_LEFT;
            }
        }
        else
        {
            //�^�񒆂̃��C���������Ă��Ȃ���
        }
        {
            //�C�����̍��E���C���������Ă���ꍇ
            if (RightRange > LeftRange) //�E���C�̕�������(����)�Ȃ�E�ɍs��
            {
                return PlayerMove.ASSIST.m_LEFT;
            }
            if (RightRange < LeftRange) //�����C�̕�������(����)�Ȃ獶�ɍs��
            {
                return PlayerMove.ASSIST.m_RIGHT;
            }
        }

        return PlayerMove.ASSIST.m_STAND;
    }   
}
