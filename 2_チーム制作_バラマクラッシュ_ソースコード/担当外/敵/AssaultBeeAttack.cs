using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultBeeAttack : EnemyAttack
{
    //�U���A�j���[�V����������Ȃ�Ȃ��Ȃ邩��
     enum ABAttack
     {
        Preparation,  //����
        PreAttack,    //�U������
        Attack,       //�U��
        EndAttack,    //�U���I��
        Return        //���̈ʒu�ɖ߂�
     }



    [SerializeField] int Atk = 1;
    [SerializeField] float AttackTilt = 40;    //�ǂꂭ�炢�X����
    [SerializeField] GameObject ShootPoint;
    [SerializeField] GameObject Needle;
    [SerializeField] int ShootSpeed = 2000;
    [SerializeField] AudioClip sound;
    private AudioSource Audiosource;

    float PerCount;           //0�`1�̊Ԃ̐���������(0�`100��)
    float PerAddNum;          //��̕ϐ��ɑ�����������
    Quaternion EndTilt;       //�ŏ�(�X���O)�̌X��������
    Quaternion StartTilt;     //�Ō�(�X������)�̌X��������
    Quaternion AtkEndTilt;    //�j���o���Ƃ��̌X��

    private ABAttack NowAttack; //�X�e�[�^�X

    override protected void Start()
    {
        Attack = Atk;
        NowAttack = ABAttack.Preparation;
        PerCount = 0.0f;
        PerAddNum = 0.0f;

        Audiosource = GetComponent<AudioSource>();
        base.Start();
    }

    
    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        if (NowState == EnemyZakoState.ZakoState.Attack)
        {
            switch(NowAttack)
            {
                //����
                case ABAttack.Preparation:
                    StartTilt = transform.rotation;
                    EndTilt = Quaternion.AngleAxis(AttackTilt, transform.right) * transform.rotation;
                    AtkEndTilt = Quaternion.AngleAxis(-50.0f, transform.right) * transform.rotation;
                    NowAttack = ABAttack.PreAttack;
                    PerCount = 0.0f;
                    PerAddNum = 0.0f;
                    break;

                //���ɉ�]
                case ABAttack.PreAttack:

                    //��]�̕��
                    transform.rotation = Quaternion.Lerp(StartTilt, EndTilt, PerCount);

                    PerAddNum += 0.005f;

                    PerCount += PerAddNum;


                    if (PerCount >= 1.0f)
                    {
                        PerCount = PerAddNum = 0.0f;

                        //���ɖ߂����[�V�����Ƀ`�F���W
                        NowAttack = ABAttack.Attack;
                    }

                    break;

                //�U��
                case ABAttack.Attack:

                    Audiosource.PlayOneShot(sound);

                    //��]�̕��
                    transform.rotation = Quaternion.Lerp(EndTilt, AtkEndTilt, PerCount);

                    PerAddNum += 0.005f;

                    PerCount += PerAddNum;

                    if (PerCount >= 1.0f)
                    {
                        PerCount = PerAddNum = 0.0f;

                        NowAttack = ABAttack.EndAttack;                        
                    }

                    break;

                //���̈ʒu�̉�]�ɖ߂�
                case ABAttack.EndAttack:
                    //�U���̏����������Ă�������

                    //�j�𐶐��H                    
                    GameObject bullet = Instantiate(Needle, ShootPoint.transform.position, Quaternion.identity);
                    Rigidbody bulletrb = bullet.GetComponent<Rigidbody>();
                    bulletrb.AddForce(transform.forward * ShootSpeed);                       
                    
                    //���ɖ߂����[�V�����Ƀ`�F���W
                   NowAttack = ABAttack.Return;                    

                    break;

                case ABAttack.Return:
                    //��]�̕��
                    transform.rotation = Quaternion.Lerp(AtkEndTilt, StartTilt, PerCount);

                    PerAddNum += 0.005f;

                    PerCount += PerAddNum;


                    if (PerCount >= 1.0f)
                    {
                        PerCount = PerAddNum = 0.0f;

                        //���ɖ߂����[�V�����Ƀ`�F���W
                        NowAttack = ABAttack.Preparation;
                        //�I�u�W�F�N�g�̏�Ԃ��ړ��ɕύX
                        State.SetEnemyState(EnemyZakoState.ZakoState.Move);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
