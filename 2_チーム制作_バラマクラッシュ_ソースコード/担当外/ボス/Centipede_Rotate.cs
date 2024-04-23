using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede_Rotate : MonoBehaviour
{
    [SerializeField]
    float Speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(Speed * 360.0f * Time.deltaTime,Vector3.right);
    }
}
