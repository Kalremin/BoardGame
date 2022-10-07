using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 지도 씬의 여관 UI
public class InnControl : MonoBehaviour
{

    [SerializeField] Button _restButton, _supplyButton;
    [SerializeField] GameObject _restDesc, _hungerDesc, _exitDesc;

    private void OnEnable()
    {
        if (ShopStuff._instance.UsedRestStage[MapManager._instance.StageIdx])
            _restButton.interactable = false;
        else
            _restButton.interactable = true;
    }

    public void OnClickRestBtn()
    {
        MapControl._instance.HealHP(50);
        MapControl._instance.UseGolds(30);
        _restButton.interactable = false;
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
                _supplyButton.interactable = false;
            }

        }
    }

    public void OnClickExitBtn()
    {
        _restDesc.SetActive(false);
        _hungerDesc.SetActive(false);
        _exitDesc.SetActive(false);

        if (ShopStuff._instance.UsedRestStage[MapManager._instance.StageIdx])
            _restButton.interactable = false;

        if (PlayerData._instance.GetGold() < 15)
        {
            _supplyButton.interactable = false;
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
