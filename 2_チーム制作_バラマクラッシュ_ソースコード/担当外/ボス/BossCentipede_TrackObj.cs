using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCentipede_TrackObj : MonoBehaviour
{
    GameObject TrackObject;
    BossCentipede_PartsManager PartsManager;
    float Distance;
    float SquarintValue; //最長距離の二乗
    
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
        float DistancePercent = NowDistance / Distance; //今の距離が正常の何パーセントか図る
        float AbnormityPercent = DistancePercent - 1.0f;
        Vector3 AddVector = DistanceVector / DistancePercent * AbnormityPercent;
        transform.position += AddVector;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetInformation();

        //オブジェクトが設定できていなかったら
        if (TrackObject == null)
        {
            return;
        }

        //まだ移動する距離になっていなかったら
        if (!CheckDistance())
        {
            return;
        }

        //ここで足したり引いたりしてやる
        Vector3 DistanceVector = TrackObject.transform.position - transform.position;

        float NowDistance = DistanceVector.magnitude;
        float DistancePercent = NowDistance / Distance; //今の距離が正常の何パーセントか図る
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

        //ターゲットまでのベクトルを求める
        Vector3 TargetVector = TrackObject.transform.position - transform.position;

        //二乗してやる
        float SquarintDistance =
            TargetVector.x * TargetVector.x +
            TargetVector.y * TargetVector.y +
            TargetVector.z * TargetVector.z;

        //距離が異常だった場合
        return SquarintDistance != SquarintValue;
    }
}
