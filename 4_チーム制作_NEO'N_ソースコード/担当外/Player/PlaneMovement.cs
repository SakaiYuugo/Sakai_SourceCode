using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    float offset = 0f;
    public float speed = 8f;
    // Start is called before the first frame update
    void Start()
    {
        offset = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex",new Vector2(offset,0f));
        offset -= Time.deltaTime * speed;
    }
}
