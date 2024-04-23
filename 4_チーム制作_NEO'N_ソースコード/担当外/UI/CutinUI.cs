using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutinUI : MonoBehaviour
{
    public int index = 0;
    public Sprite[] sprites;
    private float counter;
    public bool cutin = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (cutin == true)
        {
            if (index >= sprites.Length)
            {
                cutin = false;
                counter = 1f;
                GetComponent<Image>().enabled = false;
                FindObjectOfType<SlowMotion>().StopSlowMotion();
                return;
            }
            
            counter += 0.5f;
            if (index < sprites.Length && counter >= 1)
            {
                GetComponent<Image>().sprite = sprites[index];
                counter = 0;
                index++;
            }
        }
    }

    public void CutIn()
    {
        if (!cutin)
        {
            FindObjectOfType<SlowMotion>().StartSlowMotion(0f);
            cutin = true;
            GetComponent<Image>().enabled = true;
            counter = 0;
            index = 0;
        }
    }
    
}
