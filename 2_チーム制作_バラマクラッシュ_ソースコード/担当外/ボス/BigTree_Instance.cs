using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTree_Instance : MonoBehaviour
{
    enum State
    {
        NONE,       //‰½‚à‚È‚¢
        NORMAL,     //•’Ê‚É—§‚Á‚Ä‚¢‚é
        FALL,        //“|‚ê‚é
        DESTROY     //‰ó‚ê‚½
    }

    bool NowInstance;
    BigTree_Manager bigTreeManager;
    GameObject PrefabTree;
    GameObject PrefabStem;
    GameObject PrefabRoot;
    GameObject InstTree;
    GameObject InstStem;
    GameObject InstRoot;
    State NowState;

    private void Awake()
    {
        NowState = State.NONE;
    }
	
    void Start()
    {
        NowInstance = false;
        bigTreeManager = GameObject.Find("BigTree_Manager").GetComponent<BigTree_Manager>();
        bigTreeManager.SetGenerateBigTreeObject(this.gameObject);
        PrefabTree = bigTreeManager.GetTreePrefab();
        PrefabStem = bigTreeManager.GetStemPrefab();
        PrefabRoot = bigTreeManager.GetRootPrefab();

        //–Ø‚Ì¶¬‚ğ‚µ‚Ä‚â‚é
        InstanceBigTree();
    }

    //‘å÷‚ğ’Ç‰Á‚·‚é
    public void InstanceBigTree()
    {
        if(NowInstance)//¶¬‚µ‚Ä‚¢‚é‚É‚Í“ü‚ç‚È‚¢‚æ‚¤‚É‚·‚é
        {
            return;
        }

        //–Ø‚Ì¶¬‚ğ‚·‚é
        InstTree = Instantiate(PrefabTree,transform.position,transform.rotation);
        InstTree.transform.parent = this.gameObject.transform;
        InstTree.GetComponent<BigTree_Fall>().SetBigInstManager(this);
        InstTree.GetComponent<BigTree_Stand>().SetBigInstManager(this);
        NowInstance = true;
        NowState = State.NORMAL;
    }

    //–Ø‚ğ“|‚·‚Æ‚«‚Ìˆ—
    public void FallTree()
    {
        //¶¬‚µ‚Ä‚¢‚éê‡‚Ì‚İ“ü‚é
        if(!NowInstance)
        {
            return;
        }

        if(NowState != State.NORMAL)
        {
            return;
        }

        //–Ø‚ÌŠ²‚ğ¶¬
        InstStem = Instantiate(PrefabStem, transform.position, transform.rotation);
        InstStem.transform.parent = this.gameObject.transform;
        InstStem.GetComponent<BigTree_Stem>().SetBigInstManager(this);
        //–Ø‚Ìª‚ğ¶¬
        InstRoot = Instantiate(PrefabRoot, transform.position, transform.rotation);
        InstRoot.transform.parent = this.gameObject.transform;

        //¡‚Ì–Ø‚ğíœ
        Destroy(InstTree);
        InstTree = null;

        NowState = State.FALL;
    }

    //–Ø‚ğÄ¶‚·‚é‚Æ‚«‚Ìˆ—
    public void ReproductionTree()
    {
        if(!NowInstance)
        {
            return;
        }
		

        //¡‚Ìó‘Ô‚É‡‚í‚¹‚Äˆ—‚ğ‚·‚é
        switch (NowState)
        {
            case State.FALL:    //“|‚ê‚Ä‚¢‚½ê‡
                InstTree = Instantiate(PrefabTree, transform.position, transform.rotation);
                InstTree.GetComponent<BigTree_Stand>().SetBigInstManager(this);
                InstTree.GetComponent<BigTree_Fall>().SetBigInstManager(this);

                //ª‚ğÁ‚µ‚Ä‚â‚é
                Destroy(InstRoot);

                NowState = State.NORMAL;
                break;

            case State.DESTROY:
                //–Ø‚Ì¶¬‚ğ‚·‚é
                InstTree = Instantiate(PrefabTree, transform.position, transform.rotation);
                InstTree.GetComponent<BigTree_Fall>().SetBigInstManager(this);
                InstTree.GetComponent<BigTree_Stand>().SetBigInstManager(this);

                //Š²‚Æª‚ğÁ‚µ‚Ä‚â‚é
                Destroy(InstStem);
                Destroy(InstRoot);

                NowState = State.NORMAL;
                break;
        }

        InstStem = null;
        InstRoot = null;

    }

    public void StartGetUpTreeCount()
    {
        //–Ø‚ª“|‚ê‚Ä‚¢‚éê‡‚Ì‚İ
        if(NowState == State.FALL)
        {
            InstStem.GetComponent<BigTree_Stem>().StartGetUpTreeCount();
        }
    }

    public void FallDestroyTree()
    {
        Destroy(InstStem);
        NowState = State.DESTROY;
    }

    public void SetCanFallTree(bool Can)
    {
        if(InstTree == null)
        {
            return;
        }

        InstTree.GetComponent<BigTree_Stand>().SetCanFall(Can);
    }

    //¶¬‚µ‚Ä‚¢‚é‚©‚Ç‚¤‚©•Ô‚·
    public bool GetNowInstance()
    {
        return NowInstance;
    }

    public BigTree_Manager GetBigTreeManager()
    {
        return bigTreeManager;
    }
}
