using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCentipede_GetUpParts : MonoBehaviour
{
    enum STATE
    {
        UP = 0,
        DOWN,
        END,
        NONE,
        MAX,
    }

    [SerializeField]
    float NowHeight;

    float HeightValue;
    STATE nowState;
    BossCentipede_PartsManager PartsManager;
    Vector3 GoUPStartPoint;
    Vector3 GoUpEndPoint;
    float Distance;
    float Count;
    GameObject TargetObject;

    // Start is called before the first frame update
    void Start()
    {
        PartsManager = GetComponent<BossCentipede_PartsManager>();

        SetInformation();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        NowHeight = transform.position.y;

        if (TargetObject != null)
        {
            transform.LookAt(TargetObject.transform);
        }

        switch(nowState)
        {
            case STATE.NONE:

                break;

            case STATE.UP:
                const float UPTime = 3.0f;
                transform.localPosition = Vector3.Lerp(GoUPStartPoint, GoUpEndPoint, Count / UPTime);
                Count += Time.deltaTime;
                if(Count > UPTime)
                {
                    nowState = STATE.DOWN;
                    Count = 0.0f;
                }

                Debug.Log("上がります");

                break;

            case STATE.DOWN:
                const float DOWNTime = 0.3f;
                transform.localPosition = Vector3.Lerp(GoUPStartPoint, GoUpEndPoint, Count / UPTime);
                Count += Time.deltaTime;
                if (Count > DOWNTime)
                {
                    if (this.gameObject.name == "Head")
                    {
                        //エフェクトを出してやる
                        GameObject Effect = Instantiate(PartsManager.GetBodyManager().GetCentipedeEffect(BossCentipede_BodyManager.Centipede_Effect.THACKING), transform.position, Quaternion.identity);
                        Effect.GetComponent<AudioSource>().clip = PartsManager.GetBodyManager().GetCentipedeSE(BossCentipede_BodyManager.Centipede_SE.THACKING);
                    }

                    nowState = STATE.END;
                    Count = 0.0f;
                }
                Debug.Log("下がります");


                break;
            case STATE.END:
                //終った時の処理を書いてやる

                GameObject.Find("BodyManager").GetComponent<BossCentipede_BodyManager>().ChangeDefaultMode();
                transform.parent.GetComponent<CentipedeState>().StateChange(CentipedeState.STATE.Move);
                nowState = STATE.NONE;

                break;
            case STATE.MAX:

                break;
        }
    }

    void SetInformation()
    {
        nowState = STATE.NONE;
        Distance = PartsManager.GetDistance() * 0.1f;          //距離の取得
        TargetObject = PartsManager.GetTargetObject();  //ターゲット
        HeightValue = 10.0f;                            //高さの設定
        GoUPStartPoint = GoUpEndPoint = Vector3.zero;
    }

    public void SetGetUp(float Height)
    {
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;             //今の動きを止める処理
        rb.isKinematic = true;

        nowState = STATE.UP;
        Count = 0.0f;
        HeightValue = Height;
        int UPObjNum = 5;
        GoUPStartPoint = GoUpEndPoint = transform.localPosition;// transform.position;
        //GoUpEndPoint = GoUPStartPoint + (-transform.forward * (Distance * 0.5f));     //後ろに少し下げてやる
        //各パーツの上がる最大の高さを求める
        if (this.gameObject.name == "Head")
        {
            Vector3 TempForward = transform.forward * -1.0f;
            TempForward.y = 0.0f;
            GoUpEndPoint += (TempForward * (Distance * 0.5f));     //後ろに少し下げてやる
            GoUpEndPoint.y = HeightValue + GoUPStartPoint.y;
        }
        else if (this.gameObject.name == "Tail")
        {
            //何もしない
        }
        else
        {
            int PartsNum = PartsManager.GetPartsNum();
            if(PartsNum > UPObjNum)
            {
                return;
            }

            //昇順を降順にしてやる
            float TempPartsNum = (PartsNum * -1 + UPObjNum);

            Vector3 TempForward = transform.forward * -1.0f;
            TempForward.y = 0.0f;
            GoUpEndPoint += (TempForward.normalized * (Distance * 0.5f));     //後ろに少し下げてやる

            //上げないといけない高さを設定
            GoUpEndPoint.y = (HeightValue * (TempPartsNum / (float)UPObjNum)) + GoUPStartPoint.y;
        }

        

    }

    public void SetDefaultPos()
    {
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;             //今の動きを止める処理
        //transform.position = GoUPStartPoint;
        nowState = STATE.NONE;
    }
}
