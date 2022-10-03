using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerMagicWnd : MonoBehaviour
{

    [SerializeField]
    RectTransform _menuBtns;

    [SerializeField]
    Button[] _magicBtns;
    /*
     0: atkUP
    1: atkDOWN
    2: defUP
    3: defDOWN
    4: spdUP
    5: spdDOWN
     */

    private void OnEnable()
    {
        BattleManager._instance.SetOpenWnd(true);
        BattleManager._instance.SetDirectionWay(false);
        for(int i=0;i<_magicBtns.Length;i++)
        {
            _magicBtns[i].interactable = PlayerData._instance.GetSummonerMagics()[i];
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            BattleManager._instance.SetDirectionWay(true);
            GUIScript._instance.OpenWnd(EnumList.eUIWnd.MenuActBtns);
            gameObject.SetActive(false);
        }
    }

    public void OnClickATKUP()
    {
        BattleManager._instance.SetUnitState(EnumList.eStateUnit.AtkUp);
        BattleManager._instance.SetIsBuff(true);
        CommonMethod();
    }

    public void OnClickATKDOWN()
    {
        BattleManager._instance.SetUnitState(EnumList.eStateUnit.AtkDown);
        BattleManager._instance.SetIsBuff(false);
        CommonMethod();
    }

    public void OnClickDEFUP()
    {
        BattleManager._instance.SetUnitState(EnumList.eStateUnit.DefUp);
        BattleManager._instance.SetIsBuff(true);
        CommonMethod();
    }

    public void OnClickDEFDOWN()
    {
        BattleManager._instance.SetUnitState(EnumList.eStateUnit.DefDown);
        BattleManager._instance.SetIsBuff(false);
        CommonMethod();
    }

    public void OnClickSPDUP()
    {
        BattleManager._instance.SetUnitState(EnumList.eStateUnit.SpdUp);
        BattleManager._instance.SetIsBuff(true);
        CommonMethod();
    }

    public void OnClickSPDDOWN()
    {
        BattleManager._instance.SetUnitState(EnumList.eStateUnit.SpdDown);
        BattleManager._instance.SetIsBuff(false);
        CommonMethod();
    }

    void CommonMethod()
    {
        BoardManager._instance.MagicTile();
        BattleManager._instance.ChangeState(eBattleState.MagicAssist);
        gameObject.SetActive(false);
    }

}
