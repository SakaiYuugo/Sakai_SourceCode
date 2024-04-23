using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCollision : MonoBehaviour
{
    // Start is called before the first frame update
    virtual protected void Start()
    {
        
    }

    // Update is called once per frame
    virtual protected void FixedUpdate()
    {
        
    }

    virtual protected void OnParticleCollision(GameObject other)
    {
        if (other.transform.tag.Contains("Player"))
        {
            other.GetComponent<PlayerState>().Damage();
        }
    }

}
