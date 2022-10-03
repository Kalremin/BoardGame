using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] GameObject summonerObj;
    [SerializeField] Image _summonerImg;
    [SerializeField] Text _txtName, _txtMaxHealth, _txtDef, _txtSpd, _txtMov, _txtGold, _txtHunger;

    [SerializeField] int gold, hunger;
    [SerializeField] Sprite summonerSprite;

    Summoner summoner;

    void Start()
    {
        _summonerImg.sprite = summonerSprite;

        summoner = summonerObj.GetComponent<Summoner>();
        _txtName.text = summoner._Name.ToString();
        _txtMaxHealth.text = summoner._MaxHealth.ToString();
        _txtDef.text = summoner._Defense.ToString();
        _txtSpd.text = summoner._Speed.ToString();
        _txtMov.text = summoner._Move.ToString();

        _txtGold.text = gold.ToString();
        _txtHunger.text = hunger.ToString();

        
    }

    public void Select(int idx)
    {
        PlayerData._instance.SelectCharacter(summoner,idx, gold,hunger);
        SceneControlManager._instance.ChangeScene(EnumList.eScence.MapScene);
    }

    public void PointerEnter()
    {
        GetComponent<Image>().color = Color.gray;
    }

    public void PointerExit()
    {
        GetComponent<Image>().color = Color.white;
    }
}
