using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 지도 씬의 스테이지 지도 관리
public class MapManager : NoOverlapSingleton<MapManager>
{
    public static eBattleType readyBattleType;
    [SerializeField] GameObject MapBgObj;
    [SerializeField] MapCreator[] _stages;

    MapPoint currentPoint;
    public int StageIdx { get; set; } = 0;
    public int StageCount => _stages.Length;

    void Start()
    {
        OpenStage(true);
    }


    public bool ClickPoint(MapPoint movePoint, out eMapPointState pointState)
    {
        pointState = movePoint.GetState();

        if (!movePoint.CheckLineMoveState())
            return false;
        if (movePoint != currentPoint)
        {
            foreach (var temp in movePoint.GetLines())
            {
                if (temp.ContainPoint(currentPoint))
                {
                    temp.UsedLine();
                    break;
                }
            }
        }

        SetCurrentPoint(movePoint);

        eMapPointState state = movePoint.GetState();
        if (state != eMapPointState.Town && state != eMapPointState.UpStair && state != eMapPointState.DownStair)
        {
            if (state == eMapPointState.Boss)
                movePoint.SetState(eMapPointState.DownStair);
            else
                movePoint.SetState(eMapPointState.Clear);

        }

        return true;
    }

    // 플레이어 현재 위치 포인트 설정
    void SetCurrentPoint(MapPoint point)
    {

        if (currentPoint != null)
        {
            currentPoint.GetComponent<SpriteRenderer>().color = Color.white;
            currentPoint.SetLineState(false);
        }

        currentPoint = point;
        currentPoint.GetComponent<SpriteRenderer>().color = Color.green;
        currentPoint.SetLineState(true);

    }

    // 현재 스테이지 지도 설정
    public void MapActive(bool val)
    {
        MapBgObj.SetActive(val);
        if(StageIdx>= StageCount)
        {
            MapControl._instance.SetLast();
            return;
        }
        _stages[StageIdx].gameObject.SetActive(val);
    }

    // 다음 스테이지 지도 설정
    public void OpenStage(bool NextMap)
    {
        if (StageIdx < 0)
        {
            StageIdx = 0;
            return;
        }

        if (StageIdx >= StageCount)
        {
            StageIdx = StageCount - 1;
            return;
        }

        foreach (var temp in _stages)
        {
            
            temp.gameObject.SetActive(false);
        }

        
        _stages[StageIdx].gameObject.SetActive(true);
        if(NextMap)
            SetCurrentPoint(_stages[StageIdx].GetStartPoint());
        else
            SetCurrentPoint(_stages[StageIdx].GetEndPoint());
        
    }

    public void DeleteMapManager()
    {
        Destroy(gameObject);
    }
}
