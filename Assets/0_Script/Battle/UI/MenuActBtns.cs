using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// 유닛의 행동 버튼
public class MenuActBtns : MonoBehaviour
{
    [SerializeField]
    Button _attackBtn, _magicBtn, _summonBtn;

    private void OnEnable()
    {
        if (BattleManager._instance.IsPlaySummoner())
        {
            _attackBtn.interactable = false;
            _magicBtn.interactable = true;
            _summonBtn.interactable = true;
        }
        else
        {
            _attackBtn.interactable = true;
            _magicBtn.interactable = BattleManager._instance.IsUnitHasMagic();
            _summonBtn.interactable = false;
        }
    }


    // 대기 버튼
    public void OnClickBtnWait()
    {
        BattleManager._instance.SetDirectionWay(false);
        BattleManager._instance.ChangeState(eBattleState.ReadyWait);
        gameObject.SetActive(false);
    }

    // 공격 버튼
    public void OnClickBtnAttack()
    {
        BattleManager._instance.SetDirectionWay(false);
        BoardManager._instance.AttackTile();
        BattleManager._instance.ChangeState(eBattleState.ReadyAttack);
        gameObject.SetActive(false);
    }

    // 마법 버튼
    public void OnClickBtnMagic()
    {
        BattleManager._instance.SetDirectionWay(false);

        if (BattleManager._instance.IsPlaySummoner())
        {
            GUIScript._instance.OpenWnd(EnumList.eUIWnd.MagicWnd);
        }
        else
        {
            BoardManager._instance.MagicTile();
            BattleManager._instance.ChangeState(eBattleState.MagicAttack);

        }
        gameObject.SetActive(false);
    }

    // 소환 버튼
    public void OnClickBtnSummon()
    {
        BattleManager._instance.SetDirectionWay(false);

        GUIScript._instance.OpenWnd(EnumList.eUIWnd.SummonWnd);
        gameObject.SetActive(false);
    }

}
