using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoShit : MonoBehaviour
{
    [SerializeField] int GenerationTime = 2;
    private int Count;

    void Start()
    {
        //ŠÔ‚Ì•â³
        GenerationTime *= 60;
        Count = 0;
    }

    void FixedUpdate()
    {
        Count++;

        if (Count > GenerationTime)
        {
            Count = 0;         //‚â‚éˆÓ–¡‚È‚¢‚©‚à‚¾‚¯‚Ç‹C‚¿‚Ì–â‘è
            Destroy(this.gameObject);
        }
    }

}
