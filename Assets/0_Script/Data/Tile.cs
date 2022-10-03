using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    int x, y;
    EnumList.eTileHighlightStatus _tileStatus = EnumList.eTileHighlightStatus.None;
    public void SetTilePosition(int x, int y) { this.x = x; this.y = y; }
    public int X { get { return x; } }
    public int Y { get { return y; } }
    public EnumList.eTileHighlightStatus TileStatus { get { return _tileStatus; } set { _tileStatus = value; } }

    public int GetSpawnIdxUnit()
    {
        if (transform.childCount > 1)
        {
            return transform.GetChild(1).GetComponent<Unit>()._SpawnIdx;
        }
        return -1;
    }

    public GameObject GetTileObject() => transform.GetChild(0).gameObject;

    public Unit GetTileUnit() => transform.GetChild(1).GetComponent<Unit>();

    public GameObject GetUnitObject() => transform.GetChild(1).gameObject;

    public bool CheckUnitObject() => transform.childCount > 1;

}
