using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RendererOnOff : MonoBehaviour
{
    [SerializeField, Tooltip("�_�ŊԊu")] float fCycle = 1.0f;
    [SerializeField, Range(0.0f, 1.0f)] float fDutyRate = 0.5f;
    double dTime;
    
    List<SkinnedMeshRenderer> _Target = new List<SkinnedMeshRenderer>();
    GameObject parent; // �e�I�u�W�F�N�g


    void Start()
    {
        //�e�I�u�W�F�N�g������
        parent = GameObject.Find("bike_anim1");
        
        foreach (Transform child in parent.transform)
        {
            Debug.Log(child.name);  
            if(child.GetComponent<SkinnedMeshRenderer>())
            {
               _Target.Add(child.GetComponent<SkinnedMeshRenderer>());
                Debug.Log("���o");
            }
        }
        enabled = false;
    }

    void FixedUpdate() 
    {
        dTime += Time.deltaTime;    //�������Ԃ��o�߂�����

        float temp = Mathf.Repeat((float)dTime, fCycle);


        foreach (SkinnedMeshRenderer renderer in _Target)
        {
            renderer.enabled = temp >= fCycle * (1 - fDutyRate);
        }
    }
}
