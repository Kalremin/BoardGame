using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적 유닛의 타일 자동 선택
public static  class AutoSelect
{
    static Summoner _player;
    static Tile _targetTile = null;
    static List<Tile> attackTileList = new List<Tile>();

    // 공격 타일 자동 선택
    public static bool CheckTileAttackUnit(Unit unit)
    {
        Tile tile = unit.GetComponentInParent<Tile>();

        attackTileList.Clear();
        if (BoardManager._instance.ExistUnitInTile(tile.X - 1, tile.Y)
                && BoardManager._instance.GetTile(tile.X - 1, tile.Y).GetUnitObject().GetComponent<Unit>()._PlayerTeam)
        {
            attackTileList.Add(BoardManager._instance.GetTile(tile.X - 1, tile.Y));
        }
        if (BoardManager._instance.ExistUnitInTile(tile.X + 1, tile.Y)
                && BoardManager._instance.GetTile(tile.X + 1, tile.Y).GetUnitObject().GetComponent<Unit>()._PlayerTeam)
        {
            attackTileList.Add(BoardManager._instance.GetTile(tile.X + 1, tile.Y));
        }
        if (BoardManager._instance.ExistUnitInTile(tile.X, tile.Y - 1)
               && BoardManager._instance.GetTile(tile.X, tile.Y - 1).GetUnitObject().GetComponent<Unit>()._PlayerTeam)
        {
            attackTileList.Add(BoardManager._instance.GetTile(tile.X, tile.Y - 1));
        }
        if (BoardManager._instance.ExistUnitInTile(tile.X, tile.Y + 1)
               && BoardManager._instance.GetTile(tile.X, tile.Y + 1).GetUnitObject().GetComponent<Unit>()._PlayerTeam)
        {
            attackTileList.Add(BoardManager._instance.GetTile(tile.X, tile.Y + 1));
        }

        if (attackTileList.Count > 0)
            return true;

        return false;
    }

    // 유닛의 마법 공격 여부
    public static bool CheckTileMagicAttackUnit(Monster monster)
    {
        Tile tile = monster.GetComponentInParent<Tile>();
        int count = 0;

        attackTileList.Clear();
        for(int i=tile.X - monster.MagicRange; i <= tile.X + monster.MagicRange; i++)
        {
            if (i < 0 || i >= (int)MapManager.readyBattleType)
            {
                if (i >= tile.X)
                {
                    count--;
                }
                else
                {
                    count++;
                }
                continue;
            }

            for (int j = tile.Y - count; j <= tile.Y + count; j++)
            {
                if (j < 0 || j >= (int)MapManager.readyBattleType 
                    || !BoardManager._instance.ExistUnitInTile(i,j))
                    continue;

                if (BoardManager._instance.GetTile(i, j).GetUnitObject().GetComponent<Unit>()._PlayerTeam)
                {
                    attackTileList.Add(BoardManager._instance.GetTile(i, j));
                }

            }
        }

        if (attackTileList.Count > 0)
            return true;

        return false;
    }

    // 공격 대상 타일 선택
    public static Tile SelectTileAttack(Unit unit)
    {
        if (attackTileList == null || attackTileList.Count == 0)
            return null;

        return attackTileList[UnityEngine.Random.Range(0, attackTileList.Count)];
        
    }

    
    // 적 소환사의 소환할 타일 선택
    public static Tile SelectTileSummon(Unit unit)
    {
        Tile tempTile = null;
        Tile unitTile = unit.transform.parent.GetComponent<Tile>();
        int random;

        int ex = 0;

        while (tempTile == null)
        {
            if (++ex > 20)
                break;

            random = UnityEngine.Random.Range(0, 4);
            switch (random)
            {
                case 0: //W
                    if (BoardManager._instance.CheckInTileBound(unitTile.X - 1, unitTile.Y)
                        && !BoardManager._instance.ExistUnitInTile(unitTile.X - 1, unitTile.Y))
                    {
                        tempTile = BoardManager._instance.GetTileIdxArrays[unitTile.X - 1, unitTile.Y];
                        unit.SetWay(EnumList.eUnitWay.W);
                    }
                    break;
                case 1: //E
                    if (BoardManager._instance.CheckInTileBound(unitTile.X + 1, unitTile.Y)
                        && !BoardManager._instance.ExistUnitInTile(unitTile.X + 1, unitTile.Y))
                    {
                        tempTile = BoardManager._instance.GetTileIdxArrays[unitTile.X + 1, unitTile.Y];
                        unit.SetWay(EnumList.eUnitWay.E);
                    }
                    break;
                case 2: //S
                    if (BoardManager._instance.CheckInTileBound(unitTile.X, unitTile.Y - 1)
                        && !BoardManager._instance.ExistUnitInTile(unitTile.X, unitTile.Y - 1))
                    {
                        tempTile = BoardManager._instance.GetTileIdxArrays[unitTile.X, unitTile.Y - 1];
                        unit.SetWay(EnumList.eUnitWay.S);
                    }
                    break;
                case 3: //N
                    if (BoardManager._instance.CheckInTileBound(unitTile.X, unitTile.Y + 1)
                        && !BoardManager._instance.ExistUnitInTile(unitTile.X, unitTile.Y + 1))
                    {
                        tempTile = BoardManager._instance.GetTileIdxArrays[unitTile.X, unitTile.Y + 1];
                        unit.SetWay(EnumList.eUnitWay.N);
                    }
                    break;
            }

        }

        _targetTile = null;

        return tempTile;

    }

    // 이동할 타일 선택
    public static Tile SelectTileMove(Unit unit)
    {
        if (unit.CompareTag("Summoner"))
            return FindTileMoveSummoner(unit.transform.parent.position);
        else if (unit.CompareTag("Monster"))
            return FindTileMoveMonster(unit.transform.parent.position);

        throw new System.Exception("No Tag Object");
    }

    // 적 소환사의 이동 타일 찾기
    static Tile FindTileMoveSummoner(Vector3 summonerPos)
    {
        Tile tempTile = null;

        int size = (int)BattleManager._instance.BattleType;
        float tempDist = 0;

        // 플레이어의 유닛 수 확인
        if (BoardManager._instance.GetAiEnemyTilesIdx.Count > 0) 
        {   
            BoardManager._instance.ResetListTile();

            for (int i = 0; i < BoardManager._instance.GetAiEnemyTilesIdx.Count; i++)
            {
                int x, y;
                x = BoardManager._instance.GetAiEnemyTilesIdx[i] % size;
                y = BoardManager._instance.GetAiEnemyTilesIdx[i] / size;


                if (x - 1 > 0 && tempDist < Vector3.Distance(summonerPos, BoardManager._instance.GetTile(x - 1, y).transform.position))
                {
                    _targetTile = BoardManager._instance.GetTile(x, y);
                    tempTile = BoardManager._instance.GetTile(x - 1, y);
                    tempDist = Vector3.Distance(summonerPos, BoardManager._instance.GetTile(x - 1, y).transform.position);
                }
                if (x + 1 < size && tempDist < Vector3.Distance(summonerPos, BoardManager._instance.GetTile(x + 1, y).transform.position))
                {
                    _targetTile = BoardManager._instance.GetTile(x, y);
                    tempTile = BoardManager._instance.GetTile(x + 1, y);
                    tempDist = Vector3.Distance(summonerPos, BoardManager._instance.GetTile(x + 1, y).transform.position);
                }
                if (y - 1 > 0 && tempDist < Vector3.Distance(summonerPos, BoardManager._instance.GetTile(x, y - 1).transform.position))
                {
                    _targetTile = BoardManager._instance.GetTile(x, y);
                    tempTile = BoardManager._instance.GetTile(x, y - 1);
                    tempDist = Vector3.Distance(summonerPos, BoardManager._instance.GetTile(x, y - 1).transform.position);
                }
                if (y + 1 < size && tempDist < Vector3.Distance(summonerPos, BoardManager._instance.GetTile(x, y + 1).transform.position))
                {
                    _targetTile = BoardManager._instance.GetTile(x, y);
                    tempTile = BoardManager._instance.GetTile(x, y + 1);
                    tempDist = Vector3.Distance(summonerPos, BoardManager._instance.GetTile(x, y + 1).transform.position);
                }


            }

        }
        else
        {
            float dist;
            while (BoardManager._instance.GetResetListIdxTile.Count > 0)
            {
                int tempIdx = BoardManager._instance.GetResetListIdxTile.Dequeue();
                int x, y;
                x = tempIdx / size;
                y = tempIdx % size;

                Tile moveTile = BoardManager._instance.GetTile(x, y);
                BoardManager._instance.ResetTile(moveTile);

                dist = Vector3.Distance(GetPlayerUnitTile().transform.position, moveTile.transform.position);

                if (tempDist < dist)
                {
                    tempTile = moveTile;
                    tempDist = dist;
                }
            }

            _targetTile = null;

        }

        return tempTile;
    }

    // 적 몬스터의 이동 타일 찾기
    static Tile FindTileMoveMonster(Vector3 monsterPos)
    {
        Tile tempTile = null;

        int size = (int)BattleManager._instance.BattleType;
        float tempDist = float.MaxValue;

        // 플레이어의 유닛 수 확인
        if (BoardManager._instance.GetAiEnemyTilesIdx.Count > 0)
        {
            for (int i = 0; i < BoardManager._instance.GetAiEnemyTilesIdx.Count; i++)
            {
                int x, y;
                x = BoardManager._instance.GetAiEnemyTilesIdx[i] % size;
                y = BoardManager._instance.GetAiEnemyTilesIdx[i] / size;
                
                if (x - 1 > 0
                    && tempDist > Vector3.Distance(monsterPos, BoardManager._instance.GetTile(x - 1, y).transform.position)
                    && BoardManager._instance.GetTile(x - 1, y).TileStatus == EnumList.eTileHighlightStatus.Move)
                {
                    _targetTile = BoardManager._instance.GetTile(x, y);
                    tempTile = BoardManager._instance.GetTile(x - 1, y);
                    tempDist = Vector3.Distance(monsterPos, BoardManager._instance.GetTile(x - 1, y).transform.position);
                }
                if (x + 1 < size
                    && tempDist > Vector3.Distance(monsterPos, BoardManager._instance.GetTile(x + 1, y).transform.position)
                    && BoardManager._instance.GetTile(x + 1, y).TileStatus == EnumList.eTileHighlightStatus.Move)
                {
                    _targetTile = BoardManager._instance.GetTile(x, y);
                    tempTile = BoardManager._instance.GetTile(x + 1, y);
                    tempDist = Vector3.Distance(monsterPos, BoardManager._instance.GetTile(x + 1, y).transform.position);
                }
                if (y - 1 > 0
                    && tempDist > Vector3.Distance(monsterPos, BoardManager._instance.GetTile(x, y - 1).transform.position)
                    && BoardManager._instance.GetTile(x, y - 1).TileStatus == EnumList.eTileHighlightStatus.Move)
                {
                    _targetTile = BoardManager._instance.GetTile(x, y);
                    tempTile = BoardManager._instance.GetTile(x, y - 1);
                    tempDist = Vector3.Distance(monsterPos, BoardManager._instance.GetTile(x, y - 1).transform.position);
                }
                if (y + 1 < size
                    && tempDist > Vector3.Distance(monsterPos, BoardManager._instance.GetTile(x, y + 1).transform.position)
                    && BoardManager._instance.GetTile(x, y + 1).TileStatus == EnumList.eTileHighlightStatus.Move)
                {
                    _targetTile = BoardManager._instance.GetTile(x, y);
                    tempTile = BoardManager._instance.GetTile(x, y + 1);
                    tempDist = Vector3.Distance(monsterPos, BoardManager._instance.GetTile(x, y + 1).transform.position);
                }

            }

            BoardManager._instance.ResetListTile();
        }
        else
        {
            float dist;

            while (BoardManager._instance.GetResetListIdxTile.Count > 0)
            {
                int tempIdx = BoardManager._instance.GetResetListIdxTile.Dequeue();
                int x, y;
                x = tempIdx / size;
                y = tempIdx % size;

                Tile moveTile = BoardManager._instance.GetTile(x, y);
                BoardManager._instance.ResetTile(moveTile);

                dist = Vector3.Distance(GetPlayerUnitTile().transform.position, moveTile.transform.position);

                if (tempDist > dist)
                {
                    tempTile = moveTile;
                    tempDist = dist;
                }
            }

            _targetTile = null;

        }
        return tempTile;
    }

    // 플레이어의 소환사 설정
    public static void SetPlayerUnit(Summoner player) => _player = player;

    // 플레이어의 소환사 타일 전달
    public static Tile GetPlayerUnitTile() => _player.GetComponentInParent<Tile>();

    public static Tile TargetTile => _targetTile;

    public static void ResetTargetTile() => _targetTile = null;

}
