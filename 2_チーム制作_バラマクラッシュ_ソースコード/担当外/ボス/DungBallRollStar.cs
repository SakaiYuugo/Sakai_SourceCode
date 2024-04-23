using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBallRollStar : MonoBehaviour
{
    DungBarrier _BossBarrier;
    DungManager Manager;
    int nConstWaitFrame = 100;   //後で虫のアニメーション時間を入れる
    [SerializeField] int nConstSequelFrame = 50;   //後隙時間
    int nFrame;
    bool bChangeState;
    Vector3 CenterPoint; //マップの中心にある目印座標 (これは動く予定ないのでTransformいらんかな)
    float fLenght;  //一辺の長さ
    [ReadOnly] int nGoPoint;
    [SerializeField] float fSpeed;
    Vector3 StartPos;    //球が転がり始める開始位置
    SpriteRenderer MagicCircle; //魔法陣

    List<Vector3> starPoints = new List<Vector3>();

    public enum UpImpactSTATE
    {
        m_None,     //呼ばれていないとき
        m_ROTATE,   //飛ばす方向の後ろに虫が移動
        m_ANIME,     //アニメーション待機と空白時間
        m_MOVE,     //星形に移動中
        m_END,      //虫の目的地方向へ方向転換
    }
    [ReadOnly] public UpImpactSTATE state;

    Rigidbody rb;

    void Start()
    {
        Manager = GameObject.Find("Boss_Point").GetComponent<DungManager>();
        _BossBarrier = GameObject.Find("Boss_DungBeetle").GetComponent<DungBarrier>();
        CenterPoint = GameObject.Find("MapCenterPoint").GetComponent<Transform>().position;

        rb = GetComponent<Rigidbody>();
        state = UpImpactSTATE.m_None;
        GameObject Beetle = GameObject.Find("Boss_Dungbeetle");
        MagicCircle = GameObject.Find("MagicCircle").GetComponent<SpriteRenderer>();
        bChangeState = true;
    }

    void FixedUpdate()
    {
        //一度だけ入る
        if (bChangeState)
        {
            switch (state)
            {
                case UpImpactSTATE.m_ROTATE:

                    break;

                case UpImpactSTATE.m_ANIME:
                    Manager._DungAnime.BeamAnime(); //球を飛ばすアニメ
//                    MagicCircle.enabled = true;
                    //ここに魔法陣召喚--------------------------------------
                    nFrame = 0;
                    break;

                case UpImpactSTATE.m_MOVE:
                    Manager._DungSound.Sound(DungSound.DUNG_SOUND.SOUND_ROLLSTAR);
                    StartPos = transform.position;
                    //魔法陣削除-------------------------------------------
                    //フンコロはガード状態になる
//                    MagicCircle.enabled = false;
                    Manager._DungAnime.BarrierAnim();
                    _BossBarrier.Barrier(true, false);
                    break;

                case UpImpactSTATE.m_END:
                    Manager._DungAnime.WalkAnim();

                    break;
            }
            bChangeState = false;
        }

        switch (state)
        {
            case UpImpactSTATE.m_ROTATE:
                float maxRotation = 1.4f;

                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(starPoints[0] - this.transform.position), maxRotation);
                Manager._DBMove.Turnaround();

                // 目的の角度まで回転出来たら
                if (this.transform.rotation == Quaternion.LookRotation(starPoints[0] - this.transform.position))
                {
                    state = UpImpactSTATE.m_ANIME;
                    bChangeState = true;
                }
                break;

            case UpImpactSTATE.m_ANIME:
                nFrame++;
                if(nFrame >= nConstWaitFrame)
                {
                    state = UpImpactSTATE.m_MOVE;
                    bChangeState = true;
                    nFrame = 0;
                }
                break;

            case UpImpactSTATE.m_MOVE:
                //チェックポイントに向かって移動
                transform.position = Vector3.MoveTowards(transform.position, starPoints[nGoPoint], fSpeed);
                Manager._Db.Rotate();

                if (transform.position.x <= starPoints[nGoPoint].x + 5
                 && transform.position.x >= starPoints[nGoPoint].x - 5
                 && transform.position.z <= starPoints[nGoPoint].z + 5
                 && transform.position.z >= starPoints[nGoPoint].z - 5)  //チェックポイントに近づいたら
                {
                    nGoPoint += 1;
                    transform.LookAt(starPoints[nGoPoint]); //進行方向を向く
                    if (nGoPoint == 5)
                    {
                        transform.position = StartPos;
                        _BossBarrier.Barrier(false,false);
                        state = UpImpactSTATE.m_END;
                        bChangeState = true;
                    }
                }
                break;

            case UpImpactSTATE.m_END:
                float maxRotat = 1.4f;
          
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, 
                    Quaternion.LookRotation(Manager._DBMove.lV3CheckPoint[Manager._DBState.nGoPoint] - this.transform.position), maxRotat);
                Manager._DBMove.Turnaround();

                // 目的の角度まで回転出来たら
                if (this.transform.rotation == Quaternion.LookRotation(Manager._DBMove.lV3CheckPoint[Manager._DBState.nGoPoint] - this.transform.position))
                {
                    state = UpImpactSTATE.m_None;
                    starPoints.Clear();
                    Manager._DBState.state = DungBeetleState.DungBeetleSTATE.m_MOVE;
                    Manager._DBState.bChangeState = true;
                }
                break;
        }
    }

    //攻撃開始時に呼ぶ
    public void StartAtk(Vector3 endGoPos)
    {
        CreatePoint();
        state = UpImpactSTATE.m_ROTATE;
        nGoPoint = 0;
    }

    //玉の動くポイントを生成する
    void CreatePoint()
    {
        fLenght = Vector3.Distance(transform.position, CenterPoint) * 1.0f;    //一辺の長さ
        Vector3 nowPos = new Vector3(transform.position.x, transform.position.y + 10.0f, transform.position.z);//発動位置、for文中ではワールド座標上の目的ポイントを表す
        Vector3 Vec;
        for (int i = 0; i < 5; i++)
        {
            Vec = nowPos - CenterPoint;
            Vec = Quaternion.Euler(0, 144, 0) * Vec;
            starPoints.Add(Vec + CenterPoint);
            nowPos = Vec + CenterPoint;
        }
        starPoints.Add(starPoints[0]);
    }
}
