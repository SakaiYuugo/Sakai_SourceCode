using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//”š‚ğ•\¦‚·‚é
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

    //â‘Î‚É•Ï‚¦‚é
    private void SetNumber(int number)
    {
        Number = number;

        m_Image.sprite = Sprite[Number];
    }

    //”’l‚ª•Ï‚í‚Á‚Ä‚¢‚È‚¯‚ê‚Î‚â‚ç‚È‚¢
    public void ChangeNumber(int number)
    {
        //”š‚ª“¯‚¶‚¾‚Á‚½‚ç
        if(number == Number)
        {
            //”²‚¯‚é
            return;
        }

        SetNumber(number);
    }

    //Œ©‚¹‚éêŠ‚ğ•Ï‚¦‚é
    public void SetPos(Vector2 pos)
    {
        m_pos.anchoredPosition = pos;
    }

    public void SetSize(float size)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(size,size);
    }
}
