using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBarrier : MonoBehaviour
{ 
    public bool bBarrier = false;  //true�ɂ���ƃo���A���o�Ă���
    bool Priority = false;

    public void Barrier(bool Barrier�@,bool priority = true/*HP50���ŌĂԎ��͗�����*/)
    {//���[���X�^�[�AHP�����o���A�C�x���g���Ɏ󂯂�_���[�W��0�ɂ���
        if(!priority && bBarrier)   //�D��x���Ⴂ�����Ƀo���A��\���Ă���Ȃ�
        {
            if(Priority) return;
        }

        if (priority)   //�D��x���������̖��߂�ۑ�����
        {
            Priority = Barrier;
        }
        
        GetComponent<BossBarrier>().barrierEnable = Barrier;
        bBarrier = Barrier;
    }
}
