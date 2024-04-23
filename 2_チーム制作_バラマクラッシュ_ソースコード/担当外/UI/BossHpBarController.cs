using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarController : MonoBehaviour
{

    private Slider hpBar;
    private RectTransform hpBarRect;

    private float oldValue;   // 1�t���[���O�̒l
    private Vector3 originPosition;
    private ColorBlock cb;
    private int bossHp;
    private int maxHp;

    public enum State
    {
        Normal,
        Shake
    }

    private State nowState;

    private void Awake()
    {
        bossHp = 1000;
        maxHp = 1000;

    }

    // Start is called before the first frame update
    void Start()
    {
        hpBar = this.GetComponent<Slider>();
        hpBarRect = gameObject.GetComponent<RectTransform>();
        hpBar.value = 1.0f;
        nowState = State.Normal;
        cb = hpBar.colors;
        oldValue = hpBar.value;
        // �ŏ������Ȃ鎞������̂�Fixed����x�Ă�
        FixedUpdate();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        oldValue = hpBar.value;

        // HpBar��value�����炷
        hpBar.value = (float)bossHp / (float)maxHp;

        // �_���[�W��H�������
        if (oldValue > hpBar.value)
        {
            nowState = State.Shake;
        }

        //�����ɂȂ�����
        if (hpBar.value <= 0.5 && hpBar.value > 0.2)
        {
            // ���F�ɂ���
            cb.disabledColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);
            hpBar.colors = cb;
        }
        else if (hpBar.value <= 0.2)
        {
            // �ԐF�ɂ���
            cb.disabledColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            hpBar.colors = cb;
        }
        else
        {
            cb.disabledColor = new Color(0.0f, 1.0f, 0.0f,1.0f);
            hpBar.colors = cb;
        }
  
        switch (nowState)
        {
            case State.Normal:
                break;
            case State.Shake:
                StartCoroutine(ShakeCoroutine(0.1f));
                nowState = State.Normal;
                break;
            default:
                break;
        }
    }
    IEnumerator ShakeCoroutine(float time)
    {
        float x = Random.Range(-0.5f, 0.5f);
        float y = Random.Range(-0.5f, 0.5f);
        //transform.position += new Vector3(x, y, 0f);
        hpBarRect.position += new Vector3(x, y, 0f);
        yield return new WaitForSeconds(time);

        // time�b���Hp�o�[�����̏ꏊ�ɖ߂�
        hpBarRect.position -= new Vector3(x, y, 0f);
    }

    public void SetHp(int hp)
    {
        bossHp = hp;
    }

    public void SetMaxHp(int Maxhp)
    {
        maxHp = Maxhp;
    }

    public int GetHp()
    {
        return bossHp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    }
