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
        [Tooltip("�U���G�t�F�N�g"),SerializeField] public GameObject AtkEffect;
        [Tooltip("�����ꏊ"), SerializeField] public GameObject AtkEffectPos;
        [Tooltip("�\���G�t�F�N�g"), SerializeField] public GameObject SignEffect;
        [Tooltip("�����ꏊ"), SerializeField] public GameObject SignEffectPos;
    }

    [SerializeField] EffectInfo[] Effectinfo;

    private int num;        // �G�t�F�N�g�̌�
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
