using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : NoOverlapSingleton<ResolutionManager>
{
    //public static ResolutionManager _instance;

    int[] resWidth = { 1024, 1280, 1280, 1360, 1400, 1600 };
    int[] resHeight = { 768, 720, 960, 768, 1050, 900 };

    //private void Awake()
    //{
    //    if(_instance != null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    _instance = this;
    //    DontDestroyOnLoad(gameObject);
    //}

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(resWidth[OptionData.GetResolution()], resHeight[OptionData.GetResolution()], false);
    }


    public void ChangeResolution(int value)
    {
        Screen.SetResolution(resWidth[value], resHeight[value], false);
    }
}
