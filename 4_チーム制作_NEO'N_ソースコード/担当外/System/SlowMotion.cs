using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    public float slowMotionTimeScale;

    private float startTimeScale;
    private float startFixedTimeScale;

    private void Start()
    {
        startTimeScale = Time.timeScale;
        startFixedTimeScale = Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartSlowMotion();
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            StopSlowMotion();
        }
    }

    public void StartSlowMotion(float slow = 1f)
    {
        Camera.main.GetComponent<MotionBlur>().motionBlurMaterial.SetFloat("_SpeedCoeff", Mathf.Abs(Camera.main.GetComponent<MotionBlur>().blurpower));
        Time.timeScale = slowMotionTimeScale * slow;
        Time.fixedDeltaTime = startFixedTimeScale * slowMotionTimeScale;
    }

    public void StopSlowMotion()
    {
        Camera.main.GetComponent<MotionBlur>().motionBlurMaterial.SetFloat("_SpeedCoeff", Mathf.Abs(0));
        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedTimeScale;
    }
}
