using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    유닛은 배틀 맵에서 장기말처럼 사용된다.
    유닛의 스탯은 이름, 팀, 체력, 공격력, 방어력, 속도, 움직일 수 있는 거리, 등급
 */


public class Unit : MonoBehaviour
{
    const int MAX_STATUS = 20;

    [SerializeField]
    bool _playerTeam;    //T: 아군, F: 적군

    [SerializeField]
    int _health, _maxHealth, _attack, _defense, _speed, _move, _grade;

    [SerializeField]
    string _name;

    [SerializeField]
    GameObject _teamObj;

    [SerializeField]
    protected Animator _animator;

    EnumList.eUnitWay _way;


    int _spawnIdx=-1;
    bool[] _state = new bool[6];
    protected int _currentTurn; // 유닛이 행동할 턴
    protected GameObject _selectedObj;
    
    public bool _PlayerTeam { get { return _playerTeam; }set { _playerTeam = value; } }
    public string _Name { get { return _name; } }   // 이름
    public int _Health { get { return _health; } }  //체력

    public int _MaxHealth { get { return _maxHealth; } }
    public int _Attack { get { return _attack; } }  //공격
    public int _Defense { get { return _defense; } }    //방어
    public int _Speed { get { return _speed; } }    // 속도 -> 높으면 빨리 돌아온다.
    public int _Move { get { return _move; } }  // 움직일 거리 -> 자신의 차례에서 움직일 수 있는 거리
    public int _Grade { get { return _grade; } }    // 등급
    
    public int _SpawnIdx { get { return _spawnIdx; } set { _spawnIdx = value; } }

    public int _CurrentTurn { get { return _currentTurn; } } // 유닛이 행동할 수 있는 턴
    public int _TurnTime { get { return MAX_STATUS - _speed; } } // 행동한 후 추가로 붙일 턴 수

    public bool[] _State { get { return _state; } }

    public EnumList.eUnitWay _Way { get { return _way; } }

    private void Start()
    {

        _teamObj.GetComponent<MeshRenderer>().material = _PlayerTeam ?
            SpawnUnitManager._instance._playerColor :
            SpawnUnitManager._instance._enemyColor;

        _name = gameObject.name;
    }

    public virtual void UseMagic(GameObject obj) 
    {

        _animator.SetTrigger(EnumList.eAnimatorParameter.Tri_Magic.ToString());
        _selectedObj = obj;
        /*
         마법 모션
         */
    }
    public virtual void Move() 
    {
        _animator.SetBool(EnumList.eAnimatorParameter.Bool_Run.ToString(), true);
        /*
         이동 모션
         */
    }
    public virtual void Dead()
    {
        UnitTurn._instance.ReturnUIPreb(_SpawnIdx); // 차례 리스트 제외
        _animator.SetBool(EnumList.eAnimatorParameter.Bool_Death.ToString(), true);

        SpawnUnitManager._instance.RemoveUnitInList(gameObject);
    }

    public virtual void Idle()
    {
        _animator.SetBool(EnumList.eAnimatorParameter.Bool_Run.ToString(),false);
        
    }

    public void SetHealth(int hp) => _health = hp;
    public void SetMaxHealth(int maxHp) => _maxHealth = maxHp;
    public void SetWay(EnumList.eUnitWay way) 
    {
        transform.rotation = Quaternion.Euler(0, (int)way * 90, 0);
        _way = way; 
    }

    public void SetWay(Tile opponentTile)
    {
        Tile unitTile = GetComponentInParent<Tile>();
        Vector3 tempVec = opponentTile.transform.position - unitTile.transform.position;

        if(Vector3.Distance(unitTile.transform.position, opponentTile.transform.position) > BoardManager._instance.TileLength)
        { // 2칸 차이 이상

            transform.LookAt(opponentTile.transform);

            float angle = transform.eulerAngles.y;

            if ((angle >= 315 && angle <= 360) || (angle >= 0 && angle < 45))//if (angle >= -45 && angle < 45)//N
                SetWay(EnumList.eUnitWay.N);

            if (angle >= 45 && angle < 135)//E
                SetWay(EnumList.eUnitWay.E);

            if (angle >= 225 && angle < 315)//W
                SetWay(EnumList.eUnitWay.W);

            if ((angle >= 135 && angle < 225) || (angle >= -225 && angle < -135))//S
                SetWay(EnumList.eUnitWay.S);

        }
        else
        { // 1칸 차이
            if (tempVec.x == 0)
            {
                if (tempVec.z > 0)
                {
                    SetWay(EnumList.eUnitWay.N);
                    _way = EnumList.eUnitWay.N;
                }
                else
                {
                    SetWay(EnumList.eUnitWay.S);
                    _way = EnumList.eUnitWay.S;
                }
            }
            else
            {
                if (tempVec.x > 0)
                {
                    SetWay(EnumList.eUnitWay.E);
                    _way = EnumList.eUnitWay.E;
                }
                else
                {
                    SetWay(EnumList.eUnitWay.W);
                    _way = EnumList.eUnitWay.W;
                }
            }
        }
        

    }


    public virtual void Damaged(bool[] attackerState, int attackDamage) 
    {
        _animator.SetTrigger(EnumList.eAnimatorParameter.Tri_Damage.ToString());
        EffectManager._instance.SpawnEffect(eEffect.Hit, transform,Vector3.up);
        float def = _defense;
        float atk = attackDamage;

        if (_state[(int)EnumList.eStateUnit.DefUp] ^ _state[(int)EnumList.eStateUnit.DefDown])
        {
            if (_state[(int)EnumList.eStateUnit.DefUp])
                def *= 1.3f;

            if (_state[(int)EnumList.eStateUnit.DefDown])
                def *= 0.7f;
        }

        if(attackerState[(int)EnumList.eStateUnit.AtkUp] ^ attackerState[(int)EnumList.eStateUnit.AtkDown])
        {
            if (attackerState[(int)EnumList.eStateUnit.AtkUp])
                atk *= 1.3f;
            if (attackerState[(int)EnumList.eStateUnit.AtkDown])
                atk *= 0.7f;
        }

        if (atk > def)
            _health -= (int)atk - (int)def;
        else
            _health -= 1;
    }

    public virtual void AddTurnTime() => _currentTurn += _TurnTime;

    public virtual void AddTurnTime(int time) => _currentTurn += time;

    // 상태 추가
    public void AddState(EnumList.eStateUnit state) { _state[(int)state] = true ; }

    // 상태 해제
    public void RemoveState(EnumList.eStateUnit state) { _state[(int)state] = false; }

    public bool[] GetState() => _state;

    // 상태를 문자열로 변환
    public string CheckStateToString()
    {
        StringBuilder sb = new StringBuilder();
        for(EnumList.eStateUnit temp = EnumList.eStateUnit.AtkUp;temp<=EnumList.eStateUnit.SpdDown;temp++)
        {
            if (_state[(int)temp])
            {
                sb.Append(temp.ToString() + ",");
            }
        }

        if (sb.Length > 0)
        {
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        else
        {
            return "Normal";
        }

        
        
    }
    public void DamageAniEndEvent()
    {
        if (_health <= 0)
        {
            Dead();
            return;
        }

        BattleManager._instance.ChangeState(eBattleState.FindUnitTurn);
    }

    public virtual void DeathAniEndEvent()
    {
        EffectManager._instance.SpawnEffect(eEffect.Dead, transform, Vector3.zero, 10);
        //BattleManager._instance.ChangeState(eBattleState.BattleEnd);
    }

    public void ResetUnit()
    {
        _health = _maxHealth;
        for (int i = 0; i < _state.Length; i++)
            _state[i] = false;

        _animator.SetBool("Bool_Death", false);
    }
}