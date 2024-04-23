using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultCentipedeAttack : EnemyAttack
{
    public enum ACAttack
    {
        Bend,        //�g�̂̒��S(�l�ڂ̐�)��y�������Ɏ����グ��
        Fall,        //�����オ�����g�̂�n�ʂɓ|��
        Attack       //�g�̂��|���I����ďՌ��g����
    }

    [SerializeField] GameObject ShockWave;    //����������Ռ��g
    [SerializeField] int Atk = 1;   //�U����
    private ACAttack ACAState;      //�U���̏��
    private int AttackCount;        //�U���̃J�E���g
    private Vector3 Startpos;       //�U���J�n���̈ʒu
    private bool AttackState;       //�U���̏�ԂɂȂ��Ă��邩

    [SerializeField] AudioClip Sound;
    private AudioSource Audiosource;

    override protected void Start()
    {
        Attack = Atk;
        State = GetComponentInParent<EnemyZakoState>();
        NowState = State.GetEnemyState();
        ACAState = ACAttack.Bend;
        AttackCount = 0;
        //Startpos = this.transform.position;
        AttackState = false;

        Audiosource = GetComponent<AudioSource>();
    }

    
    override protected void FixedUpdate()
    {
        NowState = State.GetEnemyState();
        if(NowState == EnemyZakoState.ZakoState.Attack)
        {
            if(AttackState != true)
            {
                Startpos = this.transform.position;
                AttackState = true;
            }            

            switch (ACAState)
            {
                case ACAttack.Bend:
                    //�g�̂������グ��
                    //transform.Translate(0.0f, 0.3f, 0.0f);
                    Vector3 bend = this.transform.position;
                    bend.y += 0.3f;
                    this.transform.position = bend;

                    //�U���J�n�ʒu����
                    if (this.transform.position.y > Startpos.y + 4.0f)
                        SetACAState(ACAttack.Fall);

                    break;

                case ACAttack.Fall:
                    //�g�̂�ł��t����
                    transform.Translate(0.0f, -0.25f, 0.05f);

                    if(this.transform.position.y < Startpos.y)
                    {
                        Vector3 pos = this.transform.position;
                        pos.y = Startpos.y;
                        transform.position = pos;
                        SetACAState(ACAttack.Attack);
                    }
                    break;

                case ACAttack.Attack:

                    Audiosource.PlayOneShot(Sound);

                    //�Ռ��g����
                    if(AttackCount == 0)
                    {
                        Instantiate(ShockWave, this.transform.position, Quaternion.identity);
                    }

                    AttackCount++;

                    if (AttackCount >= 60)
                    {
                        AttackCount = 0;
                        AttackState = false;
                        SetACAState(ACAttack.Bend);

                        //�ړ���ԂɑJ��
                        State.SetEnemyState(EnemyZakoState.ZakoState.Move);
                    }
                    break;

                default:
                    break;
            }
            //Debug.Log(ACAState + "���݂̏��");
        }
    }

    public ACAttack GetACAState()
    {
        return ACAState;
    }

    //�O�̂���
    public void SetACAState(ACAttack state)
    {
        ACAState = state;
    }
}
