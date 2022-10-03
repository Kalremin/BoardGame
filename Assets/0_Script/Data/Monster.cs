using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


/*
    몬스터는 소환사에 의해 소환되어 생성한다.
    몬스터는 등급에 따라 마법이 존재할 수 있다.
    마법은 한 종류만 가진다.

 */

public class Monster : Unit
{
    [SerializeField] EnumList.eKindMonster _kind; 
    [SerializeField] EnumList.eMagicList _magic; 

    public EnumList.eKindMonster _Kind => _kind;
    [SerializeField]
    int _magicRange;

    public int MagicRange => _magicRange;

    public EnumList.eMagicList Magic => _magic;

    public void Attack(GameObject gameObj)
    {
        _selectedObj = gameObj;
        _animator.SetTrigger(EnumList.eAnimatorParameter.Tri_Attack.ToString());

    }

    public override void Damaged(bool[] attackerState, int attackDamage)
    {
        base.Damaged(attackerState, attackDamage);
    }

    public void AttackAniEvent()
    {
        if(_Way== _selectedObj.GetComponent<Unit>()._Way)
            _selectedObj.GetComponent<Unit>().Damaged(GetState(), (int)(_Attack*1.5));
        else
            _selectedObj.GetComponent<Unit>().Damaged(GetState(), _Attack);
        _selectedObj = null;
    }

    public void MagicAttackAniEvent()
    {

        _selectedObj.GetComponent<Unit>().Damaged(GetState(), (int)(_Attack * 1.5f));
        EffectManager._instance.SpawnEffect(eEffect.Magic, _selectedObj.transform, new Vector3(0, 5, 0), 10);
        _selectedObj = null;
    }

    

    public override void UseMagic(GameObject obj)
    {
        base.UseMagic(obj);
    }

    public override void Dead()
    {
        base.Dead();
        if(!_PlayerTeam)
            SpawnUnitManager._instance.MinusEnemyMonsterCount();
    }

    public override void DeathAniEndEvent()
    {
        
        SpawnUnitManager._instance.ReturnMonster(this);
        BattleManager._instance.ChangeState(eBattleState.FindUnitTurn);
    }




}