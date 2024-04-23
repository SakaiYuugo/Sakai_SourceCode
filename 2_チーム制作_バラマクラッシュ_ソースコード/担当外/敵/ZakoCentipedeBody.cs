using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoCentipedeBody : MonoBehaviour
{
    [SerializeField] GameObject target;     //�Ǐ]����I�u�W�F�N�g
    [SerializeField] float distance;        //�I�u�W�F�N�g�Ԃ̋���

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 targetToThis = this.transform.position - target.transform.position;                 //�����̈ʒu�ƃ^�[�Q�b�g�I�u�W�F�N�g�̈ʒu�̍�
        
        this.transform.position = target.transform.position + targetToThis.normalized * distance;   //���g�̈ʒu���^�[�Q�b�g�Ƃ̋���������
        transform.LookAt(target.transform, Vector3.up);                                             //�^�[�Q�b�g�̈ʒu������   

    }

}
