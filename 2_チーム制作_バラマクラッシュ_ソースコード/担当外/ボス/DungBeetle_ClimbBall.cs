using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBeetle_ClimbBall : MonoBehaviour
{
    enum CLIMBSTATE
    {
        CLIMB = 0,
        STAND,
        NONE,
        MAX
    }

    [SerializeField]
    GameObject BallObject;
    [SerializeField]
    float StandAngle = 30.0f;
    [SerializeField]
    Vector3 CenterCoordinate = Vector3.zero;

    Quaternion FrontRotate;
    Quaternion StartLookRotate;
    CLIMBSTATE ClimbState;
    float Count;

    // Start is called before the first frame update
    void Start()
    {
        StandAngle = StandAngle * 3.14f / 180.0f;
        transform.LookAt(BallObject.transform);
        FrontRotate = transform.rotation;
        ClimbState = CLIMBSTATE.CLIMB;
        Count = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch(ClimbState)
        {
            case CLIMBSTATE.CLIMB:

                //ずっと見ているから回転はしなくていい
                transform.LookAt(BallObject.transform);

                //内積であとどれくらいで頂点に行くか見る
                float NowRadion = Mathf.Acos(Vector3.Dot(BallObject.transform.up, -transform.forward));

                //まだ立つ角度以上の場合
                if (NowRadion < StandAngle)
                {
                    ClimbState = CLIMBSTATE.STAND;
                    StartLookRotate = transform.rotation;
                    Count = 0.0f;
                }

                break;

            case CLIMBSTATE.STAND:

                transform.rotation = Quaternion.Lerp(StartLookRotate, FrontRotate, Count / 0.5f);

                Count += Time.deltaTime;

                break;
        }

        //ボールの位置よりも下にある場合上に上る
        if (BallObject.transform.position.y > transform.position.y + CenterCoordinate.y)
        {
            transform.position += Vector3.up * 0.5f;
            return;
        }

        Vector3 BallToDungBeetleVector = transform.position - BallObject.transform.position;
        //外積
        Vector3 CrossVector = Vector3.Cross(BallObject.transform.up,BallToDungBeetleVector);

        //外積で求めた軸で回転
        Quaternion RotateQua = Quaternion.AngleAxis(-90.0f * Time.deltaTime, CrossVector);
        Vector3 TempPos = RotateQua *BallToDungBeetleVector;

        transform.position = TempPos + BallObject.transform.position;
    }
    
    void SetClimeb()
    {

    }
}
