using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveCount : MonoBehaviour
{
    int countNum;

    private void Awake()
    {
        countNum = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CaveAdd()
    {
        countNum++;
    }

    public void CaveMina()
    {
        countNum--;
    }

    public int GetCaveNum()
    {
        return countNum;
    }
}
