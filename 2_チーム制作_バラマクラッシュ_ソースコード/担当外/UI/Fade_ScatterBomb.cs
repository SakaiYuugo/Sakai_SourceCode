using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade_ScatterBomb : MonoBehaviour
{
    bool Explosion;
    RectTransform rectTransform;
    float BombCount;
    float BombTime;

    Vector2 InitPos;
    Vector2 StartPos;
    Vector2 EndPos;

    Vector2 AddBombVector;
    float SmashPower;
    float Gravity;

    bool End;
    Timer timer;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        InitPos = rectTransform.anchoredPosition;

        StartPos = InitPos;

        //最初の位置の計算(LerpStartPosObject)
        Vector2 Temp = rectTransform.anchoredPosition;
        Temp.y = Screen.height + 400.0f;
        EndPos = Temp;

        Gravity = 4000.0f;
        End = false;
        Explosion = false;

        timer = new Timer();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if(!Explosion)
        {
            return;
        }

        //座標更新
        rectTransform.anchoredPosition = rectTransform.anchoredPosition + (AddBombVector * Time.fixedDeltaTime);
        AddBombVector.y -= (Gravity * Time.fixedDeltaTime);

        float marginDistance = 30.0f;
        Vector3 NowPos = rectTransform.anchoredPosition;

        //ある程度外に出た
        if ((NowPos.x < 0.0f - marginDistance || NowPos.x > Screen.width + marginDistance) &&
            (NowPos.y < 0.0f - marginDistance || NowPos.y > Screen.width + marginDistance))
        {
            End_ExplosionBomb();
        }
    }

    //この関数を呼ぶとフェードインを始める
    public void Start_ExplosionBomb()
    {
        this.gameObject.SetActive(true);
        //最初の位置の設定
        rectTransform.anchoredPosition = StartPos;

        //吹っ飛ばす方向を決める
        float ScreenWidth = Screen.width;
        Vector3 SmashVector;
        if (ScreenWidth * 0.5f > rectTransform.anchoredPosition.x)
        {
            //左にいる
            float RandomAngle = Random.Range(-20.0f,40.0f);
            SmashVector = (Quaternion.AngleAxis(RandomAngle, Vector3.forward) * Vector3.up);
        }
        else
        {
            //右にいる
            float RandomAngle = -Random.Range(-20.0f,40.0f);
            SmashVector = (Quaternion.AngleAxis(RandomAngle, Vector3.forward) * Vector3.up);
        }
        SmashPower = Random.Range(2500.0f, 3000.0f);
        AddBombVector = SmashVector * SmashPower;
        Explosion = true;
        End = false;
    }

    //これを呼べば強制的にフェードインが終わる
    public void End_ExplosionBomb()
    {
        rectTransform.anchoredPosition = EndPos;
        Explosion = false;
        End = true;
        this.gameObject.SetActive(false);
    }

    public bool GetEnd()
    {
        return End;
    }
}
