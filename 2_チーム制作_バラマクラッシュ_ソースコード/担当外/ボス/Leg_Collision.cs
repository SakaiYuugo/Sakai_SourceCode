using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Leg_Collision : MonoBehaviour
{
    [SerializeField] GameObject SmokeEffect;
    AudioSource SE;
    bool First;
    // Start is called before the first frame update
    void Start()
    {
        SE = gameObject.GetComponent<AudioSource>();
        First = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Contains("Ground"))
        {
            Vector3 hitPos = new Vector3(0,0,0);
            foreach (ContactPoint point in collision.contacts)
            {
                hitPos = point.point;
            }
            Instantiate(SmokeEffect, hitPos, Quaternion.identity);

            if (!First)
                SE.Play();
        }
        First = false;
    }
}
