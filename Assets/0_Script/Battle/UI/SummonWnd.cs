using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWnd : MonoBehaviour
{
    public static SummonWnd _instance;

    [SerializeField]
    GameObject _monsterView, _btnMonster;//, _layoutBtn;

    [SerializeField]
    RectTransform _contentRect;

    int rectHeight = 600;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            BattleManager._instance.SetDirectionWay(true);
            GUIScript._instance.OpenWnd(EnumList.eUIWnd.MenuActBtns);
            gameObject.SetActive(false);
            
        }
    }

    private void OnEnable()
    {
        
        Debug.Log(PlayerData._instance.PlayerMonsterList.Count);

        BattleManager._instance.SetOpenWnd(true);
        _contentRect.sizeDelta = new Vector2(_contentRect.sizeDelta.x, rectHeight * PlayerData._instance.PlayerMonsterList.Count);
        //.height = rectHeight * PlayerData.Instance.PlayerMonsterList.Count;

        if (_monsterView.transform.childCount == 0)
        {

            foreach (var monster in PlayerData._instance.PlayerMonsterList)
            {
                Debug.Log(monster == null);
                GameObject temp = Instantiate(_btnMonster);

                temp.transform.SetParent(_monsterView.transform);
                temp.transform.localScale = Vector3.one;
                temp.GetComponent<MonsterSummonBtn>().SetMonster(monster);
            }
        }
    }



    private void OnDisable()
    {
        Debug.Log("OnDisable");
        // Editor 크래시
        //while (_monsterView.transform.childCount > 0) Destroy(_monsterView.transform.GetChild(0));
    }

}
