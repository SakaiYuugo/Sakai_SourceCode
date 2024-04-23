using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBeetleMove : MonoBehaviour
{
    Transform _DungBallTF;
    DungBeetleState BeetleState;
    public float fDistance;    //ボールと虫の距離
    public float fSpeed = 0.35f;

    public List<Vector3> lV3CheckPoint = new List<Vector3>();

    void Start()
    {
        _DungBallTF = GameObject.Find("DungBall").GetComponent<Transform>();
        BeetleState = GetComponent<DungBeetleState>();  

        GameObject obj = GameObject.Find("Boss_Point");
        foreach (Transform trans in obj.GetComponentsInChildren<Transform>())
        {
            lV3CheckPoint.Add(trans.position);
        }
    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, lV3CheckPoint[BeetleState.nGoPoint], fSpeed);

        //目的の地点まで移動したら
            if (transform.position.x <= lV3CheckPoint[BeetleState.nGoPoint].x + 10
             && transform.position.x >= lV3CheckPoint[BeetleState.nGoPoint].x - 10
             && transform.position.z <= lV3CheckPoint[BeetleState.nGoPoint].z + 10
             && transform.position.z >= lV3CheckPoint[BeetleState.nGoPoint].z - 10)
        {
            BeetleState.state = DungBeetleState.DungBeetleSTATE.m_ROTATION;
            BeetleState.bChangeState = true;
            BeetleState.nGoPoint += 1;
        }

        if (BeetleState.nGoPoint >= 6) BeetleState.nGoPoint = 1;
    }

    public bool Turnaround()
    {
        //y軸は追跡しない
        Vector3 temp = _DungBallTF.position - transform.forward * fDistance;
        this.transform.position = new Vector3(temp.x, this.transform.position.y, temp.z);
        this.transform.rotation = _DungBallTF.rotation;

        return this.transform.rotation == _DungBallTF.rotation && this.transform.position == new Vector3(temp.x, this.transform.position.y, temp.z);
    }

    public Vector3 GetGoPos()
    {
        return lV3CheckPoint[BeetleState.nGoPoint];
    }
}
