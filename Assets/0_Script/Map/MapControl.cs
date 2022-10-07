using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapControl : Singleton<MapControl>
{
    //public static MapControl _instance;

    public enum eText
    {
        Health,
        Gold,
        Hunger
    }

    [SerializeField] Text _health, _maxHealth, _gold, _hunger, _stage;
    [SerializeField] Text _effectHealth, _effectGold, _effectHunger, _effectStage;
    [SerializeField] GameObject _townObj;
    [SerializeField] Image _lastImg;

    int defaultHunger = 15;
    int defaultRegeneration = 2;
    //private void Awake()
    //{
    //    //_instance = this;
    //}

    // Start is called before the first frame update
    void Start()
    {
        _health.text = PlayerData._instance.GetHealth().ToString();
        _maxHealth.text = PlayerData._instance.GetMaxHealth().ToString();
        _hunger.text = PlayerData._instance.GetHunger().ToString();
        _gold.text = PlayerData._instance.GetGold().ToString();
        _stage.text = (MapManager._instance.StageIdx + 1).ToString();

        _effectHealth.text = "";
        _effectGold.text = "";
        _effectHunger.text = "";
        _effectStage.text = "";

        _townObj.SetActive(false);
        _lastImg.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(touchPos, Vector2.zero);
            RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit2D.collider != null && hit2D.collider.CompareTag("MapPoint"))
            {
                MapPoint tempPoint = hit2D.collider.GetComponent<MapPoint>();
                eMapPointState pointState;
                if (!MapManager._instance.ClickPoint(tempPoint, out pointState))
                    return;

                switch (pointState)
                {
                    case eMapPointState.Battle:
                        break;
                    case eMapPointState.Boss:
                        break;
                    case eMapPointState.Town:
                        _townObj.SetActive(true);
                        break;
                    default:
                        SetEffectText(eText.Hunger, false, 15.ToString());
                        if(PlayerData._instance.GetHunger()>0)
                            SetEffectText(eText.Health, true, 2.ToString());
                        else
                            SetEffectText(eText.Health, false, 2.ToString());
                        break;
                }

                PointEvent(pointState);

            }
        }

    }

    private void PointEvent(eMapPointState pointState)
    {
        UseHunger(defaultHunger);

        switch (pointState)
        {

            case eMapPointState.Random:
                RandomPoint();
                break;
            case eMapPointState.Treasure:
                TreasurePoint();
                break;
            case eMapPointState.Gold:
                GoldPoint();
                break;
            case eMapPointState.Battle:
                BattlePoint();
                break;
            case eMapPointState.Town:
                TownPoint();
                break;
            case eMapPointState.Boss:
                BossPoint();
                break;
            case eMapPointState.Clear:
                ClearPoint();
                break;
            case eMapPointState.UpStair:
                MapManager._instance.StageIdx--;
                MapManager._instance.OpenStage(false);
                SetTextStage();
                break;
            case eMapPointState.DownStair:
                MapManager._instance.StageIdx++;
                MapManager._instance.OpenStage(true);
                SetTextStage();
                break;
        }
    }

    void RandomPoint()
    {


        int randomInt = Random.Range(0, 100);
        if (randomInt < 5)
        {
            BattlePoint();
            return;
        }

        if (randomInt >= 5 && randomInt < 10)
        {
            HealHP(30);
            return;
        }

        if (randomInt >= 90)
        {
            GoldPoint();
            return;
        }

    }

    void TreasurePoint()
    {
        if (Random.Range(0, 100) < 70)
        {
            GetGolds(Random.Range(10, 101));
        }
        else
        {
            GetHunger(Random.Range(30, 51));
        }


    }

    void ClearPoint()
    {
        int randomInt = Random.Range(0, 100);
        if (randomInt < 5)
        {
            BattlePoint();
        }
        else if (randomInt >= 5 && randomInt < 10)
        {
            GetGolds(Random.Range(10, 100));
        }
    }

    void TownPoint()
    {

        MapManager._instance.MapActive(false);
        _townObj.SetActive(true);

    }

    void BattlePoint()
    {
        UseHunger(5);
        MapManager.readyBattleType = eBattleType.Normal;
        MapManager._instance.MapActive(false);

        SceneControlManager._instance.ChangeScene(EnumList.eScence.BattleScene);
    }

    void BossPoint()
    {
        MapManager.readyBattleType = eBattleType.Boss;
        MapManager._instance.MapActive(false);
        MapManager._instance.StageIdx++;
        MapManager._instance.OpenStage(true);
        SetTextStage();

        SceneControlManager._instance.ChangeScene(EnumList.eScence.BattleScene);
    }

    private void GoldPoint()
    {
        GetGolds(Random.Range(10, 100));
    }

    public void UseGolds(int golds)
    {
        PlayerData._instance.MinusGold(golds);
        SetEffectText(eText.Gold, false, golds.ToString());
    }

    public void GetGolds(int golds)
    {
        PlayerData._instance.AddGold(golds);
        SetEffectText(eText.Gold, true, golds.ToString());
    }

    public void UseHunger(int hunger)
    {

        PlayerData._instance.MinusHunger(hunger);
        SetEffectText(eText.Hunger, false, hunger.ToString());
        if (PlayerData._instance.GetHunger() > 0)
            HealHP(defaultRegeneration);
        else
        {
            if (PlayerData._instance.GetHealth() <= PlayerData._instance.GetMaxHealth() / 3)
                return;
            DamageHP(defaultRegeneration);
        }

    }

    public void GetHunger(int hunger)
    {
        PlayerData._instance.AddHunger(hunger);
        SetEffectText(eText.Hunger, true, hunger.ToString());
    }

    public void HealHP(int hp)
    {
        PlayerData._instance.HealHealth(hp);
        SetEffectText(eText.Health, true, hp.ToString());
    }

    public void DamageHP(int hp)
    {
        PlayerData._instance.DamageHealth(hp);
        SetEffectText(eText.Health, false, hp.ToString());
    }

    void SetTextStage()
    {
        _stage.text = (MapManager._instance.StageIdx + 1).ToString();
    }

    public void SetLast()
    {
        _lastImg.gameObject.SetActive(true);

    }

    public void OnClickLastBtn()
    {
        SceneControlManager._instance.ChangeScene(EnumList.eScence.TitleScene);
    }


    public void SetEffectText(eText enumText, bool positive, string data)
    {
        
        Text effectText = null;
        switch (enumText)
        {
            case eText.Health:
                _health.text = PlayerData._instance.GetHealth().ToString();
                effectText = _effectHealth;
                break;
            case eText.Gold:
                _gold.text = PlayerData._instance.GetGold().ToString();
                effectText = _effectGold;
                break;
            case eText.Hunger:
                _hunger.text = PlayerData._instance.GetHunger().ToString();
                effectText = _effectHunger;
                break;
        }

        effectText.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;

        if (positive)
        {
            effectText.color = new Color(0, 255, 127, 1);
            effectText.text = "+" + data;
        }
        else
        {
            effectText.color = new Color(255, 0, 0, 1);
            effectText.text = "-" + data;
        }
        StartCoroutine(FadeOutFont(effectText));
    }

    IEnumerator FadeOutFont(Text text)
    {
        text.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;

        while (text.color.a > 0)
        {
            text.GetComponent<RectTransform>().transform.position += Vector3.up * Time.deltaTime * 10;
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime * 1f));
            yield return null;
        }
        text.text = "";
    }
}
