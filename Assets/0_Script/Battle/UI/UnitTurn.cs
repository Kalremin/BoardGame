using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// 유닛 리스트 UI창
public class UnitTurn : Singleton<UnitTurn>
{
   
    [SerializeField] GameObject _enableUI, _disableUI;
    [SerializeField] GameObject _imagePref;

    Queue<GameObject> _turnObjPooling = new Queue<GameObject>();

    // 유닛 프리펩 순서 조정
    public void NextTurnUnitUI(List<GameObject> unitList)
    {
        if (unitList.Count == 0)
            return;

        for (int i = 0; i < _enableUI.transform.childCount; i++)
        {

            foreach (Transform child in _enableUI.transform)
            {
                if (child.GetComponent<TurnPref>().GetIdx() == unitList[i].GetComponent<Unit>()._SpawnIdx)
                {
                    child.GetComponent<TurnPref>().SetTurnTime(unitList[i].GetComponent<Unit>()._CurrentTurn);
                    child.SetSiblingIndex(i);
                    break;
                }
            }

        }
    }


    GameObject CreateUIPreb(int idx, int turnTime, bool team, EnumList.eKindMonster kind)
    {
        GameObject temp = Instantiate(_imagePref,_enableUI.transform);
        temp.GetComponent<TurnPref>().SetIdx(idx);
        temp.GetComponent<TurnPref>().SetImage(turnTime, team, kind);
        return temp;
    }

    GameObject CreateUIPreb(int idx, int turnTime, bool team, EnumList.eKindSummoner kind)
    {
        GameObject temp = Instantiate(_imagePref, _enableUI.transform);
        temp.GetComponent<TurnPref>().SetIdx(idx);
        temp.GetComponent<TurnPref>().SetImage(turnTime, team, kind);

        return temp;
    }

    // 유닛 순서 프리펩 생성
    public void SetTurnUIPref(Unit unit, EnumList.eKindSummoner kind)
    {
        GameObject temp;

        if (_turnObjPooling.Count > 0)
        {
            temp = _turnObjPooling.Dequeue();
            temp.SetActive(true);
            temp.transform.SetParent(_enableUI.transform);
            temp.GetComponent<TurnPref>().SetIdx(unit._SpawnIdx);
            temp.GetComponent<TurnPref>().SetImage(unit._CurrentTurn, unit._PlayerTeam, kind);

        }
        else
        {
            CreateUIPreb(unit._SpawnIdx, unit._CurrentTurn, unit._PlayerTeam, kind);
        }


    }

    public void SetTurnUIPref(Unit unit, EnumList.eKindMonster kind)
    {
        GameObject temp;

        if (_turnObjPooling.Count > 0)
        {
            temp = _turnObjPooling.Dequeue();
            temp.SetActive(true);
            temp.transform.SetParent(_enableUI.transform);
            temp.GetComponent<TurnPref>().SetIdx(unit._SpawnIdx);
            temp.GetComponent<TurnPref>().SetImage(unit._CurrentTurn, unit._PlayerTeam, kind);

        }
        else
        {
            CreateUIPreb(unit._SpawnIdx, unit._CurrentTurn, unit._PlayerTeam, kind);
        }


    }

    public void ReturnUIPreb(int idx)
    {
        for(int i = 0; i < _enableUI.transform.childCount; i++)
        {
            if (_enableUI.transform.GetChild(i).GetComponent<TurnPref>().GetIdx() == idx)
            {
                _turnObjPooling.Enqueue(_enableUI.transform.GetChild(i).gameObject);
                _enableUI.transform.GetChild(i).gameObject.SetActive(false);
                _enableUI.transform.GetChild(i).SetParent(_disableUI.transform);
                break;
            }
        }
    }



}
