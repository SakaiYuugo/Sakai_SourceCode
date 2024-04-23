using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultDungBeetleMove : EnemyAssaultmove
{
    [SerializeField] float AttackDist = 15.0f;    //�t���R���K�V�U����ԑJ�ڔ��a
    [SerializeField] int RandomAttack = 50;      //�t���R���K�V�̍U���m��
    private int AttackCount = 10;                //�����_�����������Ԋu(�U��)
    private int Count = 0;                       //���ۂ̃J�E���g

    override protected void Start()
    {
        //�I�u�W�F�N�g�̏�Ԃ����炤
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();

        AttackCount *= 60;       //���Ԃ̕␳        

        base.Start();
    }

    override protected void FixedUpdate()
    {
        NowState = EnemyState.GetEnemyState();
        if (NowState == EnemyZakoState.ZakoState.Move)
        {            

            base.FixedUpdate();

            //�n�`�U���J�ڊ֘A            
            //�U������͈͂ɓ����Ă�����
            if(this.gameObject != null && HomingObj != null)
            {
                float dist = Vector3.Distance(this.gameObject.transform.position, HomingObj.transform.position);

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
