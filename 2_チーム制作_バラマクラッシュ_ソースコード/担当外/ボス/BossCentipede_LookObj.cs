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

        //オブジェクトが設定できていなかったら
        if (LookObject == null)
        {
            return;
        }

        transform.LookAt(LookObject.transform);

        //可動域異常になった時に戻してやる
        Vector3 TempA3, TempB3;
        TempA3 = transform.forward;
        TempB3 = LookObject.transform.forward;

        TempA3.y = TempB3.y = 0.0f;

        //内積
        float Radion = Mathf.Acos(Vector3.Dot(TempA3.normalized, TempB3.normalized));
        
        //異常な角度になっていたら
        if(Radion > RangeOfMotionRadion)
        {
            //外積を使ってどっちにいるか確認
            Vector3 Anser = Vector3.Cross(TempB3, TempA3);
            float SetRadion = RangeOfMotionRadion;
            if(Anser.y > 0)
            {
                //左にいる場合
                SetRadion *= -1.0f;
            }

            //ターゲットの後ろに向いているベクトルをSetAngle回転させその場所を入れる
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

        //ターゲットの後ろに向いているベクトルをSetAngle回転させその場所を入れる
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
