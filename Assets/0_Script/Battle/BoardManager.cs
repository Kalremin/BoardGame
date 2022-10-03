using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardManager : MonoBehaviour
{
    // 싱글톤
    public static BoardManager _instance;

    
    // 프리펩
    [SerializeField]
    GameObject _tile;

    [SerializeField]
    Material _noneM, _moveM, _enemyM, _magicM;

    [SerializeField]
    int _tileLength=10;

    Tile _playerTile;
    Tile[,] tileIdxArrays; 
    int size;
    Queue<int> resetListIdxTile;
    List<int> teamTilesIdx;
    List<int> aiEnemyTilesIdx;


    public int TileLength => _tileLength;
    public Tile[,] GetTileIdxArrays => tileIdxArrays;
    public Queue<int> GetResetListIdxTile => resetListIdxTile;
    public List<int> GetTeamTilesIdx => teamTilesIdx;
    public List<int> GetAiEnemyTilesIdx => aiEnemyTilesIdx;


    private void Awake()
    {
        _instance = this;
        resetListIdxTile = new Queue<int>();
        teamTilesIdx = new List<int>();
        aiEnemyTilesIdx = new List<int>();
    }


    // 배틀 보드 생성
    public void GenerateBoard(int length)
    {
        size = length;
        tileIdxArrays = new Tile[size, size];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                GameObject temp = Instantiate(_tile, new Vector3(i, 0, j)*_tileLength, _tile.transform.rotation);
                temp.GetComponent<Tile>().SetTilePosition(i, j);
                temp.transform.GetChild(0).localScale = Vector3.one * _tileLength;
                tileIdxArrays[i, j] = temp.GetComponent<Tile>();
                temp.transform.SetParent( transform);
            }
        }
    }

    // 선택할 타일이 상황(이동, 적대, 마법)에 따라 색상 변경
    public void HighlightTile(EnumList.eTileHighlightStatus status, eBattleType battleType, int x, int y)
    {
        if (x < 0 || x > (int)battleType || x < 0 || y > (int)battleType)
            return;

        Tile tile = tileIdxArrays[x,y];

        // 색을 바꿀 타일 선택
        switch (status)
        {
            case EnumList.eTileHighlightStatus.None:
                tile.GetTileObject().GetComponent<MeshRenderer>().material = _noneM;
                break;
            case EnumList.eTileHighlightStatus.Move:
                tile.GetTileObject().GetComponent<MeshRenderer>().material = _moveM;
                break;
            case EnumList.eTileHighlightStatus.Enemy:
                tile.GetTileObject().GetComponent<MeshRenderer>().material = _enemyM;
                break;
            case EnumList.eTileHighlightStatus.Magic:
                tile.GetTileObject().GetComponent<MeshRenderer>().material = _magicM;
                break;
        }
        tile.GetComponent<Tile>().TileStatus = status;
        

        int idx = tile.X * size + tile.Y;
        resetListIdxTile.Enqueue(idx);

    }


    public void MoveTile(Unit playUnit, eBattleType battleType)
    {

        Tile tempTile = playUnit.transform.parent.GetComponent<Tile>();
        int moveCount = playUnit._Move;
        int count = 0;

        ResetListTile();
        teamTilesIdx.Clear();
        aiEnemyTilesIdx.Clear();
        for (int i = tempTile.X - moveCount; i <= tempTile.X + moveCount; i++)
        {
            if (i < 0 || i >= (int)battleType)
            {
                if (i >= tempTile.X)
                {
                    count--;
                }
                else
                {
                    count++;
                }
                continue;
            }

            for (int j = tempTile.Y - count; j <= tempTile.Y + count; j++)
            {
                if (j < 0 || j >= (int)battleType)
                    continue;

                if (ExistUnitInTile(i, j) && !playUnit.GetComponentInParent<Tile>().Equals(GetTile(i, j)))
                {
                    Unit tempUnit = GetTile(i, j).GetUnitObject().GetComponent<Unit>();

                    if (tempUnit._PlayerTeam == playUnit._PlayerTeam)
                    {
                        teamTilesIdx.Add(i + j * size);
                    }
                    else
                    {
                        aiEnemyTilesIdx.Add(i + j * size);
                    }

                    

                    continue;
                }

                HighlightTile(EnumList.eTileHighlightStatus.Move, battleType, i, j);
            }

            if (i >= tempTile.X)
            {
                count--;
            }
            else
            {
                count++;
            }
        }
        HighlightTile(EnumList.eTileHighlightStatus.Move, battleType, tempTile.X, tempTile.Y);
    }

    public void AttackTile()
    {
        Tile unitTile = BattleManager._instance.PlayUnit.transform.parent.GetComponent<Tile>();
        eBattleType battleType = BattleManager._instance.BattleType;

        ResetListTile();
        if (ExistUnitInTile(unitTile.X - 1, unitTile.Y))
            HighlightTile(EnumList.eTileHighlightStatus.Enemy, battleType, unitTile.X - 1, unitTile.Y);
        if (ExistUnitInTile(unitTile.X + 1, unitTile.Y))
            HighlightTile(EnumList.eTileHighlightStatus.Enemy, battleType, unitTile.X + 1, unitTile.Y);
        if (ExistUnitInTile(unitTile.X, unitTile.Y - 1))
            HighlightTile(EnumList.eTileHighlightStatus.Enemy, battleType, unitTile.X, unitTile.Y - 1);
        if (ExistUnitInTile(unitTile.X, unitTile.Y + 1))
            HighlightTile(EnumList.eTileHighlightStatus.Enemy, battleType, unitTile.X, unitTile.Y + 1);
    }

    public void MagicTile()
    {
        eBattleType battleType = BattleManager._instance.BattleType;

        ResetListTile();

        if (BattleManager._instance.PlayUnit.CompareTag("Monster"))
        {
            Monster temp = (Monster)BattleManager._instance.PlayUnit;
            int x = temp.GetComponentInParent<Tile>().X;
            int y = temp.GetComponentInParent<Tile>().Y;

            for (int i = x-temp.MagicRange; i <= x+temp.MagicRange; i++)
            {
                for (int j = y-temp.MagicRange; j <= y+ temp.MagicRange; j++)
                {
                    if (ExistUnitInTile(i, j))
                    {
                        HighlightTile(EnumList.eTileHighlightStatus.Magic, battleType, i, j);
                    }

                }
            }

            return;
        }

        if (BattleManager._instance.PlayUnit.CompareTag("Summoner"))
        {
            for (int i = 0; i < (int)battleType; i++)
            {
                for (int j = 0; j < (int)battleType; j++)
                {
                    if (GetTile(i, j).transform.childCount > 1)
                    {
                        HighlightTile(EnumList.eTileHighlightStatus.Magic, battleType, i, j);
                    }
                }
            }

            return;
        }
    }

    public void SummonTile()
    {
        Tile unitTile = BattleManager._instance.PlayUnit.transform.parent.GetComponent<Tile>();
        eBattleType battleType = BattleManager._instance.BattleType;

        ResetListTile();

        if (unitTile.X - 1 >= 0 && !ExistUnitInTile(unitTile.X - 1, unitTile.Y))
            HighlightTile(EnumList.eTileHighlightStatus.Magic, battleType, unitTile.X - 1, unitTile.Y);
        if (unitTile.X + 1 < (int)battleType && !ExistUnitInTile(unitTile.X + 1, unitTile.Y))
            HighlightTile(EnumList.eTileHighlightStatus.Magic, battleType, unitTile.X + 1, unitTile.Y);
        if (unitTile.Y - 1 >= 0 && !ExistUnitInTile(unitTile.X, unitTile.Y - 1))
            HighlightTile(EnumList.eTileHighlightStatus.Magic, battleType, unitTile.X, unitTile.Y - 1);
        if (unitTile.Y + 1 < (int)battleType && !ExistUnitInTile(unitTile.X, unitTile.Y + 1))
            HighlightTile(EnumList.eTileHighlightStatus.Magic, battleType, unitTile.X, unitTile.Y + 1);
    }

    // 변환된 타일 리셋
    public void ResetListTile()
    {
        while (resetListIdxTile.Count > 0)
        {
            int idx = resetListIdxTile.Dequeue();
            transform.GetChild(idx).GetChild(0).GetComponent<MeshRenderer>().material = _noneM;
            transform.GetChild(idx).GetComponent<Tile>().TileStatus = EnumList.eTileHighlightStatus.None;
        }
    }

    public void ResetTile(Tile tile)
    {
        tile.transform.GetChild(0).GetComponent<MeshRenderer>().material = _noneM;
        tile.TileStatus = EnumList.eTileHighlightStatus.None;
    }

    public Tile GetTile(int x, int y)
    {
        
        return tileIdxArrays[x,y];
    }

    // 타일 사용 가능 확인 / T: 유닛 존재, F: 유닛 비존재
    public bool ExistUnitInTile(int x, int y)
    {
        if (!CheckInTileBound(x, y))
            return false;


        return tileIdxArrays[x,y].transform.childCount > 1;
    }

    // 경계선 확인 / T: 범위 내, F: 범위 밖
    public bool CheckInTileBound(int x, int y)
    {
        if (x < 0 || x >= (int)BattleManager._instance.BattleType || y < 0 || y >= (int)BattleManager._instance.BattleType)
            return false;

        return true;
    }

}
