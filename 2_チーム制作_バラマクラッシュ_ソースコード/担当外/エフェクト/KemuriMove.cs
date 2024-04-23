using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KemuriMove : MonoBehaviour
{
    GameObject BossObj;
    BossBeeState BeeState;
    ParticleSystem ParticleKemuri;
    bool moduleEnabled;
    enum KemuriState
    {
        Play,
        Stop
    }

    KemuriState NowState;
    // Start is called before the first frame update
    void Start()
    {
        BossObj = System_ObjectManager.bossObject;
        if (BossObj.name.Contains("Boss_Bee"))
        {
            BeeState = BossObj.GetComponent<BossBeeState>();
        }
        ParticleKemuri = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (BossObj.name.Contains("Boss_Bee"))
        {
            ParticleSystem.EmissionModule emission = ParticleKemuri.emission;
            if (BeeState.GetState() == BossBeeState.State.Move)
            {

                emission.enabled = true;
            }
            else
            {
                emission.enabled = false;
            }
        }
    }
}
