using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Fade : MonoBehaviour
{
    float fadeCounter = 0f;
    public float time = 0.5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fadeCounter < time) fadeCounter += Time.deltaTime;
        else fadeCounter = time;

        if (GetComponent<TextMeshProUGUI>() != null)
        {
            Color originalColor = GetComponent<TextMeshProUGUI>().color;
            GetComponent<TextMeshProUGUI>().color = new Color(originalColor.r, originalColor.g, originalColor.b, fadeCounter/time);
        }
        else if(GetComponent<Image>() != null)
        {
            Color originalColor = GetComponent<Image>().color;
            GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, fadeCounter/time);
        }
    }
}
