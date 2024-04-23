using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingMove : MonoBehaviour
{
    public  GameObject Boss;
    public GameObject Point;

    private BossBeeAttack bossAtk;
    private BossBeeAttack.AttakType AtkType;
    private Vector3 RotateAxis = Vector3.up;

    //[SerializeField, Tooltip("���x�W��")]
    //private float SpeedFactor = 0.1f;

    [SerializeField, Tooltip("���x�W��")]
    private float Speed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        bossAtk = Boss.GetComponent<BossBeeAttack>();

    }

    private void FixedUpdate()
    {
        AtkType = bossAtk.GetAtkType;

        switch (AtkType)
        {
            case BossBeeAttack.AttakType.Lazer:
                // �w��I�u�W�F�N�g�𒆐S�ɉ�]����
                this.transform.RotateAround(
                    Point.transform.position,
                    RotateAxis,
                    360.0f / (1.0f / Speed) * Time.deltaTime
                    );
                break;
            case BossBeeAttack.AttakType.Wave:
                break;
            case BossBeeAttack.AttakType.Brow:
                break;
        }
    }

}
