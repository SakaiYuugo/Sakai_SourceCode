using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBallRollStar : MonoBehaviour
{
    DungBarrier _BossBarrier;
    DungManager Manager;
    int nConstWaitFrame = 100;   //��Œ��̃A�j���[�V�������Ԃ�����
    [SerializeField] int nConstSequelFrame = 50;   //�㌄����
    int nFrame;
    bool bChangeState;
    Vector3 CenterPoint; //�}�b�v�̒��S�ɂ���ڈ���W (����͓����\��Ȃ��̂�Transform����񂩂�)
    float fLenght;  //��ӂ̒���
    [ReadOnly] int nGoPoint;
    [SerializeField] float fSpeed;
    Vector3 StartPos;    //�����]����n�߂�J�n�ʒu
    SpriteRenderer MagicCircle; //���@�w

    List<Vector3> starPoints = new List<Vector3>();

    public enum UpImpactSTATE
    {
        m_None,     //�Ă΂�Ă��Ȃ��Ƃ�
        m_ROTATE,   //��΂������̌��ɒ����ړ�
        m_ANIME,     //�A�j���[�V�����ҋ@�Ƌ󔒎���
        m_MOVE,     //���`�Ɉړ���
        m_END,      //���̖ړI�n�����֕����]��
    }
    [ReadOnly] public UpImpactSTATE state;

    Rigidbody rb;

    void Start()
    {
        Manager = GameObject.Find("Boss_Point").GetComponent<DungManager>();
        _BossBarrier = GameObject.Find("Boss_DungBeetle").GetComponent<DungBarrier>();
        CenterPoint = GameObject.Find("MapCenterPoint").GetComponent<Transform>().position;

        rb = GetComponent<Rigidbody>();
        state = UpImpactSTATE.m_None;
        GameObject Beetle = GameObject.Find("Boss_Dungbeetle");
        MagicCircle = GameObject.Find("MagicCircle").GetComponent<SpriteRenderer>();
        bChangeState = true;
    }

    void FixedUpdate()
    {
        //��x��������
        if (bChangeState)
        {
            switch (state)
            {
                case UpImpactSTATE.m_ROTATE:

                    break;

                case UpImpactSTATE.m_ANIME:
                    Manager._DungAnime.BeamAnime(); //�����΂��A�j��
//                    MagicCircle.enabled = true;
                    //�����ɖ��@�w����--------------------------------------
                    nFrame = 0;
                    break;

                case UpImpactSTATE.m_MOVE:
                    Manager._DungSound.Sound(DungSound.DUNG_SOUND.SOUND_ROLLSTAR);
                    StartPos = transform.position;
                    //���@�w�폜-------------------------------------------
                    //�t���R���̓K�[�h��ԂɂȂ�
//                    MagicCircle.enabled = false;
                    Manager._DungAnime.BarrierAnim();
                    _BossBarrier.Barrier(true, false);
                    break;

                case UpImpactSTATE.m_END:
                    Manager._DungAnime.WalkAnim();

                    break;
            }
            bChangeState = false;
        }

        switch (state)
        {
            case UpImpactSTATE.m_ROTATE:
                float maxRotation = 1.4f;

                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(starPoints[0] - this.transform.position), maxRotation);
                Manager._DBMove.Turnaround();

                // �ړI�̊p�x�܂ŉ�]�o������
                if (this.transform.rotation == Quaternion.LookRotation(starPoints[0] - this.transform.position))
                {
                    state = UpImpactSTATE.m_ANIME;
                    bChangeState = true;
                }
                break;

            case UpImpactSTATE.m_ANIME:
                nFrame++;
                if(nFrame >= nConstWaitFrame)
                {
                    state = UpImpactSTATE.m_MOVE;
                    bChangeState = true;
                    nFrame = 0;
                }
                break;

            case UpImpactSTATE.m_MOVE:
                //�`�F�b�N�|�C���g�Ɍ������Ĉړ�
                transform.position = Vector3.MoveTowards(transform.position, starPoints[nGoPoint], fSpeed);
                Manager._Db.Rotate();

                if (transform.position.x <= starPoints[nGoPoint].x + 5
                 && transform.position.x >= starPoints[nGoPoint].x - 5
                 && transform.position.z <= starPoints[nGoPoint].z + 5
                 && transform.position.z >= starPoints[nGoPoint].z - 5)  //�`�F�b�N�|�C���g�ɋ߂Â�����
                {
                    nGoPoint += 1;
                    transform.LookAt(starPoints[nGoPoint]); //�i�s����������
                    if (nGoPoint == 5)
                    {
                        transform.position = StartPos;
                        _BossBarrier.Barrier(false,false);
                        state = UpImpactSTATE.m_END;
                        bChangeState = true;
                    }
                }
                break;

            case UpImpactSTATE.m_END:
                float maxRotat = 1.4f;
          
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, 
                    Quaternion.LookRotation(Manager._DBMove.lV3CheckPoint[Manager._DBState.nGoPoint] - this.transform.position), maxRotat);
                Manager._DBMove.Turnaround();

                // �ړI�̊p�x�܂ŉ�]�o������
                if (this.transform.rotation == Quaternion.LookRotation(Manager._DBMove.lV3CheckPoint[Manager._DBState.nGoPoint] - this.transform.position))
                {
                    state = UpImpactSTATE.m_None;
                    starPoints.Clear();
                    Manager._DBState.state = DungBeetleState.DungBeetleSTATE.m_MOVE;
                    Manager._DBState.bChangeState = true;
                }
                break;
        }
    }

    //�U���J�n���ɌĂ�
    public void StartAtk(Vector3 endGoPos)
    {
        CreatePoint();
        state = UpImpactSTATE.m_ROTATE;
        nGoPoint = 0;
    }

    //�ʂ̓����|�C���g�𐶐�����
    void CreatePoint()
    {
        fLenght = Vector3.Distance(transform.position, CenterPoint) * 1.0f;    //��ӂ̒���
        Vector3 nowPos = new Vector3(transform.position.x, transform.position.y + 10.0f, transform.position.z);//�����ʒu�Afor�����ł̓��[���h���W��̖ړI�|�C���g��\��
        Vector3 Vec;
        for (int i = 0; i < 5; i++)
        {
            Vec = nowPos - CenterPoint;
            Vec = Quaternion.Euler(0, 144, 0) * Vec;
            starPoints.Add(Vec + CenterPoint);
            nowPos = Vec + CenterPoint;
        }
        starPoints.Add(starPoints[0]);
    }
}
