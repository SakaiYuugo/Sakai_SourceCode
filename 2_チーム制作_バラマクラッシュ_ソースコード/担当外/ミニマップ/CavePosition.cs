using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CavePosition : MonoBehaviour
{
    GameObject DesCave;
    public bool Destroyflg;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Destroyflg && DesCave == null)
        {
            Destroy(this.gameObject);
        }
    
    }

    public void SetGameObj(GameObject Cave)
    {
        DesCave = Cave;
        Destroyflg = true;
    }
}
