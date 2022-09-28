using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_ActLog : MonoBehaviour
{

    public static GUI_ActLog _instance;

    [SerializeField]
    GameObject _logContent, _logItem;

    [SerializeField]
    int _logCount = 7;

    private void Awake()
    {
        _instance = this;
    }

    void AddLog(string logText)
    {
        if (_logContent.transform.childCount < _logCount)
        {
            GameObject temp = Instantiate(_logItem);
            temp.GetComponentInChildren<Text>().text = logText;
            temp.transform.SetParent( _logContent.transform);

        }
        else
        {
            GameObject temp = _logContent.transform.GetChild(0).gameObject;
            temp.transform.SetParent( null);
            temp.GetComponentInChildren<Text>().text = logText;
            temp.transform.SetParent( _logContent.transform);
        }

    }

    public void WaitLog(Unit playUnit)
    {
        AddLog(string.Format("{0} {1} 대기", (playUnit._PlayerTeam ? "아군" : "적군"), playUnit.name));
    }

    public void AttackLog(Unit attackUnit, Unit damagedUnit)
    {
        AddLog(string.Format("{0} {1}, {2} {3}에게 공격", (attackUnit._PlayerTeam ? "아군" : "적군"), attackUnit.name, (damagedUnit._PlayerTeam ? "아군" : "적군"), damagedUnit.name));
    }

    public void MagicAttackLog(Unit attackUnit, Unit damagedUnit)
    {
        AddLog(string.Format("{0} {1}, {2} {3}에게 마법 공격", (attackUnit._PlayerTeam ? "아군" : "적군"), attackUnit.name, (damagedUnit._PlayerTeam ? "아군" : "적군"), damagedUnit.name));
    }

    public void MagicAssistLog(Unit attackUnit, Unit damagedUnit)
    {
        AddLog(string.Format("{0} {1}, {2} {3}에게 마법 발동", (attackUnit._PlayerTeam ? "아군" : "적군"), attackUnit.name, (damagedUnit._PlayerTeam ? "아군" : "적군"), damagedUnit.name));
    }

    public void SummonLog(Unit summonerUnit, Unit monsterUnit)
    {
        AddLog(string.Format("{0} {1}, {2} {3}를 소환", (summonerUnit._PlayerTeam ? "아군" : "적군"), summonerUnit.name, (monsterUnit._PlayerTeam ? "아군" : "적군"), monsterUnit.name));
    }

    public void DeathLog(Unit deadUnit)
    {
        AddLog(string.Format("{0} {1} 죽음", (deadUnit._PlayerTeam ? "아군" : "적군"), deadUnit.name));
    }

}
