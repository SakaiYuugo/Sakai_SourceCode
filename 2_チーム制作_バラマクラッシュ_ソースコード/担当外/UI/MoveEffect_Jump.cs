using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffect_Jump : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField]
    float JumpPower = 10.0f;
    [SerializeField]
    float Gravity = 0.01f;

    float NowPower;
    bool NowJump;
    Vector2 LandingPos;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        LandingPos = rectTransform.anchoredPosition;
        NowJump = false;
    }

    // Update is called once per frame
    void Update()
    {

        if((int)Time.time % 5 ==0 && !NowJump)
        {
            SetJump();
        }

        rectTransform.anchoredPosition += Vector2.up * NowPower;
        NowPower -= Gravity;

        if(rectTransform.anchoredPosition.y < LandingPos.y)
        {
            NowJump = false;
            NowPower = 0.0f;
        }

    }

    public void SetJump()
    {
        NowPower = JumpPower;
        NowJump = true;
    }
}
