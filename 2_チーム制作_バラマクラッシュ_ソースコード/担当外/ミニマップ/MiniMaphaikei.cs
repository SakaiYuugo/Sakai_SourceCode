using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaphaikei : MonoBehaviour
{
    [SerializeField] private GameObject TargetCamera;//追従対象プレイヤー
    Vector3 Minihaikei;//ミニマップカメラの座標
    Vector3 eulerAngles; // ローカル変数に格納
    // Start is called before the first frame update
    void Start()
    {
        Minihaikei = this.transform.position;
        Minihaikei.y = -10.0f;//とりあえず↓に配置する
        eulerAngles = this.transform.eulerAngles; // ローカル変数に格納した値を上書き
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Minihaikei.x = TargetCamera.transform.position.x;
        Minihaikei.z = TargetCamera.transform.position.z;
        this.transform.position = Minihaikei;
        eulerAngles = TargetCamera.transform.eulerAngles; // ローカル変数に格納した値を上書き
        eulerAngles.x = 0.0f;
        this.transform.eulerAngles = eulerAngles;
    }
}
