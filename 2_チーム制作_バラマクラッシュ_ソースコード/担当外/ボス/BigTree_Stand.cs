using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTree_Stand : MonoBehaviour
{

    BigTree_Instance myInstManager;

    [SerializeField]
    bool CanFall = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {  
    }

    public void FallTree()
    {
        //�|����Ƃ��̂ݓ|��
        if(!CanFall)
        {
            return;
        }
        //�؂��|�ꂽ���̏��������肢����
        myInstManager.FallTree();
    }

    public void SetBigInstManager(BigTree_Instance Insttreemanager)
    {
        myInstManager = Insttreemanager;
    }

    //���c���[���|��邱�Ƃ��o���邩�ݒ�
    public void SetCanFall(bool Can)
    {
        CanFall = Can;
    }
}
