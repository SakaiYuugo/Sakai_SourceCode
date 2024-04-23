using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDestination : MonoBehaviour
{
    //初期位置
    private Vector3 startPosition;
    //目的地
    [SerializeField] private Vector3 destination;
    // 目標値の配列
    [SerializeField] private Transform[] targets;

    [SerializeField] private int order = 0;

    // Start is called before the first frame update
    void Start()
    {
        //　初期位置を設定
        startPosition = transform.position;
        SetDestination(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateDestination()
    {
        if (order < targets.Length - 1)
        {
            order++;
            SetDestination(new Vector3(targets[order].transform.position.x, targets[order].transform.position.y, targets[order].transform.position.z));
        }
        else
        {
            order = 0;
            SetDestination(new Vector3(targets[order].transform.position.x, targets[order].transform.position.y, targets[order].transform.position.z));
        }
    }
    //　目的地の設定
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    //　目的地の取得
    public Vector3 GetDestination()
    {
        return destination;
    }
}
