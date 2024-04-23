using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Result : MonoBehaviour
{
    public string next = "";
    Observer observer;
    public TextMeshProUGUI timeText;
    public float fadeTime = 1.5f;
    private float time;
    public int[] clearTime = { 60, 30, 10 };
    public Sprite[] RankImage;
    public Sprite[] BackGrounds;
    public GameObject BackGround;
    public GameObject RankObj;
    public GameObject resultCanvas;
    
    [HideInInspector]public bool Gameover = false;

    public AudioClip[] BGM;
    
    private void Awake()
    {
        observer = FindFirstObjectByType<Observer>().GetComponent<Observer>();
    }

    public void ShowResult()
    {
        time += Time.deltaTime;
        // クリア時間取得
        float total = (observer.minutes * 60) + observer.seconds;
        
        // 背景画像表示
        if (total > clearTime[0]) BackGround.GetComponent<Image>().sprite = BackGrounds[0];      // S
        else if (total > clearTime[1]) BackGround.GetComponent<Image>().sprite = BackGrounds[1]; // A
        else if (total >= clearTime[2]) BackGround.GetComponent<Image>().sprite = BackGrounds[2]; // B
        
        // クリアタイムの表示関連
        timeText.text = observer.minutes.ToString("00") + " : " + 
                        observer.seconds.ToString("00") + " : " + 
                        observer.milliSeconds.ToString("00");
        if (time < fadeTime)
        {
            float alpha = 0.0f + time / fadeTime;
            Color color = timeText.color;
            color.a = alpha;
            timeText.color = color;
        }
        
        // ランク表示
        if (total > clearTime[0])      RankObj.GetComponent<Image>().sprite = RankImage[0];
        else if (total > clearTime[1]) RankObj.GetComponent<Image>().sprite = RankImage[1];
        else if (total >= clearTime[2]) RankObj.GetComponent<Image>().sprite = RankImage[2];
    }

    public void SetGameOver()
    {
        // BGM変更
        AudioSource source = GameObject.Find("Fade").GetComponent<AudioSource>();
        source.clip = BGM[1];
        source.Play();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // BGM変更
            AudioSource source = GameObject.Find("Fade").GetComponent<AudioSource>();
            source.clip = BGM[0];
            source.Play();
            
            Gameover = true;
            observer.SetTime();
            ShowResult();
            resultCanvas.SetActive(true);
        }
    }
}
