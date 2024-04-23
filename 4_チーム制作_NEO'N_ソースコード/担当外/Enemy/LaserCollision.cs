using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCollision : MonoBehaviour
{
    public float speed;

    void FixedUpdate()
    {
       transform.position += transform.forward * Time.deltaTime * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (FindObjectOfType<Player>().SuperDashMode)
            {
                GameObject obj = Instantiate(other.GetComponent<Player>().obstacleDestroyEffect.gameObject, transform);
                obj.transform.parent = null;
                Destroy(obj, 0.6f);
                Destroy(gameObject);
                return;
            }
            
            other.GetComponent<Player>().Death();
        }
    }
    
}
