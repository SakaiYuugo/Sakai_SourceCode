using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNumManager : MonoBehaviour
{
    [SerializeField] int Number;
    [SerializeField] float DrawSize = 100.0f;
    [SerializeField,Range(0,1000)] float Distance;
    [SerializeField] bool Center = false; //�����ɒu��
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

    //���̊֐��ƈႢ��΂ɕς���
    //���̌����珇�Ԃɕς��Ă���
    private void SetNumber(int Num)
    {
        Number = Num;
        m_bChangeNum = true;

        //�z��ԍ�0��������Ă���
        int i = Number; 
        int digit = 1;

        while(true)
        {
            //�ŏ����̐�
            int OneNum = i % 10;

            //�O�̐��l(���X�g�̐�)���������������
            if (m_InstObjList.Count < digit)
            {
                //�����������̐ݒ�
                GameObject TempObj = Instantiate(NumberPrefab);
                TempObj.transform.parent = this.gameObject.transform;   //�q���ɂ��Ă��
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
            //�����̃Z�b�g
            m_InstNumList[digit - 1].ChangeNumber(OneNum);

            //�����������Ă��
            i /= 10;

            //���l���[���ɂȂ������߂Ă��
            if(i < 1)
            {
                break;
            }
            //���𑝂₷
            digit++;
        }

        //�O�̐��l�����������Ȃ��Ȃ��Ă����
        if (m_InstObjList.Count > digit)
        {
            for(int n = m_InstNumList.Count;n > digit;n--)
            {
                //���������Ă���
                DestroyDigit(n-1);
            }
        }

        //Vector3 BasePos = Center ? new Vector3((m_InstObjList.Count * Distance / 2.0f) - (Distance * 0.5f), 0.0f, 0.0f) : Vector3.zero;

        //for (int n = 0; n < m_InstObjList.Count; n++)
        //{
        //    m_InstRectPosList[n].anchoredPosition = new Vector3(BasePos.x - Distance * n, BasePos.y, 0.0f);
        //}

    }

    //�����ς���Ă��Ȃ���΂��Ȃ�
    public void ChangeNum(int Num)
    {
        //�������ς���Ă��Ȃ����
        if (Number == Num)
        {
            return;
        }

        SetNumber(Num);
    }

        //��������
    private void DestroyDigit(int No)
    {
        m_InstNumList.RemoveAt(No);
        m_InstRectPosList.RemoveAt(No);
        Destroy(m_InstObjList[No]);
        m_InstObjList.RemoveAt(No);
    }
}
