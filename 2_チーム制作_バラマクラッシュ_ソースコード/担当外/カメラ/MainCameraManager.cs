using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MainCameraManager : MonoBehaviour
{
    [SerializeField]
    public GameObject irekaeCamera;

    [SerializeField]
    public GameObject[] CameraList;

    GameObject m_NowCamera;
    GameObject m_NextCamera;
	
    int NowCameraNum;



	private void Awake()
	{
		System_ObjectManager.mainCameraManager = this;
	}


	void Start()
    {
        m_NowCamera = GameObject.Find("Main Camera");
        m_NextCamera = null;
		NowCameraNum = -1;
    }
	
	
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            ChangeCamera("Player_CasualCamera", true);
        }

        if(Input.GetKeyDown(KeyCode.F2))
        {
            ChangeCamera("Player_FormalCamera", true);
        }
    }

    //�J�����̐؂�ւ��֐�
    //���̃J����,��Ԃ��邩
    public void ChangeCamera(string Name,bool Interpolation = false)
    {
        //�J�����̖��O��������
        int Num;
        for (Num = 0; CameraList.Length > Num; Num++)
        {
            //���O���ꏏ�������ꍇ
            if (Name == CameraList[Num].name)
            {
                break;
            }
        }

        //�J�������Ȃ������ꍇ�A�֐����o��
        if(Num == CameraList.Length )
        {
            Debug.Log(Name + "�̖��O�̃J����������܂���");
            return;
        }

        if(NowCameraNum == Num)
        {
            return;
        }

        if(m_NextCamera != null)
        {
            Destroy(m_NextCamera);
            m_NextCamera = null;
        }

        //�J�����𖳌��ɂ���
        m_NowCamera.GetComponent<Camera>().enabled = false;

        //�J�����̏����X�V
        NowCameraNum = Num;

        //��Ԃ��邩
        if (Interpolation)
        {
            //���̃J�����̐���
            m_NextCamera = Instantiate(CameraList[Num]);
            m_NextCamera.GetComponent<Camera>().enabled = false;

            //��ԃJ�����̐���
            GameObject InterpolationObj = Instantiate(irekaeCamera);
            InterpolationObj.GetComponent<InterpolationCamera>().SetCameras(m_NowCamera, ref m_NextCamera, 60);

            //���̃J������������
            Destroy(m_NowCamera);

            //�J�����̐ݒ�
            m_NowCamera = InterpolationObj;

        }
        else
        {
            //���̃J����������
            Destroy(m_NowCamera);

            //���̃J�����̐���
            m_NowCamera = Instantiate(CameraList[Num]);

            m_NextCamera = null;
        }

        //�J������L���ɂ���
        m_NowCamera.GetComponent<Camera>().enabled = true;
    }

    //�J�����������̂ɕς���
    public void ChangeNowType()
    {
        m_NowCamera.GetComponent<Camera>().enabled = false;

        Destroy(m_NowCamera);

        m_NowCamera = Instantiate(CameraList[NowCameraNum]);

        m_NextCamera = null;

        m_NowCamera.GetComponent<Camera>().enabled = true;
    }

    public void ChangeNextCamera()
    {
        //���̃J��������������Ă�����
        if (m_NextCamera != null)
        {
            m_NowCamera.GetComponent<Camera>().enabled = false;
            
            Destroy(m_NowCamera);

            //�J���������ւ���
            m_NowCamera = m_NextCamera;
            System_ObjectManager.mainCamera = m_NextCamera;
            System_ObjectManager.mainCameraAudioSource = m_NextCamera.GetComponent<AudioSource>();

            //���̃J�����̒��͗p�Ȃ�
            m_NextCamera = null;

            m_NowCamera.GetComponent<Camera>().enabled = true;
        }
    }
}
