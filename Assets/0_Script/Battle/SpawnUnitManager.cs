using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

public class SpawnUnitManager : MonoBehaviour
{
    // 싱클톤
    public static SpawnUnitManager _instance;

    Queue<Monster>[] monsterPooling = new Queue<Monster>[(int)EnumList.eKindMonster.COUNT];

    // 프리펩
    public GameObject[] _summoners;
    public GameObject[] _monsters;
    public Material _playerColor;
    public Material _enemyColor;

    eBattleType battleType;

    bool[] summonersCheck;

    int _enemyMonsterCount;
    int _accumulateSpawnCount;
    // 소환된 유닛들
    List<GameObject> _spawnedUnits;

    // 소환 대기 몬스터
    EnumList.eKindMonster _waitMonsterSummon;

    public int EnemyMonsterCount => _enemyMonsterCount;



    Monster GetMonster(EnumList.eKindMonster kindMonster)
    {
        if (monsterPooling[(int)kindMonster].Count > 0)
        {
            Monster temp = monsterPooling[(int)kindMonster].Dequeue();
            temp.gameObject.SetActive(true);
            return temp;
        }
        else
        {
            Monster monster = Instantiate(_monsters[(int)kindMonster]).GetComponent<Monster>();
            return monster;
        }

    }

    public void ReturnMonster(Monster monster)
    {
        monster.ResetUnit();
        monster.gameObject.SetActive(false);
        monster.transform.SetParent(this.transform);
        monsterPooling[(int)monster._Kind].Enqueue(monster);

    }

    private void Awake()
    {
        _instance = this;
        summonersCheck = new bool[_summoners.Length];
        for(int i = 0; i < monsterPooling.Length; i++)
        {
            monsterPooling[i] = new Queue<Monster>();
        }
    }

    

    public void SetWaitMonster(EnumList.eKindMonster kind) => _waitMonsterSummon = kind;

    // 소환사 소환
    public void SpawnSummoner(eBattleType battleType)
    {
        this.battleType = battleType;


        int len = (int)battleType;
        _spawnedUnits = new List<GameObject>();
        _accumulateSpawnCount = 0;

        GameObject _player = Instantiate(_summoners[PlayerData._instance.IdxCharacter]);
        _player.GetComponent<Unit>()._SpawnIdx = ++_accumulateSpawnCount;
        _player.GetComponent<Unit>()._PlayerTeam = true;
        _player.GetComponent<Unit>().SetWay(EnumList.eUnitWay.N);
        _player.GetComponent<Unit>().SetHealth(PlayerData._instance.GetHealth());
        _player.GetComponent<Unit>().SetMaxHealth(PlayerData._instance.GetMaxHealth());
        
        _player.transform.SetParent( BoardManager._instance.GetTile(len - 2, 1).transform);
        _player.transform.localPosition = Vector3.zero;
        spawnUnitSortAdd(_player.GetComponent<Unit>());

        _player.GetComponent<Unit>().AddTurnTime();

        AutoSelect.SetPlayerUnit(_player.GetComponent<Summoner>());
        GUI_PlayerHP._instance.SetPlayer(_player.GetComponent<Unit>());
        UnitTurn._instance.SetTurnUIPref(_player.GetComponent<Unit>(), (EnumList.eKindSummoner)PlayerData._instance.IdxCharacter);
        UnitTurn._instance.NextTurnUnitUI(_spawnedUnits);


        if (battleType != eBattleType.Normal)  
        {
            int idx;
            _enemyMonsterCount = -1;
            if (summonersCheck.All(v => v == true))
            {
                summonersCheck = new bool[_summoners.Length];
            }

            do
            {
                idx = Random.Range(0, summonersCheck.Length);

            } while (summonersCheck[idx]);

            summonersCheck[idx] = true;

            GameObject _enemy = Instantiate(_summoners[idx]);
            _enemy.transform.SetParent(BoardManager._instance.GetTile(1, len - 2).transform);
            _enemy.transform.localPosition = Vector3.zero;
            _enemy.GetComponent<Unit>()._SpawnIdx = ++_accumulateSpawnCount;
            _enemy.GetComponent<Unit>().SetWay(EnumList.eUnitWay.S);

            spawnUnitSortAdd(_enemy.GetComponent<Unit>());

            _enemy.GetComponent<Unit>().AddTurnTime();

            UnitTurn._instance.SetTurnUIPref(_enemy.GetComponent<Unit>(), (EnumList.eKindSummoner)idx);
            UnitTurn._instance.NextTurnUnitUI(_spawnedUnits);
        }
        else        
        {
            _enemyMonsterCount = Random.Range(1, BattleManager._instance._stage + 1);

            for(int i = 0; i < _enemyMonsterCount; i++)
            {
                int x, y;
                int spawnRange = BattleManager._instance._stage + 1;
                int randomIdx = Random.Range(0, (int)EnumList.eKindMonster.COUNT);
                GameObject _monster = GetMonster((EnumList.eKindMonster)randomIdx).gameObject;

                int temp = 0;
                do
                {
                    x = Random.Range(0, spawnRange);
                    y = Random.Range(len - spawnRange, len - 1);
                    temp++;
                    if (temp > 10)
                        throw new System.Exception("loop error");
                } while (BoardManager._instance.ExistUnitInTile(x, y));

                _monster.transform.SetParent(BoardManager._instance.GetTile(x, y).transform);
                _monster.transform.localPosition = Vector3.zero;
                _monster.GetComponent<Unit>()._SpawnIdx = ++_accumulateSpawnCount;
                _monster.GetComponent<Unit>().SetWay((EnumList.eUnitWay)Random.Range(0, 4));
                _monster.GetComponent<Unit>().AddTurnTime();
                spawnUnitSortAdd(_monster.GetComponent<Unit>());

                

                UnitTurn._instance.SetTurnUIPref(_monster.GetComponent<Unit>(), (EnumList.eKindMonster)randomIdx);
                UnitTurn._instance.NextTurnUnitUI(_spawnedUnits);
            }


        }


    }

    // 소환사의 몬스터 소환
    public void SpawnMonster(Summoner summoner, Tile tile)
    {
        GameObject _monster = GetMonster(_waitMonsterSummon).gameObject;
        _monster.transform.SetParent(tile.transform);
        _monster.transform.localPosition = Vector3.zero;
        _monster.GetComponent<Monster>()._PlayerTeam = summoner._PlayerTeam;
        _monster.GetComponent<Monster>().SetWay(summoner._Way);
        _monster.GetComponent<Monster>().AddTurnTime(summoner._CurrentTurn);
        _monster.GetComponent<Monster>()._SpawnIdx = ++_accumulateSpawnCount;
        _accumulateSpawnCount++;
        
        summoner.AddTurnTime(_monster.GetComponent<Monster>()._TurnTime * 2);

        _monster.GetComponent<Unit>().AddTurnTime();

        spawnUnitSortAdd(_monster.GetComponent<Unit>());
        

        GUI_ActLog._instance.SummonLog((Unit)summoner, _monster.GetComponent<Monster>());


        UnitTurn._instance.SetTurnUIPref(_monster.GetComponent<Unit>(), _waitMonsterSummon);
        UnitTurn._instance.NextTurnUnitUI(_spawnedUnits);
    }

    // 플레이할 유닛 찾아서 반환
    public Unit TurnUnit()
    {
        Unit tempUnit;

        tempUnit = _spawnedUnits[0].GetComponent<Unit>();
        tempUnit.AddTurnTime();
        _spawnedUnits.RemoveAt(0);

        spawnUnitSortAdd(tempUnit);
        UnitTurn._instance.NextTurnUnitUI(_spawnedUnits);

        StringBuilder builder = new StringBuilder();
        foreach(GameObject tempObj in _spawnedUnits)
        {
            builder.Append(string.Format("{0} ", tempObj.GetComponent<Unit>()._CurrentTurn));
        }
        LogClass.LogWarn("unit// "+builder.ToString());

        return tempUnit;
    }

    // 죽은 유닛을 리스트에서 제외
    public void RemoveUnitInList(GameObject removeUnit)
    {
        _spawnedUnits.Remove(removeUnit);
    }

    public void MinusEnemyMonsterCount()
    {
        _enemyMonsterCount--;
        if(battleType== eBattleType.Normal && _enemyMonsterCount <= 0)
        {
            BattleManager._instance.ChangeState(eBattleState.BattleEnd);
        }
        else
        {
            BattleManager._instance.ChangeState(eBattleState.FindUnitTurn);
        }
    }

    void spawnUnitSortAdd(Unit unit)
    {
        if (_spawnedUnits.Count == 0)
        {
            _spawnedUnits.Add(unit.gameObject);
            return;
        }

        for(int i = 0; i < _spawnedUnits.Count; i++)
        {
            if(unit._CurrentTurn < _spawnedUnits[i].GetComponent<Unit>()._CurrentTurn)
            {
                _spawnedUnits.Insert(i, unit.gameObject);
                return;
            }
        }

        _spawnedUnits.Add(unit.gameObject);
    }
}
