using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//数字を表示する
public class DrawNumber : MonoBehaviour
{
    [SerializeField] Sprite[] Sprite = new Sprite[10];
    [SerializeField] int Number = 0;

    Image m_Image;
    RectTransform m_pos;

    // Start is called before the first frame update
    void Awake()
    {
        m_Image = GetComponent<Image>();
        m_pos = GetComponent<RectTransform>();

        SetNumber(Number);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //絶対に変える
    private void SetNumber(int number)
    {
        Number = number;

        m_Image.sprite = Sprite[Number];
    }

    //数値が変わっていなければやらない
    public void ChangeNumber(int number)
    {
        //数字が同じだったら
        if(number == Number)
        {
            //抜ける
            return;
        }

        SetNumber(number);
    }

    //見せる場所を変える
    public void SetPos(Vector2 pos)
    {
        m_pos.anchoredPosition = pos;
    }

    public void SetSize(float size)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(size,size);
    }
}
