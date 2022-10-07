using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 배틀씬의 UI 스크립트
public class GUIScript : Singleton<GUIScript>
{

    [SerializeField]
    GameObject _menuActBtns, _summonWnd, _magicWnd, _resultWnd;

    GameObject _pauseWnd;

    public bool ExistPauseWnd() => _pauseWnd != null;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _pauseWnd == null)
        {
            OpenWnd(EnumList.eUIWnd.PauseWnd);
        }
    }

    // UI창 활성화
    public void OpenWnd(EnumList.eUIWnd uIWnd)
    {
        switch (uIWnd)
        {
            case EnumList.eUIWnd.MenuActBtns:
                if (_menuActBtns.activeSelf)
                    return;
                Vector3 pos = Input.mousePosition;

                if (pos.x < 0)
                    pos = new Vector3(0 , pos.y, pos.z);

                if (pos.x > Screen.width - _menuActBtns.GetComponent<RectTransform>().rect.width)
                    pos = new Vector3(Screen.width - _menuActBtns.GetComponent<RectTransform>().rect.width, pos.y, pos.z);

                if (pos.y < _menuActBtns.GetComponent<RectTransform>().rect.height)
                    pos = new Vector3(pos.x, _menuActBtns.GetComponent<RectTransform>().rect.height, pos.z);

                if (pos.y > Screen.height)
                    pos = new Vector3(pos.x, Screen.height, pos.z);

                _menuActBtns.SetActive(true);
                _menuActBtns.GetComponent<RectTransform>().position = pos;
                break;

            case EnumList.eUIWnd.MagicWnd:
                if (BattleManager._instance.IsPlaySummoner())
                    _magicWnd.SetActive(true);
                else
                    BattleManager._instance.ChangeState(eBattleState.MagicAttack);
                break;

            case EnumList.eUIWnd.SummonWnd:
                _summonWnd.SetActive(true);
                break;

            case EnumList.eUIWnd.ResultWnd:
                _resultWnd.SetActive(true);
                break;

            case EnumList.eUIWnd.PauseWnd:
                _pauseWnd = Instantiate(Resources.Load("PauseWnd") as GameObject, transform);
                break;
        }
    }

    // UI창 비활성화
    public void CloseWnd(EnumList.eUIWnd uIWnd)
    {
        switch (uIWnd)
        {
            case EnumList.eUIWnd.MenuActBtns:
                _menuActBtns.SetActive(false);
                break;
            case EnumList.eUIWnd.SummonWnd:
                _summonWnd.SetActive(false);
                break;
            case EnumList.eUIWnd.MagicWnd:
                _magicWnd.SetActive(false);
                break;
            case EnumList.eUIWnd.ResultWnd:
                _resultWnd.SetActive(false);
                break;
        }
    }


}
