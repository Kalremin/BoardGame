using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OptionData
{
    const string volumeBgmStr = "VolumeBGM";
    const string volumeEffectStr = "VolumeBGM";
    const string resolutionStr = "VolumeBGM";

    //static float volumeBgm = 1f;
    //static float volumeEffect = 1f;
    //static int resolutionValue = 0;


    public static void SetResolution(int value)
    {
        PlayerPrefs.SetInt(resolutionStr, value);
        ResolutionManager._instance.ChangeResolution(value);
    }

    public static int GetResolution()
    {
        return PlayerPrefs.GetInt(resolutionStr, 5);
    }

    public static void SetVolumeBGM(float volume)
    {
        PlayerPrefs.SetFloat(volumeBgmStr, volume);
        //volumeBgm = volume;
    }

    public static float GetVolumeBGM()
    {
        return PlayerPrefs.GetFloat(volumeBgmStr, 1f); //return volumeBgm;
    }

    public static void SetVolumeEffect(float volume)
    {
        PlayerPrefs.SetFloat(volumeEffectStr, volume);
        //volumeEffect = volume;
    }

    public static float GetVolumeEffect()
    {
        return PlayerPrefs.GetFloat(volumeEffectStr, 1f); //return volumeEffect;
    }
}
