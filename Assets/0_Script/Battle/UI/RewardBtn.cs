using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 보상 종류
public enum eReward
{
    Gold,
    Hunger,
    Monster
}

// 보상 UI창의 보상 버튼
public class RewardBtn : MonoBehaviour
{
    eReward _rewardKind;

    [SerializeField]
    Image _rewardImage;

    [SerializeField]
    Sprite[] _itemSprite, _monsterSprite;

    [SerializeField]
    Text _rewardText;

    int rewardVal = 0;
    Monster rewardMonster;

    // 보상 정보 입력
    public void SetKind(eReward kind)
    {
        _rewardKind = kind;

        switch (_rewardKind)
        {
            case eReward.Gold:

                switch (BattleManager._instance.BattleType)
                {
                    case eBattleType.Normal:
                        rewardVal = Random.Range(50, 100);
                        break;
                    //case eBattleType.Elite:
                    //    rewardVal = Random.Range(150, 200);
                        //break;
                    case eBattleType.Boss:
                        rewardVal = Random.Range(300, 400);
                        break;
                }
                _rewardText.text = rewardVal.ToString() + "G";
                _rewardImage.sprite = _itemSprite[0];
                break;

            case eReward.Hunger:
                switch (BattleManager._instance.BattleType)
                {
                    case eBattleType.Normal:
                        rewardVal = Random.Range(25, 50);
                        break;
                    //case eBattleType.Elite:
                    //    rewardVal = Random.Range(75, 100);
                        //break;
                    case eBattleType.Boss:
                        rewardVal = Random.Range(150, 200);
                        break;
                }
                _rewardText.text = rewardVal.ToString() + " 허기";
                _rewardImage.sprite = _itemSprite[1];
                break;

            case eReward.Monster:
                rewardVal = -1;
                GameObject[] temp = SpawnUnitManager._instance._monsters;
                int ran = Random.Range(0, temp.Length);
                rewardMonster = temp[ran].GetComponent<Monster>();
                _rewardImage.sprite = _monsterSprite[ran];
                _rewardText.text = rewardMonster._Name;
                break;
        }
    }

    // 클릭 이벤트 (보상)
    public void OnClick()
    {
        switch (_rewardKind)
        {
            case eReward.Gold:
                PlayerData._instance.AddGold(rewardVal);
                break;
            case eReward.Hunger:
                PlayerData._instance.AddHunger(rewardVal);
                break;
            case eReward.Monster:
                PlayerData._instance.AddMonster(rewardMonster);
                break;
        }

        Destroy(gameObject);
    }
}
