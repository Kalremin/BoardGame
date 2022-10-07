using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스테이지의  상점 물품 설정
public class ShopStuff : NoOverlapSingleton<ShopStuff>
{
    //public static ShopStuff _instance;

    [SerializeField] GameObject[] _shopMonsters;

    bool[] usedRest;
    int[] _shopMagics = { -1, -1, -1 };

    List<Monster>[] monsters = new List<Monster>[3];

    public bool[] UsedRestStage => usedRest;
   
    void Start()
    {
        usedRest = new bool[MapManager._instance.StageCount];
    }

    // 각 스테이지의 판매할 몬스터 물품
    public List<Monster> GetShopMonsters()
    {
        if (monsters[MapManager._instance.StageIdx] == null)
        {
            
            monsters[MapManager._instance.StageIdx] = new List<Monster>();
            for (int j = 0; j < 3; j++)
            {
                monsters[MapManager._instance.StageIdx].Add(_shopMonsters[Random.Range(0, _shopMonsters.Length)].GetComponent<Monster>());
            }
            
        }

        return monsters[MapManager._instance.StageIdx];
    }

    // 각 스테이지의 판매할 마법 물품
    public int GetShopMagic()
    {
        if(_shopMagics[MapManager._instance.StageIdx] == -1)
        {
            _shopMagics[MapManager._instance.StageIdx] = Random.Range(0, 6);
        }

        return _shopMagics[MapManager._instance.StageIdx];
    }

}
