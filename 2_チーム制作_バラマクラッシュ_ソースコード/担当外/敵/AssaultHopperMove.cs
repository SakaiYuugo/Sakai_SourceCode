using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ˌ��o�b�^�̓���
public class AssaultHopperMove : EnemyAssaultmove
{
    [SerializeField] float AttackDist = 5.0f;    //�o�b�^�U����ԑJ�ڔ��a
    [SerializeField] int RandomAttack = 50;      //�o�b�^�̍U���m��
    [SerializeField] float JumpPowerValue = 500.0f;
    private float CoolCount = 0;
    Rigidbody rb;
    ZakoHopperState TempHopperState;

    protected override void Start()
    {
        //�I�u�W�F�N�g�̏�Ԃ����炤
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();
        CoolCount = 10.0f;
        rb = GetComponent<Rigidbody>();
        TempHopperState = EnemyState as ZakoHopperState;

        base.Start();
    }

    protected override void FixedUpdate()
    {
        NowState = EnemyState.GetEnemyState();
        if (NowState == EnemyZakoState.ZakoState.Move)
        {
            base.FixedUpdate();

            if (CoolCount > 0.0f)
            {
                //�J�E���g�_�E��
                CoolCount -= Time.deltaTime;
                if (CoolCount < 0.0f)
                {
                    CoolCount = 0.0f;
                }

                return;
            }

            //�U������͈͂ɓ����Ă�����
            Vector3 TempA = this.gameObject.transform.position, TempB = HomingObj.transform.position;
            TempA.y = TempB.y = 0.0f;
            float dist = Vector3.Distance(TempA, TempB);

            if (dist < AttackDist)
            {
                //�����_���ōU��������
                if (CoolCount == 0)
                {
                    //�U���m���v�Z
                    int rand = Random.Range(0, 100);        //�����_���Ȑ����������Ă���
                    if (rand <= RandomAttack)//�U��������
                    {
                        EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
                    }
                    CoolCount = 10.0f;
                }
            }

            
        }
    }

    public override void CollisionEnterProcess(Collision copy)
    {
        base.CollisionEnterProcess(copy);
        if(copy.gameObject.name == "Ground")
        {
            //�n�ʂɂ���
            TempHopperState.SetJumpNow(false);
        }
    }
}
