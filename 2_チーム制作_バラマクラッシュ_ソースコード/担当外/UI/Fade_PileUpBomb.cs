using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade_PileUpBomb : MonoBehaviour
{
    [SerializeField]
    float FallTime = 1.0f;
    
    bool PileUP;
    RectTransform rectTransform;
    Vector2 InitPos;
    Vector2 StartPos;
    Vector2 EndPos;
    float FallStartTime;
    float Count;
    bool End;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        InitPos = rectTransform.anchoredPosition;

        //最初の位置の計算(LerpStartPosObject)
        Vector2 Temp = rectTransform.anchoredPosition;
        Temp.y = Screen.height + 400.0f;
        StartPos = Temp;

        //最後の位置(ThisGameObject)
        EndPos = rectTransform.anchoredPosition;
        //どれくらいのタイミングで落ちるか計算
        FallStartTime = (rectTransform.anchoredPosition.y / Screen.height) * 1.0f;
        if (FallStartTime < 0.0f)
        {
            FallStartTime = 0.0f;
        }

        End = false;
        PileUP = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!PileUP)
        {
            return;
        }

        //まだ落ちきる前
        if (Count < FallStartTime + FallTime)
        {
            Count += Time.fixedDeltaTime;

            //カウントしきったら
            if(Count > FallStartTime + FallTime)
            {
                //終わる
                End_PileUPBomb();
            }
        }

        //時間が落ちる前の場合
        if (FallStartTime > Count)
        {
            return;
        }

        //補間
        float Value = Mathf.Pow((Count - FallStartTime) / FallTime, 4);

        rectTransform.anchoredPosition = Vector2.Lerp(StartPos, EndPos, Value);

    }

    //この関数を読んでやるとフェードアウトを始める
    public void Start_PileUpBomb()
    {
        this.gameObject.SetActive(true);
        //最後に最初の場所を設定
        rectTransform.anchoredPosition = StartPos;
        Count = 0.0f;
        PileUP = true;
        End = false;
    }

    //これを呼べば強制的にフェードアウトが終わる
    public void End_PileUPBomb()
    {
        rectTransform.anchoredPosition = EndPos;
        Count = FallStartTime + FallTime;
        PileUP = false;
        End = true;
    }

    //こいつがtrueになれば終わった
    public bool GetEnd()
    {
        return End;
    }
}
