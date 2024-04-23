using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffectPos : MonoBehaviour
{
    BossHP Boss;
    int MaxHp;
    int OldHp;
    float OldSizeX;
    float value;
    // Start is called before the first frame update
    void Start()
    {
        Boss = System_ObjectManager.bossObject.GetComponent<BossHP>();
        MaxHp = Boss.GetMaxHP();
        OldHp = MaxHp;
        //value = ((float)Boss.GetNowHP() * (float)gameObject.transform.localScale.x) / (float)MaxHp;
        //gameObject.transform.localScale = new Vector3(value, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        OldSizeX = gameObject.transform.localScale.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        OldSizeX = gameObject.transform.localScale.x;
        if (Boss.GetNowHP() != OldHp)
        {
            value = ((float)Boss.GetNowHP() / (float)MaxHp)*(float)gameObject.transform.localScale.x;
            gameObject.transform.localScale = new Vector3(value, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            float difference = OldSizeX - gameObject.transform.localScale.x;
            gameObject.transform.position -= new Vector3(difference, 0.0f, 0.0f);
        }
        OldHp = Boss.GetNowHP();
    }
}
