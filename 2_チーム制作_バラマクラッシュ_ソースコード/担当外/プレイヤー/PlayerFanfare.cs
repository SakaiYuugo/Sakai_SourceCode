using System;
using System.Collections;
using UnityEngine;


public class PlayerFanfare : MonoBehaviour
{
    int nFrame; //すべての動きのフレーム

    CameraGameStart start;
    Rigidbody rb;
    Transform trans;
    PlayerState _PlayerState;
    PlayerSlopeRotate _PlayerSlopeRotate;
    PlayerRay playerRay;
    bool bBend; //カーブ中
    bool bDrift;    //ドリフト中

    [NonSerialized] Vector3 v3Front;    //プレイヤーの正面方向
    [Header("このスクリプトは各ステージの入場用です")]
    [SerializeField, Tooltip("今出ている速度"), ReadOnly] float fSpeed;   //移動速度
    [SerializeField, Tooltip("加速量(1f)")] float fAccelerationf;   //加速度
    [SerializeField, Tooltip("自動ブレーキ量(1f)")] float fFriction;   //自動ブレーキ
    [SerializeField, Tooltip("最大速度")] float MaxSpeed;    //最大速度
    [SerializeField, Tooltip("カーブ時最大速度")] float MaxBendSpeed;  //カーブ中の最大速度
    [SerializeField, Tooltip("曲がる角度(1F)")] float MaxBendAngle; //普通に曲がる角度

    //---- ドリフト ----
    [SerializeField, Tooltip("傾く角度")] float MaxDriftRange = 50.0f;  //ドリフト角度ななめになる方
    [SerializeField, Tooltip("ドリフト時の曲がる角度")] float MaxDriftBend;  //ドリフト角度曲がる角度用(両方掛るので使うときはMaxBendAngleを引く)
    [SerializeField, Tooltip("ドリフト時の減速量(1F)")] float fDriftDecelerat; //ドリフト減速値

    enum FANFARE_STATE
    {
        m_WAIT,     //待機シャトル
        m_ADVANCE,  //マルゼンスキー
        m_STOP,
        m_END,
    }
    FANFARE_STATE state = FANFARE_STATE.m_WAIT;

    //---- GeterSeter ----
    public float GSNowSpeed { get { return fSpeed; } }
    public float GMaxSpeed { get { return MaxSpeed; } }
    public float GSfAccelerationf { get { return fAccelerationf; } }
    public float GSfFriction { get { return fFriction; } }

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // rigidbodyを取得
        trans = GetComponent<Transform>();  //Transformを取得  
        _PlayerState = GameObject.Find("Player").GetComponent<PlayerState>();
        _PlayerSlopeRotate = GameObject.Find("SlopeRotate").GetComponent<PlayerSlopeRotate>();
        playerRay = GameObject.Find("RayPointFront").GetComponent<PlayerRay>();

        nFrame = 0;
        start = GameObject.Find("Main Camera").GetComponent<CameraGameStart>();
        Physics.autoSimulation = false;
    }

    public void Fanfare()
    {
        Physics.Simulate(Time.fixedDeltaTime / 3);
        //---- 共通して行われる移動処理 ----（行わないパターンが出てきた場合はreturn予定）
        nFrame++;
        bBend = false;  //カーブ中フラグを戻す
        bDrift = false; //ドリフト中フラグを戻す
        v3Front = -trans.forward;  //正面を変更

        switch (state)
        {
            case FANFARE_STATE.m_WAIT:
                if (nFrame >= 30)
                {
                    state +=1;
                    nFrame = 0;
                }
                
                break;
            case FANFARE_STATE.m_ADVANCE:
                Accelerator();
                if (nFrame >= 120)
                {
                    state += 1;
                    nFrame = 0;
                }
                break;
            case FANFARE_STATE.m_STOP:
                fSpeed = 0.0f;

                if(nFrame >= 60)
                {
                    state += 1;
                    nFrame = 0;
                }
                break;
            case FANFARE_STATE.m_END:
                state = FANFARE_STATE.m_WAIT;   //初めに戻す
                nFrame = 0;
                start.EndPlayerMove();
                Physics.autoSimulation = true;

                break;
        }

        if (fSpeed < 0)    //速度がマイナスなら停止状態にする
        {
            fSpeed = 0.0f;
        }

        if (!bBend) //曲がっていないなら
        {
            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.CENTER);
        }

        if (!bDrift)
        {
            GetComponent<PlayerSE>().SoundStopDrift();
        }

        float tempY = rb.velocity.y;    //Y座標は加速しない
        rb.velocity = new Vector3(v3Front.x * fSpeed, tempY, v3Front.z * fSpeed); //実際に加速

    }

    //---- アクセル ----
    void Accelerator()
    {
        fSpeed -= fFriction;    //アクセルしていない時ブレーキになる

        if (bBend && fSpeed >= MaxBendSpeed) return;    //カーブ中、カーブ速度より出ているなら加速できない

        if (fSpeed >= MaxSpeed) return;     //MaxSpeedより出ているなら


            fSpeed += fAccelerationf + fFriction;
            GetComponent<PlayerSE>().SoundAccelerator();
            if (fSpeed <= 40.0f) //40キロ以下なら倍で加速
            {
                fSpeed += fAccelerationf;
            }
        
        {//アクセルを押していないとき
            GetComponent<PlayerSE>().SoundStopAccelerator();
        }
    }

    void BendRight()
    {
        float temp = fSpeed / MaxSpeed * MaxBendAngle;  //ハンドルの角度
        Vector3 vector3 = Vector3.Normalize(-v3Front);

        vector3 = vector3 * (trans.localScale.z / 2);
        vector3 += trans.position;
        trans.RotateAround(vector3, Vector3.up, MaxBendAngle);

        _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.RIGHT);

        bBend = true;
    }

    void BendLeft()
    {
        float temp = -fSpeed / MaxSpeed * MaxBendAngle;  //ハンドルの角度
        Vector3 vector3 = Vector3.Normalize(-v3Front);

        vector3 = vector3 * (trans.localScale.z / 2);
        vector3 += trans.position;
        trans.RotateAround(vector3, Vector3.up, -MaxBendAngle);

        _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.LEFT);

        bBend = true;
    }

    void DriftRight()
    {
        fSpeed -= fDriftDecelerat;
        GetComponent<PlayerSE>().SoundDrift();

        if (fSpeed >= 5.0f)
        {
        
            Vector3 vector3 = Vector3.Normalize(-v3Front);
            vector3 = vector3 * (trans.localScale.z / 2);
            vector3 += trans.position;
            trans.RotateAround(vector3, Vector3.up, -MaxDriftBend + MaxBendAngle);
            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.LEFT_DRIFT);

            
        }
        bDrift = true;
    }

    void DriftLeft()
    {
        fSpeed -= fDriftDecelerat;
        GetComponent<PlayerSE>().SoundDrift();

        if (fSpeed >= 5.0f)
        {
            Vector3 vector3 = Vector3.Normalize(-v3Front);
            vector3 = vector3 * (trans.localScale.z / 2);
            vector3 += trans.position;
            trans.RotateAround(vector3, Vector3.up, MaxDriftBend - MaxBendAngle);
            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.RIGHT_DRIHT);
         
        }
        bDrift = true;
    }
}
