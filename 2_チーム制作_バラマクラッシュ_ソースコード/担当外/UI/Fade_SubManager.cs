using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade_SubManager : MonoBehaviour
{
    Fade_Manager myManager;

    Fade_ScatterBomb FadeIn_Scatter;
    Fade_PileUpBomb FadeOut_PileUP;

    // Start is called before the first frame update
    void Awake()
    {
        this.gameObject.AddComponent<Fade_ScatterBomb>();
        FadeIn_Scatter = this.gameObject.GetComponent<Fade_ScatterBomb>();
        FadeOut_PileUP = this.gameObject.AddComponent<Fade_PileUpBomb>();

        //fadeManager‚É“o˜^‚ğ‚·‚é
        myManager = transform.parent.gameObject.GetComponent<Fade_Manager>();

        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        //fadeManager‚É“o˜^‚ğ‚·‚é
        if(myManager == null)
        myManager = transform.parent.gameObject.GetComponent<Fade_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn()
    {
        //‚â‚Á‚Ä‚¢‚é‰Â”\«‚ª‚ ‚é‚Ì‚ÅI‚í‚ç‚¹‚Ä‚â‚é
        FadeOut_PileUP.End_PileUPBomb();
        //”ò‚Î‚µ‚Ä‚â‚é
        FadeIn_Scatter.Start_ExplosionBomb();
    }

    public bool GetEndFadeIn()
    {
        return FadeIn_Scatter.GetEnd();
    }

    public void FadeOut()
    {
        //‚â‚Á‚Ä‚¢‚é‰Â”\«‚ª‚ ‚é‚Ì‚ÅI‚í‚ç‚¹‚Ä‚â‚é
        FadeIn_Scatter.End_ExplosionBomb();
        //ƒ{ƒ€‚ğ—‚Æ‚µ‚Ä‚â‚é
        FadeOut_PileUP.Start_PileUpBomb();
    }

    public bool GetEndFadeOut()
    {
        return FadeOut_PileUP.GetEnd();
    }

    public void DontFade()
    {
        FadeIn_Scatter.End_ExplosionBomb();
    }
}
