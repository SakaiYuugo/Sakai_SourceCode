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
		//���������ꍇ
		if (gameObject.name == "Head")
		{
			//�}�l�[�W���[�ɒǉ�
			System_ObjectManager.bossObject = this.gameObject;
		}

		//���������𐧌䂷��BodyManager��T��
		CentipedeBodyManager =
        GameObject.Find("BodyManager").GetComponent<BossCentipede_BodyManager>();

        //BodyManager�Ɏ���������o�^����
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


        //�p�[�c�ɕt�������R���|�[�l���g��t����
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
            //�����̈ʒu������
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
            //�����̈ʒu������
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

    //�����グ��Ƃ��ɌĂԊ֐�
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

    //�̂�^�������ɂ��Ă��
    public void SetStraight()
    {
        AddLookComponent.SetStraightPos();
    }

    //���̑̂��O���牽�Ԗڂ�
    public int GetPartsNum()
    {
        return PartsNum;
    }

    //����Ԃ�
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
        //false�Ȃ�
        if (!UsuallyBody.activeInHierarchy)
        {
            UsuallyBody.SetActive(true);  //���ʂ̂��        
        }
 
        //true�Ȃ�
        if (BreakBody.activeInHierarchy)
        {
            BreakBody.SetActive(false);   //��ꂽ���
        }

        //�A�j���[�V��������
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
        //false�Ȃ�
        if (UsuallyBody.activeInHierarchy)
        {
            UsuallyBody.SetActive(false);  //���ʂ̂��        
        }

        //true�Ȃ�
        if (!BreakBody.activeInHierarchy)
        {
            BreakBody.SetActive(true);   //��ꂽ���
        }

        //�A�j���[�V��������
        //PartsAnimetion = BreakBody.GetComponent<Animator>();
        //PartsAnimetion.SetFloat("delay", StartAnimeValue);

        return BreakBody;
    }

    public void ChangePos(Vector3 NextPos)
    {
        if (this.gameObject.name == "Head")
        {
            //�����̈ʒu������
            transform.position = NextPos;
        }
        else
        {
            //���̈ʒu�Ƃ̍����v�Z���ē��̈ʒu�ɑ����Ă��
            Vector3 DifPos = Head.transform.position - transform.position;
            transform.position = NextPos + DifPos;
        }
    }
}
