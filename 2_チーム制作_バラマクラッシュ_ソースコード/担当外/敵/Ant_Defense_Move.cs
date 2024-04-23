using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant_Defense_Move : EnemyDefmove
{
    private float distance;
    [SerializeField,Header("�{�X�Ƃ̋���")] float bossDistance = 80.0f;
    [SerializeField, Header("�ˌ��������n�߂�player�Ƃ̋���")] float Length = 15.0f;
    [SerializeField, Header("��]���x")] float RotateSpeed = 1.0f;

    private float speed;// �X�s�[�h�ۊǗp�ϐ�

    // Start is called before the first frame update
    override protected void Start()
    {
        BossDistance = bossDistance;
        MinRotateSpeed = RotateSpeed;
        base.Start();

        //�I�u�W�F�N�g�̏�Ԃ����炤
        EnemyState = this.transform.GetComponent<EnemyZakoState>();
        NowState = EnemyState.GetEnemyState();
        distance = 1000.0f;
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
            distance = Vector3.Distance(gameObject.transform.position, Player.transform.position);
            // ���̋����Ƀv���C���[������Ȃ�
            if (distance <= Length)
            {
                // ��~������
                base.SetEnemySpeed(0.0f);
                //�U����Ԃ�
                EnemyState.SetEnemyState(EnemyZakoState.ZakoState.Attack);
            }
            else
            {
                Debug.Log("�{�X�Ƃ̋����v�Z");
                //�{�X�Ǝ����̋������擾
                float dis = Vector3.Distance(this.gameObject.transform.position, BossObj.transform.position);

                if ((dis > BossDistance)/* && Change == false*/)
                {
                    Debug.Log("�{�X�Ƃɋ߂Â���");
                    base.TargetNears();
                }
                else
                {
                    base.TargetRotate();
                }
            }
        }
    }
}
