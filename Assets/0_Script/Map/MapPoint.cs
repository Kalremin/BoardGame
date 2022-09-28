using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eMapPointState
{
    Clear,
    DownStair,

    Random,
    Battle,

    Treasure,
    Gold,

    Town,
    Boss,
    UpStair

}

public class MapPoint : MonoBehaviour
{
    [SerializeField] Sprite[] _mapIcons;

    List<MapLine> mapLines = new List<MapLine>();
    List<MapPoint> otherPoints = new List<MapPoint>();
    
    eMapPointState _pointState;

    public void SetAddtionalState()
    {
        SetState((eMapPointState)Random.Range((int)eMapPointState.Random, (int)eMapPointState.Random + 2));

    }

    public void SetSemiEssentialState()
    {
        SetState((eMapPointState)Random.Range((int)eMapPointState.Treasure, (int)eMapPointState.Treasure + 2));
    }

    public void SetEssentialState()
    {
        SetState((eMapPointState)Random.Range((int)eMapPointState.Town, (int)eMapPointState.Town + 3));
    }

    public void SetState(eMapPointState state)
    {

        _pointState = state;
        GetComponent<SpriteRenderer>().sprite = _mapIcons[(int)_pointState];
    }

    public eMapPointState GetState() => _pointState;

    public List<MapLine> GetLines() => mapLines;

    public void AddLine(MapLine line)
    {
        mapLines.Add(line);
    }

    public bool ContainLine(MapLine line)
    {
        return mapLines.Contains(line);
    }

    public void AddPoint(MapPoint point)
    {
        otherPoints.Add(point);
    }

    public void SetLineState(bool val)
    {
        foreach(var temp in mapLines)
        {
            temp.moveState = val;
        }
    }

    public bool CheckLineMoveState()
    {
        foreach (var temp in mapLines)
        {
            if (temp.moveState)
                return true;
        }

        return false;
    }

    public void PointerClick()
    {
        PointerExit();
        GetComponent<SpriteRenderer>().sprite = _mapIcons[(int)eMapPointState.Clear];
        
    }

    public void PointerEnter()
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1);
    }

    public void PointerExit()
    {
        transform.localScale = Vector3.one;
    }

}
