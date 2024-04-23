using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    public GameObject Barrier;  //ƒvƒŒƒnƒu
    GameObject insBarrier; //” 
    

    void Start()
    {
        insBarrier = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void StartBarrier()
    {
        if (insBarrier) return;

        insBarrier = Instantiate(Barrier, transform.position, Quaternion.identity);
        insBarrier.transform.parent = transform;

    }

    public void EndBarrier()
    {
        if (!insBarrier) return;

        Destroy(insBarrier);
        insBarrier = null;
    }
}
