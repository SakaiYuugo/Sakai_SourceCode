using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BossBeeState))]
public class BossBeeAttack : MonoBehaviour
{
    [Tooltip("�U���A�j���[�V�����I���t���[��"), SerializeField] int AnimAtkFinishFrame = 300;
    [Tooltip("�@�����U���̏Ռ��g���o���I�u�W�F�N�g"), SerializeField] GameObject bossTail;

    private EffectSpawn Effectspawn;
    private BossBeeState bossState;
    private LaserAttack laser;
    private BlowAttack blow;
    private ShockWaveAttack wave;
    private BossBeeAnimation bossAnim;
    private BeeTailCollision TailCollision;

	private AoE_ShockWave AoEShockWaveEfk;
    //private AudioSource WaveAudio;
    //private AudioSource BlowAudio;
    private AudioSource audioSource;

	public enum AttakType
    {
        None,
        Lazer,
        Wave,
        Brow
    }

    private int frame;
    [System.NonSerialized]  public  AttakType attakType = AttakType.None;
    public int GetAnimAtkFinishFrame { get { return AnimAtkFinishFrame; } }
    public AttakType GetAtkType { get { return attakType; }}
    // Start is called before the first frame update
    void Start()
    {
        frame = 0;
        Effectspawn = this.GetComponent<EffectSpawn>();
        bossState = this.GetComponent<BossBeeState>();
        laser = this.GetComponent<LaserAttack>();
        blow = this.GetComponent<BlowAttack>();
        wave = this.GetComponent<ShockWaveAttack>();
        bossAnim = this.GetComponent<BossBeeAnimation>();
        TailCollision = bossTail.GetComponent<BeeTailCollision>();

		// AoEShockWave
		AoEShockWaveEfk = gameObject.GetComponent<AoE_ShockWave>();
        audioSource = gameObject.GetComponent<AudioSource>();

        //WaveAudio = bossState.GetAudio(1);
        //BlowAudio = bossState.GetAudio(3);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
       
    }

    public void Attack(int i)
    {
        
        switch (i)
        {
            case 1:
                attakType = AttakType.Lazer;
                // ���f���̓����J�n ��x�������s
                if (frame <= 0)
                {
                    bossAnim.StartLaserAnimation();
                }

                frame++;
                // �r�[���U��
                if(frame >= AnimAtkFinishFrame)
                    laser.Attack();

                // �U�����I��������
                if (laser.bFinishFlg)
                {
                    attakType = AttakType.None;
                    bossAnim.StopLaserAnimation();
                    bossState.SetStateBoss(BossBeeState.State.Move);
                    frame = 0;
                    laser.bFinishFlg = false;
                }

                break;
            case 2:
                attakType = AttakType.Wave;
                // ���f���̓����J�n ��x�������s
                if (frame <= 0)
                {
					// AoE�G�t�F�N�g����
					AoEShockWaveEfk.AoEShockWave();

                    audioSource.PlayOneShot(bossState.GetAudio(1));
                    //WaveAudio.Play();
					bossAnim.StartWaveAnim();
                    TailCollision.GSTailState = BeeTailCollision.TailState.Start;
				}
                frame++;
                // �U�����I��������
                if (wave.bFinishFlg)
                {
                    attakType = AttakType.None;
                    bossAnim.StopWaveAnim();
                    bossState.SetStateBoss(BossBeeState.State.Move);
                    frame = 0;
                    TailCollision.GSTailState = BeeTailCollision.TailState.None;
                    wave.bFinishFlg = false;
                }
                break;
            case 3:
                attakType = AttakType.Brow;
                // ���f���̓����J�n ��x�������s
                if (frame <= 0)
                {
                    audioSource.PlayOneShot(bossState.GetAudio(3));
                    //BlowAudio.Play();
                    bossAnim.StartBlowAnim();
                }
                
                frame++;
                // �Ռ��g�U��
                if (frame >= AnimAtkFinishFrame)
                {
                    audioSource.Stop();
                    blow.Attack();
                }
                    

                // �U�����I��������
                if (blow.bFinishFlg)
                {
                    attakType = AttakType.None;
                    bossAnim.StopBlowAnim();
                    bossState.SetStateBoss(BossBeeState.State.Move);
                    frame = 0;
                    blow.bFinishFlg = false;
                }
                break;
            default:
                break;
        }
        
    }
}
