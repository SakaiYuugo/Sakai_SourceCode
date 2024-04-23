using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPrevention : MonoBehaviour
{
    //---- 地面より下側に行かないようにするスクリプト ---- 
    //※BoxColliderを持つものにアタッチする
    //※すり抜けを防止する用のスクリプトで、既に下にある場合は機能しない設計
    //レイの向きを変える場合はレイのサイズの計算式も変えなければいけない
    //目標タグを変えたりレイの方向を真下でなくさせればいろいろ応用きくかも知れない

    BoxCollider _Collider; //自分のコライダーサイズ取得用

    void Start()
    {
        _Collider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        //レイの位置、長さを計算
        float Range = _Collider.size.y + _Collider.center.y; //コライダーの頭からレイを飛ばす。下までの長さ
        float rayPosY = transform.position.y + _Collider.size.y + _Collider.center.y;
        Vector3 RayPos = new Vector3(transform.position.x, rayPosY, transform.position.z);

        //レイを飛ばし、当たったものすべてを取得
        Ray ray = new Ray(RayPos, Vector3.down);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, Range))
        {
            //当たったものが地面だったら
            if (hit.collider.gameObject.tag == "Ground")
            {
                float PosY = transform.position.y + Range - hit.distance;   //地面の上に立てるY座標を計算
                transform.position = new Vector3(transform.position.x, PosY, transform.position.z); //座標を更新
            }
        }
    }
}
