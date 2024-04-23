using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawn : MonoBehaviour
{
    //[SerializeField] GameObject RangedAtkEffect;
    //[SerializeField] GameObject RangedAtkEffectPos;

    //[SerializeField] GameObject CloseRangeAtkEffect;
    //[SerializeField] GameObject CloseRangeAtkEffectPos;

    [System.Serializable]struct EffectInfo
    {
        [Tooltip("攻撃エフェクト"),SerializeField] public GameObject AtkEffect;
        [Tooltip("生成場所"), SerializeField] public GameObject AtkEffectPos;
        [Tooltip("予兆エフェクト"), SerializeField] public GameObject SignEffect;
        [Tooltip("生成場所"), SerializeField] public GameObject SignEffectPos;
    }

    [SerializeField] EffectInfo[] Effectinfo;

    private int num;        // エフェクトの個数
    public int GetSetNum { get { return num; }}
   
    // Start is called before the first frame update
    void Start()
    {
        num = Effectinfo.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AtkEffectSpawn(int i)
    {
        if(Effectinfo[i].AtkEffect != null)
        {
            GameObject effect = Instantiate(Effectinfo[i].AtkEffect, Effectinfo[i].AtkEffectPos.transform.position, Quaternion.identity);
            effect.transform.parent = Effectinfo[i].AtkEffectPos.transform;
        }
    }

    public void SignEffectSpawn(int i)
    {
        if(Effectinfo[i].SignEffect != null)
        {
            GameObject effect = Instantiate(Effectinfo[i].SignEffect, Effectinfo[i].SignEffectPos.transform.position, Quaternion.identity);
            effect.transform.parent = Effectinfo[i].SignEffectPos.transform;
        }
    }
}
