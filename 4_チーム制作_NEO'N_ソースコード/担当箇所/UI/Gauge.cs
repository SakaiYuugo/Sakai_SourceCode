using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    public float gaugepoint = 0f;
    public float gaugepointmax = 100f;
    // Start is called before the first frame update
    void Start()
    {
        gaugepoint = 0f;
        gaugepointmax = 100f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Image>().fillAmount = gaugepoint / gaugepointmax;
    }
}
