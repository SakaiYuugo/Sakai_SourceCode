using System;
using System.Collections;
using UnityEngine;
//aaa

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Transform trans;
    PlayerState _PlayerState;
    PlayerSlopeRotate _PlayerSlopeRotate;
    PlayerRay playerRay;

    public bool bBend; //カーブ中

    [NonSerialized] Vector3 v3Front;    //プレイヤーの正面方向
    [SerializeField, Tooltip("今出ている速度"), ReadOnly] float fSpeed;   //移動速度
    [SerializeField, Tooltip("加速量(1f)")] float fAccelerationf;   //加速度
    [SerializeField, Tooltip("自動ブレーキ量(1f)")] float fFriction;   //自動ブレーキ
    [SerializeField, Tooltip("最大速度")] float MaxSpeed;    //最大速度
    [SerializeField, Tooltip("カーブ時最大速度")] float MaxBendSpeed;  //カーブ中の最大速度
    [SerializeField, Tooltip("曲がる角度(1F)")] float MaxBendAngle; //普通に曲がる角度
    //曲がり反転中対策のアシスト用
    float ASMaxBendAngle; //普通に曲がる角度
    float ASMaxSpeed;    //最大速度

    //---- ドリフト ----
    [SerializeField, Tooltip("傾く角度")] float MaxDriftRange = 50.0f;  //ドリフト角度ななめになる方
    [SerializeField, Tooltip("ドリフト時の曲がる角度")] float MaxDriftBend;  //ドリフト角度曲がる角度用(両方掛るので使うときはMaxBendAngleを引く)
    [SerializeField, Tooltip("ドリフト時の減速量(1F)")] float fDriftDecelerat; //ドリフト減速値
    
    //ダッシュ
    [SerializeField, Tooltip("ダッシュ加速量")] float fDashAddSpped = 20.0f;
    [SerializeField, Tooltip("加速アイテム効果時間（秒）")] float fDashAddTime;
    [SerializeField, Tooltip("ダッシュ中か"), ReadOnly] bool bDash = false; //ダッシュ関数を一回だけ呼ぶ用
    Coroutine _DashCor; //ダッシュコルーチン保存

    //デバフ
    Coroutine _SlowCor; //減速コルーチン保存

    //吹っ飛び
    Coroutine _BlowCor;    //吹っ飛び復帰カウントコルーチン保存
    bool bNoMove;
    bool bBlowChack;    //吹っ飛ばされてからコルーチン終了後にtrue

    //アシストシステム
    AssistOperat _AssistOperat;
    public enum ASSIST
    {
        m_STAND,
        m_RIGHT,
        m_LEFT
    };
    ASSIST assist = ASSIST.m_STAND;  //入力があるか、このイーナムによって左右に動く
    [SerializeField,Tooltip("アシスト機能を使うか否か")] bool bAssist;

    //---- GeterSeter ----
    public float GSNowSpeed { get { return fSpeed; } }
    public float GMaxSpeed { get { return MaxSpeed; } }
    public float GSfAccelerationf { get { return fAccelerationf; } }
    public float GSfFriction { get { return fFriction; } }
    public float GDashAddSpped { get { return fDashAddSpped; } }

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // rigidbodyを取得
        trans = GetComponent<Transform>();  //Transformを取得  
        _PlayerState = GameObject.Find("Player").GetComponent<PlayerState>();
        _PlayerSlopeRotate = GameObject.Find("SlopeRotate").GetComponent<PlayerSlopeRotate>();
        playerRay = GameObject.Find("RayPointFront").GetComponent<PlayerRay>();
        _AssistOperat = GetComponent<AssistOperat>();

        ASMaxBendAngle = MaxBendAngle;
        ASMaxSpeed = MaxSpeed;
    }

    private void FixedUpdate()
    {
        //アシスト機能がオンなら
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("トリガーですけど？");
            bAssist = !bAssist;
            assist = ASSIST.m_STAND;
        }

        if (bAssist)
        {
            assist = _AssistOperat.AssistCheck();    //アシストするかチェック
        }


        //吹っ飛ばされた時の地面に戻った時のフラグ
        if (bBlowChack)
        {
            if (playerRay.GetRay(3.0f, "Ground") || playerRay.GetRay(3.0f, "Ruin") || playerRay.GetRay(3.0f,"StageObject"))
            {   //地面、遺跡、障害物にレイが当たったら
                bBlowChack = false;
                bNoMove = false; //動けるようにする
            }
        }
        //ダッシュアイテム
        if (_PlayerState.state.m_DASH)
        {
            if (!bDash)
            {//ダッシュ中ではない時に取った
                _DashCor = StartCoroutine(DashCoroutine());
            }
            else
            {  //ダッシュ中に取った
                StopCoroutine(_DashCor);    //コルーチン終了
                MaxSpeed -= fDashAddSpped;
                MaxBendSpeed -= fDashAddSpped;
                _DashCor = StartCoroutine(DashCoroutine());
            }
        }


        //---- 共通して行われる移動処理 ----（行わないパターンが出てきた場合はreturn予定）
        if (bNoMove) return;    //bNoMoveなら動けない
        v3Front = -trans.forward;  //正面を変更

        Accelerator();  //アクセル！！

        if (fSpeed < 0)    //速度がマイナスなら停止状態にする
        {
            fSpeed = 0.0f;
        }

        bBend = false;  //カーブ中フラグを戻す
        //Lerp・Slerp便利
        if (InputOrder.D_Key() || assist == ASSIST.m_RIGHT) //右へ
        {
            float temp = fSpeed / MaxSpeed * MaxBendAngle;  //ハンドルの角度
            Vector3 vector3 = Vector3.Normalize(-v3Front);

            vector3 = vector3 * (trans.localScale.z / 2);
            vector3 += trans.position;
            trans.RotateAround(vector3, Vector3.up, MaxBendAngle);

            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.RIGHT);


            bBend = true;
        }

        if (InputOrder.A_Key() || assist == ASSIST.m_LEFT)//左へ
        {
            float temp = -fSpeed / MaxSpeed * MaxBendAngle;  //ハンドルの角度
            Vector3 vector3 = Vector3.Normalize(-v3Front);

            vector3 = vector3 * (trans.localScale.z / 2);
            vector3 += trans.position;
            trans.RotateAround(vector3, Vector3.up, -MaxBendAngle);

            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.LEFT);


            bBend = true;
        }

        //Assist();

        if (InputOrder.SHIFT_Key())
        {
            fSpeed -= fDriftDecelerat;
            GetComponent<PlayerSE>().SoundDrift();

            if (fSpeed >= 5.0f)
            {
                if (InputOrder.A_Key())  //左へ
                {
                    Vector3 vector3 = Vector3.Normalize(-v3Front);
                    vector3 = vector3 * (trans.localScale.z / 2);
                    vector3 += trans.position;
                    trans.RotateAround(vector3, Vector3.up, -MaxDriftBend + MaxBendAngle);
                    _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.LEFT_DRIFT);
                    _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.LEFT_DRIFT);

                }

                if (InputOrder.D_Key()) //右へ
                {
                    Vector3 vector3 = Vector3.Normalize(-v3Front);
                    vector3 = vector3 * (trans.localScale.z / 2);
                    vector3 += trans.position;
                    trans.RotateAround(vector3, Vector3.up, MaxDriftBend - MaxBendAngle);
                _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.RIGHT_DRIHT);
                _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.RIGHT_DRIHT);
                }
            }
        }
        else
        {
            GetComponent<PlayerSE>().SoundStopDrift();
        }

        if (!bBend)
        {
            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.CENTER);
        }

        float tempY = rb.velocity.y;    //Y座標は加速しない
        rb.velocity = new Vector3(v3Front.x * fSpeed, tempY, v3Front.z * fSpeed); //実際に加速



        ///////////////////////////////////////////////////////

    }

    //---- アクセル ----
    private void Accelerator()
    {
        fSpeed -= fFriction;    //アクセルしていない時ブレーキになる

        if (bBend && fSpeed >= MaxBendSpeed) return;    //カーブ中、カーブ速度より出ているなら加速できない

        if (fSpeed >= MaxSpeed) return;     //MaxSpeedより出ているなら

        if (InputOrder.W_Key())
        {
            fSpeed += fAccelerationf + fFriction;
            GetComponent<PlayerSE>().SoundAccelerator();
            if (fSpeed <= 40.0f) //40キロ以下なら倍で加速
            {
                fSpeed += fAccelerationf;
            }
        }
        else
        {//アクセルを押していないとき
            GetComponent<PlayerSE>().SoundStopAccelerator();
        }
    }

    void Assist()
    {   //アシストが動作する場合は
        if (assist == ASSIST.m_RIGHT) //右へ
        {
            float temp = fSpeed / ASMaxSpeed * ASMaxBendAngle;  //ハンドルの角度
            Vector3 vector3 = Vector3.Normalize(-v3Front);

            vector3 = vector3 * (trans.localScale.z / 2);
            vector3 += trans.position;
            trans.RotateAround(vector3, Vector3.up, ASMaxBendAngle);

            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.RIGHT);


            bBend = true;
        }

        if (assist == ASSIST.m_LEFT)//左へ
        {
            float temp = -fSpeed / ASMaxSpeed * ASMaxBendAngle;  //ハンドルの角度
            Vector3 vector3 = Vector3.Normalize(-v3Front);

            vector3 = vector3 * (trans.localScale.z / 2);
            vector3 += trans.position;
            trans.RotateAround(vector3, Vector3.up, -ASMaxBendAngle );

            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.LEFT);


            bBend = true;
        }
    }

    //---- 減速 ----
    public void SlowDown(float speed, float time)
    {
        if (!_PlayerState.state.m_SPIDER_THREAD)
        {   //初めて
            _SlowCor = StartCoroutine(SlowDownCoroutine(speed, time));

        }
        else
        {//効果時間リセット
            StopCoroutine(_SlowCor);    //コルーチン終了
            MaxSpeed += speed;
            MaxBendSpeed += speed;
            _SlowCor = StartCoroutine(SlowDownCoroutine(speed, time));
        }
    }

    //吹き飛び
    public void BlowAway(Vector3 pos, float power, float upPower = 600.0f)
    {
        Vector3 vector = trans.position - pos;
        vector = vector.normalized;
        vector = vector * power;
        rb.velocity = Vector3.zero;
        vector = new Vector3(vector.x, upPower, vector.z);
        rb.AddForce(vector, ForceMode.Impulse);
        _BlowCor = StartCoroutine(BlowCoroutine());
    }

    //吹っ飛び後の時間測定コルーチン
    IEnumerator BlowCoroutine()
    {
        bNoMove = true;
        yield return new WaitForSeconds(0.5f);
        bBlowChack = true;
    }

    //ダッシュ用コルーチン
    IEnumerator DashCoroutine()
    {
        _PlayerState.state.DashEnd();
        bDash = true;
        MaxSpeed += fDashAddSpped;
        MaxBendSpeed += fDashAddSpped;
        fSpeed += fDashAddSpped;
        yield return new WaitForSeconds(fDashAddTime);
        MaxSpeed -= fDashAddSpped;
        MaxBendSpeed -= fDashAddSpped;
        bDash = false;
    }

    //減速デバフ用コルーチン
    IEnumerator SlowDownCoroutine(float speed, float time)
    {
        _PlayerState.state.m_SPIDER_THREAD = true;
        MaxSpeed -= speed;
        MaxBendSpeed -= speed;
        yield return new WaitForSeconds(time);
        _PlayerState.state.m_SPIDER_THREAD = false;
        MaxSpeed += speed;
        MaxBendSpeed += speed;
    }
}
