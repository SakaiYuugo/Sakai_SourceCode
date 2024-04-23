using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossType_Text : MonoBehaviour
{

    [SerializeField]
    string[] textlist = new string[(int)StageSelect.E_STAGE.MAX];

    Text myText;

    private void Awake()
    {
        myText = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //���I��ł���stage�ɍ��������͂�I��ł��
        myText.text = textlist[StageSelect.GetNowSelect()];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
