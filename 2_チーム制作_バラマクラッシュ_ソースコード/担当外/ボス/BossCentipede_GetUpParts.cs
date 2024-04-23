using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCentipede_GetUpParts : MonoBehaviour
{
    enum STATE
    {
        UP = 0,
        DOWN,
        END,
        NONE,
        MAX,
    }

    [SerializeField]
    float NowHeight;

    float HeightValue;
    STATE nowState;
    BossCentipede_PartsManager PartsManager;
    Vector3 GoUPStartPoint;
    Vector3 GoUpEndPoint;
    float Distance;
    float Count;
    GameObject TargetObject;

    // Start is called before the first frame update
    void Start()
    {
        PartsManager = GetComponent<BossCentipede_PartsManager>();

        SetInformation();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        NowHeight = transform.position.y;

        if (TargetObject != null)
        {
            transform.LookAt(TargetObject.transform);
        }

        switch(nowState)
        {
            case STATE.NONE:

                break;

            case STATE.UP:
                const float UPTime = 3.0f;
                transform.localPosition = Vector3.Lerp(GoUPStartPoint, GoUpEndPoint, Count / UPTime);
                Count += Time.deltaTime;
                if(Count > UPTime)
                {
                    nowState = STATE.DOWN;
                    Count = 0.0f;
                }

                Debug.Log("�オ��܂�");

                break;

            case STATE.DOWN:
                const float DOWNTime = 0.3f;
                transform.localPosition = Vector3.Lerp(GoUPStartPoint, GoUpEndPoint, Count / UPTime);
                Count += Time.deltaTime;
                if (Count > DOWNTime)
                {
                    if (this.gameObject.name == "Head")
                    {
                        //�G�t�F�N�g���o���Ă��
                        GameObject Effect = Instantiate(PartsManager.GetBodyManager().GetCentipedeEffect(BossCentipede_BodyManager.Centipede_Effect.THACKING), transform.position, Quaternion.identity);
                        Effect.GetComponent<AudioSource>().clip = PartsManager.GetBodyManager().GetCentipedeSE(BossCentipede_BodyManager.Centipede_SE.THACKING);
                    }

                    nowState = STATE.END;
                    Count = 0.0f;
                }
                Debug.Log("������܂�");


                break;
            case STATE.END:
                //�I�������̏����������Ă��

                GameObject.Find("BodyManager").GetComponent<BossCentipede_BodyManager>().ChangeDefaultMode();
                transform.parent.GetComponent<CentipedeState>().StateChange(CentipedeState.STATE.Move);
                nowState = STATE.NONE;

                break;
            case STATE.MAX:

                break;
        }
    }

    void SetInformation()
    {
        nowState = STATE.NONE;
        Distance = PartsManager.GetDistance() * 0.1f;          //�����̎擾
        TargetObject = PartsManager.GetTargetObject();  //�^�[�Q�b�g
        HeightValue = 10.0f;                            //�����̐ݒ�
        GoUPStartPoint = GoUpEndPoint = Vector3.zero;
    }

    public void SetGetUp(float Height)
    {
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;             //���̓������~�߂鏈��
        rb.isKinematic = true;

        nowState = STATE.UP;
        Count = 0.0f;
        HeightValue = Height;
        int UPObjNum = 5;
        GoUPStartPoint = GoUpEndPoint = transform.localPosition;// transform.position;
        //GoUpEndPoint = GoUPStartPoint + (-transform.forward * (Distance * 0.5f));     //���ɏ��������Ă��
        //�e�p�[�c�̏オ��ő�̍��������߂�
        if (this.gameObject.name == "Head")
        {
            Vector3 TempForward = transform.forward * -1.0f;
            TempForward.y = 0.0f;
            GoUpEndPoint += (TempForward * (Distance * 0.5f));     //���ɏ��������Ă��
            GoUpEndPoint.y = HeightValue + GoUPStartPoint.y;
        }
        else if (this.gameObject.name == "Tail")
        {
            //�������Ȃ�
        }
        else
        {
            int PartsNum = PartsManager.GetPartsNum();
            if(PartsNum > UPObjNum)
            {
                return;
            }

            //�������~���ɂ��Ă��
            float TempPartsNum = (PartsNum * -1 + UPObjNum);

            Vector3 TempForward = transform.forward * -1.0f;
            TempForward.y = 0.0f;
            GoUpEndPoint += (TempForward.normalized * (Distance * 0.5f));     //���ɏ��������Ă��

            //�グ�Ȃ��Ƃ����Ȃ�������ݒ�
            GoUpEndPoint.y = (HeightValue * (TempPartsNum / (float)UPObjNum)) + GoUPStartPoint.y;
        }

        

    }

    public void SetDefaultPos()
    {
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;             //���̓������~�߂鏈��
        //transform.position = GoUPStartPoint;
        nowState = STATE.NONE;
    }
}
