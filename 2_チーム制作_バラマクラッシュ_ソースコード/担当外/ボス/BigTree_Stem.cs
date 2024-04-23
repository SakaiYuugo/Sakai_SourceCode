using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTree_Stem : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1.0f)]
    float FallValue = 0.0f;
    [SerializeField]
    float FallTime = 1.0f;

    float Count;
    Quaternion StartRotation;
    Quaternion EndRotation;

    BigTree_Instance myInstManager;

    bool ReturnStandTree;

    float GetUpStartCount;

    // Start is called before the first frame update
    void Start()
    {
        FallValue = 0.0f;
        Count = 0.0f;
        StartRotation = transform.rotation;
        EndRotation = Quaternion.AngleAxis(90.0f, transform.right) * transform.rotation;
        ReturnStandTree = false;

        GetUpStartCount = 0.0f;
    }


    private void FixedUpdate()
    {             
        if(GetUpStartCount > 0.0f)
        {
            GetUpStartCount -= Time.deltaTime;
            return;
        }

        transform.rotation = Quaternion.Lerp(StartRotation, EndRotation, FallValue);

        if (FallValue < 1.0f)
        {
            //��Ԃ̌v�Z
            float x = Count / FallTime;

            FallValue =
                Mathf.Pow(x, 4);

            Count += Time.deltaTime;

            //�������I��������ɌĂ�
            if (Count > FallTime)
            {
                FallValue = 1.0f;
                Count = 0.0f;
                //�Đ�����ꍇ
                if(ReturnStandTree)
                {
                    myInstManager.ReproductionTree();
                    ReturnStandTree = false;
                }
            }
        }
    }

    public void SetBigInstManager(BigTree_Instance Insttreemanager)
    {
        myInstManager = Insttreemanager;
    }

    //�N���オ�肷��Ƃ��̏���
    public void SetGetUpTree()
    {
        //��]���t�ɂ��ċN���オ��悤�ɂ���
        Quaternion Temp;
        Temp = StartRotation;
        StartRotation = EndRotation;
        EndRotation = Temp;
        FallValue = 0.0f;
        ReturnStandTree = true;
    }

    public void HitWhileCentipede()
    {
        //�؂̐��������Ԃ����ꍇ
        if(ReturnStandTree)
        {
            return;
        }

        //�^�C�����J�f�ɓ��������̂ŉ�
        myInstManager.FallDestroyTree();
    }

    //�N���オ�肷��܂ł̃J�E���g�����n�߂�
    public void StartGetUpTreeCount()
    {
        //�����J�E���g���Ă�����X�V���Ȃ�
        if(GetUpStartCount > 0.0f)
        {
            return;
        }


        GetUpStartCount = 60.0f;
        SetGetUpTree();
    }
}
