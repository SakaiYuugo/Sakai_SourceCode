using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTree_Stem : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1.0f)]
    float FallValue = 0.0f;
    [SerializeField]
    float FallTime = 1.0f;

    float Count;
    Quaternion StartRotation;
    Quaternion EndRotation;

    BigTree_Instance myInstManager;

    bool ReturnStandTree;

    float GetUpStartCount;

    // Start is called before the first frame update
    void Start()
    {
        FallValue = 0.0f;
        Count = 0.0f;
        StartRotation = transform.rotation;
        EndRotation = Quaternion.AngleAxis(90.0f, transform.right) * transform.rotation;
        ReturnStandTree = false;

        GetUpStartCount = 0.0f;
    }


    private void FixedUpdate()
    {             
        if(GetUpStartCount > 0.0f)
        {
            GetUpStartCount -= Time.deltaTime;
            return;
        }

        transform.rotation = Quaternion.Lerp(StartRotation, EndRotation, FallValue);

        if (FallValue < 1.0f)
        {
            //補間の計算
            float x = Count / FallTime;

            FallValue =
                Mathf.Pow(x, 4);

            Count += Time.deltaTime;

            //動きが終わった時に呼ぶ
            if (Count > FallTime)
            {
                FallValue = 1.0f;
                Count = 0.0f;
                //再生する場合
                if(ReturnStandTree)
                {
                    myInstManager.ReproductionTree();
                    ReturnStandTree = false;
                }
            }
        }
    }

    public void SetBigInstManager(BigTree_Instance Insttreemanager)
    {
        myInstManager = Insttreemanager;
    }

    //起き上がりするときの処理
    public void SetGetUpTree()
    {
        //回転を逆にして起き上がるようにする
        Quaternion Temp;
        Temp = StartRotation;
        StartRotation = EndRotation;
        EndRotation = Temp;
        FallValue = 0.0f;
        ReturnStandTree = true;
    }

    public void HitWhileCentipede()
    {
        //木の生成がかぶった場合
        if(ReturnStandTree)
        {
            return;
        }

        //タイヤムカデに当たったので壊す
        myInstManager.FallDestroyTree();
    }

    //起き上がりするまでのカウントをし始める
    public void StartGetUpTreeCount()
    {
        //もうカウントしていたら更新しない
        if(GetUpStartCount > 0.0f)
        {
            return;
        }


        GetUpStartCount = 60.0f;
        SetGetUpTree();
    }
}
