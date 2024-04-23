using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameManager : MonoBehaviour
{
    public int FPS = 60;
    public bool Default_Draw = true;
    public bool Fixed_Draw = true;
    float DefaultCount;    //秒数のカウント
    float FixedCount;    //秒数のカウント
    int Default_FrameCount; //フレームのカウント
    int Fixed_FrameCount; //フレームのカウント

    // Start is called before the first frame update
    void Start()
    {
        DefaultCount = 0.0f;
        FixedCount = 0.0f;
        Default_FrameCount = 0; 
        Fixed_FrameCount = 0;
        Application.targetFrameRate = FPS;
    }

    // Update is called once per frame
    void Update()
    {
        if(Default_Draw)
        {
            DefaultCount += Time.deltaTime;
            Default_FrameCount++;

            if (DefaultCount > 1.0f)
            {
                Debug.Log("FPS Update:" + Default_FrameCount);
                DefaultCount = 0.0f;
                Default_FrameCount = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Fixed_Draw)
        {
            FixedCount += Time.deltaTime;
            Fixed_FrameCount++;

            if (FixedCount > 1.0f)
            {
                Debug.Log("FPS FixedUpdate:" + Fixed_FrameCount);
                FixedCount = 0.0f;
                Fixed_FrameCount = 0;
            }
        }
    }
}
