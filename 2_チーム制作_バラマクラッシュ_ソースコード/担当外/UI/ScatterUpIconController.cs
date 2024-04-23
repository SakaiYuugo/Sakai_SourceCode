using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScatterUpIconController : IconController
{

    Text Strewtext;
    GameObject player;
    StrewState Strew;
    int OriginStrewNum; // �����̂΂�T���\��
    int OldStrewNum;    // 1�t���[���O�̂΂�T����
    int NowStrewNum;     // ���݂̂΂�T����
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        Strewtext = gameObject.GetComponentInChildren<Text>();
        player = System_ObjectManager.playerObject;
        Strew = player.GetComponentInChildren<StrewState>();
        // �����̂΂�T���\��������
        OriginStrewNum = Strew.GetStrewObjectNum();
        OldStrewNum = OriginStrewNum;
        NowStrewNum = OriginStrewNum;
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        // ���݂̂΂�T���\��������
        NowStrewNum = Strew.GetStrewObjectNum();

        // �΂�T���鐔���ς���Ă��Ȃ��Ȃ�text���X�V���Ȃ�
        if (OldStrewNum != NowStrewNum)
        {
            // �ŏ��̂΂�T�����ƍ��̂΂�T����
            int num = NowStrewNum - OriginStrewNum;
            if (num >= 1)
            {
                // text�ɓ���
                Strewtext.text = string.Format("X", num);
                //���݂̂΂�T�����X�V
                OldStrewNum = num;
            }
        }
    }
}
