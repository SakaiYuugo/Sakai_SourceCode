using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMove : MonoBehaviour
{
    BossHP Boss;
    int MaxHp;
    int OldHp;
    float value;
    public GameObject Effect;
    //public GameObject criterion;
    // Start is called before the first frame update
    void Start()
    {
        Boss = System_ObjectManager.bossObject.GetComponent<BossHP>();
        MaxHp = Boss.GetMaxHP();
        OldHp = MaxHp;
        value = (float)Boss.GetNowHP() / MaxHp;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Boss.GetNowHP() != OldHp)
        {
            
            //Vector3 Position = new Vector3(criterion.transform.position.x + criterion.transform.localScale.x,
            //    criterion.transform.position.y, - 1.0f );
            //Instantiate(Effect, Position, Quaternion.identity);
        }
        OldHp = Boss.GetNowHP();
    }
}
