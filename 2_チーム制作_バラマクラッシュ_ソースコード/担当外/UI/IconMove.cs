using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconMove : MonoBehaviour
{

    [SerializeField, Header("速度上昇アイコン")] Image SpeedUpIcon;
    [SerializeField, Header("無敵アイコン")] Image InvincibleIcon;
    [SerializeField, Header("無限ばら撒きアイコン")] Image InfinitelyIcon;
    [SerializeField, Header("速度減少アイコン")] Image SpeedDownIcon;
    [SerializeField, Header("方向転換硬化アイコン")] Image HardenIcon;
    public enum IconType
    {
        None,       // 何もない(透明)
        SpeedUp,    // スピード上昇
        Invincible, // 無敵
        Infinitely, // 無限ばら撒き
        SpeedDown,  // スピードダウン
        Harden,     // 方向転換硬化
    }

    public struct IconInfo
    {
        public Image ImageObj;  // 表示するアイコン
        public IconController Controller;
        public IconType type;   // アイコンのタイプ
    }
    List<IconInfo> iconInfos;
    Canvas canvas;
    RectTransform Rect;

    private void Awake()
    {
        System_ObjectManager.BuffDebuffIconUI = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.root.gameObject.GetComponent<Canvas>();
        Rect = gameObject.GetComponent<RectTransform>();
        iconInfos = new List<IconInfo>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 確認
        for (int i = 0; i < iconInfos.Count; i++)
        {
            if (iconInfos[i].ImageObj == null)
                continue;

            // ポジション変更
            iconInfos[i].ImageObj.rectTransform.anchoredPosition =
                new Vector2(Rect.anchoredPosition.x,
                Rect.anchoredPosition.y + 125.0f* (iconInfos.Count - 1 - i));

            // 消していいなら
            if (iconInfos[i].Controller.GetDestroyFlg())
            {
                Destroy(iconInfos[i].ImageObj);
                // リストから消す
                iconInfos.RemoveAt(i);
                i--;
            }
        }
    }

    //---------------------------
    // 第一引数:アイコンのタイプ
    // 第二引数:描画時間(秒単位)
    // --------------------------
    public bool SetIcon(IconType type,float DrawFrame)
    {
        // すでに表示しているなら(リストに入っているなら)リストに入れずにDrawFrameだけ更新する
        for (int i = 0; i < iconInfos.Count;i++)
        {
            // 同じものがあったら
            if (iconInfos[i].type == type)
            {
                // 描画時間を更新して関数から抜ける
                iconInfos[i].ImageObj.GetComponent<IconController>().SetDrawFrame(DrawFrame);
                return false;
            }
        }

        IconInfo icon;

        // 種類を設定
        icon.type = type;

        // アイコンのタイプ別に生成
        switch (type)
        {
            case IconType.SpeedUp:
                // 生成
                icon.ImageObj = Instantiate(SpeedUpIcon);
                icon.Controller = icon.ImageObj.GetComponent<IconController>();
                icon.Controller.SetDrawFrame(DrawFrame);
                // Canvasの子オブジェクトに
                icon.ImageObj.transform.SetParent(canvas.transform,false);
                // リストに追加
                iconInfos.Add(icon);
                break;
            case IconType.Invincible:
                // 生成
                icon.ImageObj = Instantiate(InvincibleIcon);
                icon.Controller = icon.ImageObj.GetComponent<IconController>();
                icon.Controller.SetDrawFrame(DrawFrame);
                // Canvasの子オブジェクトに
                icon.ImageObj.transform.SetParent(canvas.transform, false);
                // リストに追加
                iconInfos.Add(icon);
                break;
            case IconType.Infinitely:
                // 生成
                icon.ImageObj = Instantiate(InfinitelyIcon);
                icon.Controller = icon.ImageObj.GetComponent<IconController>();
                icon.Controller.SetDrawFrame(DrawFrame);
                // Canvasの子オブジェクトに
                icon.ImageObj.transform.SetParent(canvas.transform, false);
                // リストに追加
                iconInfos.Add(icon);
                break;
            case IconType.SpeedDown:
                // 生成
                icon.ImageObj = Instantiate(SpeedDownIcon);
                icon.Controller = icon.ImageObj.GetComponent<IconController>();
                icon.Controller.SetDrawFrame(DrawFrame);
                // Canvasの子オブジェクトに
                icon.ImageObj.transform.SetParent(canvas.transform, false);
                // リストに追加
                iconInfos.Add(icon);
                break;
            case IconType.Harden:
                // 生成
                icon.ImageObj = Instantiate(HardenIcon);
                icon.Controller = icon.ImageObj.GetComponent<IconController>();
                icon.Controller.SetDrawFrame(DrawFrame);
                // Canvasの子オブジェクトに
                icon.ImageObj.transform.SetParent(canvas.transform, false);
                // リストに追加
                iconInfos.Add(icon);
                break;
        }
        return true;
    } 

    public void StopIcon(IconType type)
    {
        for (int i = 0; i < iconInfos.Count; i++)
        {
            // バフの種類が一致したのを見つけたら消す
            if (iconInfos[i].type == type)
            {
                Destroy(iconInfos[i].ImageObj);
                // リストから消す
                iconInfos.RemoveAt(i);
            }
        }
    }
}
