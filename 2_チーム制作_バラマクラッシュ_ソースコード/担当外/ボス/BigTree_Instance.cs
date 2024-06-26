using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTree_Instance : MonoBehaviour
{
    enum State
    {
        NONE,       //何もない
        NORMAL,     //普通に立っている
        FALL,        //倒れる
        DESTROY     //壊れた
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

        //木の生成をしてやる
        InstanceBigTree();
    }

    //大樹を追加する
    public void InstanceBigTree()
    {
        if(NowInstance)//生成している時には入らないようにする
        {
            return;
        }

        //木の生成をする
        InstTree = Instantiate(PrefabTree,transform.position,transform.rotation);
        InstTree.transform.parent = this.gameObject.transform;
        InstTree.GetComponent<BigTree_Fall>().SetBigInstManager(this);
        InstTree.GetComponent<BigTree_Stand>().SetBigInstManager(this);
        NowInstance = true;
        NowState = State.NORMAL;
    }

    //木を倒すときの処理
    public void FallTree()
    {
        //生成している場合のみ入る
        if(!NowInstance)
        {
            return;
        }

        if(NowState != State.NORMAL)
        {
            return;
        }

        //木の幹を生成
        InstStem = Instantiate(PrefabStem, transform.position, transform.rotation);
        InstStem.transform.parent = this.gameObject.transform;
        InstStem.GetComponent<BigTree_Stem>().SetBigInstManager(this);
        //木の根を生成
        InstRoot = Instantiate(PrefabRoot, transform.position, transform.rotation);
        InstRoot.transform.parent = this.gameObject.transform;

        //今の木を削除
        Destroy(InstTree);
        InstTree = null;

        NowState = State.FALL;
    }

    //木を再生するときの処理
    public void ReproductionTree()
    {
        if(!NowInstance)
        {
            return;
        }
		

        //今の状態に合わせて処理をする
        switch (NowState)
        {
            case State.FALL:    //倒れていた場合
                InstTree = Instantiate(PrefabTree, transform.position, transform.rotation);
                InstTree.GetComponent<BigTree_Stand>().SetBigInstManager(this);
                InstTree.GetComponent<BigTree_Fall>().SetBigInstManager(this);

                //根を消してやる
                Destroy(InstRoot);

                NowState = State.NORMAL;
                break;

            case State.DESTROY:
                //木の生成をする
                InstTree = Instantiate(PrefabTree, transform.position, transform.rotation);
                InstTree.GetComponent<BigTree_Fall>().SetBigInstManager(this);
                InstTree.GetComponent<BigTree_Stand>().SetBigInstManager(this);

                //幹と根を消してやる
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
        //木が倒れている場合のみ
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

    //生成しているかどうか返す
    public bool GetNowInstance()
    {
        return NowInstance;
    }

    public BigTree_Manager GetBigTreeManager()
    {
        return bigTreeManager;
    }
}
