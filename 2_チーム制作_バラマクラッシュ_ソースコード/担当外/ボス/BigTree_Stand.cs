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
        //“|‚¹‚é‚Æ‚«‚Ì‚İ“|‚·
        if(!CanFall)
        {
            return;
        }
        //–Ø‚ª“|‚ê‚½‚Ìˆ—‚ğ‚¨Šè‚¢‚·‚é
        myInstManager.FallTree();
    }

    public void SetBigInstManager(BigTree_Instance Insttreemanager)
    {
        myInstManager = Insttreemanager;
    }

    //¡ƒcƒŠ[‚ª“|‚ê‚é‚±‚Æ‚ªo—ˆ‚é‚©İ’è
    public void SetCanFall(bool Can)
    {
        CanFall = Can;
    }
}
