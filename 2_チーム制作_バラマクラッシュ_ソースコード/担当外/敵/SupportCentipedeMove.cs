using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportCentipedeMove : EnemySupportmove
{
    [SerializeField] float SEnemyDistance = 15.0f;
    private float random;
    private GameObject HomingHead;

    override protected void Start()
    {        
        EnemyDistance = SEnemyDistance;
        random = Random.Range(-5.0f, 5.0f);

        base.Start();

        //�C���X�^���X�������炷���ɒT��
        HomingTarget = SerchNearEnemy("Enemy");
        if (HomingTarget != null) HomingHead = HomingTarget.transform.GetChild(0).gameObject;

        //�I�u�W�F�N�g�̏�Ԃ����炤
        EnemyState = GetComponentInParent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();
    }

    override protected void FixedUpdate()
    {
        NowState = EnemyState.GetEnemyState();

        if (NowState == EnemyZakoState.ZakoState.Move)
        {
            base.FixedUpdate();

            //�^�[�Q�b�g�I�u�W�F�N�g�����ł��Ă�����V�����߂��ɂ���G���^�[�Q�b�g�ɂ���
            if (HomingTarget == null)
            {
                HomingTarget = SerchNearEnemy("Enemy");
                if(HomingTarget != null) HomingHead = HomingTarget.transform.GetChild(0).gameObject;
            }

            if (!(HomingHead == null))
            {
                
                //�^�[�Q�b�g�Ƃ̋���
                float enemydist = Vector3.Distance(transform.position, HomingHead.transform.position);
                
                this.transform.LookAt(HomingHead.transform.position);  //��Ƀ^�[�Q�b�g�̕���������
                Vector3 tmo = this.transform.rotation.eulerAngles;
                tmo.x = 0f;
                tmo.z = 0f;
                this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);
                
                //�^�[�Q�b�g����苗���ȉ��Ȃ�~�܂�
                if ((enemydist < EnemyDistance) && (HomingTarget != PlayerObj)) transform.Translate(0.0f, 0.0f, 0.0f);
                else transform.Translate(0.0f, 0.0f, MoveSpeed);    //�^�[�Q�b�g�̕���Z�����ʂŌ������Ă���
                //transform.Translate(0.0f, 0.0f, MoveSpeed);
                
                //this.transform.LookAt(HomingHead.transform);
                //transform.Translate(0.0f, 0.0f, MoveSpeed);
                                
            }
            else
            {
                //�^�[�Q�b�g���v���C���[�ɂ���
                //HomingTarget = PlayerObj;

                this.transform.LookAt(PlayerObj.transform);  //��Ƀ^�[�Q�b�g�̕���������
                Vector3 tmo = this.transform.rotation.eulerAngles;
                tmo.x = 0f;
                tmo.z = 0f;
                this.transform.rotation = Quaternion.Euler(tmo.x, tmo.y, tmo.z);

                transform.Translate(0.0f, 0.0f, MoveSpeed);
            }

            transform.Translate(0.08f * Mathf.Sin((Time.time + random) * 6), 0.0f, 0.0f);
        }

        //Debug.Log(HomingTarget);
    }
    
}
