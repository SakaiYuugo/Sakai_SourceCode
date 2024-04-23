using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCentipede_BodyManager : MonoBehaviour
{
    public enum Centipede_Effect
    {
        THACKING = 0,
        MAX,
    }

    public enum Centipede_SE
    {
        THACKING = 0,
        MAX,
    }

    enum BODYSTATE
    {
        DEFAULT = 0,
        STAN,
        WHEEL,
        STAY,
        HEADUP,
    }

    List<GameObject> PartsList;
    GameObject HeadObject;

    [SerializeField]
    BODYSTATE NowState;

    [SerializeField]
    bool Jump = false;
    [SerializeField]
    bool Straight = false;
    [SerializeField]
    bool GetUp = false;
    [SerializeField]
    float JumpPower = 1.0f;
    [SerializeField]
    GameObject WheelCentipede;
    [SerializeField]
    float GetUPHeight = 10.0f;
    [SerializeField]
    GameObject[] CentipedeEffect;
    [SerializeField]
    AudioClip[] CentipedeSE;

    GameObject InstWheelCentipede;

    //木を作るなどの処理
    BigTree_Manager bigTreeManager;

    // Start is called before the first frame update
    void Awake()
    {
        PartsList = new List<GameObject>();
        NowState = BODYSTATE.DEFAULT;
    }

    private void Start()
    {
        bigTreeManager = GameObject.Find("BigTree_Manager").GetComponent<BigTree_Manager>();
    }

    private void FixedUpdate()
    {
        return;
        //if(Jump)
        //{
        //    CentipedeJump(JumpPower);
        //    Jump = false;
        //}

        //if(Straight)
        //{
        //    SetStraight();
        //    Straight = false;
        //}

        //if(GetUp)
        //{
        //    ChangeHeadGetUPMode();
        //    GetUp = false;
        //}

    }

    public void SetCentipedeParts(GameObject ThisGameObject)
    {
        PartsList.Add(ThisGameObject);
        //頭だった場合
        if (ThisGameObject.name == "Head")
        {
            HeadObject = ThisGameObject;
        }
    }

    public void CentipedeJump(float jumpPower)
    {
        float TempPower;

        foreach(GameObject copy in PartsList)
        {
            TempPower = JumpPower * Random.Range(0.98f, 1.02f);

            copy.GetComponent<BossCentipede_Jump>().SetJump(TempPower);
        }
    }

    public void ChangeDefaultMode(bool warp = false)
    {
        BODYSTATE TempState = NowState;

        NowState = BODYSTATE.DEFAULT;
        foreach (GameObject copy in PartsList)
        {
            //タイヤモードだった場合
            if (TempState == BODYSTATE.WHEEL)
            {
                //位置を設定し動けるようにする
                BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
                partsManager.LoadTentavitePos();
                if (warp) partsManager.LoadWarp();        //体を曲げる
                System_ObjectManager.bossObject = HeadObject;       //頭を今のオブジェクトに変えてやる
                partsManager.SetComponentEneble(true);              //追跡などのすべてのコンポーネントを動かす
                StartRigidBody(copy);
                bigTreeManager.EndWheelCentipede();         //木の再生処理など

            }

            if (TempState == BODYSTATE.HEADUP)
            {
                //位置を設定し動けるようにする
                BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
                if (warp) partsManager.LoadWarp();
                partsManager.SetEndGetUP();
            }
        }
    }

    public void ChangeDefaultMode(Vector3 Pos,bool warp = false)
    {
        BODYSTATE TempState = NowState;

        NowState = BODYSTATE.DEFAULT;

        Pos.y += 5.0f;

        HeadObject.transform.position = Pos;

        foreach (GameObject copy in PartsList)
        {
            //タイヤモードだった場合
            if (TempState == BODYSTATE.WHEEL)
            {
                //位置を設定し動けるようにする
                BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
                partsManager.LoadTentavitePos(Pos);
                if (warp) partsManager.LoadWarp();        //体を曲げる
                System_ObjectManager.bossObject = HeadObject;       //頭を今のオブジェクトに変えてやる
                partsManager.SetComponentEneble(true);    //追跡などのすべてのコンポーネントを動かす
                StartRigidBody(copy);
                bigTreeManager.EndWheelCentipede();         //木の再生処理など
            }

            if(TempState == BODYSTATE.HEADUP)
            {
                //位置を設定し動けるようにする
                BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
                partsManager.LoadTentavitePos(Pos);
                if (warp) partsManager.LoadWarp();
                partsManager.SetEndGetUP();                
            }
        }
    }

    //頭を上げるモーション
    public void ChangeHeadGetUPMode()
    {
        BODYSTATE TempState = NowState;

        NowState = BODYSTATE.HEADUP;

        foreach(GameObject copy in PartsList)
        {
            if(TempState == BODYSTATE.DEFAULT)
            {
                //位置を設定し動けるようにする
                BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
                partsManager.SetStartGetUP(5.0f);
                //エフェクトを止めてやる

            }
        }
    }

    //スタンモード
    public void ChangeStanMode(Vector3 Pos)
    {
        BODYSTATE TempState = NowState;

        NowState = BODYSTATE.STAN;

        HeadObject.transform.position = Pos;

        foreach (GameObject copy in PartsList)
        {
            //タイヤモードだった場合
            if (TempState == BODYSTATE.WHEEL)
            {
                BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
                partsManager.LoadTentavitePos(Pos);                            //場所を移動する
                partsManager.LoadWarp();                                        //体を曲げてやる
                System_ObjectManager.bossObject = HeadObject;
                partsManager.SetComponentEneble(true);                     //追跡などのすべてのコンポーネントを動かす
                StartRigidBody(copy);
                bigTreeManager.EndWheelCentipede();         //木の再生処理など
            }
            //スタンするときに呼ぶ関数
        }
        AnimationSpeed(0.5f,1.0f);
        
    }

    //タイヤモード変換
    public void ChangeWheelMode()
    {
        NowState = BODYSTATE.WHEEL;

        //タイヤムカデを生成
        InstWheelCentipede = Instantiate(WheelCentipede, HeadObject.transform.position, HeadObject.transform.rotation);          //頭の位置に生成する
        InstWheelCentipede.transform.parent = transform.root.gameObject.transform;                                              //親の設定
        System_ObjectManager.bossObject = InstWheelCentipede;

        //木を作るなどの処理
        bigTreeManager.StartWheelCentipede();

        //生成をした後に今の体を見えない場所に置いておく
        //体のパーツの設定
        foreach (GameObject copy in PartsList)
        {
            //-------------------------
            //見えないように体は地面の下に置いておく

            BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
            partsManager.StopJump();                //ジャンプを止める
            partsManager.SaveTentativePos();        //今いる位置をセーブする
            partsManager.SetComponentEneble(false); //追跡などのすべてのコンポーネントを止める
            StopRigidBody(copy);
            copy.transform.position = new Vector3(0.0f, -100.0f, 0.0f);
            //partsManager.SetPartsActive(false);
        }
    }

    public void ChangeStayMode()
    {
        NowState = BODYSTATE.STAY;
        foreach (GameObject copy in PartsList)
        {
            copy.GetComponent<BossCentipede_PartsManager>().SetPartsActive(false);
        }
    }

    void StartRigidBody(GameObject temp)
    {
        Rigidbody rb = temp.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;             //今の動きを止める処理
        rb.isKinematic = false;
    }

    void StopRigidBody(GameObject temp)
    {
        Rigidbody rb = temp.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;             //今の動きを止める処理
        rb.isKinematic = true;
    }

    public void AnimationSpeed(float Speed,float MaxSpeed)
    {
        Speed = Speed / MaxSpeed;

        foreach (GameObject copy in PartsList)
        {
            copy.GetComponent<BossCentipede_PartsManager>().SetAnimeSpeed(Speed);
        }
    }

    //頭を中心に移動してやる
    public void ChangePos(Vector3 Pos)
    {
        foreach(GameObject copy in PartsList)
        {
            //位置を設定し動けるようにする
            BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
            partsManager.ChangePos(Pos);
        }
    }


    //体を真っすぐにしてやる
    public void SetStraight()
    {
        foreach (GameObject copy in PartsList)
        {
            copy.GetComponent<BossCentipede_PartsManager>().SetStraight();
        }
    }

    //体を曲げる
    public void SetWarp()
    {
        foreach(GameObject copy in PartsList)
        {
            //位置を設定し動けるようにする
            BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
            partsManager.LoadWarp();
        }
    }


    public int GetPartsNum()
    {
        return PartsList.Count;
    }

    public GameObject GetCentipedeEffect(Centipede_Effect effectType)
    {
        return CentipedeEffect[(int)effectType];
    }

    public AudioClip GetCentipedeSE(Centipede_SE seType)
    {
        return CentipedeSE[(int)seType];
    }
}
