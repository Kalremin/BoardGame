using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopControl : MonoBehaviour
{
    [SerializeField] Button[] stuffsBtn;
    [SerializeField] MapControl control;

    // Start is called before the first frame update
    void Start()
    {
        ResetBtn();
    }


    public void ResetBtn()
    {
        List<Monster> tempMonsters = ShopStuff._instance.GetShopMonsters();

        for(int i = 0; i < tempMonsters.Count; i++)
        {
            stuffsBtn[i].GetComponentInChildren<Text>().text = tempMonsters[i]._Name +" [50G]";

            if (PlayerData._instance.PlayerMonsterList.Contains(tempMonsters[i]))
            {
                stuffsBtn[i].GetComponentInChildren<Text>().text += "\n[소유]";
                stuffsBtn[i].interactable = false;
            }
            else if(PlayerData._instance.GetGold() < 50)
            {
                stuffsBtn[i].GetComponentInChildren<Text>().text += "\n[골드 부족]";
                stuffsBtn[i].interactable = false;
            }


        }

        stuffsBtn[3].GetComponentInChildren<Text>().text = ((EnumList.eMagicList)(ShopStuff._instance.GetShopMagic() + 100)).ToString() + " [100G]";
        if (PlayerData._instance.GetSummonerMagics()[ShopStuff._instance.GetShopMagic()])
        {
            stuffsBtn[3].GetComponentInChildren<Text>().text += "\n[소유]";
            stuffsBtn[3].interactable = false;
        }
        else if(PlayerData._instance.GetGold() < 100)
        {
            stuffsBtn[3].GetComponentInChildren<Text>().text += "\n[골드 부족]";
            stuffsBtn[3].interactable = false;
        }
            
        
    }

    public void OnClickMonster1()
    {
        MapControl._instance.UseGolds(50);
        PlayerData._instance.AddMonster(
            ShopStuff._instance.GetShopMonsters()[0].GetComponent<Monster>()
        );
        
        ResetBtn();
    }

    public void OnClickMonster2()
    {

        MapControl._instance.UseGolds(50);
        PlayerData._instance.AddMonster(
            ShopStuff._instance.GetShopMonsters()[1].GetComponent<Monster>()
        );
        ResetBtn();
    }
    public void OnClickMonster3()
    {

        MapControl._instance.UseGolds(50);
        PlayerData._instance.AddMonster(
            ShopStuff._instance.GetShopMonsters()[2].GetComponent<Monster>()
        );
        
        ResetBtn();
    }

    public void OnClickMagic()
    {
        MapControl._instance.UseGolds(100);
        PlayerData._instance.EnableMagic(
            (EnumList.eMagicList)(ShopStuff._instance.GetShopMagic() + 100)
        );
        ResetBtn();
    }

    public void OnClickExit()
    {
        gameObject.SetActive(false);
    }
    
}
