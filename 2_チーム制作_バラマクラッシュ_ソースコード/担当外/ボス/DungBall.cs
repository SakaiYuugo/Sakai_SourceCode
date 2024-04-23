using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBall : MonoBehaviour
{
    Transform BeetleTF;
    DungBeetleState BeetleState;

    int nGoPozint;   //ポイントの何番に向かうか
    List<Vector3> lV3CheckPoint = new List<Vector3>();

    Rigidbody rb;
    float fOldPosY;
    bool bChangeState = true;
    Transform childBall;    //ボールモデルの子オブジェクト

    [SerializeField] int MaxDungHP = 10;  //糞のHP
    [SerializeField, ReadOnly] int nDungHP; //ふんのHP
    

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
        if(nDungHP <= 0)    //ボールが壊れたら
        {
            BeetleState.state = DungBeetleState.DungBeetleSTATE.m_STUN;
            BeetleState.bChangeState = true;    //stateを変えるときは呼ぶ
        }


    }

    public void FollowBeetle(float distance)    //追従
    {
        //y軸は追跡しない
        Vector3 temp = BeetleTF.position + transform.forward * distance;
        this.transform.position = new Vector3(temp.x,this.transform.position.y,temp.z);
        this.transform.rotation = BeetleTF.rotation;
        this.Rotate();
    }

    public void Rotate()
    {
        childBall.transform.RotateAround(transform.position, transform.right, 1.5f); //回転（転がる）
    }

    public void Turnaround()    //方向転換
    {
        float maxRotation = 2f;

        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(lV3CheckPoint[BeetleState.nGoPoint] -this.transform.position), maxRotation);
        
        // 目的の角度まで回転出来たら
        if (this.transform.rotation == Quaternion.LookRotation(lV3CheckPoint[BeetleState.nGoPoint] - this.transform.position))
        {
            BeetleState.state = DungBeetleState.DungBeetleSTATE.m_MOVE;
            BeetleState.bChangeState = true;
        }
    }
    

    public void Reproduction()  //復帰
    {
        if (nDungHP > 0) return;
        nDungHP = MaxDungHP;
    }

    public void Damage()
    {
        Debug.Log("フンにダメージ");
        nDungHP -= 1;
    }
}


