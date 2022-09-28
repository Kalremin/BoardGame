using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPref : MonoBehaviour
{
    [SerializeField] Image _teamBG, _unitImg;

    [SerializeField] Sprite[] _monsterSprite;
    [SerializeField] Sprite[] _summonerSprite;

    int _turnTime = 0;
    int _idx = -1;
    public int _Turn => _turnTime;
    public void SetTurnTime(int time) => _turnTime = time;

    public void SetIdx(int unitIdx) => _idx = unitIdx;

    public int GetIdx() => _idx;

    public void SetImage(int time, bool playerTeam, EnumList.eKindMonster kindMonster)
    {
        _turnTime = time;

        if (playerTeam)
        {
            _teamBG.color = Color.blue;
        }
        else
            _teamBG.color = Color.red;

        _unitImg.sprite = _monsterSprite[(int)kindMonster];
    }

    public void SetImage(int time, bool playerTeam, EnumList.eKindSummoner kindSummoner)
    {
        _turnTime = time;

        if (playerTeam)
        {
            _teamBG.color = Color.blue;
        }
        else
            _teamBG.color = Color.red;

        _unitImg.sprite = _summonerSprite[(int)kindSummoner];
    }
}
