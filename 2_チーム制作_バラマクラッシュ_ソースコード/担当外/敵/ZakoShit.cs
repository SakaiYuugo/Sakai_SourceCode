using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoShit : MonoBehaviour
{
    [SerializeField] int GenerationTime = 2;
    private int Count;

    void Start()
    {
        //時間の補正
        GenerationTime *= 60;
        Count = 0;
    }

    void FixedUpdate()
    {
        Count++;

        if (Count > GenerationTime)
        {
            Count = 0;         //やる意味ないかもだけど気持ちの問題
            Destroy(this.gameObject);
        }
    }

}
