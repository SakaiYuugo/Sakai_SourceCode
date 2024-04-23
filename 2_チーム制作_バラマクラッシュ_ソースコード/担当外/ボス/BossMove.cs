using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossMove : MonoBehaviour
{
    [Tooltip("�i�ރX�s�[�h"),SerializeField] float Speed = 3;      // �X�s�[�h

    [SerializeField] GameObject[] target;       // �ڕW�n�_

    [SerializeField] float Length = 100;


    private float m_Speed;             //�@�X�s�[�h�i�[�ϐ�
    private int numElements;           // �v�f��
    private float m_Distance;          // �^�[�Q�b�g�Ƃ̋���
    private BossBeeAnimation bossAnim;
    public bool FinishFlg;
    public bool GetFinishFlg { get { return FinishFlg; } }

   // private Animator animator;

    private enum MoveState
    {
        Moving,
        Rotation,
        Stop
    }
    private MoveState moveState;
    private MoveState oldState;
    private int frame;

    // Start is called before the first frame update
    void Start()
    {
        m_Speed = Speed;
        numElements = 0;
        moveState = MoveState.Rotation;
        bossAnim = this.GetComponent<BossBeeAnimation>();
        FinishFlg = false;
        frame = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        switch (moveState)
        {
            case MoveState.Moving:

                // �ڕW�l�֌�����
                this.transform.position = Vector3.MoveTowards(this.transform.position, target[numElements].transform.position, m_Speed * Time.deltaTime);

                // �����v�Z
                m_Distance = Vector3.Distance(transform.position, target[numElements].transform.position);

                this.transform.forward = (target[numElements].transform.position - this.transform.position).normalized;

                // ���̋����܂ŋ߂Â����玟�̃^�[�Q�b�g�ɕύX
                if (m_Distance <= Length)
                {
                    numElements++;
                    // �Ō�̖ڕW�n�_�ɕt������ŏ��̖ڕW�n�_�ɖ߂�
                    if (numElements >= target.Length)
                        numElements = 0;
                    moveState = MoveState.Rotation;
                }
                break;
            case MoveState.Rotation:
                Vector3 forward = target[numElements].transform.position - this.transform.position;
                Quaternion rot = Quaternion.LookRotation(forward);

                rot = Quaternion.Slerp(this.transform.rotation, rot, Time.deltaTime * 1);
                this.transform.rotation = rot;

                // �p�x�v�Z
                float angle = Vector3.Angle(forward, this.transform.forward);

                // ���̊p�x�܂ŉ�]�o������
                if (angle < 5 )
                {
                    moveState = MoveState.Moving;
                }
                break;
            case MoveState.Stop:
                frame++;
                if (frame >= 180)
                {
                    FinishFlg = true;
                    frame = 0;
                }

                break;
        }
       

        
    }

    public void StopBoss()
    {
        bossAnim.StopWalkAnimation();
        oldState = moveState;
        m_Speed = 0;
        moveState = MoveState.Stop;
    }

    public void StartBoss()
    {
        bossAnim.StartWalkAnimation();
        m_Speed = Speed;
        moveState = oldState;
        FinishFlg = false;
    }
}
