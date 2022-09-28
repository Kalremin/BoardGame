using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnControl : MonoBehaviour
{

    [SerializeField] Button _rest, _hunger;
    [SerializeField] GameObject _restDesc, _hungerDesc, _exitDesc;

    public void OnClickRestBtn()
    {
        MapControl._instance.HealHP(50);
        MapControl._instance.UseGolds(30);
        _rest.interactable = false;
        ShopStuff._instance.UsedRestStage[MapManager._instance.StageIdx] = true;
    }

    public void OnClickHungerBtn()
    {
        if (PlayerData._instance.GetGold() >= 15)
        {
            int hunger = Random.Range(10, 51);
            MapControl._instance.UseGolds(15);
            MapControl._instance.GetHunger(hunger);

            if (PlayerData._instance.GetGold() < 15)
            {
                _hunger.interactable = false;
            }

        }
    }

    public void OnClickExitBtn()
    {
        _restDesc.SetActive(false);
        _hungerDesc.SetActive(false);
        _exitDesc.SetActive(false);

        if (ShopStuff._instance.UsedRestStage[MapManager._instance.StageIdx])
            _rest.interactable = false;

        if (PlayerData._instance.GetGold() < 15)
        {
            _hunger.interactable = false;
        }

        gameObject.SetActive(false);
    }

    public void OnPointerEnterRest()
    {
        _restDesc.SetActive(true);
    }
    public void OnPointerExitRest()
    {
        _restDesc.SetActive(false);
    }

    public void OnPointerEnterHunger()
    {
        _hungerDesc.SetActive(true);
    }
    public void OnPointerExitHunger()
    {
        _hungerDesc.SetActive(false);
    }
    public void OnPointerEnterExit()
    {
        _exitDesc.SetActive(true);
    }
    public void OnPointerExitExit()
    {
        _exitDesc.SetActive(false);
    }
}
