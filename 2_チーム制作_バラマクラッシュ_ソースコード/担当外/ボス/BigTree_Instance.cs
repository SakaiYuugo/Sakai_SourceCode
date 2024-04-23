using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTree_Instance : MonoBehaviour
{
    enum State
    {
        NONE,       //�����Ȃ�
        NORMAL,     //���ʂɗ����Ă���
        FALL,        //�|���
        DESTROY     //��ꂽ
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

        //�؂̐��������Ă��
        InstanceBigTree();
    }

    //�����ǉ�����
    public void InstanceBigTree()
    {
        if(NowInstance)//�������Ă��鎞�ɂ͓���Ȃ��悤�ɂ���
        {
            return;
        }

        //�؂̐���������
        InstTree = Instantiate(PrefabTree,transform.position,transform.rotation);
        InstTree.transform.parent = this.gameObject.transform;
        InstTree.GetComponent<BigTree_Fall>().SetBigInstManager(this);
        InstTree.GetComponent<BigTree_Stand>().SetBigInstManager(this);
        NowInstance = true;
        NowState = State.NORMAL;
    }

    //�؂�|���Ƃ��̏���
    public void FallTree()
    {
        //�������Ă���ꍇ�̂ݓ���
        if(!NowInstance)
        {
            return;
        }

        if(NowState != State.NORMAL)
        {
            return;
        }

        //�؂̊��𐶐�
        InstStem = Instantiate(PrefabStem, transform.position, transform.rotation);
        InstStem.transform.parent = this.gameObject.transform;
        InstStem.GetComponent<BigTree_Stem>().SetBigInstManager(this);
        //�؂̍��𐶐�
        InstRoot = Instantiate(PrefabRoot, transform.position, transform.rotation);
        InstRoot.transform.parent = this.gameObject.transform;

        //���̖؂��폜
        Destroy(InstTree);
        InstTree = null;

        NowState = State.FALL;
    }

    //�؂��Đ�����Ƃ��̏���
    public void ReproductionTree()
    {
        if(!NowInstance)
        {
            return;
        }
		

        //���̏�Ԃɍ��킹�ď���������
        switch (NowState)
        {
            case State.FALL:    //�|��Ă����ꍇ
                InstTree = Instantiate(PrefabTree, transform.position, transform.rotation);
                InstTree.GetComponent<BigTree_Stand>().SetBigInstManager(this);
                InstTree.GetComponent<BigTree_Fall>().SetBigInstManager(this);

                //���������Ă��
                Destroy(InstRoot);

                NowState = State.NORMAL;
                break;

            case State.DESTROY:
                //�؂̐���������
                InstTree = Instantiate(PrefabTree, transform.position, transform.rotation);
                InstTree.GetComponent<BigTree_Fall>().SetBigInstManager(this);
                InstTree.GetComponent<BigTree_Stand>().SetBigInstManager(this);

                //���ƍ��������Ă��
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
        //�؂��|��Ă���ꍇ�̂�
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

    //�������Ă��邩�ǂ����Ԃ�
    public bool GetNowInstance()
    {
        return NowInstance;
    }

    public BigTree_Manager GetBigTreeManager()
    {
        return bigTreeManager;
    }
}
