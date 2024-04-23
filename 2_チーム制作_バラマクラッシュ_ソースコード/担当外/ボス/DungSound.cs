using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungSound : MonoBehaviour
{
    //----- ���֌W -----
    //�d�Ȃ�Ȃ������Đ�����(walk,impact,upBall,stan,kick,death)
    [SerializeField] AudioSource _AudioSource;  

    //�I�[�f�B�I�N���b�v
    public AudioClip acWalk;    //����
    public AudioClip acRollStar;    //���[���X�^�[
    public AudioClip acImpact;  //�Ռ��g
    public AudioClip acUpBall;   //��ɋ����R��
    public AudioClip acStan;    //�X�^��
    public AudioClip acDeath;   //�|���ꂽ
    public AudioClip acBallBreak;   //�{�[�����󂳂ꂽ

    public enum DUNG_SOUND
    {
        SOUND_WALK,
        SOUND_ROLLSTAR,
        SOUND_IMPACT,
        SOUND_UPBALL,
        SOUND_STAN,
        SOUND_DEATH,
        SOUND_BALLBREAK,
    };

    public void Sound(DUNG_SOUND sound)
    {
        switch (sound)
        {
            case DUNG_SOUND.SOUND_WALK:
                if (!_AudioSource.isPlaying)
                {
                    _AudioSource.PlayOneShot(acWalk);
                }

                break;
            case DUNG_SOUND.SOUND_ROLLSTAR:
                _AudioSource.PlayOneShot(acRollStar);

                break;
            case DUNG_SOUND.SOUND_IMPACT:
                _AudioSource.PlayOneShot(acImpact);

                break;
            case DUNG_SOUND.SOUND_UPBALL:
                _AudioSource.PlayOneShot(acUpBall);

                break;
            case DUNG_SOUND.SOUND_STAN:
                _AudioSource.PlayOneShot(acStan);

                break;
            case DUNG_SOUND.SOUND_DEATH:
                _AudioSource.PlayOneShot(acDeath);

                break;
            case DUNG_SOUND.SOUND_BALLBREAK:
                _AudioSource.PlayOneShot(acBallBreak);

                break;
        }

    }
}
