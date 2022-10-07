using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnumList
{
    // 소환사 종류
    public enum eKindSummoner
    {
        Tester, OldMan, CastleGuard,
        COUNT
    }

    // 몬스터 종류
    public enum eKindMonster
    {
        FireStone = 0 , GoblinRogue, Grunt, Bat, Ghost, Slime, Rabbit,
        Wizard,
        COUNT
    }

    // 유닛 상태
    public enum eStateUnit
    {
        AtkUp,
        AtkDown,
        DefUp,
        DefDown,
        SpdUp,
        SpdDown
    }

    // 방향
    public enum eUnitWay
    {
        N, E, S, W
    }

    // 마법 종류
    public enum eMagicList
    {
        None = -1,
        WizardMagic=0, 
        MonsterMagic2, 
        MonsterMagic3,
        ATKUP=100,
        ATKDOWN,
        DEFUP,
        DEFDOWN,
        SPDUP,
        SPDDOWN
    }

    // 타일 상태
    public enum eTileHighlightStatus
    {
        None,
        Move,
        Enemy,
        Magic
    }

    // 타일의 몬스터 존재 여부
    public enum eTileUnit
    {
        Wall=-1,
        Empty = 0,
        Summoner,
        Monster
    }

    // UI창
    public enum eUIWnd
    {
        MenuActBtns = 0,
        SummonWnd,
        MagicWnd,
        ResultWnd,
        PauseWnd
    }

    // 애니메이션 매개변수
    public enum eAnimatorParameter
    {
        Tri_Attack,
        Tri_Magic,
        Tri_Summon,
        Tri_Damage,
        Bool_Run,
        Bool_Death
    }

    // 씬 종류
    public enum eScence
    {
        TitleScene=1,
        MapScene,
        BattleScene
    }

    // 유닛 방향 
    public struct sUnitWay
    {
        Vector3 N { get { return new Vector3(0, 0, 0); } }
        Vector3 E { get { return new Vector3(0, 0, 90); } }
        Vector3 S { get { return new Vector3(0, 0, 180); } }
        Vector3 W { get { return new Vector3(0, 0, 270); } }
    }
}
