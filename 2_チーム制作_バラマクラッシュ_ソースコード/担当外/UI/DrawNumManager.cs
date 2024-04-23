using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNumManager : MonoBehaviour
{
    [SerializeField] int Number;
    [SerializeField] float DrawSize = 100.0f;
    [SerializeField,Range(0,1000)] float Distance;
    [SerializeField] bool Center = false; //中央に置く
    [SerializeField] GameObject NumberPrefab;

    RectTransform m_pos;
    List<GameObject> m_InstObjList;
    List<DrawNumber> m_InstNumList;
    List<RectTransform> m_InstRectPosList;
    bool m_bChangeNum;

    // Start is called before the first frame update
    void Awake()
    {
        m_pos = GetComponent<RectTransform>();

        m_InstObjList = new List<GameObject>();
        m_InstNumList = new List<DrawNumber>();
        m_InstRectPosList = new List<RectTransform>();

        SetNumber(Number);
    }

    // Update is called once per frame
    void Update()
    {
    }

    //下の関数と違い絶対に変える
    //下の桁から順番に変えていく
    private void SetNumber(int Num)
    {
        Number = Num;
        m_bChangeNum = true;

        //配列番号0から入っていく
        int i = Number; 
        int digit = 1;

        while(true)
        {
            //最小桁の数
            int OneNum = i % 10;

            //前の数値(リストの数)よりも桁が多ければ
            if (m_InstObjList.Count < digit)
            {
                //生成した桁の設定
                GameObject TempObj = Instantiate(NumberPrefab);
                TempObj.transform.parent = this.gameObject.transform;   //子供にしてやる
                DrawNumber TempDrawNum = TempObj.GetComponent<DrawNumber>();
                TempDrawNum.SetSize(DrawSize);
                RectTransform TempRectPos = TempObj.GetComponent<RectTransform>();
                Vector3 BasePos = Center ? new Vector3((m_InstObjList.Count * Distance / 2.0f) - (Distance * 0.5f), 0.0f, 0.0f) : Vector3.zero;
                TempRectPos.localScale = Vector3.one;
                TempRectPos.localPosition = new Vector3(BasePos.x - Distance * digit - 1, BasePos.y, 0.0f);
                m_InstObjList.Add(TempObj);
                m_InstNumList.Add(TempDrawNum);
                m_InstRectPosList.Add(TempRectPos);
            }
            //数字のセット
            m_InstNumList[digit - 1].ChangeNumber(OneNum);

            //下桁を消してやる
            i /= 10;

            //数値がゼロになったらやめてやる
            if(i < 1)
            {
                break;
            }
            //桁を増やす
            digit++;
        }

        //前の数値よりも桁が少なくなっていれば
        if (m_InstObjList.Count > digit)
        {
            for(int n = m_InstNumList.Count;n > digit;n--)
            {
                //桁を消していく
                DestroyDigit(n-1);
            }
        }

        //Vector3 BasePos = Center ? new Vector3((m_InstObjList.Count * Distance / 2.0f) - (Distance * 0.5f), 0.0f, 0.0f) : Vector3.zero;

        //for (int n = 0; n < m_InstObjList.Count; n++)
        //{
        //    m_InstRectPosList[n].anchoredPosition = new Vector3(BasePos.x - Distance * n, BasePos.y, 0.0f);
        //}

    }

    //桁が変わっていなければやらない
    public void ChangeNum(int Num)
    {
        //数字が変わっていなければ
        if (Number == Num)
        {
            return;
        }

        SetNumber(Num);
    }

        //桁を消す
    private void DestroyDigit(int No)
    {
        m_InstNumList.RemoveAt(No);
        m_InstRectPosList.RemoveAt(No);
        Destroy(m_InstObjList[No]);
        m_InstObjList.RemoveAt(No);
    }
}
