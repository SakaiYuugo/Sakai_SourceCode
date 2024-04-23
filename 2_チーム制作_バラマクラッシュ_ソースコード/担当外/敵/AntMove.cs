using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMove : EnemyAssaultmove
{
    private float distance;
    [SerializeField,Header("�ˌ��������n�߂�player�Ƃ̋���")] float Length = 15.0f;

    private float speed;// �X�s�[�h�ۊǗp�ϐ�
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //�I�u�W�F�N�g�̏�Ԃ����炤
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();
        distance = 1000.0f;
        speed = base.MoveSpeed;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        NowState = EnemyState.GetEnemyState();
        if (NowState == EnemyZakoState.ZakoState.Move)
        {
            base.SetEnemySpeed(speed);
            base.FixedUpdate();
            // player�Ƃ̋������v�Z
            distance = Vector3.Distance(gameObject.transform.position, base.GetHomingObj().transform.position);
            // ���̋����Ƀv���C���[������Ȃ�
            if(distance <= Length)
            {
                // ��~������
                base.SetEnemySpeed(0.0f);
                //�U����Ԃ�
                EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
            }
        }
    }
}
