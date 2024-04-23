
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    private TextMeshProUGUI timeText;
    private bool timeStop = false;
    
    public int maxTime = 120;
    public float time;
    [HideInInspector] public int minute;
    [HideInInspector] public int second;
    [HideInInspector] public int milliSecond;


    private void Awake()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        time = maxTime;
    }

    private void FixedUpdate()
    {
        if (Player.player.playerDestroy) return;
        
        if (!FindFirstObjectByType<Result>().Gameover)  time -= Time.deltaTime;


        if (time <= 0) Player.player.playerDestroy = true;
        
        minute = (int)(time / 60f);
        second = (int)time % 60;
        milliSecond = (int)((time % 1) * 100);
        timeText.text = minute.ToString("00") + " : " + second.ToString("00") + " : " + milliSecond.ToString("00");
    }
}
