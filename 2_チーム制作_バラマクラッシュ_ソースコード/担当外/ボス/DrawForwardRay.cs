using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawForwardRay : MonoBehaviour
{
    [SerializeField]
    float Distance = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * Distance, Color.red);
    }
}
