using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLengthRotate : MonoBehaviour
{
    PlayerRay FrontRay;
    PlayerRay BackRay;

    [SerializeField, Tooltip("毎フレーム傾ける角度")] float fAngle;   //毎フレーム傾ける角度
    [SerializeField, Tooltip("角度を変えない閾値")] float fOverVol; //バイクを傾ける猶予となる値
    [SerializeField,Tooltip("飛ばすレイの長さ")] float fRange;  //飛ばすレイの長さ（両方レイの届かない＝空中にいる）
    [SerializeField,ReadOnly] float fFront;
    [SerializeField, ReadOnly] float fBack;
    int nFrame = 0; //連続で同じ方向に回ったフレームをカウント
    bool bFront;
    bool bBack;
    [SerializeField] float PlayerAngle;    //こっちで独立してプレイヤーの前後の角度を管理

    void Start()
    {
        FrontRay = GameObject.Find("RayPointFront").GetComponent<PlayerRay>();
        BackRay = GameObject.Find("RayPointBack").GetComponent<PlayerRay>();
    }

    void FixedUpdate()
    {
        //プレイヤーの前と後ろについてるレイの長さを取得
        fFront = FrontRay.GetDistance(-Vector3.up, fRange, "Ground", "Ruin");
        fBack = BackRay.GetDistance(-Vector3.up, fRange, "Ground", "Ruin");

        //後で変える

        if (fBack < -100.0f || fFront < -100.0f) return;    //レイが地面に届かない時(坂から飛び出したりとか)

        //角度を変える
        float sub = fBack - fFront; 
        if (sub > fOverVol) //差がしきい値より大きいなら
        {
            bBack = true;
            nFrame++;
            transform.RotateAround(transform.position, transform.right, fAngle * ((nFrame + 9) / 10.0f));
            PlayerAngle += fAngle * ((nFrame + 9) / 10.0f);
        }
        else
        {
            bBack = false;
        }


        sub = fFront - fBack;
        if (sub > fOverVol) //差がしきい値より大きいなら
        {
            bFront = true;
            nFrame++;
            transform.RotateAround(transform.position, -transform.right, fAngle * ((nFrame + 9) / 10.0f));
            PlayerAngle -= fAngle * ((nFrame + 9) / 10.0f);
        }
        else
        {
            bFront = false;
        }

        if(!bFront && bBack)    //角度が変わらない(平面にいる)なら
        {
            nFrame = 0;
        }

    }
}
