using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoDungBallState : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            transform.localScale = Vector3.zero;
        }
            
    }

}
