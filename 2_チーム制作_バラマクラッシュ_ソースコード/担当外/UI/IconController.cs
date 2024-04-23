using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconController : MonoBehaviour
{
    [SerializeField, Header("残り何秒で激しい点滅を始めるか")] float FierceFrame = 3.0f;
    [SerializeField, Header("残り何秒で緩い点滅を始めるか")] float LenientFrame = 5.0f;

    protected enum state
    {
        NotTransparent, // 不透明
        Transparent,    // 透明
        Destroy,        // 削除
    }
    protected state NowState;
    protected float DrawFrame;
    protected float m_frame;
    protected bool m_DestroyFlg;

    protected Image imageObj;

    protected void Awake()
    {
        DrawFrame = 60.0f * 60 * 60;
    }
    // Start is called before the first frame update
    virtual protected void Start()
    {
        NowState = state.NotTransparent;
        m_frame = 0.0f;
        m_DestroyFlg = false;
        imageObj = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    virtual protected void FixedUpdate()
    {
        if (DrawFrame <= 0.0f)
        {
            NowState = state.Destroy;
            m_DestroyFlg = true;
        }

        if(DrawFrame <= FierceFrame)// 激しい点滅
        {
            // 1秒間ずつ点滅させる
            switch (NowState)
            {
                case state.NotTransparent:
                    m_frame += Time.deltaTime;
                    // 1秒経ったら透明へ
                    if(m_frame >= 0.1f)
                    {
                        m_frame = 0.0f;
                        imageObj.color = new Color(256.0f, 256.0f, 256.0f, 0.0f);
                        NowState = state.Transparent;
                    }
                    break;
                case state.Transparent:
                    m_frame += Time.deltaTime;
                    // 1秒経ったら不透明へ
                    if (m_frame >= 0.1f)
                    {
                        m_frame = 0.0f;
                        imageObj.color = new Color(256.0f, 256.0f, 256.0f, 256.0f);
                        NowState = state.NotTransparent;
                    }
                    break;
            }
        }
        else if(DrawFrame <= LenientFrame) // 緩い点滅
        {
            // 0.5秒間ずつ点滅させる
            switch (NowState)
            {
                case state.NotTransparent:
                    m_frame += Time.deltaTime;
                    // 1秒経ったら透明へ
                    if (m_frame >= 0.5f)
                    {
                        m_frame = 0.0f;
                        imageObj.color = new Color(256.0f, 256.0f, 256.0f, 0.0f);
                        NowState = state.Transparent;
                    }
                    break;
                case state.Transparent:
                    m_frame += Time.deltaTime;
                    // 1秒経ったら不透明へ
                    if (m_frame >= 0.5f)
                    {
                        m_frame = 0.0f;
                        imageObj.color = new Color(256.0f, 256.0f, 256.0f, 256.0f);
                        NowState = state.NotTransparent;
                    }
                    break;
            }
        }
        DrawFrame -= Time.deltaTime;
    }

    public void SetDrawFrame(float frame)
    {
        NowState = state.NotTransparent;
        DrawFrame = frame;
    }

    public bool GetDestroyFlg()
    {
        return m_DestroyFlg;
    }

    


}
