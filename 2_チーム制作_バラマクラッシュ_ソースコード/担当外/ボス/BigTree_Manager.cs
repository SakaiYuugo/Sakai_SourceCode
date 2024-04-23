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

    //タイヤモードが始まった時に呼ばれる
    public void StartWheelCentipede()
    {
        foreach(var copy in GenerateBigTreeObjectList)
        {
            copy.GetComponent<BigTree_Instance>().SetCanFallTree(true); //木を倒すことができるようにする
        }
    }

    //タイヤモードが終了したときに呼ぶ
    public void EndWheelCentipede()
    {
        foreach (var copy in GenerateBigTreeObjectList)
        {
            BigTree_Instance TempInst = copy.GetComponent<BigTree_Instance>();
            TempInst.SetCanFallTree(false);     //木を倒すことができないようにする
            TempInst.StartGetUpTreeCount();     //倒れている木を1分後に再生するためにカウントする
        }
    }

    //木を生成するところを設定
    public void SetGenerateBigTreeObject(GameObject ThisGameObject)
    {
        //配列に追加
        GenerateBigTreeObjectList.Add(ThisGameObject);
    }

    //木の生成(もう使わない)
    public void InstanceBigTree(int AddTree, int MaxTree)
    {
        //異常な数字判定
        if (AddTree < 0 || MaxTree < AddTree)
        {
            return;
        }

        //最大分生成する場所があるか
        if (GenerateBigTreeObjectList.Count < MaxTree)
        {
            MaxTree = GenerateBigTreeObjectList.Count;
        }

        //生成した後Maxを超えていたら
        if (AddTree + NowTreeNum > MaxTree)
        {
            //残り何個追加できるか計算
            AddTree = MaxTree - NowTreeNum;
        }

        //ランダムな場所に生成してやる
        for (int i = AddTree; i > 0; i--)
        {
            int ElementNum = GenerateBigTreeObjectList.Count;   //配列の数
            int InstNum = Random.Range(0, ElementNum);  //生成する添え字
            int Count = 0;  //生成している数  

            while (true)
            {
                BigTree_Instance Temp = GenerateBigTreeObjectList[InstNum].GetComponent<BigTree_Instance>();

                //生成出来たら
                if (Temp != null && !Temp.GetNowInstance())
                {
                    //生成処理
                    Temp.InstanceBigTree();
                    NowTreeNum++;
                    break;
                }

                //生成できない
                InstNum++;
                InstNum = InstNum % ElementNum;
                Count++;

                //念のためにすべて見終わって空いていなかったら
                if (Count > ElementNum)
                {
                    break;
                }
            }
        }
    }

}
