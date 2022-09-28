using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnumList
{
    public enum eKindSummoner
    {
        Tester, OldMan, CastleGuard,
        COUNT
    }

    public enum eKindMonster
    {
        FireStone = 0 , GoblinRogue, Grunt, Bat, Ghost, Slime, Rabbit,
        Wizard,
        COUNT
    }

    public enum eStateUnit
    {
        AtkUp,
        AtkDown,
        DefUp,
        DefDown,
        SpdUp,
        SpdDown
    }

    public enum eUnitWay
    {
        N, E, S, W
    }

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

    public enum eTileHighlightStatus
    {
        None,
        Move,
        Enemy,
        Magic
    }

    public enum eTileUnit
    {
        Wall=-1,
        Empty = 0,
        Summoner,
        Monster
    }

    public enum eUIWnd
    {
        MenuActBtns = 0,
        SummonWnd,
        MagicWnd,
        ResultWnd,
        PauseWnd
    }

    public enum eAnimatorParameter
    {
        Tri_Attack,
        Tri_Magic,
        Tri_Summon,
        Tri_Damage,
        Bool_Run,
        Bool_Death
    }

    public enum eScence
    {
        TitleScene=1,
        MapScene,
        BattleScene
    }

    public struct sUnitWay
    {
        Vector3 N { get { return new Vector3(0, 0, 0); } }
        Vector3 E { get { return new Vector3(0, 0, 90); } }
        Vector3 S { get { return new Vector3(0, 0, 180); } }
        Vector3 W { get { return new Vector3(0, 0, 270); } }
    }
}
