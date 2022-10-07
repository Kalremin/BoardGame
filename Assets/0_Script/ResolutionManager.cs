using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 해상도 조절
public class ResolutionManager : NoOverlapSingleton<ResolutionManager>
{
    int[] resWidth = { 1024, 1280, 1280, 1360, 1400, 1600 };
    int[] resHeight = { 768, 720, 960, 768, 1050, 900 };

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
