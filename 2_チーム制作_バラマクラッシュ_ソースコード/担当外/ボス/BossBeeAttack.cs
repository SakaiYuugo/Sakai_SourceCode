using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BossBeeState))]
public class BossBeeAttack : MonoBehaviour
{
    [Tooltip("攻撃アニメーション終了フレーム"), SerializeField] int AnimAtkFinishFrame = 300;
    [Tooltip("叩きつけ攻撃の衝撃波を出すオブジェクト"), SerializeField] GameObject bossTail;

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
                // モデルの動き開始 一度だけ実行
                if (frame <= 0)
                {
                    bossAnim.StartLaserAnimation();
                }

                frame++;
                // ビーム攻撃
                if(frame >= AnimAtkFinishFrame)
                    laser.Attack();

                // 攻撃が終了したら
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
                // モデルの動き開始 一度だけ実行
                if (frame <= 0)
                {
					// AoEエフェクト生成
					AoEShockWaveEfk.AoEShockWave();

                    audioSource.PlayOneShot(bossState.GetAudio(1));
                    //WaveAudio.Play();
					bossAnim.StartWaveAnim();
                    TailCollision.GSTailState = BeeTailCollision.TailState.Start;
				}
                frame++;
                // 攻撃が終了したら
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
                // モデルの動き開始 一度だけ実行
                if (frame <= 0)
                {
                    audioSource.PlayOneShot(bossState.GetAudio(3));
                    //BlowAudio.Play();
                    bossAnim.StartBlowAnim();
                }
                
                frame++;
                // 衝撃波攻撃
                if (frame >= AnimAtkFinishFrame)
                {
                    audioSource.Stop();
                    blow.Attack();
                }
                    

                // 攻撃が終了したら
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
