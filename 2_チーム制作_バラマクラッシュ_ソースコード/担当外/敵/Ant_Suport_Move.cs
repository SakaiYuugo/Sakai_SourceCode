using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant_Suport_Move : EnemySupportmove
{
    [SerializeField, Header("�ˌ��������n�߂�player�Ƃ̋���")] float Length = 15.0f;

    private float distance;
    private float speed;// �X�s�[�h�ۊǗp�ϐ�
    // Start is called before the first frame update
    override protected void Start()
    {
        //�I�u�W�F�N�g�̏�Ԃ����炤
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();
        base.Start();
        speed = base.MoveSpeed;
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        NowState = EnemyState.GetEnemyState();
        if (NowState == EnemyZakoState.ZakoState.Move)
        {
            base.SetEnemySpeed(speed);
            base.FixedUpdate();

            // player�Ƃ̋������v�Z
            distance = Vector3.Distance(gameObject.transform.position, PlayerObj.transform.position);
            // ���̋����Ƀv���C���[������Ȃ�
            if (distance <= Length)
            {
                // ��~������
                base.SetEnemySpeed(0.0f);
                //�U����Ԃ�
                EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
            }


            //�^�[�Q�b�g�I�u�W�F�N�g�����ł��Ă�����V�����߂��ɂ���G���^�[�Q�b�g�ɂ���
            if (HomingTarget == null) HomingTarget = SerchNearEnemy("Enemy");

            if (!(HomingTarget == null))
            {
                //�^�[�Q�b�g�Ƃ̋���
                float enemydist = Vector3.Distance(transform.position, HomingTarget.transform.position);

                this.transform.LookAt(HomingTarget.transform.position);  //��Ƀ^�[�Q�b�g�̕���������
                Vector3 tmo = this.transform.rotation.eulerAngles;
                tmo.x = 0f;
                tmo.z = 0f;
                this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);

                //�^�[�Q�b�g����苗���ȉ��Ȃ�~�܂�
                if ((enemydist < EnemyDistance) && (HomingTarget != PlayerObj)) transform.Translate(0.0f, 0.0f, 0.0f);
                else transform.Translate(0.0f, 0.0f, MoveSpeed);    //�^�[�Q�b�g�̕���Z�����ʂŌ������Ă���
            }
            else
            {
                //�^�[�Q�b�g���v���C���[�ɂ���
                HomingTarget = PlayerObj;

                this.transform.LookAt(HomingTarget.transform.position);  //��Ƀ^�[�Q�b�g�̕���������
                Vector3 tmo = this.transform.rotation.eulerAngles;
                tmo.x = 0f;
                tmo.z = 0f;
                this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);

                transform.Translate(0.0f, 0.0f, MoveSpeed);
            }
        }
    }
}
