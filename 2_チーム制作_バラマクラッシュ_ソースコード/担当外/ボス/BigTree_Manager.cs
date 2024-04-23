using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTree_Manager : MonoBehaviour
{
    [SerializeField]
    GameObject TreePrefab;
    [SerializeField]
    GameObject StemPrefab;
    [SerializeField]
    GameObject RootPrefab;
    [SerializeField]
    int InstanceNum = 10;


    [SerializeField]
    List<GameObject> GenerateBigTreeObjectList;
    bool GenerateInit = false;
    int NowTreeNum;

    private void Awake()
    {
        GenerateBigTreeObjectList = new List<GameObject>();
        GenerateInit = true;
        NowTreeNum = 0;
    }

    public GameObject GetTreePrefab()
    {
        return TreePrefab;
    }

    public GameObject GetStemPrefab()
    {
        return StemPrefab;
    }

    public GameObject GetRootPrefab()
    {
        return RootPrefab;
    }

    //�^�C�����[�h���n�܂������ɌĂ΂��
    public void StartWheelCentipede()
    {
        foreach(var copy in GenerateBigTreeObjectList)
        {
            copy.GetComponent<BigTree_Instance>().SetCanFallTree(true); //�؂�|�����Ƃ��ł���悤�ɂ���
        }
    }

    //�^�C�����[�h���I�������Ƃ��ɌĂ�
    public void EndWheelCentipede()
    {
        foreach (var copy in GenerateBigTreeObjectList)
        {
            BigTree_Instance TempInst = copy.GetComponent<BigTree_Instance>();
            TempInst.SetCanFallTree(false);     //�؂�|�����Ƃ��ł��Ȃ��悤�ɂ���
            TempInst.StartGetUpTreeCount();     //�|��Ă���؂�1����ɍĐ����邽�߂ɃJ�E���g����
        }
    }

    //�؂𐶐�����Ƃ����ݒ�
    public void SetGenerateBigTreeObject(GameObject ThisGameObject)
    {
        //�z��ɒǉ�
        GenerateBigTreeObjectList.Add(ThisGameObject);
    }

    //�؂̐���(�����g��Ȃ�)
    public void InstanceBigTree(int AddTree, int MaxTree)
    {
        //�ُ�Ȑ�������
        if (AddTree < 0 || MaxTree < AddTree)
        {
            return;
        }

        //�ő啪��������ꏊ�����邩
        if (GenerateBigTreeObjectList.Count < MaxTree)
        {
            MaxTree = GenerateBigTreeObjectList.Count;
        }

        //����������Max�𒴂��Ă�����
        if (AddTree + NowTreeNum > MaxTree)
        {
            //�c�艽�ǉ��ł��邩�v�Z
            AddTree = MaxTree - NowTreeNum;
        }

        //�����_���ȏꏊ�ɐ������Ă��
        for (int i = AddTree; i > 0; i--)
        {
            int ElementNum = GenerateBigTreeObjectList.Count;   //�z��̐�
            int InstNum = Random.Range(0, ElementNum);  //��������Y����
            int Count = 0;  //�������Ă��鐔  

            while (true)
            {
                BigTree_Instance Temp = GenerateBigTreeObjectList[InstNum].GetComponent<BigTree_Instance>();

                //�����o������
                if (Temp != null && !Temp.GetNowInstance())
                {
                    //��������
                    Temp.InstanceBigTree();
                    NowTreeNum++;
                    break;
                }

                //�����ł��Ȃ�
                InstNum++;
                InstNum = InstNum % ElementNum;
                Count++;

                //�O�̂��߂ɂ��ׂČ��I����ċ󂢂Ă��Ȃ�������
                if (Count > ElementNum)
                {
                    break;
                }
            }
        }
    }

}
