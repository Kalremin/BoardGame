using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 게임 씬의 전환
public class SceneControlManager : Singleton<SceneControlManager>
{
    EnumList.eScence _sceneState;

    protected new void Awake()
    {
        //_instance = this;
        base.Awake();
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
