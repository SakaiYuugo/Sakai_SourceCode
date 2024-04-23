using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefDungBeetleMove : EnemyDefmove
{
    [SerializeField] float DBossDistance = 80.0f;     //�{�X�Ƃ̋���
    [SerializeField] float DAttackDistance = 20.0f;   //�{�X�̌�q����U������Ƃ��̋���
    [SerializeField] float DMinRotateSpeed = 1.0f;    //��]���x

    [SerializeField] float AttackDist = 15.0f;    //�t���R���K�V�U����ԑJ�ڔ��a
    [SerializeField] int RandomAttack = 50;      //�t���R���K�V�̍U���m��
    private int AttackCount = 10;                //�����_�����������Ԋu(�U��)
    private int Count = 0;                       //���ۂ̃J�E���g

    override protected void Start()
    {
        BossDistance = DBossDistance;
        AttackDistance = DAttackDistance;
        MinRotateSpeed = DMinRotateSpeed;

        //�I�u�W�F�N�g�̏�Ԃ����炤
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();

        base.Start();
    }

    override protected void FixedUpdate()
    {
        NowState = EnemyState.GetEnemyState();

        if (NowState == EnemyZakoState.ZakoState.Move)
        {
            base.FixedUpdate();

            //�{�X�Ǝ����̋������擾
            float dis = Vector3.Distance(this.gameObject.transform.position, BossObj.transform.position);

            if ((dis > BossDistance)/* && Change == false*/)
            {
                TargetNears();
            }
            else
            {
                TargetRotate();

                //�n�`�U���J�ڊ֘A            
                //�U������͈͂ɓ����Ă�����
                float dist = Vector3.Distance(this.gameObject.transform.position, PlayerObj.transform.position);
                if (dist < AttackDist)
                {
                    bool Atk = false;
                    if (Count == 0)
                    {
                        //�����_���v�Z
                        int rand = Random.Range(0, 100);
                        if (rand <= RandomAttack) Atk = true;
                    }

                    //�U���J��
                    if (Atk == true)
                    {
                        transform.LookAt(PlayerObj.transform.position);
                        EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
                        Count++;
                    }
                    else
                    {
                        Count++;
                        if (Count > AttackCount) Count = 0;
                    }
                }
            }
        }
    }
}
