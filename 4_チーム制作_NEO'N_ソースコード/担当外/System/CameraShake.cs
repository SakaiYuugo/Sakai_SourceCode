using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public void CamShake(float duration, float strength)
    {
        // 引数で時間と揺れ具合を渡すようにしてください
        transform.DOShakePosition(duration, strength);
    }
}
