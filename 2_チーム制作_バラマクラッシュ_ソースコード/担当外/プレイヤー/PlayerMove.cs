using System;
using System.Collections;
using UnityEngine;
//aaa

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Transform trans;
    PlayerState _PlayerState;
    PlayerSlopeRotate _PlayerSlopeRotate;
    PlayerRay playerRay;

    public bool bBend; //�J�[�u��

    [NonSerialized] Vector3 v3Front;    //�v���C���[�̐��ʕ���
    [SerializeField, Tooltip("���o�Ă��鑬�x"), ReadOnly] float fSpeed;   //�ړ����x
    [SerializeField, Tooltip("������(1f)")] float fAccelerationf;   //�����x
    [SerializeField, Tooltip("�����u���[�L��(1f)")] float fFriction;   //�����u���[�L
    [SerializeField, Tooltip("�ő呬�x")] float MaxSpeed;    //�ő呬�x
    [SerializeField, Tooltip("�J�[�u���ő呬�x")] float MaxBendSpeed;  //�J�[�u���̍ő呬�x
    [SerializeField, Tooltip("�Ȃ���p�x(1F)")] float MaxBendAngle; //���ʂɋȂ���p�x
    //�Ȃ��蔽�]���΍�̃A�V�X�g�p
    float ASMaxBendAngle; //���ʂɋȂ���p�x
    float ASMaxSpeed;    //�ő呬�x

    //---- �h���t�g ----
    [SerializeField, Tooltip("�X���p�x")] float MaxDriftRange = 50.0f;  //�h���t�g�p�x�ȂȂ߂ɂȂ��
    [SerializeField, Tooltip("�h���t�g���̋Ȃ���p�x")] float MaxDriftBend;  //�h���t�g�p�x�Ȃ���p�x�p(�����|��̂Ŏg���Ƃ���MaxBendAngle������)
    [SerializeField, Tooltip("�h���t�g���̌�����(1F)")] float fDriftDecelerat; //�h���t�g�����l
    
    //�_�b�V��
    [SerializeField, Tooltip("�_�b�V��������")] float fDashAddSpped = 20.0f;
    [SerializeField, Tooltip("�����A�C�e�����ʎ��ԁi�b�j")] float fDashAddTime;
    [SerializeField, Tooltip("�_�b�V������"), ReadOnly] bool bDash = false; //�_�b�V���֐�����񂾂��Ăԗp
    Coroutine _DashCor; //�_�b�V���R���[�`���ۑ�

    //�f�o�t
    Coroutine _SlowCor; //�����R���[�`���ۑ�

    //�������
    Coroutine _BlowCor;    //������ѕ��A�J�E���g�R���[�`���ۑ�
    bool bNoMove;
    bool bBlowChack;    //������΂���Ă���R���[�`���I�����true

    //�A�V�X�g�V�X�e��
    AssistOperat _AssistOperat;
    public enum ASSIST
    {
        m_STAND,
        m_RIGHT,
        m_LEFT
    };
    ASSIST assist = ASSIST.m_STAND;  //���͂����邩�A���̃C�[�i���ɂ���č��E�ɓ���
    [SerializeField,Tooltip("�A�V�X�g�@�\���g�����ۂ�")] bool bAssist;

    //---- GeterSeter ----
    public float GSNowSpeed { get { return fSpeed; } }
    public float GMaxSpeed { get { return MaxSpeed; } }
    public float GSfAccelerationf { get { return fAccelerationf; } }
    public float GSfFriction { get { return fFriction; } }
    public float GDashAddSpped { get { return fDashAddSpped; } }

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // rigidbody���擾
        trans = GetComponent<Transform>();  //Transform���擾  
        _PlayerState = GameObject.Find("Player").GetComponent<PlayerState>();
        _PlayerSlopeRotate = GameObject.Find("SlopeRotate").GetComponent<PlayerSlopeRotate>();
        playerRay = GameObject.Find("RayPointFront").GetComponent<PlayerRay>();
        _AssistOperat = GetComponent<AssistOperat>();

        ASMaxBendAngle = MaxBendAngle;
        ASMaxSpeed = MaxSpeed;
    }

    private void FixedUpdate()
    {
        //�A�V�X�g�@�\���I���Ȃ�
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("�g���K�[�ł����ǁH");
            bAssist = !bAssist;
            assist = ASSIST.m_STAND;
        }

        if (bAssist)
        {
            assist = _AssistOperat.AssistCheck();    //�A�V�X�g���邩�`�F�b�N
        }


        //������΂��ꂽ���̒n�ʂɖ߂������̃t���O
        if (bBlowChack)
        {
            if (playerRay.GetRay(3.0f, "Ground") || playerRay.GetRay(3.0f, "Ruin") || playerRay.GetRay(3.0f,"StageObject"))
            {   //�n�ʁA��ՁA��Q���Ƀ��C������������
                bBlowChack = false;
                bNoMove = false; //������悤�ɂ���
            }
        }
        //�_�b�V���A�C�e��
        if (_PlayerState.state.m_DASH)
        {
            if (!bDash)
            {//�_�b�V�����ł͂Ȃ����Ɏ����
                _DashCor = StartCoroutine(DashCoroutine());
            }
            else
            {  //�_�b�V�����Ɏ����
                StopCoroutine(_DashCor);    //�R���[�`���I��
                MaxSpeed -= fDashAddSpped;
                MaxBendSpeed -= fDashAddSpped;
                _DashCor = StartCoroutine(DashCoroutine());
            }
        }


        //---- ���ʂ��čs����ړ����� ----�i�s��Ȃ��p�^�[�����o�Ă����ꍇ��return�\��j
        if (bNoMove) return;    //bNoMove�Ȃ瓮���Ȃ�
        v3Front = -trans.forward;  //���ʂ�ύX

        Accelerator();  //�A�N�Z���I�I

        if (fSpeed < 0)    //���x���}�C�i�X�Ȃ��~��Ԃɂ���
        {
            fSpeed = 0.0f;
        }

        bBend = false;  //�J�[�u���t���O��߂�
        //Lerp�ESlerp�֗�
        if (InputOrder.D_Key() || assist == ASSIST.m_RIGHT) //�E��
        {
            float temp = fSpeed / MaxSpeed * MaxBendAngle;  //�n���h���̊p�x
            Vector3 vector3 = Vector3.Normalize(-v3Front);

            vector3 = vector3 * (trans.localScale.z / 2);
            vector3 += trans.position;
            trans.RotateAround(vector3, Vector3.up, MaxBendAngle);

            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.RIGHT);


            bBend = true;
        }

        if (InputOrder.A_Key() || assist == ASSIST.m_LEFT)//����
        {
            float temp = -fSpeed / MaxSpeed * MaxBendAngle;  //�n���h���̊p�x
            Vector3 vector3 = Vector3.Normalize(-v3Front);

            vector3 = vector3 * (trans.localScale.z / 2);
            vector3 += trans.position;
            trans.RotateAround(vector3, Vector3.up, -MaxBendAngle);

            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.LEFT);


            bBend = true;
        }

        //Assist();

        if (InputOrder.SHIFT_Key())
        {
            fSpeed -= fDriftDecelerat;
            GetComponent<PlayerSE>().SoundDrift();

            if (fSpeed >= 5.0f)
            {
                if (InputOrder.A_Key())  //����
                {
                    Vector3 vector3 = Vector3.Normalize(-v3Front);
                    vector3 = vector3 * (trans.localScale.z / 2);
                    vector3 += trans.position;
                    trans.RotateAround(vector3, Vector3.up, -MaxDriftBend + MaxBendAngle);
                    _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.LEFT_DRIFT);
                    _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.LEFT_DRIFT);

                }

                if (InputOrder.D_Key()) //�E��
                {
                    Vector3 vector3 = Vector3.Normalize(-v3Front);
                    vector3 = vector3 * (trans.localScale.z / 2);
                    vector3 += trans.position;
                    trans.RotateAround(vector3, Vector3.up, MaxDriftBend - MaxBendAngle);
                _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.RIGHT_DRIHT);
                _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.RIGHT_DRIHT);
                }
            }
        }
        else
        {
            GetComponent<PlayerSE>().SoundStopDrift();
        }

        if (!bBend)
        {
            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.CENTER);
        }

        float tempY = rb.velocity.y;    //Y���W�͉������Ȃ�
        rb.velocity = new Vector3(v3Front.x * fSpeed, tempY, v3Front.z * fSpeed); //���ۂɉ���



        ///////////////////////////////////////////////////////

    }

    //---- �A�N�Z�� ----
    private void Accelerator()
    {
        fSpeed -= fFriction;    //�A�N�Z�����Ă��Ȃ����u���[�L�ɂȂ�

        if (bBend && fSpeed >= MaxBendSpeed) return;    //�J�[�u���A�J�[�u���x���o�Ă���Ȃ�����ł��Ȃ�

        if (fSpeed >= MaxSpeed) return;     //MaxSpeed���o�Ă���Ȃ�

        if (InputOrder.W_Key())
        {
            fSpeed += fAccelerationf + fFriction;
            GetComponent<PlayerSE>().SoundAccelerator();
            if (fSpeed <= 40.0f) //40�L���ȉ��Ȃ�{�ŉ���
            {
                fSpeed += fAccelerationf;
            }
        }
        else
        {//�A�N�Z���������Ă��Ȃ��Ƃ�
            GetComponent<PlayerSE>().SoundStopAccelerator();
        }
    }

    void Assist()
    {   //�A�V�X�g�����삷��ꍇ��
        if (assist == ASSIST.m_RIGHT) //�E��
        {
            float temp = fSpeed / ASMaxSpeed * ASMaxBendAngle;  //�n���h���̊p�x
            Vector3 vector3 = Vector3.Normalize(-v3Front);

            vector3 = vector3 * (trans.localScale.z / 2);
            vector3 += trans.position;
            trans.RotateAround(vector3, Vector3.up, ASMaxBendAngle);

            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.RIGHT);


            bBend = true;
        }

        if (assist == ASSIST.m_LEFT)//����
        {
            float temp = -fSpeed / ASMaxSpeed * ASMaxBendAngle;  //�n���h���̊p�x
            Vector3 vector3 = Vector3.Normalize(-v3Front);

            vector3 = vector3 * (trans.localScale.z / 2);
            vector3 += trans.position;
            trans.RotateAround(vector3, Vector3.up, -ASMaxBendAngle );

            _PlayerSlopeRotate.Rotate(PlayerSlopeRotate.DIRECTION.LEFT);


            bBend = true;
        }
    }

    //---- ���� ----
    public void SlowDown(float speed, float time)
    {
        if (!_PlayerState.state.m_SPIDER_THREAD)
        {   //���߂�
            _SlowCor = StartCoroutine(SlowDownCoroutine(speed, time));

        }
        else
        {//���ʎ��ԃ��Z�b�g
            StopCoroutine(_SlowCor);    //�R���[�`���I��
            MaxSpeed += speed;
            MaxBendSpeed += speed;
            _SlowCor = StartCoroutine(SlowDownCoroutine(speed, time));
        }
    }

    //�������
    public void BlowAway(Vector3 pos, float power, float upPower = 600.0f)
    {
        Vector3 vector = trans.position - pos;
        vector = vector.normalized;
        vector = vector * power;
        rb.velocity = Vector3.zero;
        vector = new Vector3(vector.x, upPower, vector.z);
        rb.AddForce(vector, ForceMode.Impulse);
        _BlowCor = StartCoroutine(BlowCoroutine());
    }

    //������ь�̎��ԑ���R���[�`��
    IEnumerator BlowCoroutine()
    {
        bNoMove = true;
        yield return new WaitForSeconds(0.5f);
        bBlowChack = true;
    }

    //�_�b�V���p�R���[�`��
    IEnumerator DashCoroutine()
    {
        _PlayerState.state.DashEnd();
        bDash = true;
        MaxSpeed += fDashAddSpped;
        MaxBendSpeed += fDashAddSpped;
        fSpeed += fDashAddSpped;
        yield return new WaitForSeconds(fDashAddTime);
        MaxSpeed -= fDashAddSpped;
        MaxBendSpeed -= fDashAddSpped;
        bDash = false;
    }

    //�����f�o�t�p�R���[�`��
    IEnumerator SlowDownCoroutine(float speed, float time)
    {
        _PlayerState.state.m_SPIDER_THREAD = true;
        MaxSpeed -= speed;
        MaxBendSpeed -= speed;
        yield return new WaitForSeconds(time);
        _PlayerState.state.m_SPIDER_THREAD = false;
        MaxSpeed += speed;
        MaxBendSpeed += speed;
    }
}
