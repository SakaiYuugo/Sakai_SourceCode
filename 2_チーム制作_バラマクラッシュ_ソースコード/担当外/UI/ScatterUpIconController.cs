using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScatterUpIconController : IconController
{

    Text Strewtext;
    GameObject player;
    StrewState Strew;
    int OriginStrewNum; // ‰Šú‚Ì‚Î‚çT‚«‰Â”\”
    int OldStrewNum;    // 1ƒtƒŒ[ƒ€‘O‚Ì‚Î‚çT‚«”
    int NowStrewNum;     // Œ»İ‚Ì‚Î‚çT‚«”
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        Strewtext = gameObject.GetComponentInChildren<Text>();
        player = System_ObjectManager.playerObject;
        Strew = player.GetComponentInChildren<StrewState>();
        // ‰Šú‚Ì‚Î‚çT‚«‰Â”\”‚ğ“ü‚ê‚é
        OriginStrewNum = Strew.GetStrewObjectNum();
        OldStrewNum = OriginStrewNum;
        NowStrewNum = OriginStrewNum;
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        // Œ»İ‚Ì‚Î‚çT‚«‰Â”\”‚ğ“ü‚ê‚é
        NowStrewNum = Strew.GetStrewObjectNum();

        // ‚Î‚çT‚¯‚é”‚ª•Ï‚í‚Á‚Ä‚¢‚È‚¢‚È‚çtext‚ğXV‚µ‚È‚¢
        if (OldStrewNum != NowStrewNum)
        {
            // Å‰‚Ì‚Î‚çT‚«”‚Æ¡‚Ì‚Î‚çT‚«”
            int num = NowStrewNum - OriginStrewNum;
            if (num >= 1)
            {
                // text‚É“ü—Í
                Strewtext.text = string.Format("X", num);
                //Œ»İ‚Ì‚Î‚çT‚«”XV
                OldStrewNum = num;
            }
        }
    }
}
