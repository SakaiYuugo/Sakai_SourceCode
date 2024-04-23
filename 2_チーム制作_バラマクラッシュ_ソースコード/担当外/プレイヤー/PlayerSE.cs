using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSE : MonoBehaviour
{
   [SerializeField] AudioSource _AudioSource;
   [SerializeField] AudioSource _asAccelerator;
   [SerializeField] AudioSource _asDrift;

    public AudioClip acDamage;

    PlayerMove _PlayerMove;

    void Start()
    {
        _PlayerMove = GetComponent<PlayerMove>();
    }

    public void FixedUpdate()
    {

    }

    public void SoundDamage()
    {   
        _AudioSource.PlayOneShot(acDamage);
    }

    public void SoundDrift()
    {
        if (_asDrift.time >= 0.4f)  //�w�莞�Ԉȏ�Đ����Ă���Ȃ�
        {
            _asDrift.Stop();
            _asDrift.time = 0.05f;
            _asDrift.Play();
            return;
        }

        if (_asDrift.isPlaying) return; //�w�莞�Ԉȉ��ōĐ�����Ă���Ȃ烊�^�[��
        _asDrift.time = 0.0f;   //�O�t���[���Đ�����Ă��Ȃ��Ȃ�
        _asDrift.Play();
    }

    public void SoundStopDrift() { _asDrift.Stop(); }
    
    public void SoundAccelerator()
    {
        if(_asAccelerator.time >= 0.8f) //�w�莞�Ԉȏ�Đ����Ă���Ȃ�
        {
            _asAccelerator.Stop();
            _asAccelerator.time = 0.05f;
            _asAccelerator.Play();
            return;
        }

        if (_asAccelerator.isPlaying) return;   //�w�莞�Ԉȉ��ōĐ�����Ă���Ȃ烊�^�[��

        if (_PlayerMove.GSNowSpeed >= 1.0f) //�O�t���[���Đ�����Ă��Ȃ��Ȃ�
        {
            _asDrift.time = 0.0f;
            _asAccelerator.Play();
        }
    }

    public void SoundStopAccelerator() { _asAccelerator.Stop(); }
}
