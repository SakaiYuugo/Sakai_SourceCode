using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[DisallowMultipleComponent]

public class ZakoBuff_Base : MonoBehaviour
{
    //[SerializeField]
    protected float BuffTime = 5.0f;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    virtual public void Start()
    {
        
    }

    // Update is called once per frame
    virtual public void FixedUpdate()
    {
        BuffTime -= Time.deltaTime;
        if(BuffTime < 0.0f)
        {
            ReleaseBuff();
            Destroy(this);
        }
    }

    virtual public void ReleaseBuff()
    {

    }

    public void SetBuffTime(float Time)
    {
        BuffTime = Time;
    }
}
