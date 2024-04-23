using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBall : MonoBehaviour
{
    Transform BeetleTF;
    DungBeetleState BeetleState;

    int nGoPozint;   //�|�C���g�̉��ԂɌ�������
    List<Vector3> lV3CheckPoint = new List<Vector3>();

    Rigidbody rb;
    float fOldPosY;
    bool bChangeState = true;
    Transform childBall;    //�{�[�����f���̎q�I�u�W�F�N�g

    [SerializeField] int MaxDungHP = 10;  //����HP
    [SerializeField, ReadOnly] int nDungHP; //�ӂ��HP
    

    void Start()
    {
        GameObject Beetle = GameObject.Find("Boss_DungBeetle");
        BeetleTF = Beetle.GetComponent<Transform>();
        BeetleState = Beetle.GetComponent<DungBeetleState>();

        GameObject obj = GameObject.Find("Boss_Point");
        foreach (Transform trans in obj.GetComponentsInChildren<Transform>())
        {
            lV3CheckPoint.Add(trans.position);
        }

        rb = GetComponent<Rigidbody>();

        nDungHP = MaxDungHP;

        childBall = GameObject.Find("hun").GetComponent<Transform>();
    }


    void FixedUpdate()
    {
        if(nDungHP <= 0)    //�{�[������ꂽ��
        {
            BeetleState.state = DungBeetleState.DungBeetleSTATE.m_STUN;
            BeetleState.bChangeState = true;    //state��ς���Ƃ��͌Ă�
        }


    }

    public void FollowBeetle(float distance)    //�Ǐ]
    {
        //y���͒ǐՂ��Ȃ�
        Vector3 temp = BeetleTF.position + transform.forward * distance;
        this.transform.position = new Vector3(temp.x,this.transform.position.y,temp.z);
        this.transform.rotation = BeetleTF.rotation;
        this.Rotate();
    }

    public void Rotate()
    {
        childBall.transform.RotateAround(transform.position, transform.right, 1.5f); //��]�i�]����j
    }

    public void Turnaround()    //�����]��
    {
        float maxRotation = 2f;

        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(lV3CheckPoint[BeetleState.nGoPoint] -this.transform.position), maxRotation);
        
        // �ړI�̊p�x�܂ŉ�]�o������
        if (this.transform.rotation == Quaternion.LookRotation(lV3CheckPoint[BeetleState.nGoPoint] - this.transform.position))
        {
            BeetleState.state = DungBeetleState.DungBeetleSTATE.m_MOVE;
            BeetleState.bChangeState = true;
        }
    }
    

    public void Reproduction()  //���A
    {
        if (nDungHP > 0) return;
        nDungHP = MaxDungHP;
    }

    public void Damage()
    {
        Debug.Log("�t���Ƀ_���[�W");
        nDungHP -= 1;
    }
}


