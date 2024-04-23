using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCentipede_TrackObj : MonoBehaviour
{
    GameObject TrackObject;
    BossCentipede_PartsManager PartsManager;
    float Distance;
    float SquarintValue; //�Œ������̓��
    
    // Start is called before the first frame update
    void Start()
    {
        PartsManager = GetComponent<BossCentipede_PartsManager>();

        SetInformation();

        if(TrackObject == null)
        {
            return;
        }

        SquarintValue = Distance * Distance;
        Vector3 DistanceVector = TrackObject.transform.position - transform.position;

        float NowDistance = DistanceVector.magnitude;
        float DistancePercent = NowDistance / Distance; //���̋���������̉��p�[�Z���g���}��
        float AbnormityPercent = DistancePercent - 1.0f;
        Vector3 AddVector = DistanceVector / DistancePercent * AbnormityPercent;
        transform.position += AddVector;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetInformation();

        //�I�u�W�F�N�g���ݒ�ł��Ă��Ȃ�������
        if (TrackObject == null)
        {
            return;
        }

        //�܂��ړ����鋗���ɂȂ��Ă��Ȃ�������
        if (!CheckDistance())
        {
            return;
        }

        //�����ő�������������肵�Ă��
        Vector3 DistanceVector = TrackObject.transform.position - transform.position;

        float NowDistance = DistanceVector.magnitude;
        float DistancePercent = NowDistance / Distance; //���̋���������̉��p�[�Z���g���}��
        float AbnormityPercent = DistancePercent - 1.0f;
        Vector3 AddVector = DistanceVector / DistancePercent * AbnormityPercent;
        transform.position += AddVector;
    }

    void SetInformation()
    {
        Distance = PartsManager.GetDistance();
        TrackObject = PartsManager.GetTargetObject();
    }

    bool CheckDistance()
    {
        SquarintValue = Distance * Distance;

        //�^�[�Q�b�g�܂ł̃x�N�g�������߂�
        Vector3 TargetVector = TrackObject.transform.position - transform.position;

        //��悵�Ă��
        float SquarintDistance =
            TargetVector.x * TargetVector.x +
            TargetVector.y * TargetVector.y +
            TargetVector.z * TargetVector.z;

        //�������ُ킾�����ꍇ
        return SquarintDistance != SquarintValue;
    }
}
