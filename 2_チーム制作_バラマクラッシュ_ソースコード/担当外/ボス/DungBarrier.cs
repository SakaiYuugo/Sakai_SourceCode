using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBarrier : MonoBehaviour
{ 
    public bool bBarrier = false;  //trueにするとバリアが出てる状態
    bool Priority = false;

    public void Barrier(bool Barrier　,bool priority = true/*HP50％で呼ぶ時は略して*/)
    {//ロールスター、HP割合バリアイベント時に受けるダメージを0にする
        if(!priority && bBarrier)   //優先度が低いかつ既にバリアを貼っているなら
        {
            if(Priority) return;
        }

        if (priority)   //優先度が高い時の命令を保存する
        {
            Priority = Barrier;
        }
        
        GetComponent<BossBarrier>().barrierEnable = Barrier;
        bBarrier = Barrier;
    }
}
