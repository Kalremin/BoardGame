using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopStuff : MonoBehaviour
{
    public static ShopStuff _instance;

    [SerializeField] GameObject[] _shopMonsters;
    [SerializeField] GameObject _town;
    bool[] usedRest;
    int[] _shopMagics = { -1, -1, -1 };
    int stageIdx = 0;

    List<Monster>[] monsters = new List<Monster>[3];

    public bool[] UsedRestStage => usedRest;
    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

    }

    
    void Start()
    {
        usedRest = new bool[MapManager._instance.StageCount];
    }

    public void SetStageIdx(int idx)
    {
        stageIdx = idx;
    }

    public List<Monster> GetShopMonsters()
    {
        if (monsters[stageIdx] == null)
        {
            
            monsters[stageIdx] = new List<Monster>();
            for (int j = 0; j < 3; j++)
            {
                monsters[stageIdx].Add(_shopMonsters[Random.Range(0, _shopMonsters.Length)].GetComponent<Monster>());
            }
            

            
        }

        return monsters[stageIdx];
    }

    public int GetShopMagic()
    {
        if(_shopMagics[stageIdx] == -1)
        {
            _shopMagics[stageIdx] = Random.Range(0, 6);
        }

        return _shopMagics[stageIdx];
    }

}
