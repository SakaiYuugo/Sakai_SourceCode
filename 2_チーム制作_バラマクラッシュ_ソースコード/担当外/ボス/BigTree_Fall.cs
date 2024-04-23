using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTree_Fall : MonoBehaviour
{
    public enum TREETYPE
    {
        TREE = 0,
        STEM,
        ROOT,
        MAX
    }

    [SerializeField]
    TREETYPE TreeType = TREETYPE.TREE;
    
    BigTree_Instance myInstManager;

    private void Awake()
    {
        myInstManager = null;
    }

    //ì|ÇÍÇÈÇ∆Ç´ÇÃèàóù
    public void HitBomb()
    {
        switch(TreeType)
        {
            case TREETYPE.TREE:
                GetComponent<BigTree_Stand>().FallTree();
                break;
        }
    }

    public void SetBigInstManager(BigTree_Instance Insttreemanager)
    {
        myInstManager = Insttreemanager;
    }

    public TREETYPE GetNowTreeType()
    {
        return TreeType;
    }
}
