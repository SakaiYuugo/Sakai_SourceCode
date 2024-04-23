using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumType_Text : MonoBehaviour
{
    [SerializeField]
    string[] textlist;

    Text myText;
    int Number;

    private void Awake()
    {
        myText = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTextNum(int No)
    {
        Number = No;
        //今選んでいるstageに合った文章を選んでやる
        myText.text = textlist[Number];
    }

    public int GetListLength()
    {
        return textlist.Length;
    }
}
