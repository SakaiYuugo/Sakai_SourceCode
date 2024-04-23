using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{

    [SerializeField] private GameObject TargetPlayer;//追従対象プレイヤー
    Vector3 MiniCamera;//ミニマップカメラの座標
    Vector3 eulerAngles; // ローカル変数に格納
    public GameObject PlayerCamObj;//メインカメラオブジェクトとりたいんじゃ

    // Use this for initialization
    void Start()
    {
        MiniCamera = this.transform.position;
        MiniCamera.y = 300.0f;//とりあえず上に配置する
        eulerAngles = this.transform.eulerAngles; // ローカル変数に格納した値を上書き
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerCamObj = GameObject.FindWithTag("MainCamera");
        MiniCamera.x = TargetPlayer.transform.position.x;
        MiniCamera.z = TargetPlayer.transform.position.z;
        this.transform.position = MiniCamera;
        eulerAngles = PlayerCamObj.transform.eulerAngles; // ローカル変数に格納した値を上書き
        eulerAngles.x = 90.0f;
        this.transform.eulerAngles = eulerAngles;
    }

    public void SetPlayerCam(GameObject PlayerCam)
    {
        PlayerCamObj = PlayerCam;
    }
}
