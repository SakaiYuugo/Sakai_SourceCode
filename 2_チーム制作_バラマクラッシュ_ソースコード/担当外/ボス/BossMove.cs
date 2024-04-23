using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossMove : MonoBehaviour
{
    [Tooltip("進むスピード"),SerializeField] float Speed = 3;      // スピード

    [SerializeField] GameObject[] target;       // 目標地点

    [SerializeField] float Length = 100;


    private float m_Speed;             //　スピード格納変数
    private int numElements;           // 要素数
    private float m_Distance;          // ターゲットとの距離
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

                // 目標値へ向かう
                this.transform.position = Vector3.MoveTowards(this.transform.position, target[numElements].transform.position, m_Speed * Time.deltaTime);

                // 距離計算
                m_Distance = Vector3.Distance(transform.position, target[numElements].transform.position);

                this.transform.forward = (target[numElements].transform.position - this.transform.position).normalized;

                // 一定の距離まで近づいたら次のターゲットに変更
                if (m_Distance <= Length)
                {
                    numElements++;
                    // 最後の目標地点に付いたら最初の目標地点に戻す
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

                // 角度計算
                float angle = Vector3.Angle(forward, this.transform.forward);

                // 一定の角度まで回転出来たら
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
