using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownControl : MonoBehaviour
{
    [SerializeField] GameObject _innObj, _shopObj, _innDesc,_shopDesc,_exitDesc;

    // Start is called before the first frame update
    void Start()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        _innObj.SetActive(false);
        _shopObj.SetActive(false);
    }

    public void OnClickInnBtn()
    {
        _innObj.SetActive(true);
    }

    public void OnClickShopBtn()
    {
        _shopObj.SetActive(true);
    }
    public void OnClickExitBtn()
    {
        MapManager._instance.MapActive(true);
        gameObject.SetActive(false);
    }

    public void OnPointerEnterInn()
    {
        _innDesc.SetActive(true);
    }

    public void OnPointerExitInn()
    {
        _innDesc.SetActive(false);
    }

    public void OnPointerEnterShop()
    {
        _shopDesc.SetActive(true);
    }
    public void OnPointerExitShop()
    {
        _shopDesc.SetActive(false);
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
