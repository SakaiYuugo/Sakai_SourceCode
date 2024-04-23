using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Effect : MonoBehaviour
{
    Vector3 StartScale;
    Vector3 EndScale;
    float ChangeCountTime;     //バフを掛ける間隔
    float BuffTime;            //バフを掛けている時間
    float CountTime;　　　　　　//計測用の時間
    float BuffEffTime;         //補間計測用の時間

    private void Start()
    {
        StartScale = Vector3.zero;
        EndScale = Vector3.one;
        ChangeCountTime = 9.0f;
        BuffTime = 1.0f;
        CountTime = 0.0f;
        BuffEffTime = 0.0f;
    }

    private void FixedUpdate()
    {
        CountTime += Time.deltaTime;

        //7秒間消す
        if (CountTime < ChangeCountTime)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            BuffEffTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(StartScale, EndScale, BuffEffTime / BuffTime);

            if(BuffEffTime > BuffTime)
            {
                CountTime = 0.0f;
                BuffEffTime = 0.0f;
            }
        }
    }

    /*
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }
    */
    /*
    float ChangeScaleTime = 3.0f;
    Vector3 EndScale;
    float CountTime;
    // Start is called before the first frame update
    void Start()
    {
        ChangeScaleTime = 1.0f;
        EndScale = Vector3.one;
        CountTime = ChangeScaleTime;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(CountTime < ChangeScaleTime)
        {
            CountTime += Time.deltaTime;
            if(CountTime > ChangeScaleTime)
            {
                CountTime = ChangeScaleTime;
                transform.gameObject.SetActive(false);
            }
        }

        transform.localScale = Vector3.Lerp(Vector3.zero, EndScale, CountTime / ChangeScaleTime);
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        CountTime = 0.0f;
    }
    */
}
