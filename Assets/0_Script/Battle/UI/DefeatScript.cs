using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeatScript : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(MapManager._instance.gameObject);
        Destroy(PlayerData._instance.gameObject);
        Destroy(ShopStuff._instance.gameObject);
        SoundManager._instance.PlayBackgroundSound(eBackgroundSound.Defeat);
        Invoke("NextMethod", 3);
    }

    void NextMethod()
    {
        SceneControlManager._instance.ChangeScene(EnumList.eScence.TitleScene);
    }
}
