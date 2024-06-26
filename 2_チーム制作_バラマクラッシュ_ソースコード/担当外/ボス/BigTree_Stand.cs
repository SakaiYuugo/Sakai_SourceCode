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
        //倒せるときのみ倒す
        if(!CanFall)
        {
            return;
        }
        //木が倒れた時の処理をお願いする
        myInstManager.FallTree();
    }

    public void SetBigInstManager(BigTree_Instance Insttreemanager)
    {
        myInstManager = Insttreemanager;
    }

    //今ツリーが倒れることが出来るか設定
    public void SetCanFall(bool Can)
    {
        CanFall = Can;
    }
}
