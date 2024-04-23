using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAssaultmove : MonoBehaviour
{
    protected bool Homing = true;                         //�Ǐ]���邩�̃t���O
    protected GameObject HomingObj;                       //�Ǐ]����I�u�W�F�N�g
    protected float MoveSpeed;                            //�ړ����x
    protected bool ColCheck = false;                      //���̓G�ƂԂ����Ă��邩
    protected int rand;                                   //���������Ƃ��ɂ͂����H(����Ȃ�����)
    protected Vector2Int RandTargetPos;                          //�^�[�Q�[�g�ʒu�����炷���߂̃����_���i�[�p
    protected Vector3 TargetposPlus;                      //�^�[�Q�b�g�̈ʒu���炷�p
    protected EnemyZakoState EnemyState;                  //���      
    protected EnemyZakoState.ZakoState NowState;          //���݂̎��g�̏��
    protected BossHP BossHp;

    virtual protected void Start()
    {
        HomingObj = System_ObjectManager.playerObject;     //�v���C���[�̖��O�̃I�u�W�F�N�g���擾
        //�v�Z�̎d�����Ⴄ�̂ł������œK���ɏC��        
        MoveSpeed = (ZakoEnemySpawn.MoveSpeed / 50) * (1.0f / 3.0f);

        //�_���^�[�Q�b�g�̈ʒu�����炷(�p�b�N�}������)
        RandTargetPos.x = Random.Range(-50, 50);
        RandTargetPos.y = Random.Range(-50, 50);
        TargetposPlus = new Vector3(RandTargetPos.x, 0, RandTargetPos.y);

        if(!TutorialManager.TutorialNow)
        {
            BossHp = System_ObjectManager.bossHp;
        }        
    }

    virtual protected void FixedUpdate()
    {                
        if(HomingObj != null)
        {
            //�Ǐ]�̏���
            if (Homing != false && ColCheck == false)
            {
                if (HomingObj == null)
                {
                    HomingObj = System_ObjectManager.playerObject;
                }

                float distance = Vector3.Distance(this.transform.position, HomingObj.transform.position);

                if (distance <= 20)
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

                    //�^�[�Q�b�g�̈ʒu�ɂ��Ă��܂�����V�����^�[�Q�b�g�̈ʒu��I��
                    if (Mathf.Abs(this.transform.position.x - (HomingObj.transform.position.x + TargetposPlus.x)) < 2.0f
                        && Mathf.Abs(this.transform.position.z - (HomingObj.transform.position.z + TargetposPlus.z)) < 2.0f)
                    {
                        RandTargetPos.x = Random.Range(-50, 50);
                        RandTargetPos.y = Random.Range(-50, 50);
                        TargetposPlus = new Vector3(RandTargetPos.x, 0, RandTargetPos.y);
                    }
                }

            }

            else if (ColCheck == true)
            {

                rand = Random.Range(-2, 2);
                transform.Translate(MoveSpeed * rand, 0.0f, -MoveSpeed * 3);

                ColCheck = false;
            }
        }
             
        if(GameEventManager.bGameClear)
        {
            MoveSpeed = 0.0f;
        }
        if(!TutorialManager.TutorialNow && BossHp.GetNowHP() <= 0)
        {
            MoveSpeed = 0.0f;
        }

    }

    //�Ǐ]���Ă���I�u�W�F�N�g���擾(�킴�킴������点�Ă������[)
    public GameObject GetHomingObj()
    {
        return HomingObj;
    }

    public virtual void CollisionEnterProcess(Collision copy)
    {

    }

    public virtual void CollisionReleaseProcess(Collision copy)
    {

    }

    //����Ȃ�����
    public virtual void OnCollisionEnter(Collision collision)
    {
        //���̓G�ɏՓ˂��Ă�����
        if (collision.gameObject.tag == "Enemy")
        {
            ColCheck = true;
        }

        CollisionEnterProcess(collision);
    }

    //����Ȃ�����
    public virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            ColCheck = false;
        }
        CollisionReleaseProcess(collision);
    }
    
    //Set�֐�(�^�[�Q�b�g)
    public void SetTarget(GameObject gb)
    {
        HomingObj = gb;
    }

    //���x��Set�֐�
    public void SetEnemySpeed(float Speed) { MoveSpeed = Speed; }

    
    //�^�[�Q�b�g�߂��p�̊֐�(������������g�������H)
    public void ReturnTarget()
    {
        HomingObj = System_ObjectManager.playerObject;
    }
    
}
