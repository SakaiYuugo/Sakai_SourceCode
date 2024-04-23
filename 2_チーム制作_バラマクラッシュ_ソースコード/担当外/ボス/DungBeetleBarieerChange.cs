using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBeetleBarieerChange : MonoBehaviour
{
    [SerializeField] GameObject DungBeetle;
    GameObject Cave;

    CaveCount caveCount;
    DungBarrier dungBarrier;
    BossHP bossHP;
    private bool IsBarrier;     // ƒoƒŠƒA‚ð“\‚Á‚Ä‚¢‚é‚©‚Ç‚¤‚©


    // Start is called before the first frame update
    void Start()
    {
        Cave = this.gameObject;
        caveCount = Cave.GetComponent<CaveCount>();
        dungBarrier = DungBeetle.GetComponent<DungBarrier>();
        bossHP = DungBeetle.GetComponent<DungBeetleState>();
        IsBarrier = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(bossHP.GetNowHP() < bossHP.GetMaxHP() * 0.5f  && caveCount.GetCaveNum() >= 1 && !IsBarrier)
        {
            dungBarrier.Barrier(true);
            IsBarrier = true;
        }
        if(caveCount.GetCaveNum() <= 0)
        {
            dungBarrier.Barrier(false);
            IsBarrier = false;
        }
    }
}
