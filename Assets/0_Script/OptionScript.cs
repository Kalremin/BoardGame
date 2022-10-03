using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionScript : MonoBehaviour
{
    [SerializeField] Slider _background;
    [SerializeField] Slider _effect;
    [SerializeField] Dropdown _resolution;

    void Start()
    {
        _background.value = OptionData.GetVolumeBGM();
        _effect.value = OptionData.GetVolumeEffect();
        _resolution.value = OptionData.GetResolution();


        _background.onValueChanged.AddListener(SetBGMVolumeSlider);
        _effect.onValueChanged.AddListener(SetEffectVolumeSlider);
        _resolution.onValueChanged.AddListener(SetSolutionValue);
    }

    private void SetSolutionValue(int arg0)
    {
        OptionData.SetResolution(arg0);
    }

    void SetBGMVolumeSlider(float volume)
    {
        SoundManager._instance.ChangeMusicVolume(volume);
        LogClass.LogWarn(volume);
    }

    void SetEffectVolumeSlider(float volume)
    {
        SoundManager._instance.ChangeEffectVolume(volume);
    }



    public void ClickBackBtn()
    {
        Destroy(gameObject);
    }
}
