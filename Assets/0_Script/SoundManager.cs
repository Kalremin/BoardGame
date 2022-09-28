﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eEffectSound
{
    Hit,
    Magic,
    Summon,
    Buff,
    Debuff
}

public enum eBackgroundSound
{
    Title,
    Map,
    Battle_Normal,
    Victory,
    Defeat
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;

    [SerializeField] 
    AudioSource _playerSound;
    [SerializeField]
    AudioClip[] _effectSound, _backgroundSound;

    //float _effectSoundVol = 1f,_backgroundSoundVol = 1f;

    //public float GetEffectVol() => _effectSoundVol;
    //public float GetBgmVol() => _backgroundSoundVol;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _playerSound.minDistance = 0;
        _playerSound.maxDistance = 1;

        _playerSound.clip = _backgroundSound[0];
        _playerSound.volume = OptionData.GetVolumeBGM();
        _playerSound.Play();
    }

    public void PlayEffectSound(eEffectSound effectSound, Transform parentTrans, bool isLooping = false)
    {
        GameObject go = new GameObject(effectSound.ToString());
        go.transform.SetParent(parentTrans);
        
        AudioSource soundPlayer = go.AddComponent<AudioSource>();
        soundPlayer.clip = _effectSound[(int)effectSound];
        soundPlayer.minDistance = 0;
        soundPlayer.maxDistance = 1;
        soundPlayer.volume = OptionData.GetVolumeEffect();
        soundPlayer.loop = isLooping;
        soundPlayer.Play();

        Destroy(go, 2f);
    }

    public void PlayBackgroundSound(eBackgroundSound backgroundSound, bool isLooping = true)
    {
        _playerSound.clip = _backgroundSound[(int)backgroundSound];
        _playerSound.volume = OptionData.GetVolumeBGM();
        _playerSound.loop = isLooping;
        _playerSound.Play();
    }

    public void ChangeMusicVolume(float volume)
    {
        //_backgroundSoundVol = volume;
        OptionData.SetVolumeBGM(volume);
        _playerSound.volume = volume;
    }

    public void ChangeEffectVolume(float volume)
    {
        OptionData.SetVolumeEffect(volume);
        //_effectSoundVol = volume;
    }

    //public void ChangeVolume(bool bgm, float volume)
    //{
    //    if (bgm)
    //    {
    //        _playerSound.volume = volume;
    //        _backgroundSoundVol = volume;
    //    }
    //    else
    //    {
    //        _effectSoundVol = volume;

    //    }
    //}

    
}