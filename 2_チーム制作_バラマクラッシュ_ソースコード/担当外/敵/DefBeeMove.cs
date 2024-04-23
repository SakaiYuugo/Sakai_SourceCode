using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefBeeMove : EnemyDefmove
{
    [SerializeField] float DBossDistance = 80.0f;   //�{�X�Ƃ̋���
    [SerializeField] float DAttackDistance = 20.0f; //�{�X�̌�q����U������Ƃ��̋���
    [SerializeField] float DMinRotateSpeed = 1.0f;    //��]���x
    [SerializeField] GameObject DBossObj;
    [SerializeField] float AttackDist = 5.0f;
    [SerializeField] int RandomAttack = 50;      //�n�`�̍U���m��
    private int AttackCount = 10;                //�����_�����������Ԋu
    private int Count = 0;                       //���ۂ̃J�E���g

    // Start is called before the first frame update
    override protected void Start()
    {
        BossDistance = DBossDistance;
        AttackDistance = DAttackDistance;
        MinRotateSpeed = DMinRotateSpeed;
        BossObj = DBossObj;

        //�I�u�W�F�N�g�̏�Ԃ����炤
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();

        base.Start();
    }

    // Update is called once per frame
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
