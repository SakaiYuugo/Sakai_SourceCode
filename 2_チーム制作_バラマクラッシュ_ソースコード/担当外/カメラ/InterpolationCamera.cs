using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpolationCamera : MonoBehaviour
{
    GameObject NextCamera;
    Vector3 vec_Dis;
    Quaternion qua_Dis;

    Vector3 BasePos;
    Quaternion BaseQua;
    int m_CountNow;
    int m_CountEnd;



	private void Awake()
	{
		System_ObjectManager.mainCamera = this.gameObject;
		System_ObjectManager.mainCameraAudioSource = this.GetComponent<AudioSource>();
	}


	void Start()
    {
        m_CountNow = 0;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(BasePos, NextCamera.transform.position, (float)m_CountNow / (float)m_CountEnd);

        //Quaternion.Lerp(Quaternion.identity, qua_Dis, (float)m_CountNow / (float)m_CountEnd);
        transform.rotation = Quaternion.Slerp(BaseQua, NextCamera.transform.rotation, (float)m_CountNow / (float)m_CountEnd);

        m_CountNow++;

        if (m_CountEnd <= m_CountNow)
        {
            //カメラの切り替え
            System_ObjectManager.mainCameraManager.GetComponent<MainCameraManager>().ChangeNextCamera();
        }
    }

    public void SetCameras(GameObject Now, ref GameObject Next, int Frame)
    {
        m_CountEnd = Frame;

        //今のカメラと情報を合わせる
        BasePos = transform.position = Now.transform.position;
        BaseQua = transform.rotation = Now.transform.rotation;

        //距離を入れる
        //vec_Dis = Next.transform.position - Now.transform.position;
        //qua_Dis.eulerAngles = Next.transform.rotation.eulerAngles - Now.transform.rotation.eulerAngles;

        //次のカメラの情報を持ってくる
        NextCamera = Next;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
