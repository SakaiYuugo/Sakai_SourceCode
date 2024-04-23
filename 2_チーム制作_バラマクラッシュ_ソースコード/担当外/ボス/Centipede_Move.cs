using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Centipede_Move : MonoBehaviour
{
    [SerializeField]
    float remainingTime = 30.0f;
    [SerializeField]
    float Speed = 1.0f;
    [SerializeField]
    float RotateTime = 3.0f;
    [SerializeField]
    string ChangePointName = "InstCheckPoint";
    

    GameObject ChangePointManagerObject;

    Centipede_CreateCheckPos ChangePointManager;
    Vector3 TargetPos;
    Quaternion StartRotate;
    Quaternion EndRotate;
    float Count;

    // Start is called before the first frame update
    void Start()
    {
        ChangePointManagerObject = GameObject.Find(ChangePointName);
        ChangePointManager = ChangePointManagerObject.GetComponent<Centipede_CreateCheckPos>();
        SetTargetPoint(ChangePointManager.ChangeCheckPoint());
    }

    // Update is called once per frame
    void Update()
    {
        //�^�[�Q�b�g�������ƌ��Ă���
        transform.rotation = Quaternion.Lerp(StartRotate, EndRotate, Count / RotateTime);
        
        //�ړ�
        transform.position += transform.forward * Speed * Time.deltaTime;

        //�ǂꂭ�炢�̋����ɂ��邩
        if(DistanceCheckTrigger(TargetPos,transform.position,�@Speed))
        {
            //�^�[�Q�b�g��؂�ւ�
            SetTargetPoint(ChangePointManager.ChangeCheckPoint());
        }

        //������ς��Ă���Ƃ��̏���
        if(Count < RotateTime)
        {
            //��]��
            Vector3 TempA = TargetPos, TempB = transform.position;
            TempA.y = TempB.y = 0.0f;
            Vector3 TargetVector = TempA - TempB;
            EndRotate = Quaternion.LookRotation(TargetVector, Vector3.up);

            Count += Time.deltaTime;

            if(Count > RotateTime)
            {
                Count = RotateTime;
            }
        }

        remainingTime -= Time.deltaTime;

        //���Ԃ��z����Ό��̏�Ԃɖ߂�
        if(remainingTime < 0.0f)
        {
            transform.position += new Vector3(0.0f, 5.0f, 0.0f);
            GameObject.Find("BodyManager").GetComponent<BossCentipede_BodyManager>().ChangeDefaultMode(transform.position,true);
            transform.parent.GetComponent<CentipedeState>().StateChange(CentipedeState.STATE.Move);
            Destroy(this.gameObject);
        }
    }

    public void SetTargetPoint(Vector3 targetPos)
    {
        TargetPos = targetPos;
        StartRotate = transform.rotation;
        Count = 0.0f;
    }

    bool DistanceCheckTrigger(Vector3 Target, Vector3 This , float CheckDistance)
    {
        Vector3 TargetVector = Target - This;
        
        float NowDistance =
            TargetVector.x * TargetVector.x +
            TargetVector.z * TargetVector.z;

        return NowDistance < CheckDistance * CheckDistance;
    }

    public void SetWheelTime(float Time)
    {
        remainingTime = Time;
    }
}
