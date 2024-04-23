using System;
using System.Collections;
using UnityEngine;


public class PlayerFanfare : MonoBehaviour
{
    int nFrame; //���ׂĂ̓����̃t���[��

    CameraGameStart start;
    Rigidbody rb;
    Transform trans;
    PlayerState _PlayerState;
    PlayerSlopeRotate _PlayerSlopeRotate;
    PlayerRay playerRay;
    bool bBend; //�J�[�u��
    bool bDrift;    //�h���t�g��

    [NonSerialized] Vector3 v3Front;    //�v���C���[�̐��ʕ���
    [Header("���̃X�N���v�g�͊e�X�e�[�W�̓���p�ł�")]
    [SerializeField, Tooltip("���o�Ă��鑬�x"), ReadOnly] float fSpeed;   //�ړ����x
    [SerializeField, Tooltip("������(1f)")] float fAccelerationf;   //�����x
    [SerializeField, Tooltip("�����u���[�L��(1f)")] float fFriction;   //�����u���[�L
    [SerializeField, Tooltip("�ő呬�x")] float MaxSpeed;    //�ő呬�x
    [SerializeField, Tooltip("�J�[�u���ő呬�x")] float MaxBendSpeed;  //�J�[�u���̍ő呬�x
    [SerializeField, Tooltip("�Ȃ���p�x(1F)")] float MaxBendAngle; //���ʂɋȂ���p�x

    //---- �h���t�g ----
    [SerializeField, Tooltip("�X���p�x")] float MaxDriftRange = 50.0f;  //�h���t�g�p�x�ȂȂ߂ɂȂ��
    [SerializeField, Tooltip("�h���t�g���̋Ȃ���p�x")] float MaxDriftBend;  //�h���t�g�p�x�Ȃ���p�x�p(�����|��̂Ŏg���Ƃ���MaxBendAngle������)
    [SerializeField, Tooltip("�h���t�g���̌�����(1F)")] float fDriftDecelerat; //�h���t�g�����l

    enum FANFARE_STATE
    {
        m_WAIT,     //�ҋ@�V���g��
        m_ADVANCE,  //�}���[���X�L�[
        m_STOP,
        m_END,
    }
    FANFARE_STATE state = FANFARE_STATE.m_WAIT;

    //---- GeterSeter ----
    public float GSNowSpeed { get { return fSpeed; } }
    public float GMaxSpeed { get { return MaxSpeed; } }
    public float GSfAccelerationf { get { return fAccelerationf; } }
    public float GSfFriction { get { return fFriction; } }

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // rigidbody���擾
        trans = GetComponent<Transform>();  //Transform���擾  
        _PlayerState = GameObject.Find("Player").GetComponent<PlayerState>();
        _PlayerSlopeRotate = GameObject.Find("SlopeRotate").GetComponent<PlayerSlopeRotate>();
        playerRay = GameObject.Find("RayPointFront").GetComponent<PlayerRay>();

        nFrame = 0;
        start = GameObject.Find("Main Camera").GetComponent<CameraGameStart>();
        Physics.autoSimulation = false;
    }

    public void Fanfare()
    {
        Physics.Simulate(Time.fixedDeltaTime / 3);
        //---- ���ʂ��čs����ړ����� ----�i�s��Ȃ��p�^�[�����o�Ă����ꍇ��return�\��j
        nFrame++;
        bBend = false;  //�J�[�u���t���O��߂�
        bDrift = false; //�h���t�g���t���O��߂�
        v3Front = -trans.forward;  //���ʂ�ύX

        switch (state)
        {
            case FANFARE_STATE.m_WAIT:
                if (nFrame >= 30)
                {
                    state +=1;
                    nFrame = 0;
                }
                
                break;
            case FANFARE_STATE.m_ADVANCE:
                Accelerator();
                if (nFrame >= 120)
                {
                    state += 1;
                    nFrame = 0;
                }
                break;
            case FANFARE_STATE.m_STOP:
                fSpeed = 0.0f;

                if(nFrame >= 60)
                {
                    state += 1;
                    nFrame = 0;
                }
                break;
            case FANFARE_STATE.m_END:
                state = FANFARE_STATE.m_WAIT;   //���߂ɖ߂�
                nFrame = 0;
                start.EndPlayerMove();
                Physics.autoSimulation = true;

                break;
        }

        if (fSpeed < 0)    //���x���}�C�i�X�Ȃ��~��Ԃɂ���
        {
            fSpeed = 0.0f;
        }

        if (!bBend) //�Ȃ����Ă��Ȃ��Ȃ�
        {
            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.CENTER);
        }

        if (!bDrift)
        {
            GetComponent<PlayerSE>().SoundStopDrift();
        }

        float tempY = rb.velocity.y;    //Y���W�͉������Ȃ�
        rb.velocity = new Vector3(v3Front.x * fSpeed, tempY, v3Front.z * fSpeed); //���ۂɉ���

    }

    //---- �A�N�Z�� ----
    void Accelerator()
    {
        fSpeed -= fFriction;    //�A�N�Z�����Ă��Ȃ����u���[�L�ɂȂ�

        if (bBend && fSpeed >= MaxBendSpeed) return;    //�J�[�u���A�J�[�u���x���o�Ă���Ȃ�����ł��Ȃ�

        if (fSpeed >= MaxSpeed) return;     //MaxSpeed���o�Ă���Ȃ�


            fSpeed += fAccelerationf + fFriction;
            GetComponent<PlayerSE>().SoundAccelerator();
            if (fSpeed <= 40.0f) //40�L���ȉ��Ȃ�{�ŉ���
            {
                fSpeed += fAccelerationf;
            }
        
        {//�A�N�Z���������Ă��Ȃ��Ƃ�
            GetComponent<PlayerSE>().SoundStopAccelerator();
        }
    }

    void BendRight()
    {
        float temp = fSpeed / MaxSpeed * MaxBendAngle;  //�n���h���̊p�x
        Vector3 vector3 = Vector3.Normalize(-v3Front);

        vector3 = vector3 * (trans.localScale.z / 2);
        vector3 += trans.position;
        trans.RotateAround(vector3, Vector3.up, MaxBendAngle);

        _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.RIGHT);

        bBend = true;
    }

    void BendLeft()
    {
        float temp = -fSpeed / MaxSpeed * MaxBendAngle;  //�n���h���̊p�x
        Vector3 vector3 = Vector3.Normalize(-v3Front);

        vector3 = vector3 * (trans.localScale.z / 2);
        vector3 += trans.position;
        trans.RotateAround(vector3, Vector3.up, -MaxBendAngle);

        _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.LEFT);

        bBend = true;
    }

    void DriftRight()
    {
        fSpeed -= fDriftDecelerat;
        GetComponent<PlayerSE>().SoundDrift();

        if (fSpeed >= 5.0f)
        {
        
            Vector3 vector3 = Vector3.Normalize(-v3Front);
            vector3 = vector3 * (trans.localScale.z / 2);
            vector3 += trans.position;
            trans.RotateAround(vector3, Vector3.up, -MaxDriftBend + MaxBendAngle);
            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.LEFT_DRIFT);

            
        }
        bDrift = true;
    }

    void DriftLeft()
    {
        fSpeed -= fDriftDecelerat;
        GetComponent<PlayerSE>().SoundDrift();

        if (fSpeed >= 5.0f)
        {
            Vector3 vector3 = Vector3.Normalize(-v3Front);
            vector3 = vector3 * (trans.localScale.z / 2);
            vector3 += trans.position;
            trans.RotateAround(vector3, Vector3.up, MaxDriftBend - MaxBendAngle);
            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.RIGHT_DRIHT);
         
        }
        bDrift = true;
    }
}
