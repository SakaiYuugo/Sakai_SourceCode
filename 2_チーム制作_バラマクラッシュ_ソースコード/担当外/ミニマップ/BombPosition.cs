using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombPosition : MonoBehaviour
{
    GameObject DesBomb;
    public bool Destroyflg;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Destroyflg && DesBomb == null)
        {
            Destroy(this.gameObject);
        }
    
    }

    public void SetGameObj(GameObject Bomb)
    {
        DesBomb = Bomb;
        Destroyflg = true;
    }
}
