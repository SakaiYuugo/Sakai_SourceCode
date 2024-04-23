using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    //レイを飛ばして目的のものまでの長さを返す
    //引数 レイの向き、レイの長さ、目的のタグ、レイを飛ばす位置の補正値
    public float GetDistance(Vector3 direction, float range, string tag, string tag2 = "NULL", string tag3 = "NULL", string tag4 = "NULL")
    {
        //当たった目的のタグ物の内、一番近いものを戻り値にする
        List<float> Distance = new List<float>();
        bool bOneHit = false;   //そもそも目的物に一回でも当たったか

        Ray ray = new Ray(transform.position, direction);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, range))
        {
            //当たったものが目的のタグだったら
            if (hit.collider.gameObject.tag == tag2
             || hit.collider.gameObject.tag == tag
             || hit.collider.gameObject.tag == tag3
             || hit.collider.gameObject.tag == tag4)
            {
                Distance.Add(hit.distance); //当たった目的物の距離を格納
                bOneHit = true;
            }
        }

        //List内のうち、最小値を戻り値にする(Minは使わない)
        if(bOneHit)//一度でも目的物に当たっていたなら
        {
            float min = Distance[0];
            for(int i= 0; i < Distance.Count; i++)
            {
                min = Distance[i] >= min ? min : Distance[i];
            }
            return min; //最小値を返す
        }

        return -999.9f;  //レイが目的の物に届かない時(坂から飛び出したりとか)
    }

    //当たったものが目的のタグならtrueを返す
    public bool GetRay(float range,string tag, string tag2 = "NULL", string tag3 = "NULL" , string tag4 = "NULL")
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, range))
        {
            //当たったものが目的のタグだったら
            if (hit.collider.gameObject.tag == tag
             || hit.collider.gameObject.tag == tag2
             || hit.collider.gameObject.tag == tag3
             || hit.collider.gameObject.tag == tag4)
            {
                return true;
            }
        }

        return false;
    }

}
