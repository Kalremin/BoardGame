using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlManager : MonoBehaviour
{
    public static SceneControlManager _instance;

    EnumList.eScence _sceneState;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeScene(EnumList.eScence.TitleScene);
    }

    public void ChangeScene(EnumList.eScence scene)
    {
        _sceneState = scene;
        switch (_sceneState)
        {
            case EnumList.eScence.TitleScene:
                SoundManager._instance.PlayBackgroundSound(eBackgroundSound.Title);
                break;
            case EnumList.eScence.MapScene:
                SoundManager._instance.PlayBackgroundSound(eBackgroundSound.Map);
                break;
            case EnumList.eScence.BattleScene:
                SoundManager._instance.PlayBackgroundSound(eBackgroundSound.Battle_Normal);
                break;
        }
        SceneManager.LoadScene((int)scene);
    }
}
