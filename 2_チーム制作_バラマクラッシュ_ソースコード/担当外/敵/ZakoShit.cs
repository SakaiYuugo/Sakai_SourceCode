using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoShit : MonoBehaviour
{
    [SerializeField] int GenerationTime = 2;
    private int Count;

    void Start()
    {
        //���Ԃ̕␳
        GenerationTime *= 60;
        Count = 0;
    }

    void FixedUpdate()
    {
        Count++;

        if (Count > GenerationTime)
        {
            Count = 0;         //���Ӗ��Ȃ����������ǋC�����̖��
            Destroy(this.gameObject);
        }
    }

}
