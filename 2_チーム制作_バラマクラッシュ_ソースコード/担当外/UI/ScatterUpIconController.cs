using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScatterUpIconController : IconController
{

    Text Strewtext;
    GameObject player;
    StrewState Strew;
    int OriginStrewNum; // 初期のばら撒き可能数
    int OldStrewNum;    // 1フレーム前のばら撒き数
    int NowStrewNum;     // 現在のばら撒き数
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        Strewtext = gameObject.GetComponentInChildren<Text>();
        player = System_ObjectManager.playerObject;
        Strew = player.GetComponentInChildren<StrewState>();
        // 初期のばら撒き可能数を入れる
        OriginStrewNum = Strew.GetStrewObjectNum();
        OldStrewNum = OriginStrewNum;
        NowStrewNum = OriginStrewNum;
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        // 現在のばら撒き可能数を入れる
        NowStrewNum = Strew.GetStrewObjectNum();

        // ばら撒ける数が変わっていないならtextを更新しない
        if (OldStrewNum != NowStrewNum)
        {
            // 最初のばら撒き数と今のばら撒き数
            int num = NowStrewNum - OriginStrewNum;
            if (num >= 1)
            {
                // textに入力
                Strewtext.text = string.Format("X", num);
                //現在のばら撒き数更新
                OldStrewNum = num;
            }
        }
    }
}
