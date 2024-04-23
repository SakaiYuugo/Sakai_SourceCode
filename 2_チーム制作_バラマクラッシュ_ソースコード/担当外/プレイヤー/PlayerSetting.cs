using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class PlayerSetting : MonoBehaviour
{
    Rigidbody rb;
    Transform trans;
    GameObject ob;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // rigidbody‚ðŽæ“¾
        trans = GetComponent<Transform>();  //Transform‚ðŽæ“¾
        rb.mass = 30.0f;
        rb.drag = 0.0f;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        trans.localScale = new Vector3(1.0f,1.0f,1.0f);
        this.tag = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
