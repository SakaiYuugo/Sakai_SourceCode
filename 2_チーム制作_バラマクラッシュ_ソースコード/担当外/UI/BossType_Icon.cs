using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossType_Icon : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites = new Sprite[(int)StageSelect.E_STAGE.MAX];

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = sprites[StageSelect.GetNowSelect()];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
