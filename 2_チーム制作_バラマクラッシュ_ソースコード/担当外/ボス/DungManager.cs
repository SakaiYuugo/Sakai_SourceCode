using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//増えすぎたDungBeetleとDungBall系統のクラスインクルードと名前を統一化するためのクラス

public class DungManager : MonoBehaviour
{   
    //虫
    public DungBeetleState _DBState;
    public DungBeetleMove _DBMove;
    public Rigidbody _DBRigidbody;
    public DungBeetleDefeat _DBDefeat;

    //玉
    public DungBall _Db;
    public DungBallRollStar _DbRollstar;
    public DungBallUpImpact _DbUpImpact;
    public Rigidbody _DbRigidbody;

    //音
    public DungSound _DungSound;

    //アニメ
    public DungAnime _DungAnime;

    void Start()
    {
        GameObject Beetle = GameObject.Find("Boss_DungBeetle");
        _DBState = Beetle.GetComponent<DungBeetleState>();
        _DBMove = Beetle.GetComponent<DungBeetleMove>();
        _DBRigidbody = Beetle.GetComponent<Rigidbody>();
        _DBDefeat = Beetle.GetComponent<DungBeetleDefeat>();


        GameObject Ball = GameObject.Find("DungBall");
        _Db = Ball.GetComponent<DungBall>();
        _DbRollstar = Ball.GetComponent<DungBallRollStar>();
        _DbUpImpact = Ball.GetComponent<DungBallUpImpact>();
        _DbRigidbody = Ball.GetComponent<Rigidbody>();

        _DungSound = Beetle.GetComponent<DungSound>();

        _DungAnime = Beetle.GetComponent<DungAnime>();
    }




}

