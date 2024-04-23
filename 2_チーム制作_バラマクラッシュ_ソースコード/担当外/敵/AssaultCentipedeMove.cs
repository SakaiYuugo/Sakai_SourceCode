using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultCentipedeMove : EnemyAssaultmove
{
    [SerializeField] float AttackDist = 5.0f;    //���J�f�U����ԑJ�ڔ��a
    [SerializeField] int RandomAttack = 30;      //���J�f�̍U���m��
    private int AttackCount = 10;                //�����_�����������Ԋu
    private int Count = 0;                       //���ۂ̃J�E���g
    private float random;                        //���˂��˓������̃����_����

    override protected void Start()
    {
        EnemyState = GetComponentInParent<ZakoCentipedeState>();
        NowState = EnemyState.GetEnemyState();
        
        base.Start();

        AttackCount = AttackCount * 60;             //���Ԃ̕␳
        random = Random.Range(-5.0f, 5.0f);
 
    }

    
    override protected void FixedUpdate()
    {
        
        NowState = EnemyState.GetEnemyState();
        if (NowState == EnemyZakoState.ZakoState.Move)
        {                                                                 
            float distance = Vector3.Distance(this.transform.position, HomingObj.transform.position);
            if (distance <= 30)
            {//�v���C���[�Ƃ̋�����30��菬�����Ȃ�����v���C���[�^�[�Q�b�g
                this.transform.LookAt(HomingObj.transform.position);
                transform.Translate(0.0f, 0.0f, MoveSpeed);
            }
            else
            {
                //��Ƀv���C���[(������Ƃ��炷)�̕���������
                this.transform.LookAt(HomingObj.transform.position + TargetposPlus);                
                Vector3 tmo = this.transform.rotation.eulerAngles;
                tmo.x = 0f;
                tmo.z = 0f;
                this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);
                transform.Translate(0.0f, 0.0f, MoveSpeed);
                    
                //�^�[�Q�b�g�̈ʒu�ɂ��Ă��܂�����V�����^�[�Q�b�g�̈ʒu��I��(x����z���̋�����2.0f�ȉ��ɂȂ��Ă�����)
                if (Mathf.Abs(this.transform.position.x - (HomingObj.transform.position.x + TargetposPlus.x)) < 2.0f
                     && Mathf.Abs(this.transform.position.z - (HomingObj.transform.position.z + TargetposPlus.z)) < 2.0f)
                {
                    RandTargetPos.x = Random.Range(-50, 50);
                    RandTargetPos.y = Random.Range(-50, 50);
                    TargetposPlus = new Vector3(RandTargetPos.x, 0, RandTargetPos.y);
                }                    
            }                                     

            //���˂��˓���
            transform.Translate(0.08f * Mathf.Sin((Time.time + random) * 6), 0.0f, 0.0f);

            //�v���C���[�Ƃ̋����Ń_���[�W����(���炭���C���[�֌W�Ȃ��������͂�)
            float dist = Vector3.Distance(this.gameObject.transform.position, HomingObj.transform.position);
            if (HomingObj.transform.name == "Player")
            {              
                if (dist <= 1.0) EnemyState.SetDamege(1);
            }
            
            //���J�f�U���J�ڊ֘A            
            //�U������͈͂ɓ����Ă�����
            if (dist < AttackDist)
            {
                bool Atk = false;    //�U�����邩�̔���p
                if(Count == 0)
                {//�U�����莞��
                    //�����_���v�Z
                    int rand = Random.Range(0, 100);
                    if (rand <= RandomAttack) Atk = true;
                }

                //�U���J��
                if(Atk == true)
                {
                    Count++;
                    EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
                }
                else
                {
                    Count++;
                    if (Count > AttackCount) Count = 0;
                }
            }            
        }

        if(GameEventManager.bGameClear)
        {
            MoveSpeed = 0.0f;
        }
        if (!TutorialManager.TutorialNow && BossHp.GetNowHP() <= 0)
        {
            MoveSpeed = 0.0f;
        }
    }
    
}
