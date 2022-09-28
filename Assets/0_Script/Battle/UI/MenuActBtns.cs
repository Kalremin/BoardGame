using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuActBtns : MonoBehaviour
{
    //public static MenuActBtns _instance = new MenuActBtns();

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

    public void OpenWnd(Vector3 pos)
    {
        gameObject.SetActive(true);
        GetComponent<RectTransform>().position = pos + (Vector3.right + Vector3.down) * 30 ;
    }


    public void OnClickBtnWait()
    {
        BattleManager._instance.SetDirectionWay(false);
        BattleManager._instance.ChangeState(eBattleState.ReadyWait);
        gameObject.SetActive(false);
    }
    public void OnClickBtnAttack()
    {
        BattleManager._instance.SetDirectionWay(false);
        BoardManager._instance.AttackTile();
        BattleManager._instance.ChangeState(eBattleState.ReadyAttack);
        gameObject.SetActive(false);
    }

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

    public void OnClickBtnSummon()
    {
        BattleManager._instance.SetDirectionWay(false);

        GUIScript._instance.OpenWnd(EnumList.eUIWnd.SummonWnd);
        gameObject.SetActive(false);
    }

}
