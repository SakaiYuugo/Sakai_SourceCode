using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeCoreMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FexedUpdate()
    {
        this.transform.localScale +=  new Vector3( 10.0f,10.0f,10.0f);
    }
}
