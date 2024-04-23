using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuff : MonoBehaviour
{
    //今後増えるであろうバフ効果のリスト
    public enum BuffEfficacy
    {
        Enemy_SpeedUp = 0,
        Enemy_HPUp,
        Enemy_SpeedHpUp,
        PlayerSpeedDown,
        PlayerHarden,
        Max
    }

    [SerializeField] BuffEfficacy BuffType = BuffEfficacy.Max;
    [SerializeField] EnemyZakoState.TYPEBEETLE SetBeetleType = EnemyZakoState.TYPEBEETLE.NONE;
    [SerializeField] float AddBuff_TimeInterval = 10.0f;
    [SerializeField] int size = 50;
    [SerializeField] GameObject[] BuffEffects = new GameObject[(int)BuffEfficacy.Max];
    [SerializeField] AudioClip[] Sound;
    private AudioSource audioSource;

    private Timer BuffTimer;
    private List<GameObject> BuffObjects;
    public bool BuffCheck = false;
    GameObject player;
    float dist;

    private void Awake()
    {
        BuffObjects = new List<GameObject>();
    }

    void Start()
    {
        EnemySupportmove move = transform.parent.GetComponent<EnemySupportmove>();
        BuffType = (BuffEfficacy)move.BuffType;
        //BuffType = (BuffEfficacy)Random.Range(0, (int)BuffEfficacy.Max);

        //この後に出すエフェクトの設定をしないと見た目でバフの判断がつかない
        foreach (GameObject copy in BuffEffects)
        {
            copy.SetActive(false);
        }

        gameObject.transform.localScale = Vector3.one * size;
        BuffTimer = new Timer();
        BuffTimer.Set(AddBuff_TimeInterval);

        //エフェクトを出してやる
        BuffEffects[(int)BuffType].SetActive(true);

        player = System_ObjectManager.playerObject;
        dist = 0.0f;

        //SE関連
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        //一定時間ごとにバフをかけてやる
        if (BuffTimer.ScaledUpdate())
        {
            SetBuffer();
            AudioSE();  //SEを鳴らす
            BuffTimer.Set(AddBuff_TimeInterval);
        }
    }

    //付けたいバフを書いてください
    void SetBuffer()
    {
        dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist < 30.0f)
        {
            switch (BuffType)
            {
                case BuffEfficacy.PlayerHarden:
                    player.AddComponent<PlayerDeBuff_Speed>();
                    break;
                case BuffEfficacy.PlayerSpeedDown:
                    player.AddComponent<PlayerDeBuff_FallDown>();
                    break;
            }
        }

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 30.0f, Vector3.forward, 0.01f, LayerMask.GetMask("Enemy"));
        GameObject[] obj = new GameObject[hits.Length];
        for (int i = 0; i < hits.Length; i++)
        {
            obj[i] = hits[i].collider.gameObject;
        }


        if (hits.Length == 0)
        {
            return;
        }

        for (int i = 0; i < obj.Length; i++)
        {
            //GameObject copy = hits[i].collider.gameObject;
            /*
            if(obj[i] == null)
            {
                BuffObjects.RemoveAt(i);
                i--;
                return;
            }
            */
            switch (obj[i].tag)
            {
                case "Enemy":
                    {
                        //オブジェクトなどを仮に置いていやる
                        GameObject Beetle = obj[i].transform.root.gameObject;
                        EnemyZakoState TempState = Beetle.GetComponent<EnemyZakoState>();

                        //もし雑魚のstateがなかった場合
                        if (TempState == null)
                            break;

                        //虫の種類を持ってきて
                        EnemyZakoState.TYPEBEETLE BeetleType = TempState.GetBeetleType();
                        
                        //まずバフをつける虫ではなかったら出る
                        if (SetBeetleType != EnemyZakoState.TYPEBEETLE.NONE)
                        {
                            if (BeetleType != SetBeetleType)
                                break;
                        }
                        
                        //バトルタイプを持ってくる
                        EnemyZakoState.BATTLETYPE BattleType = TempState.GetBattleType();

                        //サポートだった場合抜ける
                        if (BattleType == EnemyZakoState.BATTLETYPE.SUPPORT)
                        {
                            break;
                        }

                        //AddComponentすればいいところを設定してやる
                        switch (BeetleType)
                        {
                            case EnemyZakoState.TYPEBEETLE.BEE:
                                break;
                            case EnemyZakoState.TYPEBEETLE.CENTIPEDE:
                                {
                                    Beetle = Beetle.transform.Find("CentipedeHead").gameObject;
                                    //Beetle = Beetle.transform.GetChild(0).gameObject;
                                }
                                break;
                            case EnemyZakoState.TYPEBEETLE.DUNGBEETLE:
                                {

                                }
                                break;
                            case EnemyZakoState.TYPEBEETLE.HOPPER:
                                {

                                }
                                break;
                            case EnemyZakoState.TYPEBEETLE.KATYDID:
                                {

                                }
                                break;
                            case EnemyZakoState.TYPEBEETLE.ANT:
                                {

                                }
                                break;
                            case EnemyZakoState.TYPEBEETLE.SPIDER:

                                break;
                        }

                        switch (BuffType)
                        {
                            case BuffEfficacy.Enemy_SpeedUp:
                                {
                                    switch (BeetleType)
                                    {
                                        case EnemyZakoState.TYPEBEETLE.BEE:
                                            {
                                                Beetle.AddComponent<BeeBuff_SpeedUp>();                                             
                                            }
                                            break;
                                        case EnemyZakoState.TYPEBEETLE.CENTIPEDE:
                                            {
                                                Beetle.AddComponent<CentipedeBuff_SpeedUp>();
                                            }
                                            break;
                                        case EnemyZakoState.TYPEBEETLE.DUNGBEETLE:
                                            {
                                                Beetle.AddComponent<DungBeetleBuff_SpeedUp>();
                                            }
                                            break;
                                    }
                                }
                                break;
                            case BuffEfficacy.Enemy_HPUp:
                                {
                                    switch (BeetleType)
                                    {
                                        case EnemyZakoState.TYPEBEETLE.BEE:
                                        case EnemyZakoState.TYPEBEETLE.CENTIPEDE:
                                        case EnemyZakoState.TYPEBEETLE.DUNGBEETLE:
                                        case EnemyZakoState.TYPEBEETLE.HOPPER:
                                        case EnemyZakoState.TYPEBEETLE.KATYDID:
                                        case EnemyZakoState.TYPEBEETLE.ANT:
                                        case EnemyZakoState.TYPEBEETLE.SPIDER:
                                            {
                                                Beetle.AddComponent<EnemyBuff_HpUp>();
                                            }
                                            break;
                                    }
                                }
                                break;
                            case BuffEfficacy.Enemy_SpeedHpUp:
                                {
                                    switch (BeetleType)
                                    {
                                        case EnemyZakoState.TYPEBEETLE.BEE:
                                            {
                                                Beetle.AddComponent<BeeBuff_SpeedUp>();
                                                Beetle.AddComponent<EnemyBuff_HpUp>();
                                            }
                                            break;
                                        case EnemyZakoState.TYPEBEETLE.CENTIPEDE:
                                            {
                                                Beetle.AddComponent<CentipedeBuff_SpeedUp>();
                                                Beetle.AddComponent<EnemyBuff_HpUp>();
                                            }
                                            break;
                                        case EnemyZakoState.TYPEBEETLE.DUNGBEETLE:
                                            {
                                                Beetle.AddComponent<DungBeetleBuff_SpeedUp>();
                                                Beetle.AddComponent<EnemyBuff_HpUp>();
                                            }
                                            break;
                                        case EnemyZakoState.TYPEBEETLE.HOPPER:
                                        case EnemyZakoState.TYPEBEETLE.KATYDID:
                                        case EnemyZakoState.TYPEBEETLE.ANT:
                                        case EnemyZakoState.TYPEBEETLE.SPIDER:
                                            Beetle.AddComponent<BeeBuff_SpeedUp>();
                                            Beetle.AddComponent<EnemyBuff_HpUp>();
                                            break;
                                    }

                                }
                                break;
                        }
                    }
                    break;
                /*
            case "Player":
                {
                    switch (BuffType)
                    {
                        case BuffEfficacy.PlayerHarden:
                            copy.AddComponent<PlayerDeBuff_Speed>();
                            break;

                        case BuffEfficacy.PlayerSpeedDown:
                            copy.AddComponent<PlayerDeBuff_FallDown>();
                            break;
                    }
                }
                break;
                */
                default:
                    break;
            }
        }
    }

    //SEを鳴らす
    private void AudioSE()
    {
        if ((int)BuffType < 3)
            audioSource.PlayOneShot(Sound[0]);
        else if ((int)BuffType < 5)
            audioSource.PlayOneShot(Sound[1]);
    }
}

    //GetCompornentをUpdateでするのは危険なのでこの下で色々やる
    //エリアの中に入っていて配列の中に無ければ格納
    /*
    private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "Enemy":
            case "Player":
                break;
            default:
                return;
        }

        if(BuffObjects.Count != 0)
        {
            for (int i = 0; i < BuffObjects.Count; i++)
            {
                if (BuffObjects[i] == null)
                {
                    BuffObjects.RemoveAt(i);
                    i--;
                }
            }
        }

        GameObject InObject = other.gameObject;
        BuffObjects.Add(InObject);

        return;
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
            case "Player":
                break;
            default:
                return;
        }

        if(BuffObjects.Count != 0)
        {
            for (int i = 0; i < BuffObjects.Count; i++)
            {
                if (BuffObjects[i] == null)
                {
                    BuffObjects.RemoveAt(i);
                    i--;
                }
            }
        }



        BuffObjects.Remove(other.gameObject);
    }
    */    