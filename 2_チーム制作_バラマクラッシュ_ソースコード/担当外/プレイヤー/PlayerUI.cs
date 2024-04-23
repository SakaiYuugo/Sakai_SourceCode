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

    float MaxSpped; //�����A�C�e�����݂̍ō����x
    int MaxHP;
    float NowHP;

    Coroutine _ImpactCor; //�U���R���[�`���ۑ�
    Vector3 YetAddPos = new Vector3(0,0,0);  //�O�t���[���̈ړ��ʂ�ۑ�
    bool bImact; //�U������

    void Start()
    {
        //UI��\������f�[�^���擾
        GameObject player;
        player = GameObject.Find("Player");
        _PlayerState = player.GetComponent<PlayerState>();
        _PlayerMove = player.GetComponent<PlayerMove>();

        //�_���[�W���󂯂ėh�炷����
        HPCircle = GameObject.Find("PlayerUI");

        //����̓Q�[�����ς��Ȃ��͂�
        MaxSpped = _PlayerMove.GMaxSpeed + _PlayerMove.GDashAddSpped;
        MaxHP = _PlayerState.GSMAX_HP + 3; //����HP9�Ɖ��肵����

        //UI�I�u�W�F�N�g�̋@�\���擾
        CircleImage = GameObject.Find("PlayerHPCircle").GetComponent<Image>();
        HPpoint = GameObject.Find("UI_PlayerHP").GetComponent<TextMeshProUGUI>();
        SpeedMeter = GameObject.Find("SpeedMeter").GetComponent<Image>();
        FixedUpdate();
    }
    

    void FixedUpdate()
    {
        if (NowHP > _PlayerState.GSnHP + 3)
        {//�O�t���[�����HP���Ⴉ������
            _ImpactCor = StartCoroutine(ImpactHPCoroutine());
        }

        if(bImact)
        {
            //�����_���Ɉړ��ʂ����߂�
            float tempX = Random.Range(-0.2f, 0.2f);
            float tempY = Random.Range(-0.2f, 0.2f);
            Vector3 AddPos = new Vector3(tempX,tempY,0.0f);            

            //����̃t���[�����������đO�t���[�����߂�
            HPCircle.GetComponent<Transform>().position += AddPos - YetAddPos;
            
            //�ړ��ʕۑ�
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
        //���ƂȂ���W��ۑ�
        Vector3 BasePos = HPCircle.GetComponent<Transform>().position;

        bImact = true;
        yield return new WaitForSeconds(1.0f);
        bImact = false;
        
        //���W�����ɖ߂�
        HPCircle.GetComponent<Transform>().position = BasePos;
    }
}
