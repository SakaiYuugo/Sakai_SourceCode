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

    //カメラの切り替え関数
    //つぎのカメラ,補間するか
    public void ChangeCamera(string Name,bool Interpolation = false)
    {
        //カメラの名前を見つける
        int Num;
        for (Num = 0; CameraList.Length > Num; Num++)
        {
            //名前が一緒だった場合
            if (Name == CameraList[Num].name)
            {
                break;
            }
        }

        //カメラがなかった場合、関数を出る
        if(Num == CameraList.Length )
        {
            Debug.Log(Name + "の名前のカメラがありません");
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

        //カメラを無効にする
        m_NowCamera.GetComponent<Camera>().enabled = false;

        //カメラの情報を更新
        NowCameraNum = Num;

        //補間するか
        if (Interpolation)
        {
            //次のカメラの生成
            m_NextCamera = Instantiate(CameraList[Num]);
            m_NextCamera.GetComponent<Camera>().enabled = false;

            //補間カメラの生成
            GameObject InterpolationObj = Instantiate(irekaeCamera);
            InterpolationObj.GetComponent<InterpolationCamera>().SetCameras(m_NowCamera, ref m_NextCamera, 60);

            //今のカメラ情報を消す
            Destroy(m_NowCamera);

            //カメラの設定
            m_NowCamera = InterpolationObj;

        }
        else
        {
            //今のカメラを消す
            Destroy(m_NowCamera);

            //つぎのカメラの生成
            m_NowCamera = Instantiate(CameraList[Num]);

            m_NextCamera = null;
        }

        //カメラを有効にする
        m_NowCamera.GetComponent<Camera>().enabled = true;
    }

    //カメラを自分のに変える
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
        //つぎのカメラが生成されていたら
        if (m_NextCamera != null)
        {
            m_NowCamera.GetComponent<Camera>().enabled = false;
            
            Destroy(m_NowCamera);

            //カメラを入れ替える
            m_NowCamera = m_NextCamera;
            System_ObjectManager.mainCamera = m_NextCamera;
            System_ObjectManager.mainCameraAudioSource = m_NextCamera.GetComponent<AudioSource>();

            //次のカメラの中は用なし
            m_NextCamera = null;

            m_NowCamera.GetComponent<Camera>().enabled = true;
        }
    }
}
