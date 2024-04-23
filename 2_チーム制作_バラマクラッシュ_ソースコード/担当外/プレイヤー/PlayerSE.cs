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
        if (_asDrift.time >= 0.4f)  //指定時間以上再生しているなら
        {
            _asDrift.Stop();
            _asDrift.time = 0.05f;
            _asDrift.Play();
            return;
        }

        if (_asDrift.isPlaying) return; //指定時間以下で再生されているならリターン
        _asDrift.time = 0.0f;   //前フレーム再生されていないなら
        _asDrift.Play();
    }

    public void SoundStopDrift() { _asDrift.Stop(); }
    
    public void SoundAccelerator()
    {
        if(_asAccelerator.time >= 0.8f) //指定時間以上再生しているなら
        {
            _asAccelerator.Stop();
            _asAccelerator.time = 0.05f;
            _asAccelerator.Play();
            return;
        }

        if (_asAccelerator.isPlaying) return;   //指定時間以下で再生されているならリターン

        if (_PlayerMove.GSNowSpeed >= 1.0f) //前フレーム再生されていないなら
        {
            _asDrift.time = 0.0f;
            _asAccelerator.Play();
        }
    }

    public void SoundStopAccelerator() { _asAccelerator.Stop(); }
}
