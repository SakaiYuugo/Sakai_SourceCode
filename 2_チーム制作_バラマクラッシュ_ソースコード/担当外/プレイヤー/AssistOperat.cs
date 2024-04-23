using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistOperat : MonoBehaviour
{
    PlayerRay _CenterRay;
    PlayerRay _RightRay;
    PlayerRay _LeftRay;
    PlayerRay _CenterRay2;
    PlayerRay _RightRay2;
    PlayerRay _LeftRay2;
    [SerializeField,Tooltip("どのくらい遠くから障害物を発見するか")] float fRayRange;    //飛ばすレイの長さ

    void Start()
    {
        _CenterRay = GameObject.Find("RayCenter").GetComponent<PlayerRay>();
        _RightRay = GameObject.Find("RayRight").GetComponent<PlayerRay>();
        _LeftRay = GameObject.Find("RayLeft").GetComponent<PlayerRay>();
        _CenterRay2 = GameObject.Find("RayCenter2").GetComponent<PlayerRay>();
        _RightRay2 = GameObject.Find("RayRight2").GetComponent<PlayerRay>();
        _LeftRay2 = GameObject.Find("RayLeft2").GetComponent<PlayerRay>();
    }

    public PlayerMove.ASSIST AssistCheck()
    {
        if (!InputOrder.W_Key()) return PlayerMove.ASSIST.m_STAND;    //前進していないなら処理を中断する
        if (InputOrder.A_Key() || InputOrder.D_Key()) return PlayerMove.ASSIST.m_STAND; //左右に移動しているなら処理を中断する
        //つまり、前進しているが左右に移動していなくて、オブジェクトに当たりそうになった時に働く

        float UseCenterRange; //下か上の使うレイを代入する
        //まず下側のセンターレイをチェック
        UseCenterRange = _CenterRay.GetDistance(-transform.forward, fRayRange, "Ruin", "StageObject", "Wall", "Cave");   //障害物チェック

        if (UseCenterRange <= -100) //下側のレイが当たらないなら上側も試してみる
        {
            //無いなら上側のセンターレイをチェック
            UseCenterRange = _CenterRay2.GetDistance(-transform.forward, fRayRange, "Ruin", "StageObject", "Wall", "Cave");   //障害物チェック
        }


        float RightRange;
        float LeftRange;
        //①下の左右からレイを飛ばす
        RightRange = _RightRay.GetDistance(-transform.forward, fRayRange, "Ruin", "StageObject", "Wall", "Cave");
        LeftRange = _LeftRay.GetDistance(-transform.forward, fRayRange, "Ruin", "StageObject", "Wall", "Cave");

        //②もし下側の左右レイが当たらないなら上側も試してみる
        if (RightRange <= -100)
        {
            RightRange = _RightRay2.GetDistance(-transform.forward, fRayRange, "Ruin", "StageObject", "Wall", "Cave");   //障害物チェック
        }
        if (LeftRange <= -100)
        {
            LeftRange = _LeftRay2.GetDistance(-transform.forward, fRayRange, "Ruin", "StageObject", "Wall", "Cave");   //障害物チェック
        }

        //③レイの結果に応じて処理
        if (UseCenterRange >= -100) //真ん中のレイが当たったか
        {
            //左右のレイの長さが同じ、もしくはどちらも当たっていないなら処理を中断（当たらなければ-999が戻る）
            if (RightRange == LeftRange)
            {
                Debug.Log("ー９９９");
                return PlayerMove.ASSIST.m_STAND;
            }

            //③-①どちらかのレイが当たっていない場合
            if (RightRange < 0)   //右レイが当たっていないなら
            {
                if (UseCenterRange > LeftRange) //中央レイの方が長いなら右に行く
                {
                    return PlayerMove.ASSIST.m_RIGHT;
                }
                if (UseCenterRange < LeftRange) //左レイの方が長いなら左に行く
                {
                    return PlayerMove.ASSIST.m_LEFT;
                }
            }
            if (RightRange > 0)   //左レイが当たっていないなら
            {
                if (UseCenterRange > RightRange)    //中央レイの方が長いなら左に行く
                {
                    return PlayerMove.ASSIST.m_LEFT;
                }
                if (UseCenterRange < LeftRange) //左レイの方が長いなら右に行く
                {
                    return PlayerMove.ASSIST.m_RIGHT;
                }
            }
            //④両方の左右レイが当たっている場合
            if (RightRange > LeftRange) //右レイの方が長い(遠い)なら右に行く
            {
                return PlayerMove.ASSIST.m_RIGHT;
            }
            if (RightRange < LeftRange) //左レイの方が長い(遠い)なら左に行く
            {
                return PlayerMove.ASSIST.m_LEFT;
            }
        }
        else
        {
            //真ん中のレイが当たっていなくて
        }
        {
            //④両方の左右レイが当たっている場合
            if (RightRange > LeftRange) //右レイの方が長い(遠い)なら右に行く
            {
                return PlayerMove.ASSIST.m_LEFT;
            }
            if (RightRange < LeftRange) //左レイの方が長い(遠い)なら左に行く
            {
                return PlayerMove.ASSIST.m_RIGHT;
            }
        }

        return PlayerMove.ASSIST.m_STAND;
    }   
}
