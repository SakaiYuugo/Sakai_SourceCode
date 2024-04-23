using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCentipede_LookObj : MonoBehaviour
{
    BossCentipede_PartsManager PartsManager;

    GameObject LookObject;
    float Distance;
    float RangeOfMotionRadion;

    // Start is called before the first frame update
    void Start()
    {
        PartsManager = GetComponent<BossCentipede_PartsManager>();

        SetInformation();
    }

    private void FixedUpdate()
    {
        //Debug.DrawRay(transform.position, transform.forward * Distance, Color.red);
        SetInformation();

        //�I�u�W�F�N�g���ݒ�ł��Ă��Ȃ�������
        if (LookObject == null)
        {
            return;
        }

        transform.LookAt(LookObject.transform);

        //����ُ�ɂȂ������ɖ߂��Ă��
        Vector3 TempA3, TempB3;
        TempA3 = transform.forward;
        TempB3 = LookObject.transform.forward;

        TempA3.y = TempB3.y = 0.0f;

        //����
        float Radion = Mathf.Acos(Vector3.Dot(TempA3.normalized, TempB3.normalized));
        
        //�ُ�Ȋp�x�ɂȂ��Ă�����
        if(Radion > RangeOfMotionRadion)
        {
            //�O�ς��g���Ăǂ����ɂ��邩�m�F
            Vector3 Anser = Vector3.Cross(TempB3, TempA3);
            float SetRadion = RangeOfMotionRadion;
            if(Anser.y > 0)
            {
                //���ɂ���ꍇ
                SetRadion *= -1.0f;
            }

            //�^�[�Q�b�g�̌��Ɍ����Ă���x�N�g����SetAngle��]�������̏ꏊ������
            Vector3 RotatePos = LookObject.transform.position;
            Vector3 BackVector = LookObject.transform.forward * -1.0f * Distance;
            RotatePos.x += (BackVector.x * Mathf.Cos(SetRadion)) - (BackVector.z * Mathf.Sin(SetRadion));
            RotatePos.y = transform.position.y;
            RotatePos.z += (BackVector.x * Mathf.Sin(SetRadion)) + (BackVector.z * Mathf.Cos(SetRadion));

            transform.position = RotatePos;
        }
    }

    void SetInformation()
    {
        LookObject = PartsManager.GetTargetObject();
        Distance = PartsManager.GetDistance();
        RangeOfMotionRadion = PartsManager.GetAxisAngleToRadion();
    }

    public void SetwarpPos()
    {
        if(LookObject == null)
        {
            return;
        }

        float SetRadion = RangeOfMotionRadion;

        //�^�[�Q�b�g�̌��Ɍ����Ă���x�N�g����SetAngle��]�������̏ꏊ������
        Vector3 RotatePos = LookObject.transform.position;
        Vector3 BackVector = LookObject.transform.forward * -1.0f * Distance;
        RotatePos.x += (BackVector.x * Mathf.Cos(SetRadion)) - (BackVector.z * Mathf.Sin(SetRadion));
        RotatePos.y = transform.position.y;
        RotatePos.z += (BackVector.x * Mathf.Sin(SetRadion)) + (BackVector.z * Mathf.Cos(SetRadion));

        transform.position = RotatePos;
    }

    public void SetStraightPos()
    {
        if(LookObject == null)
        {
            return;
        }

        GameObject TempHead = PartsManager.GetHead();
        int PartsNum = PartsManager.GetPartsNum();

        Vector3 SetVector = TempHead.transform.forward * -1.0f * Distance * PartsNum;

        transform.position = TempHead.transform.position + SetVector;

        transform.LookAt(LookObject.transform);
    }
}
