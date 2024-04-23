using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTypeText : MonoBehaviour
{
    [SerializeField]
    string[] textlist = new string[(int)SystemLevelManager.LEVELS.MAX];

    Text myText;

    private void Awake()
    {
        myText = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //ç°ëIÇÒÇ≈Ç¢ÇÈstageÇ…çáÇ¡ÇΩï∂èÕÇëIÇÒÇ≈Ç‚ÇÈ
        myText.text = textlist[SystemLevelManager.GetLevel()];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
