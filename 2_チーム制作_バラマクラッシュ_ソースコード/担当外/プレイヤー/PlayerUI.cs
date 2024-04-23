using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    TextMeshProUGUI HPpoint;
    Image CircleImage;
    Image SpeedMeter;
    GameObject HPCircle;

    PlayerState _PlayerState;
    PlayerMove _PlayerMove;

    float MaxSpped; //加速アイテム込みの最高速度
    int MaxHP;
    float NowHP;

    Coroutine _ImpactCor; //振動コルーチン保存
    Vector3 YetAddPos = new Vector3(0,0,0);  //前フレームの移動量を保存
    bool bImact; //振動中か

    void Start()
    {
        //UIを表示するデータを取得
        GameObject player;
        player = GameObject.Find("Player");
        _PlayerState = player.GetComponent<PlayerState>();
        _PlayerMove = player.GetComponent<PlayerMove>();

        //ダメージを受けて揺らすもの
        HPCircle = GameObject.Find("PlayerUI");

        //これはゲーム中変わらないはず
        MaxSpped = _PlayerMove.GMaxSpeed + _PlayerMove.GDashAddSpped;
        MaxHP = _PlayerState.GSMAX_HP + 3; //仮にHP9と仮定した策

        //UIオブジェクトの機能を取得
        CircleImage = GameObject.Find("PlayerHPCircle").GetComponent<Image>();
        HPpoint = GameObject.Find("UI_PlayerHP").GetComponent<TextMeshProUGUI>();
        SpeedMeter = GameObject.Find("SpeedMeter").GetComponent<Image>();
        FixedUpdate();
    }
    

    void FixedUpdate()
    {
        if (NowHP > _PlayerState.GSnHP + 3)
        {//前フレームよりHPが低かったら
            _ImpactCor = StartCoroutine(ImpactHPCoroutine());
        }

        if(bImact)
        {
            //ランダムに移動量を決める
            float tempX = Random.Range(-0.2f, 0.2f);
            float tempY = Random.Range(-0.2f, 0.2f);
            Vector3 AddPos = new Vector3(tempX,tempY,0.0f);            

            //今回のフレーム分動かして前フレーム分戻す
            HPCircle.GetComponent<Transform>().position += AddPos - YetAddPos;
            
            //移動量保存
            YetAddPos = AddPos;
        }

        NowHP = _PlayerState.GSnHP + 3;
        float NowSpeed = _PlayerMove.GSNowSpeed;
        if (_PlayerState.GSnHP >= 6) CircleImage.color = new Color32(127,255,0,255);
        if (_PlayerState.GSnHP <= 5) CircleImage.color = new Color32(255,140,0,255);
        if (_PlayerState.GSnHP <= 2) CircleImage.color = new Color32(255,0,0,255);
        CircleImage.fillAmount = NowHP / MaxHP;
        HPpoint.text = _PlayerState.GSnHP.ToString(); 
        SpeedMeter.fillAmount = (NowSpeed / MaxSpped) * 0.57f  + 0.07f;
        
    }

    IEnumerator ImpactHPCoroutine()
    {
        //元となる座標を保存
        Vector3 BasePos = HPCircle.GetComponent<Transform>().position;

        bImact = true;
        yield return new WaitForSeconds(1.0f);
        bImact = false;
        
        //座標を元に戻す
        HPCircle.GetComponent<Transform>().position = BasePos;
    }
}
