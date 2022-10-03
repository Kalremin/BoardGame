using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    소환사는 배틀 시작시 미리 생성한다.
    소환사의 등급은 5로 고정이다.
    소환사는 몬스터를 소환한다.
    소환사는 공격이 불가능하다.
    소환사는 여러 개의 마법을 가진다.
    마법은 오로지 지원 마법을 사용한다.
 */

public class Summoner : Unit
{
    [SerializeField] EnumList.eKindSummoner _kind;

    Tile _selectedTile;

    public override void Damaged(bool[] attackerState, int attackDamage)
    {
        base.Damaged(attackerState, attackDamage);
        if (_PlayerTeam)
            PlayerData._instance.SetHealth(_Health);
    }

    public override void UseMagic(GameObject obj) 
    {
        base.UseMagic(obj);
    }

    public void IdleEvent()
    {

    }

    public void MagicAniEvent()
    {
        if(BattleManager._instance.GetIsBuff())
            EffectManager._instance.SpawnEffect(eEffect.Buff, _selectedObj.transform, Vector3.zero, 10);
        else
            EffectManager._instance.SpawnEffect(eEffect.Debuff, _selectedObj.transform, Vector3.zero, 10);

        LogClass.LogInfo("Anistart");
    }

    public void MagicAniEndEvent()
    {
        _selectedObj = null;
        BattleManager._instance.ChangeState(eBattleState.FindUnitTurn);
        LogClass.LogInfo("Aniend");
    }

    public void Summon(Tile tile) 
    {
        _selectedTile = tile;
        _animator.SetTrigger(EnumList.eAnimatorParameter.Tri_Summon.ToString());
    }

    public void SummonAniEvent()
    {
        SpawnUnitManager._instance.SpawnMonster(this, _selectedTile);
        EffectManager._instance.SpawnEffect(eEffect.Summon, _selectedTile.GetUnitObject().transform, Vector3.up);
        _selectedTile = null;
    }
    public void SummonAniEndEvent()
    {
        BattleManager._instance.ChangeState(eBattleState.FindUnitTurn);
    }

    public override void Dead()
    {
        base.Dead();
    }

    public override void DeathAniEndEvent()
    {
        BattleManager._instance.ChangeState(eBattleState.BattleEnd);
    }
}