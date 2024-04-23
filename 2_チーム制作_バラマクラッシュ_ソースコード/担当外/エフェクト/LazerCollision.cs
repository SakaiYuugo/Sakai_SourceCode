using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.transform.tag.Contains("Player"))
        {
            other.GetComponent<PlayerState>().Damage();
        }
    }
}
