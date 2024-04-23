using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    //���C���΂��ĖړI�̂��̂܂ł̒�����Ԃ�
    //���� ���C�̌����A���C�̒����A�ړI�̃^�O�A���C���΂��ʒu�̕␳�l
    public float GetDistance(Vector3 direction, float range, string tag, string tag2 = "NULL", string tag3 = "NULL", string tag4 = "NULL")
    {
        //���������ړI�̃^�O���̓��A��ԋ߂����̂�߂�l�ɂ���
        List<float> Distance = new List<float>();
        bool bOneHit = false;   //���������ړI���Ɉ��ł�����������

        Ray ray = new Ray(transform.position, direction);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, range))
        {
            //�����������̂��ړI�̃^�O��������
            if (hit.collider.gameObject.tag == tag2
             || hit.collider.gameObject.tag == tag
             || hit.collider.gameObject.tag == tag3
             || hit.collider.gameObject.tag == tag4)
            {
                Distance.Add(hit.distance); //���������ړI���̋������i�[
                bOneHit = true;
            }
        }

        //List���̂����A�ŏ��l��߂�l�ɂ���(Min�͎g��Ȃ�)
        if(bOneHit)//��x�ł��ړI���ɓ������Ă����Ȃ�
        {
            float min = Distance[0];
            for(int i= 0; i < Distance.Count; i++)
            {
                min = Distance[i] >= min ? min : Distance[i];
            }
            return min; //�ŏ��l��Ԃ�
        }

        return -999.9f;  //���C���ړI�̕��ɓ͂��Ȃ���(�₩���яo������Ƃ�)
    }

    //�����������̂��ړI�̃^�O�Ȃ�true��Ԃ�
    public bool GetRay(float range,string tag, string tag2 = "NULL", string tag3 = "NULL" , string tag4 = "NULL")
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, range))
        {
            //�����������̂��ړI�̃^�O��������
            if (hit.collider.gameObject.tag == tag
             || hit.collider.gameObject.tag == tag2
             || hit.collider.gameObject.tag == tag3
             || hit.collider.gameObject.tag == tag4)
            {
                return true;
            }
        }

        return false;
    }

}
