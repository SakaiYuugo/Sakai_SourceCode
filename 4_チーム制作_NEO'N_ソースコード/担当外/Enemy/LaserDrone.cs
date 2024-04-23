using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDrone : MonoBehaviour
{
    public AnimationCurve curveX;
    public AnimationCurve curveY;
    public AnimationCurve curveZ;
    private float counter;

    public Vector3 offsetValue = new Vector3(1f,20f,40f);
    public GameObject particle;
    public GameObject charge;

    private bool sound = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("AttackLaser");
    }

    void FixedUpdate()
    {
        counter += Time.deltaTime;

        int dir = (int)(transform.localPosition.x / (Mathf.Abs(transform.localPosition.x) + 0.01f));
        float x = (dir * curveX.Evaluate(counter) * offsetValue.x);
        transform.localPosition = new Vector3(x, curveY.Evaluate(counter) * offsetValue.y, curveZ.Evaluate(counter) * offsetValue.z);
    }

    IEnumerator AttackLaser()
    {
        yield return new WaitForSeconds(1f);
    
        Destroy(Instantiate(charge, transform),2f);
    
        yield return new WaitForSeconds(1f);

        GameObject a = Instantiate(particle, transform);
        a.transform.parent = null;
        Destroy(a,5f);
        
        
        if (sound == false)
        {
            GetComponent<AudioSource>().Play();
            sound = true;
        }
        
        yield return new WaitForSecondsRealtime(7f);
        Destroy(gameObject);
    }
    
}
