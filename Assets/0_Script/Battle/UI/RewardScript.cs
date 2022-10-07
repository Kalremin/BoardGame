using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RewardScript : MonoBehaviour
{
    [SerializeField]
    GameObject _rewardBG, _rewardContent, _rewardBtn;

    private void OnEnable()
    {
        SoundManager._instance.PlayBackgroundSound(eBackgroundSound.Victory);
        Invoke("NextMethod", 3);
    }

    void NextMethod()
    {
        _rewardBG.SetActive(true);
        
        if (Random.Range(0, 100) < 80)
        {
            GameObject temp = Instantiate(_rewardBtn);
            temp.transform.SetParent(_rewardContent.transform);
            temp.transform.localScale = Vector3.one;
            temp.GetComponent<RewardBtn>().SetKind(eReward.Gold);
        }

        if (Random.Range(0, 100) < 30)
        {
            GameObject temp = Instantiate(_rewardBtn);
            temp.transform.SetParent(_rewardContent.transform);
            temp.transform.localScale = Vector3.one;
            temp.GetComponent<RewardBtn>().SetKind(eReward.Hunger);
        }

        if (BattleManager._instance.BattleType != eBattleType.Normal)
        {
            GameObject temp = Instantiate(_rewardBtn);
            temp.transform.SetParent(_rewardContent.transform);
            temp.transform.localScale = Vector3.one;
            temp.GetComponent<RewardBtn>().SetKind(eReward.Monster);
        }
    }

    public void OnClickToChangeScene()
    {
        MapManager._instance.MapActive(true);
        SceneControlManager._instance.ChangeScene(EnumList.eScence.MapScene);
    }

}
