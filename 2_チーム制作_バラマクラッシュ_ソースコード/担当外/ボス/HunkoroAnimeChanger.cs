using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunkoroAnimeChanger : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    GameObject animeObject;
    [SerializeField]
    bool JITABATA = false;
    [SerializeField]
    bool WALK = false;
    [SerializeField]
    bool WALKUP = false;
    [SerializeField]
    bool BARRIER = false;
    [SerializeField]
    bool BARRIERBREAKER = false;
    [SerializeField]
    bool BEAM = false;
    [SerializeField]
    bool BEAMUP = false;
    [SerializeField]
    bool JUMP = false;
    [SerializeField]
    bool destroy = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = animeObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(JITABATA)
        {
            animator.SetTrigger("Jitabata");
            JITABATA = false;
        }

        if(WALK)
        {
            animator.SetTrigger("Walk");
            WALK = false;
        }
        if(WALKUP)
        {
            animator.SetTrigger("WakeUp");
            WALKUP = false;
        }
        if(BARRIER)
        {
            animator.SetTrigger("Barrier");
            BARRIER = false;
        }
        if(BARRIERBREAKER)
        {
            animator.SetTrigger("BarrierBrake");
            BARRIERBREAKER = false;
        }
        if(BEAM)
        {
            animator.SetTrigger("Beam");
            BEAM = false;
        }
        if(BEAMUP)
        {
            animator.SetTrigger("BeamUp");
            BEAMUP = false;
        }
        if (JUMP)
        {
            animator.SetTrigger("Jump");
            JUMP = false;
        }

        if(destroy)
        {
            animator.SetTrigger("Destroy");
            destroy = false;
        }
    }
}
