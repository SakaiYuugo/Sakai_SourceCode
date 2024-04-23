using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoCentipedeBody : MonoBehaviour
{
    [SerializeField] GameObject target;     //追従するオブジェクト
    [SerializeField] float distance;        //オブジェクト間の距離

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 targetToThis = this.transform.position - target.transform.position;                 //自分の位置とターゲットオブジェクトの位置の差
        
        this.transform.position = target.transform.position + targetToThis.normalized * distance;   //自身の位置をターゲットとの距離分離す
        transform.LookAt(target.transform, Vector3.up);                                             //ターゲットの位置を向く   

    }

}
