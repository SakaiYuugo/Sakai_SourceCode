using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade_Manager : MonoBehaviour
{
    [SerializeField]
    bool FadeOut = false;

    [SerializeField]
    bool FadeIn = false;

    [SerializeField]
    AudioClip SE_Strew;
    [SerializeField]
    AudioClip SE_Bomb;

    public bool StartDontFade = false;

    bool FadeOutNow;
    bool FadeInNow;

    List<Fade_SubManager> subManagerList;
    int SubCheckNum;

    AudioSource audioSource;

    private void Awake()
    {
        subManagerList = new List<Fade_SubManager>();
        audioSource = GetComponent<AudioSource>();
    }


    private void Start()
    {
        //���g�������t�F�[�h�ɂ��Ă��
        System_SceneChange.instance.SetNowFadePanel(this);

        //�q�I�u�W�F�N�g�����ׂēo�^����
        foreach(Transform child in this.transform)
        {
            subManagerList.Add(child.gameObject.GetComponent<Fade_SubManager>());
        }

        if (StartDontFade)
        {
            DontDrawFade();
        }

        FadeInNow = FadeOutNow = false;
    }


    private void Update()
    {
        if(FadeOut)
        {
            IsFadeOut();
            FadeOut = false;
            
        }

        if(FadeIn)
        {
            IsFadeIn();
            FadeIn = false;
        }
    }

    public virtual void IsFadeIn()
    {
        foreach(Fade_SubManager copy in subManagerList)
        {
            copy.FadeIn();
        }
        FadeInNow = true;
        SubCheckNum = 0;

        audioSource.PlayOneShot(SE_Bomb);
    }

    public virtual void IsFadeOut()
    {
        foreach (Fade_SubManager copy in subManagerList)
        {
            copy.FadeOut();
        }

        FadeOutNow = true;
        SubCheckNum = 0;

        audioSource.PlayOneShot(SE_Strew);
    }

    public bool CheckFadeOutEnd()
    {
        //�t�F�[�h������Ă�����
        if (FadeOutNow)
        {
            for (; SubCheckNum < subManagerList.Count; SubCheckNum++)
            {
                //�t�F�[�h�A�E�g�o���Ă��Ȃ������ł������
                if (!subManagerList[SubCheckNum].GetEndFadeOut())
                {
                    return false;
                }
            }
        }

        //�����܂ŗ�����true
        FadeOutNow = false;
        return true;
    }

    public bool CheckFadeInEnd()
    {
        //�t�F�[�h������Ă�����
        if (FadeInNow)
        {
            for (; SubCheckNum < subManagerList.Count; SubCheckNum++)
            {
                //�t�F�[�h�A�E�g�o���Ă��Ȃ������ł������
                if (!subManagerList[SubCheckNum].GetEndFadeIn())
                {
                    return false;
                }
            }
        }

        //�����܂ŗ�����true
        FadeInNow = false;
        return true;
    }

    public void DontDrawFade()
    {
        foreach (Fade_SubManager copy in subManagerList)
        {
            copy.DontFade();
        }
    }
}
