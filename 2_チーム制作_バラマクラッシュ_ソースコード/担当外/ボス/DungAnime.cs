using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungAnime : MonoBehaviour
{
    Animator animetor;


    void Start()
    {
        animetor = GameObject.Find("rig_All").GetComponent<Animator>();
    }

    public void WalkAnim()
    {
        animetor.SetTrigger("Walk");
    }

    public void StunAnime()
    {
        animetor.SetTrigger("Jitabata");
    }

    public void WakeUpAnime()
    {
        animetor.SetTrigger("WakeUp");
    }

    public void BarrierAnim()
    {
        animetor.SetTrigger("Barrier");
    }

    public void BarrierBrakeAnime()
    {
        animetor.SetTrigger("BarrierBrake");
    }

    public void BeamAnime()
    {
        animetor.SetTrigger("Beam");
    }

    public void BeamUpAnime()
    {
        animetor.SetTrigger("BeamUp");
    }

    public void JumpAnime()
    {
        animetor.SetTrigger("Jump");
    }

    public void DestroyAnime()
    {
        animetor.SetTrigger("Destroy");
    }
}
