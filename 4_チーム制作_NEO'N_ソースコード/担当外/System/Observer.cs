using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Observer : MonoBehaviour
{
    public bool clearflg = false;

    [HideInInspector] public float minutes;
    [HideInInspector] public float seconds;
    [HideInInspector] public float milliSeconds;
    [HideInInspector] public float totaltime;
    [HideInInspector] public float TopSpeed;
    [HideInInspector] public int JustAvoid;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Application.targetFrameRate = 60;
        Reset();
    }



    public void Reset()
    {
        minutes = 0;
        seconds = 0;
        milliSeconds = 0;
        totaltime = 0;
        TopSpeed = 0;
        JustAvoid = 0;
    }

    // クリア時、残り秒数取得
    public void SetTime()
    {
        minutes = FindObjectOfType<TimeLimit>().minute;
        seconds = FindObjectOfType<TimeLimit>().second;
        milliSeconds = FindObjectOfType<TimeLimit>().milliSecond;
    }

    // ジャスト回避回数取得
    public void JustAvoidCnt()
    {
        JustAvoid++;
    }

    // ゴール時の最高スピード取得
    public void GetTopSpeed()
    {
        TopSpeed = FindObjectOfType<Player>().GetComponent<Rigidbody>().velocity.magnitude;
    }
    
}
