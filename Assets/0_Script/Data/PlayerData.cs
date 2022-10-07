using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    //public static PlayerData _instance;

    [SerializeField] Monster[] _haveMonsters = new Monster[3];

    int _idxCharacter;

    int _health;
    int _maxHealth;
    int _defense;
    int _speed;
    int _move;
    int _grade = 5;

    int _gold = 100;
    int _hunger = 50;

    
    List<Monster> _monsters = new List<Monster>();
    bool[] _summonerMagics = new bool[6];

    private new void Awake()
    {
        //_instance = this;
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    public void Initialize(int idxChar, int health, int maxHealth, int defense, int speed, int move, int gold, int hunger)
    {
        _idxCharacter = idxChar;
        _health = health;
        _maxHealth = maxHealth;
        _defense = defense;
        _speed = speed;
        _move = move;
        _gold = gold;
        _hunger = hunger;
    }

    public void SelectCharacter(Unit unit, int idxChar, int gold, int hunger)
    {
        _idxCharacter = idxChar;
        _health = unit._Health;
        _maxHealth = unit._MaxHealth;
        _defense = unit._Defense;
        _speed = unit._Speed;
        _move = unit._Move;
        _gold = gold;
        _hunger = hunger;

        AddMonster(_haveMonsters[UnityEngine.Random.Range(0,_haveMonsters.Length)]);
        EnableMagic(EnumList.eMagicList.ATKUP);
        
    }

    public int IdxCharacter => _idxCharacter;

    public void AddMonster(Monster monster) => _monsters.Add(monster);

    public void RemoveMonster(Monster monster) => _monsters.Remove(monster);

    public List<Monster> PlayerMonsterList => _monsters;

    public void EnableMagic(EnumList.eMagicList magic)
    {
        int temp = (int)magic;

        if(temp >= 100)
            _summonerMagics[temp % 100] = true;
    }

    public void DisableMagic(EnumList.eMagicList magic)
    {
        int temp = (int)magic;

        if (temp >= 100)
            _summonerMagics[temp % 100] = false;
    }

    public void SetHealth(int health) => _health = health;

    public int GetHealth() => _health;

    public void HealHealth(int health)
    {
        _health += health;
        if (_health > _maxHealth)
            _health = _maxHealth;
    }

    public void DamageHealth(int health) => _health -= health;

    public int GetMaxHealth() => _maxHealth;


    public void AddGold(int gold) => _gold +=gold;
    public bool MinusGold(int gold)
    {
        if (_gold < gold)
            return false;

        _gold -= gold;
        return true;
    }

    public int GetGold() => _gold;

    public void AddHunger(int hunger)
    {
        _hunger += hunger;
    }

    public void MinusHunger(int hunger)
    {
        if (_hunger < hunger)
        {
            _hunger = 0;
        }
        else
        {
            _hunger -= hunger;
        }

    }

    public int GetHunger() => _hunger;

    public bool[] GetSummonerMagics() => _summonerMagics;


}