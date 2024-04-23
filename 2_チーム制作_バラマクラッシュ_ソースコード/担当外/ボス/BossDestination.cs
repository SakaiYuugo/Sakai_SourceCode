using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDestination : MonoBehaviour
{
    //�����ʒu
    private Vector3 startPosition;
    //�ړI�n
    [SerializeField] private Vector3 destination;
    // �ڕW�l�̔z��
    [SerializeField] private Transform[] targets;

    [SerializeField] private int order = 0;

    // Start is called before the first frame update
    void Start()
    {
        //�@�����ʒu��ݒ�
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
    //�@�ړI�n�̐ݒ�
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    //�@�ړI�n�̎擾
    public Vector3 GetDestination()
    {
        return destination;
    }
}
