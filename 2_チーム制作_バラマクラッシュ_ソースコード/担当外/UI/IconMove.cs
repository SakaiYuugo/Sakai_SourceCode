using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconMove : MonoBehaviour
{

    [SerializeField, Header("���x�㏸�A�C�R��")] Image SpeedUpIcon;
    [SerializeField, Header("���G�A�C�R��")] Image InvincibleIcon;
    [SerializeField, Header("�����΂�T���A�C�R��")] Image InfinitelyIcon;
    [SerializeField, Header("���x�����A�C�R��")] Image SpeedDownIcon;
    [SerializeField, Header("�����]���d���A�C�R��")] Image HardenIcon;
    public enum IconType
    {
        None,       // �����Ȃ�(����)
        SpeedUp,    // �X�s�[�h�㏸
        Invincible, // ���G
        Infinitely, // �����΂�T��
        SpeedDown,  // �X�s�[�h�_�E��
        Harden,     // �����]���d��
    }

    public struct IconInfo
    {
        public Image ImageObj;  // �\������A�C�R��
        public IconController Controller;
        public IconType type;   // �A�C�R���̃^�C�v
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
        // �m�F
        for (int i = 0; i < iconInfos.Count; i++)
        {
            if (iconInfos[i].ImageObj == null)
                continue;

            // �|�W�V�����ύX
            iconInfos[i].ImageObj.rectTransform.anchoredPosition =
                new Vector2(Rect.anchoredPosition.x,
                Rect.anchoredPosition.y + 125.0f* (iconInfos.Count - 1 - i));

            // �����Ă����Ȃ�
            if (iconInfos[i].Controller.GetDestroyFlg())
            {
                Destroy(iconInfos[i].ImageObj);
                // ���X�g�������
                iconInfos.RemoveAt(i);
                i--;
            }
        }
    }

    //---------------------------
    // ������:�A�C�R���̃^�C�v
    // ������:�`�掞��(�b�P��)
    // --------------------------
    public bool SetIcon(IconType type,float DrawFrame)
    {
        // ���łɕ\�����Ă���Ȃ�(���X�g�ɓ����Ă���Ȃ�)���X�g�ɓ��ꂸ��DrawFrame�����X�V����
        for (int i = 0; i < iconInfos.Count;i++)
        {
            // �������̂���������
            if (iconInfos[i].type == type)
            {
                // �`�掞�Ԃ��X�V���Ċ֐����甲����
                iconInfos[i].ImageObj.GetComponent<IconController>().SetDrawFrame(DrawFrame);
                return false;
            }
        }

        IconInfo icon;

        // ��ނ�ݒ�
        icon.type = type;

        // �A�C�R���̃^�C�v�ʂɐ���
        switch (type)
        {
            case IconType.SpeedUp:
                // ����
                icon.ImageObj = Instantiate(SpeedUpIcon);
                icon.Controller = icon.ImageObj.GetComponent<IconController>();
                icon.Controller.SetDrawFrame(DrawFrame);
                // Canvas�̎q�I�u�W�F�N�g��
                icon.ImageObj.transform.SetParent(canvas.transform,false);
                // ���X�g�ɒǉ�
                iconInfos.Add(icon);
                break;
            case IconType.Invincible:
                // ����
                icon.ImageObj = Instantiate(InvincibleIcon);
                icon.Controller = icon.ImageObj.GetComponent<IconController>();
                icon.Controller.SetDrawFrame(DrawFrame);
                // Canvas�̎q�I�u�W�F�N�g��
                icon.ImageObj.transform.SetParent(canvas.transform, false);
                // ���X�g�ɒǉ�
                iconInfos.Add(icon);
                break;
            case IconType.Infinitely:
                // ����
                icon.ImageObj = Instantiate(InfinitelyIcon);
                icon.Controller = icon.ImageObj.GetComponent<IconController>();
                icon.Controller.SetDrawFrame(DrawFrame);
                // Canvas�̎q�I�u�W�F�N�g��
                icon.ImageObj.transform.SetParent(canvas.transform, false);
                // ���X�g�ɒǉ�
                iconInfos.Add(icon);
                break;
            case IconType.SpeedDown:
                // ����
                icon.ImageObj = Instantiate(SpeedDownIcon);
                icon.Controller = icon.ImageObj.GetComponent<IconController>();
                icon.Controller.SetDrawFrame(DrawFrame);
                // Canvas�̎q�I�u�W�F�N�g��
                icon.ImageObj.transform.SetParent(canvas.transform, false);
                // ���X�g�ɒǉ�
                iconInfos.Add(icon);
                break;
            case IconType.Harden:
                // ����
                icon.ImageObj = Instantiate(HardenIcon);
                icon.Controller = icon.ImageObj.GetComponent<IconController>();
                icon.Controller.SetDrawFrame(DrawFrame);
                // Canvas�̎q�I�u�W�F�N�g��
                icon.ImageObj.transform.SetParent(canvas.transform, false);
                // ���X�g�ɒǉ�
                iconInfos.Add(icon);
                break;
        }
        return true;
    } 

    public void StopIcon(IconType type)
    {
        for (int i = 0; i < iconInfos.Count; i++)
        {
            // �o�t�̎�ނ���v�����̂������������
            if (iconInfos[i].type == type)
            {
                Destroy(iconInfos[i].ImageObj);
                // ���X�g�������
                iconInfos.RemoveAt(i);
            }
        }
    }
}
