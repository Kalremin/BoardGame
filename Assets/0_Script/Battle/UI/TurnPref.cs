using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 순서리스트 UI의 유닛 순서 오브젝트
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

    // 몬스터
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

    // 소환사
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
