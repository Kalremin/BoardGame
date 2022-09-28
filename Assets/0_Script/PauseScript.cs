using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField] GameObject _optionWnd;

    public void ClickContinueBtn()
    {
        Destroy(gameObject);
    }

    public void ClickOptionBtn()
    {
        Instantiate(_optionWnd, transform.parent);
    }

    public void ClickTitleBtn()
    {
        Destroy(MapManager._instance.gameObject);
        Destroy(PlayerData._instance.gameObject);
        Destroy(ShopStuff._instance.gameObject);
        SceneControlManager._instance.ChangeScene(EnumList.eScence.TitleScene);
    }

    public void ClickQuitBtn()
    {
        Application.Quit();
    }
}
