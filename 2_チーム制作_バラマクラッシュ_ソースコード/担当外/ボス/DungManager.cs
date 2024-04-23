using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����������DungBeetle��DungBall�n���̃N���X�C���N���[�h�Ɩ��O�𓝈ꉻ���邽�߂̃N���X

public class DungManager : MonoBehaviour
{   
    //��
    public DungBeetleState _DBState;
    public DungBeetleMove _DBMove;
    public Rigidbody _DBRigidbody;
    public DungBeetleDefeat _DBDefeat;

    //��
    public DungBall _Db;
    public DungBallRollStar _DbRollstar;
    public DungBallUpImpact _DbUpImpact;
    public Rigidbody _DbRigidbody;

    //��
    public DungSound _DungSound;

    //�A�j��
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

