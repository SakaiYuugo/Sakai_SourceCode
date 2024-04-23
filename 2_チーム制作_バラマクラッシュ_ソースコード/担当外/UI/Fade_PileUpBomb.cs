using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade_PileUpBomb : MonoBehaviour
{
    [SerializeField]
    float FallTime = 1.0f;
    
    bool PileUP;
    RectTransform rectTransform;
    Vector2 InitPos;
    Vector2 StartPos;
    Vector2 EndPos;
    float FallStartTime;
    float Count;
    bool End;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        InitPos = rectTransform.anchoredPosition;

        //�ŏ��̈ʒu�̌v�Z(LerpStartPosObject)
        Vector2 Temp = rectTransform.anchoredPosition;
        Temp.y = Screen.height + 400.0f;
        StartPos = Temp;

        //�Ō�̈ʒu(ThisGameObject)
        EndPos = rectTransform.anchoredPosition;
        //�ǂꂭ�炢�̃^�C�~���O�ŗ����邩�v�Z
        FallStartTime = (rectTransform.anchoredPosition.y / Screen.height) * 1.0f;
        if (FallStartTime < 0.0f)
        {
            FallStartTime = 0.0f;
        }

        End = false;
        PileUP = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!PileUP)
        {
            return;
        }

        //�܂���������O
        if (Count < FallStartTime + FallTime)
        {
            Count += Time.fixedDeltaTime;

            //�J�E���g����������
            if(Count > FallStartTime + FallTime)
            {
                //�I���
                End_PileUPBomb();
            }
        }

        //���Ԃ�������O�̏ꍇ
        if (FallStartTime > Count)
        {
            return;
        }

        //���
        float Value = Mathf.Pow((Count - FallStartTime) / FallTime, 4);

        rectTransform.anchoredPosition = Vector2.Lerp(StartPos, EndPos, Value);

    }

    //���̊֐���ǂ�ł��ƃt�F�[�h�A�E�g���n�߂�
    public void Start_PileUpBomb()
    {
        this.gameObject.SetActive(true);
        //�Ō�ɍŏ��̏ꏊ��ݒ�
        rectTransform.anchoredPosition = StartPos;
        Count = 0.0f;
        PileUP = true;
        End = false;
    }

    //������Ăׂ΋����I�Ƀt�F�[�h�A�E�g���I���
    public void End_PileUPBomb()
    {
        rectTransform.anchoredPosition = EndPos;
        Count = FallStartTime + FallTime;
        PileUP = false;
        End = true;
    }

    //������true�ɂȂ�ΏI�����
    public bool GetEnd()
    {
        return End;
    }
}
