using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MapManager : MonoBehaviour
{
    public static MapManager _instance;
    public static eBattleType readyBattleType;
    [SerializeField] GameObject MapBgObj;
    [SerializeField] MapCreator[] _stages;

    MapPoint currentPoint;
    public int StageIdx { get; set; } = 0;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
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

    public void MapActive(bool val)
    {
        MapBgObj.SetActive(val);
        if(StageIdx>= StageCount)
        {
            ///  마무리
            MapControl._instance.SetLast();
            return;
        }
        _stages[StageIdx].gameObject.SetActive(val);
    }

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

    public int StageCount => _stages.Length;

    

}
