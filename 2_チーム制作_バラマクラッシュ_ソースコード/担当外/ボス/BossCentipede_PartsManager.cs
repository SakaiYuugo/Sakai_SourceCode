using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class BossCentipede_PartsManager : MonoBehaviour
{
    [SerializeField]
    int PartsNum = 0;

    [SerializeField]
    float Distance = 15.0f;

    [SerializeField]
    GameObject TargetObject = null;

    [SerializeField]
    float AxisAngle = 20.0f;

    [SerializeField,Range(0.0f,1.0f)]
    float StartAnimeValue = 0.0f;

    [SerializeField]
    GameObject UsuallyBody;
    [SerializeField]
    GameObject BreakBody;

    BossCentipede_BodyManager CentipedeBodyManager;
    Vector3 TentativePos;
    GameObject Head;


    BossCentipede_LookObj AddLookComponent;
    BossCentipede_TrackObj AddTrackComponent;
    BossCentipede_Jump AddJumpComponent;
    BossCentipede_GetUpParts AddUpParts;

    Animator PartsAnimetion;
    int NowBodyNum;
    bool ThisHead;

    
    void Start()
	{
		//頭だった場合
		if (gameObject.name == "Head")
		{
			//マネージャーに追加
			System_ObjectManager.bossObject = this.gameObject;
		}

		//自分たちを制御するBodyManagerを探す
		CentipedeBodyManager =
        GameObject.Find("BodyManager").GetComponent<BossCentipede_BodyManager>();

        //BodyManagerに自分たちを登録する
        CentipedeBodyManager.SetCentipedeParts(this.gameObject);

        TentativePos = transform.position;

        Head = GameObject.Find("Head");

        SetGetUsuallyBody();
    }

    private void Awake()
    {
        if(gameObject.name != "Head" && gameObject.name != "Tail")
        {
            PartsNum = int.Parse(Regex.Replace(gameObject.name, @"[^0-9]", ""));
        }
        else 
        {
            PartsNum = 100;
        }


        //パーツに付けたいコンポーネントを付ける
        this.gameObject.AddComponent<BossCentipede_TrackObj>();
        this.gameObject.AddComponent<BossCentipede_LookObj>();
        this.gameObject.AddComponent<BossCentipede_Jump>();
        this.gameObject.AddComponent<BossCentipede_GetUpParts>();

        AddTrackComponent = GetComponent<BossCentipede_TrackObj>();
        AddLookComponent  = GetComponent<BossCentipede_LookObj>();
        AddJumpComponent  = GetComponent<BossCentipede_Jump>();
        AddUpParts = GetComponent<BossCentipede_GetUpParts>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetDistance()
    {
        return Distance;
    }

    public GameObject GetTargetObject()
    {
        return TargetObject;
    }

    public float GetAxisAngleToRadion()
    {
        return AxisAngle * 3.14f / 180.0f;
    }

    public BossCentipede_BodyManager GetBodyManager()
    {
        return CentipedeBodyManager;
    }

    public void SetPartsActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void StopJump()
    {
        GetComponent<BossCentipede_Jump>().StopJump();
    }

    public void SaveTentativePos()
    {
        if(this.gameObject.name == "Head")
        {
            //自分の位置を入れる
            TentativePos = transform.position;
        }
        else
        {
            TentativePos = transform.position - Head.transform.position;
        }

        TentativePos.y = 0.0f;
    }

    public void LoadTentavitePos()
    {
        LoadTentavitePos(TentativePos);
    }

    public void LoadTentavitePos(Vector3 NewPos)
    {
        if (this.gameObject.name == "Head")
        {
            //自分の位置を入れる
            transform.position = NewPos;
        }
        else
        {
            transform.position = TentativePos + NewPos;
        }
    }

    public void LoadWarp()
    {
        AddLookComponent.SetwarpPos();
    }

    public void SetComponentEneble(bool active)
    {
        AddTrackComponent.enabled =
            AddLookComponent.enabled =
                AddJumpComponent.enabled =
                    AddUpParts.enabled = active;
    }

    //頭を上げるときに呼ぶ関数
    public void SetStartGetUP(float Height)
    {
        AddUpParts.SetGetUp(Height);
        AddTrackComponent.enabled =
            AddLookComponent.enabled =
                AddJumpComponent.enabled = false;
    }

    public void SetEndGetUP()
    {
        AddUpParts.SetDefaultPos();
        AddTrackComponent.enabled =
            AddLookComponent.enabled =
                AddJumpComponent.enabled = true;
    }

    public void SetAnimeSpeed(float Speed)
    {
        if (this.gameObject.name != "Head" && this.gameObject.name != "Tail")
        {
            PartsAnimetion = transform.GetChild(0).GetComponent<Animator>();
            PartsAnimetion.SetFloat("speed", Speed);
        }
    }

    //体を真っすぐにしてやる
    public void SetStraight()
    {
        AddLookComponent.SetStraightPos();
    }

    //この体が前から何番目か
    public int GetPartsNum()
    {
        return PartsNum;
    }

    //頭を返す
    public GameObject GetHead()
    {
        return Head;
    }

    public GameObject SetGetUsuallyBody()
    {
        if (this.gameObject.name == "Head" || this.gameObject.name == "Tail")
        {
            return null;
        }
        //falseなら
        if (!UsuallyBody.activeInHierarchy)
        {
            UsuallyBody.SetActive(true);  //普通のやつ        
        }
 
        //trueなら
        if (BreakBody.activeInHierarchy)
        {
            BreakBody.SetActive(false);   //壊れたやつ
        }

        //アニメーション処理
        PartsAnimetion = UsuallyBody.GetComponent<Animator>();
        PartsAnimetion.SetFloat("delay", StartAnimeValue);
        return UsuallyBody;

    }

    public GameObject SetGetBreakBody()
    {
        if (this.gameObject.name == "Head" || this.gameObject.name == "Tail")
        {
            return null;
        }
        //falseなら
        if (UsuallyBody.activeInHierarchy)
        {
            UsuallyBody.SetActive(false);  //普通のやつ        
        }

        //trueなら
        if (!BreakBody.activeInHierarchy)
        {
            BreakBody.SetActive(true);   //壊れたやつ
        }

        //アニメーション処理
        //PartsAnimetion = BreakBody.GetComponent<Animator>();
        //PartsAnimetion.SetFloat("delay", StartAnimeValue);

        return BreakBody;
    }

    public void ChangePos(Vector3 NextPos)
    {
        if (this.gameObject.name == "Head")
        {
            //自分の位置を入れる
            transform.position = NextPos;
        }
        else
        {
            //頭の位置との差を計算して頭の位置に足してやる
            Vector3 DifPos = Head.transform.position - transform.position;
            transform.position = NextPos + DifPos;
        }
    }
}
