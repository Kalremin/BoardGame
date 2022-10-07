using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 몬스터 등급 색깔
struct MonsterColor
{
    public static Color Normal => Color.gray;
    public static Color Rare => Color.green;
    public static Color Unique => Color.red;
}

// 소환 UI창의 몬스터 버튼
public class MonsterSummonBtn : MonoBehaviour
{

    EnumList.eKindMonster kindMonster;

    [SerializeField]
    Text _name, _health, _attack, _defense, _speed;

    [SerializeField]
    Image _monsterImage;

    [SerializeField]
    Sprite[] _monsterSprite = new Sprite[8];
    Image bg;

    // 몬스터 정보 입력
    public void SetMonster(Monster monster)
    {
        bg = transform.GetComponent<Image>();

        kindMonster = monster._Kind;
        _name.text = monster._Name;
        _health.text = monster._Health.ToString();
        _attack.text = monster._Attack.ToString();
        _defense.text = monster._Defense.ToString();
        _speed.text = monster._Speed.ToString();

        switch (monster._Grade)
        {
            case 1:
                bg.color = MonsterColor.Normal;
                break;
            case 2:
                bg.color = MonsterColor.Rare;
                break;
            case 3:
                bg.color = MonsterColor.Unique;
                break;
        }

        _monsterImage.sprite = _monsterSprite[(int)kindMonster];


    }

    // 클릭 이벤트 (소환)
    public void OnClickBtn()
    {

        SpawnUnitManager._instance.SetWaitMonster(kindMonster);
        BoardManager._instance.SummonTile();
        BattleManager._instance.ChangeState(eBattleState.SelectSummon);
        SummonWnd._instance.gameObject.SetActive(false);
    }

}
