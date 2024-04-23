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

    //�؂����Ȃǂ̏���
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
        //���������ꍇ
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
            //�^�C�����[�h�������ꍇ
            if (TempState == BODYSTATE.WHEEL)
            {
                //�ʒu��ݒ肵������悤�ɂ���
                BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
                partsManager.LoadTentavitePos();
                if (warp) partsManager.LoadWarp();        //�̂��Ȃ���
                System_ObjectManager.bossObject = HeadObject;       //�������̃I�u�W�F�N�g�ɕς��Ă��
                partsManager.SetComponentEneble(true);              //�ǐՂȂǂ̂��ׂẴR���|�[�l���g�𓮂���
                StartRigidBody(copy);
                bigTreeManager.EndWheelCentipede();         //�؂̍Đ������Ȃ�

            }

            if (TempState == BODYSTATE.HEADUP)
            {
                //�ʒu��ݒ肵������悤�ɂ���
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
            //�^�C�����[�h�������ꍇ
            if (TempState == BODYSTATE.WHEEL)
            {
                //�ʒu��ݒ肵������悤�ɂ���
                BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
                partsManager.LoadTentavitePos(Pos);
                if (warp) partsManager.LoadWarp();        //�̂��Ȃ���
                System_ObjectManager.bossObject = HeadObject;       //�������̃I�u�W�F�N�g�ɕς��Ă��
                partsManager.SetComponentEneble(true);    //�ǐՂȂǂ̂��ׂẴR���|�[�l���g�𓮂���
                StartRigidBody(copy);
                bigTreeManager.EndWheelCentipede();         //�؂̍Đ������Ȃ�
            }

            if(TempState == BODYSTATE.HEADUP)
            {
                //�ʒu��ݒ肵������悤�ɂ���
                BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
                partsManager.LoadTentavitePos(Pos);
                if (warp) partsManager.LoadWarp();
                partsManager.SetEndGetUP();                
            }
        }
    }

    //�����グ�郂�[�V����
    public void ChangeHeadGetUPMode()
    {
        BODYSTATE TempState = NowState;

        NowState = BODYSTATE.HEADUP;

        foreach(GameObject copy in PartsList)
        {
            if(TempState == BODYSTATE.DEFAULT)
            {
                //�ʒu��ݒ肵������悤�ɂ���
                BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
                partsManager.SetStartGetUP(5.0f);
                //�G�t�F�N�g���~�߂Ă��

            }
        }
    }

    //�X�^�����[�h
    public void ChangeStanMode(Vector3 Pos)
    {
        BODYSTATE TempState = NowState;

        NowState = BODYSTATE.STAN;

        HeadObject.transform.position = Pos;

        foreach (GameObject copy in PartsList)
        {
            //�^�C�����[�h�������ꍇ
            if (TempState == BODYSTATE.WHEEL)
            {
                BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
                partsManager.LoadTentavitePos(Pos);                            //�ꏊ���ړ�����
                partsManager.LoadWarp();                                        //�̂��Ȃ��Ă��
                System_ObjectManager.bossObject = HeadObject;
                partsManager.SetComponentEneble(true);                     //�ǐՂȂǂ̂��ׂẴR���|�[�l���g�𓮂���
                StartRigidBody(copy);
                bigTreeManager.EndWheelCentipede();         //�؂̍Đ������Ȃ�
            }
            //�X�^������Ƃ��ɌĂԊ֐�
        }
        AnimationSpeed(0.5f,1.0f);
        
    }

    //�^�C�����[�h�ϊ�
    public void ChangeWheelMode()
    {
        NowState = BODYSTATE.WHEEL;

        //�^�C�����J�f�𐶐�
        InstWheelCentipede = Instantiate(WheelCentipede, HeadObject.transform.position, HeadObject.transform.rotation);          //���̈ʒu�ɐ�������
        InstWheelCentipede.transform.parent = transform.root.gameObject.transform;                                              //�e�̐ݒ�
        System_ObjectManager.bossObject = InstWheelCentipede;

        //�؂����Ȃǂ̏���
        bigTreeManager.StartWheelCentipede();

        //������������ɍ��̑̂������Ȃ��ꏊ�ɒu���Ă���
        //�̂̃p�[�c�̐ݒ�
        foreach (GameObject copy in PartsList)
        {
            //-------------------------
            //�����Ȃ��悤�ɑ̂͒n�ʂ̉��ɒu���Ă���

            BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
            partsManager.StopJump();                //�W�����v���~�߂�
            partsManager.SaveTentativePos();        //������ʒu���Z�[�u����
            partsManager.SetComponentEneble(false); //�ǐՂȂǂ̂��ׂẴR���|�[�l���g���~�߂�
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
        rb.velocity = Vector3.zero;             //���̓������~�߂鏈��
        rb.isKinematic = false;
    }

    void StopRigidBody(GameObject temp)
    {
        Rigidbody rb = temp.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;             //���̓������~�߂鏈��
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

    //���𒆐S�Ɉړ����Ă��
    public void ChangePos(Vector3 Pos)
    {
        foreach(GameObject copy in PartsList)
        {
            //�ʒu��ݒ肵������悤�ɂ���
            BossCentipede_PartsManager partsManager = copy.GetComponent<BossCentipede_PartsManager>();
            partsManager.ChangePos(Pos);
        }
    }


    //�̂�^�������ɂ��Ă��
    public void SetStraight()
    {
        foreach (GameObject copy in PartsList)
        {
            copy.GetComponent<BossCentipede_PartsManager>().SetStraight();
        }
    }

    //�̂��Ȃ���
    public void SetWarp()
    {
        foreach(GameObject copy in PartsList)
        {
            //�ʒu��ݒ肵������悤�ɂ���
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
